using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace AsLimc.SmootherLight
{
    internal static class Patches
    {
        [HarmonyPatch(typeof(DiscreteShadowCaster), "GetVisibleCells")]
        internal class DiscreteShadowCaster_GetVisibleCells
        {
            public static bool Prefix(int cell, List<int> visiblePoints, int range, LightShape shape)
            {
                LightGridTool.GetVisibleCells(cell, visiblePoints, range, shape);
                return false;
            }
        }

        [HarmonyPatch(typeof(MeshTileConfig), "CreateBuildingDef")]
        internal class MeshTileConfig_CreateBuildingDef
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.BlockTileIsTransparent = Settings.Get().LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(MeshTileConfig), "ConfigureBuildingTemplate")]
        internal class MeshTileConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<SimCellOccupier>().setTransparent = Settings.Get().LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(GasPermeableMembraneConfig), "CreateBuildingDef")]
        internal class GasPermeableMembraneConfig_CreateBuildingDef
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.BlockTileIsTransparent = Settings.Get().LightThroughMeshTiles;
            }
        }

        [HarmonyPatch(typeof(GasPermeableMembraneConfig), "ConfigureBuildingTemplate")]
        internal class GasPermeableMembraneConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddOrGet<SimCellOccupier>().setTransparent = Settings.Get().LightThroughMeshTiles;
            }
        }
    }
}