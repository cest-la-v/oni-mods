using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace AsLimc.SmootherLight
{
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    public class Settings
    {
        private static Settings _instance;
        public static Settings Get() => _instance;

        public static void Init(Settings settings) {
            _instance ??= settings ?? new Settings();
        }

        [Option("Light through Mesh Tiles", "Can light go through Mesh Tiles. (Restart Needed)")]
        [JsonProperty]
        public bool LightThroughMeshTiles { get; set; }

        [Option("Light Range Tolerance", "Tolerance when checking if a cell is in range, the bigger the farther.")]
        [JsonProperty]
        public int LightRangeTolerance { get; set; }

        public Settings()
        {
            LightThroughMeshTiles = false;
            LightRangeTolerance = 1;
        }
    }
}