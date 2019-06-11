using System;
using UnityEngine;
namespace UnityMMO{
    public static class TimeEx
    {
        private static long ServerTimeMSOnStart = 0;
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
                return (long)Math.Floor(ServerTimeMSOnStart+1000*Time.realtimeSinceStartup+0.5);
            }
            set 
            {
                ServerTimeMSOnStart = (long)Math.Floor(value-Time.realtimeSinceStartup*1000+0.5);
            }
        }

    }
}