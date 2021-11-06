using System.Collections.Generic;
using AsLimc.Commons;
using TUNING;
using UnityEngine;

namespace AsLimc.LaserTurret {
    public class LaserTurretConfig : VBuildingConfig {
        public const string ID = "LaserTurret";
        private readonly int rangeX;
        private readonly int rangeY;
        private readonly int rangeWidth;
        private readonly int rangeHeight;

        public LaserTurretConfig() : base(
            LocStrings.LaserTurret.NAME,
            LocStrings.LaserTurret.DESC,
            LocStrings.LaserTurret.EFFECT,
            ID,
            "laser_turret_kanim",
            1,
            1,
            "Food",
            "AnimalControl",
            new Dictionary<string, float>() {
                {MATERIALS.REFINED_METALS[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]}
            },
            BUILDINGS.HITPOINTS.TIER0,
            BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
            BUILDINGS.MELTING_POINT_KELVIN.TIER1,
            buildLocationRule: BuildLocationRule.OnFoundationRotatable,
            decor: BUILDINGS.DECOR.PENALTY.TIER1,
            noise: NOISE_POLLUTION.NOISY.TIER0,
            overlayTags: OverlayScreen.SolidConveyorIDs
            ) {
            var radius = Settings.Get().Radius;
            rangeX = -radius;
            rangeY = 0;
            rangeWidth = width + radius * 2;
            rangeHeight = height + radius;
        }

        protected override void ConfigureBuildingDef(BuildingDef buildingDef) {
            base.ConfigureBuildingDef(buildingDef);
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = BUILDINGS.ENERGY_CONSUMPTION_WHEN_ACTIVE.TIER3;
            buildingDef.ExhaustKilowattsWhenActive = BUILDINGS.SELF_HEAT_KILOWATTS.TIER0;
            buildingDef.SelfHeatKilowattsWhenActive = BUILDINGS.SELF_HEAT_KILOWATTS.TIER0;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag) {
            go.AddOrGet<Operational>();
            go.AddOrGet<LoopingSounds>();
            go.AddOrGet<MiningSounds>();
            go.AddOrGet<KSelectable>();
            go.AddOrGet<LogicOperationalController>();
            var storage = go.AddOrGet<Storage>();
            storage.allowItemRemoval = false;
            storage.showDescriptor = true;
            var filters = new List<Tag>();
            filters.AddRange(STORAGEFILTERS.BAGABLE_CREATURES);
            filters.AddRange(STORAGEFILTERS.SWIMMING_CREATURES);
            storage.storageFilters = filters;
            storage.allowSettingOnlyFetchMarkedItems = false;
            go.AddOrGet<TreeFilterable>();
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) {
            base.DoPostConfigurePreview(def, go);
            AddVisualizer(go, true);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go) {
            base.DoPostConfigureUnderConstruction(go);
            go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.IncreaseRanchingMedium.Id;
            AddVisualizer(go, false);
        }

        public override void DoPostConfigureComplete(GameObject go) {
            var turret = go.AddOrGet<LaserTurret>();
            turret.rangeX = rangeX;
            turret.rangeY = rangeY;
            turret.rangeWidth = rangeWidth;
            turret.rangeHeight = rangeHeight;
            AddVisualizer(go, false);
        }

        private void AddVisualizer(GameObject go, bool movable) {
            var visualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
            visualizer.x = rangeX;
            visualizer.y = rangeY;
            visualizer.width = rangeWidth;
            visualizer.height = rangeHeight;
            visualizer.movable = movable;
            visualizer.blocking_tile_visible = false;
            go.GetComponent<KPrefabID>().instantiateFn += o => o.GetComponent<StationaryChoreRangeVisualizer>().blocking_cb = LaserTurret.RangeBlockingCallback;
        }
    }
}