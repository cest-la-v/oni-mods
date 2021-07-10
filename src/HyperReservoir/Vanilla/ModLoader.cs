using AsLimc.Commons;
using PeterHan.PLib;
using PeterHan.PLib.Datafiles;
using PeterHan.PLib.Options;

namespace AsLimc.HyperReservoir {
    internal class ModLoader {
        public static void OnLoad() {
            PUtil.InitLibrary();
            PLocalization.Register();
            VLib.Init();
            POptions.RegisterOptions(typeof(Settings));
            Settings.Init(POptions.ReadSettings<Settings>());
        }
    }
}