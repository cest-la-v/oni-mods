using AsLimc.commons;
using HarmonyLib;
using KMod;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace AsLimc.SimplerPipPlantRule
{
    public class ModLoader : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            POptions.RegisterOptions(typeof(Settings));
            VLib.Init();
            Settings.Init();
        }
    }
}