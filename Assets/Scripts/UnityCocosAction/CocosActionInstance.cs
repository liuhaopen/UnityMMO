using UnityEngine;

namespace Cocos
{
    using CallFuncType = System.Action;
    using CallFuncWithTargetType = System.Action<Transform>;
    public class ActionInstance : FiniteTimeAction
    {
        private bool done;
        public override bool IsDone()
        {
            return done;
        }

        public override void StartWithTarget(UnityEngine.Transform target)
        {
            base.StartWithTarget(target);
            done = false;
        }

        public override void Step(float dt)
        {
            Update(1);
        }

        public override void Update(float progress)
        {
            done = true;
        }
    }
    
    //
    // CallFunc
    //
    public class CallFunc : ActionInstance
    {
        System.Action callFunc;
        public static CallFunc Create(System.Action callFunc)
        {
            CallFunc ret = new CallFunc();
            if (ret != null && ret.InitWithFunction(callFunc))
                return ret;
            return null;
        }

        public bool InitWithFunction(System.Action callFunc)
        {
            this.callFunc = callFunc;
            return true;
        }

        public virtual void Execute()
        {
            callFunc();
        }

        public override void Update(float progress)
        {
            base.Update(progress);
            Execute();
        }

        public override Action Reverse()
        {
            return CallFunc.Create(callFunc);
        }

        public override Action Clone()
        {
            return CallFunc.Create(callFunc);
        }
    }

    //
    // CallFunc
    //
    public class CallFuncWithTarget : ActionInstance
    {
        CallFuncWithTargetType callFunc;
        public static CallFuncWithTarget Create(CallFuncWithTargetType callFunc)
        {
            CallFuncWithTarget ret = new CallFuncWithTarget();
            if (ret != null && ret.InitWithFunction(callFunc))
                return ret;
            return null;
        }

        public bool InitWithFunction(CallFuncWithTargetType callFunc)
        {
            this.callFunc = callFunc;
            return true;
        }

        public virtual void Execute()
        {
            callFunc(target);
        }

        public override void Update(float progress)
        {
            base.Update(progress);
            Execute();
        }

        public override Action Reverse()
        {
            return CallFuncWithTarget.Create(callFunc);
        }

        public override Action Clone()
        {
            return CallFuncWithTarget.Create(callFunc);
        }
    }

    //
    // Remove Self
    //
    public class RemoveSelf : ActionInstance
    {
        float delayTime = 0.0f;
        bool isImmediate = false;
        bool allowDestroyingAssets=false;
        public static RemoveSelf Create(float delayTime = 0.0f, bool isImmediate = false, bool allowDestroyingAssets=false)
        {
            RemoveSelf ret = new RemoveSelf();
            if (ret != null && ret.Init(delayTime, isImmediate, allowDestroyingAssets))
                return ret;
            return null;
        }

        public bool Init(float delayTime = 0.0f, bool isImmediate = false, bool allowDestroyingAssets=false)
        {
            this.delayTime = delayTime;
            this.isImmediate = isImmediate;
            this.allowDestroyingAssets = allowDestroyingAssets;
            return true;
        }

        public override void Update(float progress)
        {
            base.Update(progress);
            if (isImmediate)
                UnityEngine.GameObject.DestroyImmediate(target.gameObject, allowDestroyingAssets);
            else
                UnityEngine.GameObject.Destroy(target.gameObject, delayTime);
        }

        public override Action Reverse()
        {
            return RemoveSelf.Create(delayTime, isImmediate, allowDestroyingAssets);
        }

        public override Action Clone()
        {
            return RemoveSelf.Create(delayTime, isImmediate, allowDestroyingAssets);
        }
    }

}