using System;
using System.Reflection;

namespace AsLimc.commons {
    public static class VLib {
        private static bool _IS_INIT;
        private static bool _IS_VANILLA;

        /**
         * current working assembly
         */
        private static Assembly _CALLING_ASSEMBLY;

        public static void Init() {
            var callingAssembly = Assembly.GetCallingAssembly();
            if (_IS_INIT)
                return;
            lock (callingAssembly) {
                if (_IS_INIT)
                    return;
                _CALLING_ASSEMBLY = callingAssembly;
                InitInternal();
                _IS_INIT = true;
            }
        }

        private static void InitInternal() {
            VUtils.Log("Initializing {0}", nameof(VLib));

            _IS_VANILLA = string.Equals(VUtils.GetActiveDlcId(), "vanilla_id", StringComparison.OrdinalIgnoreCase);

            VUtils.InitTechAdder(_IS_VANILLA);
        }

        public static void OnDbInitializeEnd(Db db) {
            VUtils.RegisterAllBuildings(_CALLING_ASSEMBLY, db);
            VUtils.BindAllLocStrings(_CALLING_ASSEMBLY);
        }
    }
}