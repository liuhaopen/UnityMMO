using System.Collections.Generic;
using Unity.Entities;

namespace UnityMMO
{
    namespace Component
    {
        public struct LocomotionState : IComponentData
        {
            public enum State
            {
                Idle,
                Run,
                Sprint,
                Jump,
                DoubleJump,
                TrebleJump,
                InAir,
                BeHit,
                Dead,
                Dizzy,
                StateNum,
            }
            public enum EndType {
                None,
                PlayAnimationOnce,//播放完动作就结束当前状态
                EndTime,//依据状态结束时间
            }
            private State locoState;
            public State LocoState
            {
                get => locoState;
                set
                {
                    locoState = value;
                    StateEndType = EndType.None;
                    StartTime = TimeEx.ServerTime;
                    EndTime = 0;
                    // Debug.Log("set locostate : "+value+" track:"+new System.Diagnostics.StackTrace().ToString());
                }
            }
            public LocomotionState(State state, long startTime=0, EndType endType=EndType.None, long endTime=0)
            {
                locoState = state;
                StateEndType = endType;
                StartTime = startTime;
                EndTime = endTime;
            }
            public EndType StateEndType;

            public bool IsOnGround()
            {
                return LocoState == LocomotionState.State.Idle || LocoState == LocomotionState.State.Run || LocoState == LocomotionState.State.Sprint || LocoState == LocomotionState.State.BeHit || LocoState == LocomotionState.State.Dizzy;
            }
            public bool IsInJump()
            {
                return LocoState == LocomotionState.State.Jump || LocoState == LocomotionState.State.DoubleJump || LocoState == LocomotionState.State.TrebleJump || LocoState == LocomotionState.State.InAir;
            }
            public static bool IsStateNeedStack(State state)
            {
                return state == State.Idle || state == State.Dizzy;
            }
            public long StartTime;
            public long EndTime;
        }
    }
}