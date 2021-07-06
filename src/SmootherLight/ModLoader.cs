using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;

namespace AsLimc.SmootherLight
{
    public class ModLoader : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            Settings.Init(this);
        }
    }
}