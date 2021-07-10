using AsLimc.Commons;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Database;
using PeterHan.PLib.Options;

namespace AsLimc.HyperReservoir {
    public class ModLoader: UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new PLocalization().Register();
            VLib.Register();
            new POptions().RegisterOptions(this, typeof(Settings));
            Settings.Init(POptions.ReadSettings<Settings>());
        }
    }
}