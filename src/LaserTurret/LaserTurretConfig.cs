using System.Collections.Generic;
using System.Linq;
using AsLimc.Commons;
using TUNING;
using UnityEngine;

namespace AsLimc.LaserTurret {
    public class LaserTurretConfig : VBuildingConfig {
        public const string ID = "LaserTurret";
        private readonly int radius;
        private readonly int _rangeMinX;
        private readonly int _rangeMinY;
        private readonly int _rangeWidth;
        private readonly int _rangeHeight;

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
            radius = Settings.Get().Radius;
            _rangeMinX = -radius;
            _rangeMinY = 0;
            _rangeWidth = width + radius * 2;
            _rangeHeight = height + radius;
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
            var operational = go.AddOrGet<Operational>();
            operational.SetActive(false);
            go.AddOrGet<LoopingSounds>();
            go.AddOrGet<MiningSounds>();
            go.AddOrGet<KSelectable>();
            go.AddOrGet<LogicOperationalController>();
            var storage = go.AddOrGet<Storage>();
            storage.allowItemRemoval = false;
            storage.showDescriptor = true;
            var filters = new HashSet<Tag>();
            filters.UnionWith(STORAGEFILTERS.BAGABLE_CREATURES);
            filters.UnionWith(STORAGEFILTERS.SWIMMING_CREATURES);
            storage.storageFilters = filters.ToList();
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
            turret.rangeX = _rangeMinX;
            turret.rangeY = _rangeMinY;
            turret.rangeWidth = _rangeWidth;
            turret.rangeHeight = _rangeHeight;
            AddVisualizer(go, false);
        }

        private void AddVisualizer(GameObject go, bool movable) {
            var visualizer = go.AddOrGet<RangeVisualizer>();
            visualizer.RangeMin.x = -radius;
            visualizer.RangeMin.y = 0;
            visualizer.RangeMax.x = radius;
            visualizer.RangeMax.y = radius;
            // visualizer.OriginOffset = new Vector2I(0, 1);
            visualizer.BlockingTileVisible = false;
            go.GetComponent<KPrefabID>().instantiateFn += o => o.GetComponent<RangeVisualizer>().BlockingCb = LaserTurret.RangeBlockingCallback;
        }
    }
}