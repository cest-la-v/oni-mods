using HarmonyLib;
using UnityEngine;

namespace MightyVincent
{
    internal class Patches
    {
        /*
        [HarmonyPatch(typeof(MicrobeMusherConfig), "DoPostConfigurePreview")]
        internal class MicrobeMusherConfig_DoPostConfigurePreview
        {
            private static void Postfix(MicrobeMusherConfig __instance , BuildingDef def, GameObject go)
            {
                Debug.Log($"--------------patched: {__instance.GetType()}, {def}, {go}, {go.AddOrGet<LightShapePreview>()}");
                var lightShapePreview = go.AddOrGet<LightShapePreview>();
                lightShapePreview.lux = 1000;
                lightShapePreview.radius = 2f;
                lightShapePreview.shape = LightShape.Circle;
                lightShapePreview.offset = new CellOffset(def.BuildingComplete.GetComponent<Light2D>().Offset);
            }
        }
        */

        [HarmonyPatch(typeof(MicrobeMusherConfig), "DoPostConfigureComplete")]
        internal class MicrobeMusherConfig_DoPostConfigureComplete
        {
            private static void Postfix(MicrobeMusherConfig __instance, GameObject go) {
                Customizer.DoPostConfigureComplete(go);
            }
        }
    }
}