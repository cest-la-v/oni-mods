using System.Collections.Generic;
using AsLimc.Commons;
using TUNING;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR;

namespace AsLimc.HyperReservoir {
    public abstract class HyperReservoirConfig : VBuildingConfig {
        private readonly List<Tag> storageFilters;
        private readonly float capacityKg;
        private readonly ConduitType conduitType;
        private readonly float energyConsumptionWhenActive;

        protected HyperReservoirConfig(LocString name, LocString desc, LocString effect,
            string id, string anim, int width, int height,
            Dictionary<string, float> constructionRecipe, HashSet<Tag> overlayTags, HashedString viewMode,
            List<Tag> storageFilters, float capacityKg, ConduitType conduitType, float energyConsumptionWhenActive)
            : base(name, desc, effect,
                id, anim, width, height, "Base", "ValveMiniaturization",
                constructionRecipe, BUILDINGS.HITPOINTS.TIER3, BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER5,
                buildLocationRule: BuildLocationRule.OnFloor,
                decor: BUILDINGS.DECOR.PENALTY.TIER1,
                noise: NOISE_POLLUTION.NOISY.TIER0,
                overlayTags: overlayTags,
                viewMode: viewMode) {
            this.storageFilters = storageFilters;
            this.capacityKg = capacityKg;
            this.conduitType = conduitType;
            this.energyConsumptionWhenActive = energyConsumptionWhenActive;
        }

        protected override void ConfigureBuildingDef(BuildingDef buildingDef) {
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "HollowMetal";

            // 资源
            buildingDef.InputConduitType = conduitType;
            buildingDef.OutputConduitType = conduitType;
            buildingDef.UtilityInputOffset = new CellOffset(1, 2);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);

            // 电力
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = energyConsumptionWhenActive;
            buildingDef.ExhaustKilowattsWhenActive = BUILDINGS.SELF_HEAT_KILOWATTS.TIER0;
            buildingDef.SelfHeatKilowattsWhenActive = BUILDINGS.SELF_HEAT_KILOWATTS.TIER4;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);

            // 信号
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port> {
                LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 0), LOGIC_PORT, LOGIC_PORT_ACTIVE, LOGIC_PORT_INACTIVE)
            };
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag) {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGet<SmartReservoir>();
            go.AddOrGet<HyperReservoir>();
            go.AddOrGet<EnergyConsumerEx>();

            var storage = BuildingTemplates.CreateDefaultStorage(go);
            storage.showDescriptor = true;
            storage.allowItemRemoval = false;
            storage.storageFilters = storageFilters;
            storage.capacityKg = capacityKg;
            storage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
            // storage.showCapacityStatusItem = true;
            VUtils.TrySetField(typeof(Storage), "showCapacityStatusItem", storage, true);
            // storage.showCapacityAsMainStatus = true;
            VUtils.TrySetField(typeof(Storage), "showCapacityAsMainStatus", storage, true);

            var conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = conduitType;
            conduitConsumer.ignoreMinMassCheck = true;
            conduitConsumer.forceAlwaysSatisfied = true;
            conduitConsumer.alwaysConsume = false;
            conduitConsumer.capacityKG = storage.capacityKg;

            var conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.conduitType = conduitType;
            conduitDispenser.alwaysDispense = true;
        }

        public override void DoPostConfigureComplete(GameObject go) {
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
            go.AddOrGetDef<StorageController.Def>();
        }
    }

    public class HyperLiquidReservoirConfig : HyperReservoirConfig {
        public const string ID = "HyperbaricLiquidReservoir";

        public HyperLiquidReservoirConfig()
            : base(LocStrings.HyperLiquidReservoir.NAME, LocStrings.HyperLiquidReservoir.DESC, LocStrings.HyperLiquidReservoir.EFFECT,
                ID, "liquidreservoir_kanim", 2, 3,
                new Dictionary<string, float> {
                    {SimHashes.Steel.ToString(), Settings.Get().LiquidReservoirSteelMassKg},
                    {MATERIALS.PLASTICS[0], Settings.Get().LiquidReservoirPlasticMassKg}
                },
                OverlayScreen.LiquidVentIDs,
                OverlayModes.LiquidConduits.ID,
                STORAGEFILTERS.LIQUIDS,
                5000f * Settings.Get().LiquidReservoirCapacityMultiplier,
                ConduitType.Liquid,
                Settings.Get().LiquidReservoirPowerConsumptionWatts) {
        }
    }

    public class HyperGasReservoirConfig : HyperReservoirConfig {
        public const string ID = "HyperbaricGasReservoir";

        public HyperGasReservoirConfig()
            : base(LocStrings.HyperGasReservoir.NAME, LocStrings.HyperGasReservoir.DESC, LocStrings.HyperGasReservoir.EFFECT,
                ID, "gasstorage_kanim", 5, 3,
                new Dictionary<string, float> {
                    {SimHashes.Steel.ToString(), Settings.Get().GasReservoirSteelMassKg},
                    {MATERIALS.PLASTICS[0], Settings.Get().GasReservoirPlasticMassKg}
                },
                OverlayScreen.GasVentIDs,
                OverlayModes.GasConduits.ID,
                STORAGEFILTERS.GASES,
                150f * Settings.Get().GasReservoirCapacityMultiplier,
                ConduitType.Gas,
                Settings.Get().GasReservoirPowerConsumptionWatts) {
        }
    }
}