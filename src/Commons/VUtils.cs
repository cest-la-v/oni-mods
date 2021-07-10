using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AsLimc.Commons {
    public static class VUtils {
        private static Dictionary<string, LocString> ListLocStrings(Assembly assembly = null) {
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

        private static IEnumerable<VBuildingConfig> ListBuildingConfigs(Assembly assembly = null) {
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
                    VLogger.Log($"Registering building {buildingConfig.id} in Plan '{buildingConfig.planName}'");
                    ModUtil.AddBuildingToPlanScreen(buildingConfig.planName, buildingConfig.id);

                    VLogger.Log($"Registering building {buildingConfig.id} in Tech '{buildingConfig.techId}'");
                    BuildingUtils.AddTech(db, buildingConfig.techId, buildingConfig.id);

                    VLogger.Log($"Registering building {buildingConfig.id} in Strings");
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