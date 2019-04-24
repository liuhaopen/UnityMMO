using System;
using UnityEngine;

namespace Cocos
{
    public class ActionInterval : FiniteTimeAction
    {
        private float elapsed;
        private bool firstTick;
        protected bool done;

        public float Elapsed { get => elapsed; }
        protected bool FirstTick { get => firstTick; }

        protected bool InitWithDuration(float d)
        {
            duration = d;
            elapsed = 0;
            firstTick = true;
            done = false;
            return true;
        }

        public override void Step(float dt)
        {
            if (firstTick)
            {
                firstTick = false;
                elapsed = float.Epsilon;
            }
            else
            {
                elapsed += dt;
            }
            float updateDt = Math.Max(0.0f, Math.Min(1.0f, elapsed/duration));
            Update(updateDt);
            done = elapsed >= duration;
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            elapsed = 0.0f;
            firstTick = true;
            done = false;
        }

        public override bool IsDone()
        {
            return done;
        }

        // public override Action Clone()
        // {
        //     Debug.Log("ActionInterval Clone");
        //     return null;
        // }
    }
    
    public class ExtraAction : FiniteTimeAction
    {
        public static ExtraAction Create()
        {
            ExtraAction ret = new ExtraAction();
            return ret;
        }

        public override Action Clone()
        {
            return ExtraAction.Create();
        }

        public override void Update(float progress)
        {
        }

        public override void Step(float dt)
        {
        }

        public override Action Reverse()
        {
            return ExtraAction.Create();
        }
    }
  
    //
    // DelayTime
    //
    public class DelayTime : ActionInterval
    {
        public static DelayTime Create(float d)
        {
            DelayTime action = new DelayTime();
            if (action != null && action.InitWithDuration(d))
                return action;
            return null;
        }

        public override Action Clone()
        {
            return DelayTime.Create(duration);
        }

        public override void Update(float progress)
        {

        }

        public override Action Reverse()
        {
            return DelayTime.Create(duration);
        }
    }

}