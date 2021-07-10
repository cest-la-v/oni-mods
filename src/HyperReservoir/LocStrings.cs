using STRINGS;

// ReSharper disable UnusedType.Global
#pragma warning disable 414

namespace AsLimc.HyperReservoir {
    internal static class LocStrings {
        public static class Settings {
            public static LocString MOD_NAME = "Hyper Reservoir";

            public static class LiquidReservoirSteelMassKg {
                public static LocString NAME = "Liquid Reservoir steel mass (kg)";
                public static LocString TOOLTIP = "The steel mass of building a Liquid Reservoir";
            }

            public static class LiquidReservoirPlasticMassKg {
                public static LocString NAME = "Liquid Reservoir plastic mass (kg)";
                public static LocString TOOLTIP = "The plastic mass of building a Liquid Reservoir";
            }

            public static class LiquidReservoirPowerConsumptionWatts {
                public static LocString NAME = "Liquid Reservoir power consumption (watts)";
                public static LocString TOOLTIP = "The power consumption of Liquid Reservoir";
            }

            public static class LiquidReservoirCapacityMultiplier {
                public static LocString NAME = "Liquid Reservoir capacity multiplier";
                public static LocString TOOLTIP = "The capacity multiplier of Liquid Reservoir";
            }

            public static class GasReservoirSteelMassKg {
                public static LocString NAME = "Gas Reservoir steel mass (kg)";
                public static LocString TOOLTIP = "The steel mass of building a Gas Reservoir";
            }

            public static class GasReservoirPlasticMassKg {
                public static LocString NAME = "Gas Reservoir plastic mass (kg)";
                public static LocString TOOLTIP = "The plastic mass of building a Gas Reservoir";
            }

            public static class GasReservoirPowerConsumptionWatts {
                public static LocString NAME = "Gas Reservoir power consumption (watts)";
                public static LocString TOOLTIP = "The power consumption of Gas Reservoir";
            }

            public static class GasReservoirCapacityMultiplier {
                public static LocString NAME = "Gas Reservoir capacity multiplier";
                public static LocString TOOLTIP = "The capacity multiplier of Gas Reservoir";
            }
        }

        public static class HyperLiquidReservoir {
            public static LocString NAME = UI.FormatAsLink("Hyper Liquid Reservoir", HyperLiquidReservoirConfig.ID);

            public static LocString DESC =
                $"Hyper Liquid Reservoir's input needs {UI.FormatAsLink("Power", "POWER")} to contain more resource and its' output can be controlled by <link=\"LOGIC\">Automation</link>." +
                " It cannot receive manually delivered resources.";

            public static LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID} resources piped into it.";
        }

        public static class HyperGasReservoir {
            public static LocString NAME = UI.FormatAsLink("Hyper Gas Reservoir", HyperGasReservoirConfig.ID);

            public static LocString DESC =
                $"Hyper Gas Reservoir's input needs {UI.FormatAsLink("Power", "POWER")} to contain more resource and its' output can be controlled by <link=\"LOGIC\">Automation</link>." +
                " It cannot receive manually delivered resources.";

            public static LocString EFFECT = $"Stores any {UI.CODEX.CATEGORYNAMES.ELEMENTSGAS} resources piped into it.";
        }
    }
}