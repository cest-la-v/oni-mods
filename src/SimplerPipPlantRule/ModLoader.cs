using AsLimc.Commons;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;

namespace AsLimc.SimplerPipPlantRule {
    public class ModLoader : UserMod2 {
        public override void OnLoad(Harmony harmony) {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            VLib.Init();
            Settings.Init(this);
        }
    }
}