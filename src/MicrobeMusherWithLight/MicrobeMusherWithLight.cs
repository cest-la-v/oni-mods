using HarmonyLib;
using TUNING;
using UnityEngine;

namespace MightyVincent
{
    internal class MicrobeMusherWithLightPatches
    {
/*
        [HarmonyPatch(typeof(MicrobeMusherConfig), "DoPostConfigurePreview")]
        internal class MicrobeMusherConfig_DoPostConfigurePreview
        {
            private static void Postfix(MicrobeMusherConfig __instance , BuildingDef def, GameObject go)
            {
                if (__instance.GetType() != typeof(MicrobeMusherConfig)) return;
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
            private static void Postfix(MicrobeMusherConfig __instance, GameObject go)
            {
                go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource);
                var light2D = go.AddOrGet<Light2D>();
                light2D.overlayColour = LIGHT2D.LIGHT_OVERLAY;
                light2D.Color = LIGHT2D.LIGHT_YELLOW;
                light2D.Range = 3f;
                light2D.Angle = 45.0f;
                light2D.Direction = LIGHT2D.DEFAULT_DIRECTION;
                light2D.Offset = new Vector2(1.4f, 2.5f);
                light2D.shape = LightShape.Circle;
                light2D.drawOverlay = true;
                go.AddOrGetDef<WorkingLightController.Def>();
            }
        }
    }
}