using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace AsLimc.SmootherLight {
    public class ModLoader {
        public static void OnLoad() {
            PUtil.InitLibrary();
            POptions.RegisterOptions(typeof(Settings));
            Settings.Init(POptions.ReadSettings<Settings>());
        }
    }
}