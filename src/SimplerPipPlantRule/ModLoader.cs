using AsLimc.commons;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace AsLimc.SimplerPipPlantRule
{
    public class ModLoader : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new POptions().RegisterOptions(this,typeof(Settings));
            // POptions.RegisterOptions(this,typeof(Settings));
            VLib.Init();
            Settings.Init();
        }
    }
}