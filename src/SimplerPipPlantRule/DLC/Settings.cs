using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace AsLimc.SimplerPipPlantRule
{
    [ModInfo("AsLimc.HyperReservoir.LocStrings.Settings.MOD_NAME", "https://github.com/as-limc/oni-mods", collapse: true)]
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
        
        [Option("Search Min Interval (seconds)", "min interval for seed search.", null)]
        [JsonProperty]
        public float SearchMinInterval { get; set; }

        [Option("Search Max Interval (seconds)", "max interval for seed search.", null)]
        [JsonProperty]
        public float SearchMaxInterval { get; set; }

        [Option("Plant Detection Radius (tiles)", "detection area is a square area with 'plantDetectionRadius * 2 + 1' length of the sides and centered at the target tile.", null)]
        [JsonProperty]
        public int PlantDetectionRadius { get; set; }

        [Option("Max Plants In Radius (tiles)", "max allowed plants in detection area of the target tile is 'maxPlantsInRadius + 1'.", null)]
        [JsonProperty]
        public int MaxPlantsInRadius { get; set; }

        public Settings()
        {
            SearchMinInterval = 60f;
            SearchMaxInterval = 300f;
            PlantDetectionRadius = 1;
            MaxPlantsInRadius = 0;
        }
    }
}