using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace AsLimc.SimplerPipPlantRule {
    internal static class Patches {
        
        [HarmonyPatch(typeof(SeedPlantingMonitor.Def), MethodType.Constructor)]
        internal class SeedPlantingMonitor_Def_Constructor {
            public static void Postfix(ref float ___searchMinInterval, ref float ___searchMaxInterval) {
                ___searchMinInterval = Settings.Get().SearchMinInterval;
                ___searchMaxInterval = Settings.Get().SearchMaxInterval;
            }
        }

        [HarmonyPatch(typeof(PlantableCellQuery), MethodType.Constructor)]
        internal class PlantableCellQuery_Constructor {
            public static void Postfix(ref int ___plantDetectionRadius, ref int ___maxPlantsInRadius) {
                ___plantDetectionRadius = Settings.Get().PlantDetectionRadius;
                ___maxPlantsInRadius = Settings.Get().MaxPlantsInRadius;
            }
        }

        [HarmonyPatch(typeof(PlantableCellQuery), "CountNearbyPlants")]
        internal class PlantableCellQuery_CountNearbyPlants {
            public static bool Prefix(int cell, int radius, out int __result) {
//                Debug.Log($"CountNearbyPlants Prefix: {cell}, {radius}");
                Grid.CellToXY(cell, out var x, out var y);
                var side = radius * 2 + 1;
                var x_bottomLeft = x - radius;
                var y_bottomLeft = y - radius;
                var pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();

                GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, side, side, GameScenePartitioner.Instance.plants, pooledList);
                var countWithoutTreeBud = pooledList.Count(entry => !(bool) ((Component) entry.obj).GetComponent<TreeBud>());
//                var total = pooledList.Count;
                pooledList.Recycle();
//                Debug.Log($"countWithoutTreeBud: ({x},{y}), {countWithoutTreeBud} / {total}");
                __result = countWithoutTreeBud;
                return false;
            }
        }
    }
}