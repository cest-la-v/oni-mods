using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace AsLimc.commons {
    public abstract class VBuildingConfig : IBuildingConfig {
        public LocString name { get; }
        public LocString desc { get; }
        public LocString effect { get; }
        public readonly string id;
        private readonly string anim;
        protected readonly int width;
        protected readonly int height;

        /**
         * <see cref="TUNING.BUILDINGS.PLANORDER">TUNING.BUILDINGS.PLANORDER</see> 
         */
        public readonly string planName;

        /**
         * <see cref="Database.Techs.Init">Database.Techs.Init</see>
         */
        public readonly string techId;

        private readonly Dictionary<string, float> constructionRecipe;
        private string[] constructionMaterials => constructionRecipe.Keys.ToArray();
        private float[] constructionMass => constructionRecipe.Values.ToArray();
        private readonly int hitpoints;
        private readonly float constructionTime;
        private readonly float meltingPoint;
        private readonly float temperatureModificationMassScale;
        private readonly BuildLocationRule buildLocationRule;

        /**
         * <see cref="BUILDINGS.DECOR">BUILDINGS.DECOR</see>
         */
        private readonly EffectorValues decor;

        /**
         * <see cref="NOISE_POLLUTION">NOISE_POLLUTION</see>
         */
        private readonly EffectorValues noise;

        /**
         * <see cref="OverlayScreen">OverlayScreen</see>
         */
        private readonly HashSet<Tag> overlayTags;

        /**
         * <see cref="OverlayModes">OverlayModes</see>
         */
        private readonly HashedString viewMode;

        protected VBuildingConfig(
            LocString name,
            LocString desc,
            LocString effect,
            string id,
            string anim,
            int width,
            int height,
            string planName,
            string techId,
            Dictionary<string, float> constructionRecipe,
            int hitpoints = BUILDINGS.HITPOINTS.TIER0,
            float constructionTime = BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
            float meltingPoint = BUILDINGS.MELTING_POINT_KELVIN.TIER0,
            float temperatureModificationMassScale = BUILDINGS.MASS_TEMPERATURE_SCALE,
            BuildLocationRule buildLocationRule = BuildLocationRule.Anywhere,
            EffectorValues decor = default,
            EffectorValues noise = default,
            HashSet<Tag> overlayTags = null,
            HashedString viewMode = default
        ) {
            this.name = name;
            this.desc = desc;
            this.effect = effect;
            this.id = id;
            this.planName = planName;
            this.techId = techId;
            this.anim = anim;
            this.width = width;
            this.height = height;
            this.constructionRecipe = constructionRecipe;
            this.hitpoints = hitpoints;
            this.constructionTime = constructionTime;
            this.meltingPoint = meltingPoint;
            this.temperatureModificationMassScale = temperatureModificationMassScale;
            this.buildLocationRule = buildLocationRule;
            this.decor = decor;
            this.noise = noise;
            this.overlayTags = overlayTags;
            this.viewMode = viewMode;
        }

        /**
         * minimal init
         */
        public sealed override BuildingDef CreateBuildingDef() {
            var buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim,
                hitpoints, constructionTime, constructionMass, constructionMaterials, meltingPoint, buildLocationRule, decor, noise, temperatureModificationMassScale);

            if (overlayTags != null) {
                GeneratedBuildings.RegisterWithOverlay(overlayTags, id);
            }

            if (viewMode != null) {
                buildingDef.ViewMode = viewMode;
            }

            ConfigureBuildingDef(buildingDef);
            return buildingDef;
        }

        protected virtual void ConfigureBuildingDef(BuildingDef buildingDef) {
        }
    }
}