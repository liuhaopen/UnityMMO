using System;
using UnityEngine;
namespace UnityMMO{
    public static class TimeEx
    {
        private static double ServerTimeMSOnStart = 0;
        //获取服务器时间，单位秒
        public static long ServerTimeSec{
            get 
            {
                return (long)Math.Floor((ServerTimeMSOnStart+1000*Time.realtimeSinceStartup)/1000+0.5f);
            }
        }

        //获取服务器时间，单位毫秒
        public static long ServerTime{
            get 
            {
                // Debug.Log("ServerTimeMSOnStart get : "+ServerTimeMSOnStart+" start"+Time.realtimeSinceStartup);
                return (long)Math.Floor(ServerTimeMSOnStart+Time.realtimeSinceStartup*1000+0.5);
            }
        }

        public static void UpdateServerTime(double value)
        {
            ServerTimeMSOnStart = Math.Floor(value-Time.realtimeSinceStartup*1000+0.5);
            // Debug.Log("ServerTimeMSOnStart set : "+ServerTimeMSOnStart+" value:"+value+" start:"+Time.realtimeSinceStartup);
        }

    }
}