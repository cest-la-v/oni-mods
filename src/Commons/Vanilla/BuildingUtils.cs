using Database;
using Harmony;

namespace AsLimc.Commons {
    public static class BuildingUtils {
        public static void AddTech(Db db, string techId, string itemId) {
            Techs.TECH_GROUPING[techId]?.Add(itemId);
        }
    }
}