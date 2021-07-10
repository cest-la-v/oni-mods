using AsLimc.Commons;
using PeterHan.PLib;
using PeterHan.PLib.Datafiles;

namespace AsLimc.HyperReservoir {
    internal class ModLoader {
        public static void OnLoad() {
            PUtil.InitLibrary();
            PLocalization.Register();
            VLib.Register();
        }
    }
}