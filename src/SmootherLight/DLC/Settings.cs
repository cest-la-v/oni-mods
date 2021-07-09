using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace AsLimc.SmootherLight
{
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    public class Settings
    {
        private static Settings _INSTANCE;
        public static Settings Get() => _INSTANCE;

        public static void Init(Settings settings) {
            _INSTANCE ??= settings ?? new Settings();
        }

        [Option("Light through Mesh Tiles", "Can light go through Mesh Tiles. (Restart Needed)")]
        [JsonProperty]
        public bool LightThroughMeshTiles { get; set; }

        public Settings()
        {
            LightThroughMeshTiles = false;
        }
    }
}