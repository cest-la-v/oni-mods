using KMod;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace AsLimc.SmootherLight
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        private static Settings _INSTANCE;
        public static Settings Get() => _INSTANCE;

        public static void Init(UserMod2 modLoader) {
            new POptions().RegisterOptions(modLoader, typeof(Settings));
            _INSTANCE ??= POptions.ReadSettings<Settings>() ?? new Settings();
        }

        [Option("Light through Mesh Tiles", "Can light go through Mesh Tiles. (Restart Needed)", null)]
        [JsonProperty]
        public bool LightThroughMeshTiles { get; set; }

        public Settings()
        {
            LightThroughMeshTiles = false;
        }
    }
}