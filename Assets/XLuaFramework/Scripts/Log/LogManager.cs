using UnityEngine;
using System;
namespace XLuaFramework
{
    public class LogManager
    {
        private static Action<string, LogType> log_callback;
        private static bool _EnableLog = false;

        public static bool EnableLog
        {
            get
            {
                return _EnableLog;
            }
            set
            {
                _EnableLog = value;
                Debug.unityLogger.logEnabled = _EnableLog;
            }
        }


        public static void RegisterLogCallback(Action<string, LogType> func)
        {
            log_callback = func;
        }

        public static void Log(string message)
        {
            if (EnableLog)
            {
                if (log_callback != null)
                {
                    log_callback(message, LogType.Log);
                }
            }
        }
        public static void LogError(string message)
        {
            if (EnableLog)
            {
                if (log_callback != null)
                {
                    log_callback(message, LogType.Error);
                }
            }
        }
        public static void LogWarning(string message)
        {
            if (EnableLog)
            {
                if (log_callback != null)
                {
                    log_callback(message, LogType.Warning);
                }
            }
        }
    }
}
