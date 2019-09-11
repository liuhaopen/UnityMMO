using System.Collections.Generic;
using UnityEngine;

namespace UnityMMO
{
namespace Component
{
    //因为速度会有多方同时改变到，所以获取当前速度时应该要考虑各方的设置，比如 A 状态下设置速度为0，状态结束后设回原来速度，但如果 A 状态期间又发生 B 状况需要把速度改成其它值，这时是不让改的，所以必须记录各种状态的速度设置。
    public class SpeedData : MonoBehaviour
    {
        private float curSpeed;//实际使用的，变动的
        private float baseSpeed;//基础速度，所有速度加成都以此值为因子
        private Dictionary<string, float> bod;

        public float CurSpeed { get => curSpeed; }
        public float BaseSpeed { get => baseSpeed; }

        public SpeedData()
        {
            curSpeed = 0;
            baseSpeed = 0;
            bod = new Dictionary<string, float>();
        }

        public void InitSpeed(float baseSpeed)
        {
            this.curSpeed = baseSpeed;
            this.baseSpeed = baseSpeed;
        }

        /*
        bodName : 每次更改速度都要指定是哪个董事会成员的意见, 
        isSet: true 时即发表意见，false 时为撤消意见
        offset：变更万分比，0时即不动
        */
        public void ChangeSpeed(string bodName, bool isSet, float offset)
        {
            if (isSet)
                bod[bodName] = offset;
            else
                bod.Remove(bodName);
            UpdateSpeed();
        }

        public void UpdateSpeed()
        {
            var hasZero = HasZero();
            if (hasZero)
            {
                curSpeed = 0;
            }
            else
            {
                float factor = 0;
                foreach (var offset in bod)
                {
                    factor += offset.Value;
                }
                curSpeed = baseSpeed + baseSpeed * factor;
            }
        }

        public bool HasZero()
        {
            var hasZero = false;
            foreach (var offset in bod)
            {
                if (offset.Value >= -float.Epsilon && offset.Value <= float.Epsilon)
                {
                    hasZero = true;
                    break;
                }
            }
            return hasZero;
        }

    }
}
}