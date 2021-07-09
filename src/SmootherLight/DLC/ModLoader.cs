using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace AsLimc.SmootherLight {
    public class ModLoader : UserMod2 {
        public override void OnLoad(Harmony harmony) {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new POptions().RegisterOptions(this, typeof(Settings));
            Settings.Init(POptions.ReadSettings<Settings>());
        }
    }
}