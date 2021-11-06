using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace AsLimc.LaserTurret
{
    [ModInfo("AsLimc.LaserTurret.LocStrings.Settings.MOD_NAME", "https://github.com/v-limc/oni-mods", collapse: true)]
    [RestartRequired]
    [JsonObject(MemberSerialization.OptIn)]
    internal class Settings
    {
        private static Settings _INSTANCE = new Settings();
        public static Settings Get() => _INSTANCE;

        public static void Init(Settings settings) {
            if (settings != null) {
                _INSTANCE = settings;
            }
        }
        
        [Option("Radius (tiles)", "Radius of the range for creature search.", null)]
        [JsonProperty]
        public int Radius { get; set; }

        public Settings()
        {
            Radius = 7;
        }
    }
}