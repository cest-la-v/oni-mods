using System;
using HarmonyLib;

namespace AsLimc.Commons {
    public class ReflectionUtils {

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
    }
}