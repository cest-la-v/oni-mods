using System.Collections.Generic;
using System.Reflection;

namespace AsLimc.Commons {
    public static class VLib {
        private static volatile bool INITIALIZED;
        private static readonly object INITIALIZE_LOCK = new object();
        private static readonly HashSet<Assembly> ASSEMBLIES = new HashSet<Assembly>();

        public static void Register() {
            ASSEMBLIES.Add(Assembly.GetCallingAssembly());
        }

        public static void Init(Db db) {
            if (INITIALIZED)
                return;
            lock (INITIALIZE_LOCK) {
                if (INITIALIZED)
                    return;
                INITIALIZED = true;
                foreach (var assembly in ASSEMBLIES) {
                    VLogger.Log($"Initializing for {assembly}");
                    VUtils.RegisterAllBuildings(assembly, db);
                    VUtils.BindAllLocStrings(assembly);
                }
            }
        }
    }
}