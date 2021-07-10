using AsLimc.Commons;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Database;

namespace AsLimc.HyperReservoir {
    public class ModLoader: UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new PLocalization().Register();
            VLib.Register();
        }
    }
}