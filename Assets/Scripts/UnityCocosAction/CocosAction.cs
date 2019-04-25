using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cocos
{
    public abstract class Action
    {
        protected Transform originalTarget = null;
        protected Transform target = null;

        public Transform Target { get => target; set => target = value; }

        protected Action()
        {
        }
        public virtual void StartWithTarget(Transform target)
        {
            this.target = target;
            this.originalTarget = target;
        }

        public virtual void Update(float progress)
        {
        }

        public virtual void Step(float deltaTime)
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
            Debug.Log("Action Clone");
            return null;
        }

        public virtual Action Reverse()
        {
            return null;
        }

    }

    public class FiniteTimeAction : Action
    {
        protected float duration;
        public float Duration { get => duration; set => duration = value; }

        protected FiniteTimeAction()
        {
            Duration = 0;
        }

        // public override Action Clone()
        // {
        //     Debug.Log("FiniteTimeAction Clone");
        //     return null;
        // }
    }

}