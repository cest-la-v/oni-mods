using HarmonyLib;

namespace AsLimc.Commons {
    public class VPatches {

        [HarmonyPatch(typeof(Db), "Initialize")]
        internal class Db_Initialize {
            private static void Postfix(Db __instance) {
                VLib.Init(__instance);
            }
        }
    }
}