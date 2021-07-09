using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Database;
using Harmony;

namespace AsLimc.Commons {
    public static class VUtils {
        private static FieldInfo loggingDisabledField => AccessTools.Field(typeof(Debug), "s_loggingDisabled");

        /**
         * vanilla AddTech
         */
        private static Dictionary<string, string[]> _TECH_GROUPING;

        /**
         * dlc AddTech
         */
        private static FieldInfo _UNLOCKED_ITEM_IDS_FIELD;

        /**
         * Tech adder
         */
        private static Action<Db, string, string> _TECH_ADDER;

        public static void Log(object obj, params object[] args) {
            if (loggingDisabledField.GetValue(null) is bool loggingDisabled && loggingDisabled)
                return;
            var assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            if (args == null || args.Length == 0) {
                Debug.LogFormat("[{0}] {1}", assemblyName, obj);
            }
            else if (obj is string format) {
                Debug.LogFormat("[{0}] {1}", assemblyName, string.Format(format, args));
            }
            else {
                Debug.LogFormat("[{0}] {1} {2}", assemblyName, obj, args);
            }
        }

        public static void TrySetField(Type type, string fieldName, object instance, object value) {
            var field = AccessTools.Field(type, fieldName);
            if (field == null)
                return;
            try {
                field.SetValue(instance, value);
            }
            catch (Exception e) {
                DebugUtil.LogException(null, $"Exception while trying to set field '{fieldName}' of type '{type}'", e);
            }
        }

        public static bool IsVanilla() {
            // var activeDlcId = "vanilla_id";
            // var activeDlcId = DlcManager.GetActiveDlcId().ToLower();
            // if (activeDlcId == "")
            // {
            //     activeDlcId = "vanilla_id";
            // }
            // string.Equals(VUtils.IsVanilla(), "vanilla_id", StringComparison.OrdinalIgnoreCase)
            var method = AccessTools.Method(AccessTools.TypeByName("DlcManager"), "IsPureVanilla");
            return method?.Invoke(null, null) is bool;
        }

        public static void InitTechAdder(bool isVanilla) {
            if (isVanilla) {
                _TECH_GROUPING = AccessTools.Field(typeof(Techs), "TECH_GROUPING")?.GetValue(null) is Dictionary<string, string[]> map ? map : null;
                _TECH_ADDER = (db, techId, itemId) => {
                    // support for vanilla
                    // Techs.TECH_GROUPING[id] = Techs.TECH_GROUPING[id].Append(itemId)
                    // if (_TECH_GROUPING[techId] != null) {
                    //     _TECH_GROUPING[techId] = _TECH_GROUPING[techId].Append(itemId);
                    // }
                    // _TECH_GROUPING[techId]?.Add(itemId);
                };
            }
            else {
                _UNLOCKED_ITEM_IDS_FIELD = AccessTools.Field(typeof(Tech), "unlockedItemIDs");
                _TECH_ADDER = (db, techId, itemId) => {
                    // support for dlc
                    // db.Techs.Get(id)?.unlockedItemIDs?.AddRange(itemIDs);
                    if (_UNLOCKED_ITEM_IDS_FIELD == null)
                        return;
                    var tech = db.Techs?.Get(techId);
                    if (tech == null)
                        return;
                    if (_UNLOCKED_ITEM_IDS_FIELD.GetValue(tech) is List<string> unlockedItemIDs) {
                        unlockedItemIDs.Add(itemId);
                    }
                };
            }
        }

        public static Dictionary<string, LocString> ListLocStrings(Assembly assembly = null) {
            if (assembly == null) {
                assembly = Assembly.GetCallingAssembly();
            }

            var locStrings = new Dictionary<string, LocString>();

            var types = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsNested)
                .ToList();
            for (int i = 0, len = types.Count; i < len; i++) {
                types.AddRange(types[i].GetNestedTypes());
                len = types.Count;
            }

            foreach (var type in types) {
                var prefix = type.FullName == null ? "" : $"{type.FullName.Replace("+", ".")}.";
                foreach (var field in type.GetFields()) {
                    if (field.FieldType != typeof(LocString))
                        continue;
                    var locString = (LocString) field.GetValue(null);
                    var key = $"{prefix}{field.Name}";
                    locStrings.Add(key, locString);
                }
            }

            return locStrings;
        }

        public static IEnumerable<VBuildingConfig> ListBuildingConfigs(Assembly assembly = null) {
            if (assembly == null) {
                assembly = Assembly.GetCallingAssembly();
            }

            var buildingConfigs = new List<VBuildingConfig>();
            foreach (var type in assembly.GetTypes()) {
                if (!typeof(VBuildingConfig).IsAssignableFrom(type) || type.IsAbstract || type.IsInterface)
                    continue;
                var instance = Activator.CreateInstance(type);
                if (instance is VBuildingConfig buildingConfig) {
                    buildingConfigs.Add(buildingConfig);
                }
            }

            return buildingConfigs;
        }

        public static void RegisterAllBuildings(Assembly assembly, Db db) {
            var buildingConfigs = ListBuildingConfigs(assembly);
            foreach (var buildingConfig in buildingConfigs) {
                try {
                    Log($"Registering building {buildingConfig.id} in Plan '{buildingConfig.planName}'");
                    ModUtil.AddBuildingToPlanScreen(buildingConfig.planName, buildingConfig.id);

                    Log($"Registering building {buildingConfig.id} in Tech '{buildingConfig.techId}'");
                    _TECH_ADDER(db, buildingConfig.techId, buildingConfig.id);

                    Log($"Registering building {buildingConfig.id} in Strings");
                    var id = buildingConfig.id;
                    var name = buildingConfig.name;
                    var desc = buildingConfig.desc;
                    var effect = buildingConfig.effect;
                    var prefix = $"STRINGS.BUILDINGS.PREFABS.{id.ToUpperInvariant()}.";
                    if (name != null)
                        Strings.Add($"{prefix}NAME", name);
                    if (desc != null)
                        Strings.Add($"{prefix}DESC", desc);
                    if (effect != null)
                        Strings.Add($"{prefix}EFFECT", effect);
                }
                catch (Exception ex) {
                    DebugUtil.LogException(null, $"Exception while registering building {buildingConfig.id}", ex);
                }
            }
        }

        public static void BindAllLocStrings(Assembly assembly) {
            foreach (var pair in ListLocStrings(assembly)) {
                var key = pair.Key;
                var locString = pair.Value;
                if (Strings.TryGet(locString.key, out var stringEntry)) {
                    stringEntry.String = locString.text;
                }
                else {
                    Strings.Add(key, locString.text);
                }
            }
        }
    }
}