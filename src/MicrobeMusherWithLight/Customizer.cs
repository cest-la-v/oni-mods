using TUNING;
using UnityEngine;

namespace MightyVincent {
    internal static class Customizer {
        public static void DoPostConfigureComplete(GameObject go) {
            Debug.Log($"DoPostConfigureComplete: {go}");
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
            light2D.enabled = false;
            go.AddOrGetDef<WorkingLightController.Def>();
        }
    }
}