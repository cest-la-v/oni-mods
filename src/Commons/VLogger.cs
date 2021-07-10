using System.Reflection;

namespace AsLimc.Commons {
    public static class VLogger {
        public static void Log(object obj, params object[] args) {
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
    }
}