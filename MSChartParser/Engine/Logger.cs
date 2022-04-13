using System.Diagnostics;

#if UNITY_DEBUGGING_ENABLED
using UnityEngine;
#endif

namespace MoonscraperEngine
{
    public static class Logger
    {
#if !UNITY_DEBUGGING_ENABLED
        [Conditional("UNITY_DEBUGGING_ENABLED")]      // Strips out method completely upon release
#endif
        public static void LogException(System.Exception e, string errorContextMessage)
        {
            string fullMessage = string.Format("Exception Logged-\nContext: {0}\nMessage: {1} \nStack Trace: {2}", errorContextMessage, e.Message, e.StackTrace.ToString());

#if UNITY_DEBUGGING_ENABLED
            Debug.LogError(fullMessage);
#endif
        }

#if !UNITY_DEBUGGING_ENABLED
        [Conditional("UNITY_DEBUGGING_ENABLED")]      // Strips out method completely upon release
#endif
        public static void Log(string message)
        {
#if UNITY_DEBUGGING_ENABLED
            Debug.Log(message);
#endif
        }

        [Conditional("UNITY_DEBUGGING_ENABLED")]      // Strips out method completely upon release
        public static void Assert(bool condition, string message = "")
        {
#if UNITY_DEBUGGING_ENABLED
            Debug.Assert(condition, message);
#endif
        }
    }
}
