using System;
using UnityEngine;

namespace Cocos
{
    //
    // Repeat
    //
    public class Repeat : ActionInterval
    {
        uint times;
        uint total;
        float nextDt;
        bool actionInstant;
        FiniteTimeAction innerAction;
        public static Repeat Create(FiniteTimeAction action, uint times)
        {
            Repeat repeat = new Repeat();
            if (repeat != null && repeat.InitWithAction(action, times))
                return repeat;
            return null;
        }

        public bool InitWithAction(FiniteTimeAction action, uint times)
        {
            if (action != null && base.InitWithDuration(action.Duration * times))
            {
                this.times = times;
                this.innerAction = action;
                actionInstant = action is ActionInstance;
                total = 0;
                return true;
            }
            return false;
        }

        public override Action Clone()
        {
            return Repeat.Create(innerAction.Clone() as FiniteTimeAction, times);
        }

        public override void StartWithTarget(Transform target)
        {
            total = 0;
            nextDt = innerAction.Duration / duration;
            base.StartWithTarget(target);
            innerAction.StartWithTarget(target);
        }

        public override void Stop()
        {
            innerAction.Stop();
            base.Stop();
        }

        public override void Update(float dt)
        {
            if (dt >= nextDt)
            {
                while (dt >= nextDt && total < times)
                {
                    innerAction.Update(1.0f);
                    total++;

                    innerAction.Stop();
                    innerAction.StartWithTarget(target);
                    nextDt = innerAction.Duration/duration * (total+1);
                }

                if (Math.Abs(dt - 1.0f) < float.Epsilon && total < times)
                {
                    innerAction.Update(1.0f);
                    
                    total++;
                }

                if (!actionInstant)
                {
                    if (total == times)
                    {
                        innerAction.Stop();
                    }
                    else
                    {
                        innerAction.Update(dt - (nextDt - innerAction.Duration/duration));
                    }
                }
            }
            else
            {
                var progress = (dt * times) % 1.0f;
                innerAction.Update(progress);
            }
        }

        public override bool IsDone()
        {
            return total == times;
        }

        public override Action Reverse()
        {
            return Repeat.Create(innerAction.Reverse() as FiniteTimeAction, times);
        }
    }

    //
    // RepeatForever
    //
    public class RepeatForever : ActionInterval
    {
        ActionInterval innerAction;
        public static RepeatForever Create(ActionInterval action)
        {
            RepeatForever ret = new RepeatForever();
            if (ret != null && ret.InitWithAction(action))
                return ret;
            return null;
        }

        protected bool InitWithAction(ActionInterval action)
        {
            if (action == null)
            {
                Debug.LogError("RepeatForever::initWithAction error:action is nullptr!");
                return false;
            }
            innerAction = action;
            return true;
        }

        public override Action Clone()
        {
            return RepeatForever.Create(innerAction.Clone() as ActionInterval);
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            innerAction.StartWithTarget(target);
        }

        public override void Step(float dt)
        {
            innerAction.Step(dt);
            if (innerAction.IsDone() && innerAction.Duration > 0)
            {
                float diff = innerAction.Elapsed - innerAction.Duration;
                if (diff > innerAction.Duration)
                    diff = diff % innerAction.Duration;
                innerAction.StartWithTarget(target);
                innerAction.Step(0.0f);
                innerAction.Step(diff);
            }
        }

        public override bool IsDone()
        {
            return false;
        }

        public override Action Reverse()
        {
            return RepeatForever.Create(innerAction.Reverse() as ActionInterval);
        }
    }
}
