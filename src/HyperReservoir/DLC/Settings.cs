using Newtonsoft.Json;
using PeterHan.PLib.Options;

// ReSharper disable InconsistentNaming

namespace AsLimc.HyperReservoir {
    [ModInfo("AsLimc.HyperReservoir.LocStrings.Settings.MOD_NAME", "https://github.com/viva-la-v/oni-mods", collapse: true)]
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    public class Settings {
        private static Settings _INSTANCE = new Settings();
        public static Settings Get() => _INSTANCE;

        public static void Init(Settings settings) {
            if (settings != null) {
                _INSTANCE = settings;
            }
        }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirSteelMassKg.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirSteelMassKg.TOOLTIP",
            null)]
        [JsonProperty]
        public float LiquidReservoirSteelMassKg { get; set; }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirPlasticMassKg.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirPlasticMassKg.TOOLTIP",
            null)]
        [JsonProperty]
        public float LiquidReservoirPlasticMassKg { get; set; }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirPowerConsumptionWatts.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirPowerConsumptionWatts.TOOLTIP",
            null)]
        [JsonProperty]
        public float LiquidReservoirPowerConsumptionWatts { get; set; }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirCapacityMultiplier.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.LiquidReservoirCapacityMultiplier.TOOLTIP",
            null)]
        [Limit(1, 20)]
        [JsonProperty]
        public float LiquidReservoirCapacityMultiplier { get; set; }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirSteelMassKg.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirSteelMassKg.TOOLTIP",
            null)]
        [JsonProperty]
        public float GasReservoirSteelMassKg { get; set; }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirPlasticMassKg.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirPlasticMassKg.TOOLTIP",
            null)]
        [JsonProperty]
        public float GasReservoirPlasticMassKg { get; set; }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirPowerConsumptionWatts.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirPowerConsumptionWatts.TOOLTIP",
            null)]
        [JsonProperty]
        public float GasReservoirPowerConsumptionWatts { get; set; }

        [Option("AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirCapacityMultiplier.NAME",
            "AsLimc.HyperReservoir.LocStrings.Settings.GasReservoirCapacityMultiplier.TOOLTIP",
            null)]
        [Limit(1, 666)]
        [JsonProperty]
        public float GasReservoirCapacityMultiplier { get; set; }

        public Settings() {
            LiquidReservoirSteelMassKg = 800f;
            LiquidReservoirPlasticMassKg = 100f;
            LiquidReservoirPowerConsumptionWatts = 40f;
            LiquidReservoirCapacityMultiplier = 4;
            GasReservoirSteelMassKg = 800f;
            GasReservoirPlasticMassKg = 100f;
            GasReservoirPowerConsumptionWatts = 40f;
            GasReservoirCapacityMultiplier = 4;
        }

    }
}