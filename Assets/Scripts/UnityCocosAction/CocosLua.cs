using System;
using UnityEngine;

namespace Cocos
{
    public class LuaFunc : ActionInterval
    {
        Action<Transform> startWithFunc;
        Action<float> updateFunc;

        public static LuaFunc Create(Action<Transform> startWithFunc, Action<float> updateFunc)
        {
            LuaFunc ret = new LuaFunc();
            if (ret != null && ret.Init(startWithFunc, updateFunc))
                return ret;
            return null;
        }

        public virtual bool Init(Action<Transform> startWithFunc, Action<float> updateFunc)
        {
            return true;
        }

        public virtual void StartWithTarget(Transform target)
        {
            this.target = target;
            this.originalTarget = target;
        }

        public virtual void Update(float progress)
        {
        }

        public virtual void Stop()
        {
        }

        public virtual bool IsDone()
        {
            return true;
        }

        public virtual Action Clone()
        {
            return null;
        }

        public virtual Action Reverse()
        {
            return null;
        }

    }
}