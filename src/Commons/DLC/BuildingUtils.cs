namespace AsLimc.Commons {
    public class BuildingUtils {
        public static void AddTech(Db db, string techId, string itemId) {
            db.Techs.Get(techId)?.unlockedItemIDs.Add(itemId);
        }
    }
}