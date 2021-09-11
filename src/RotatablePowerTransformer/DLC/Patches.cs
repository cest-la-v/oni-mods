﻿using HarmonyLib;

namespace MightyVincent
{
    [HarmonyPatch(typeof(PowerTransformerConfig), "CreateBuildingDef")]
    public class PowerTransformerConfigPatch
    {
        public static void Postfix(BuildingDef __result)
        {
            __result.PermittedRotations = PermittedRotations.R360;
            __result.BuildLocationRule = BuildLocationRule.OnFoundationRotatable;
            __result.ContinuouslyCheckFoundation = true;
        }
    }

    [HarmonyPatch(typeof(PowerTransformerSmallConfig), "CreateBuildingDef")]
    public class PowerTransformerSmallConfigPatch
    {
        public static void Postfix(BuildingDef __result)
        {
            __result.PermittedRotations = PermittedRotations.R360;
            __result.BuildLocationRule = BuildLocationRule.OnFoundationRotatable;
            __result.ContinuouslyCheckFoundation = true;
        }
    }
}