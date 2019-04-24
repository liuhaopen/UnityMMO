using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cocos
{
    //
    // Sequence
    //
    public class Sequence : ActionInterval
    {
        FiniteTimeAction[] actions = new FiniteTimeAction[2];
        float split;
        int last;

        public static Sequence Create(FiniteTimeAction action1, params FiniteTimeAction[] args)
        {
            FiniteTimeAction now;
            FiniteTimeAction prev = action1;
            bool bOneAction = true;
            for (int i = 0; i < args.Length; i++)
            {
                now = args[i];
                prev = CreateWithTwoActions(prev, now);
                bOneAction = false;
            }
            if (bOneAction)
                prev = CreateWithTwoActions(prev, ExtraAction.Create());
            return prev as Sequence;
        }

        public static Sequence CreateWithTwoActions(FiniteTimeAction actionOne, FiniteTimeAction actionTwo)
        {
            Sequence sequence = new Sequence();
            if (sequence != null && sequence.InitWithTwoActions(actionOne, actionTwo))
                return sequence;
            return null;
        }

        public static Sequence CreateWithList(List<FiniteTimeAction> arrayOfActions)
        {
            Sequence seq = new Sequence();
            if (seq != null && seq.Init(arrayOfActions))
                return seq;
            return null;
        }

        public bool Init(List<FiniteTimeAction> arrayOfActions)
        {
            var count = arrayOfActions.Count;
            if (count == 0)
                return false;
            if (count == 1)
                return InitWithTwoActions(arrayOfActions[0], ExtraAction.Create());
            
            var prev = arrayOfActions[0];
            for (int i = 1; i < count-1; i++)
            {
                prev = CreateWithTwoActions(prev, arrayOfActions[i]);
            }
            return InitWithTwoActions(prev, arrayOfActions[count-1]);
        }

        public bool InitWithTwoActions(FiniteTimeAction actionOne, FiniteTimeAction actionTwo)
        {
            if (actionOne == null || actionTwo == null)
            {
                Debug.LogError("Sequence::initWithTwoActions error: action is null!!");
                return false;
            }
            float d = actionOne.Duration + actionTwo.Duration;
            base.InitWithDuration(d);
            actions[0] = actionOne;
            actions[1] = actionTwo;
            return true;
        }

        public override bool IsDone()
        {
            if (actions[1] is ActionInstance)
                return (done && actions[1].IsDone());
            else
                return done;
        }

        public override Action Clone()
        {
            if (actions[0]!=null && actions[1]!=null)
                return Sequence.Create(actions[0].Clone() as FiniteTimeAction, actions[1].Clone() as FiniteTimeAction);
            return null;
        } 

        protected Sequence()
        {
            split = 0;
            actions[0] = null;
            actions[1] = null;
        }

        public override void StartWithTarget(Transform target)
        {
            if (target == null)
            {
                Debug.Log("Sequence::startWithTarget error: target is null!");
                return;
            }
            if (actions[0] == null || actions[1] == null)
            {
                Debug.Log("Sequence::startWithTarget error: _actions[0] or _actions[1] is null!");
                return;
            }
            if (duration > float.Epsilon)
                split = actions[0].Duration > float.Epsilon ? actions[0].Duration / duration : 0;
            
            base.StartWithTarget(target);
            last = -1;
        }

        public override void Stop()
        {
            if (last != -1 && actions[last] != null)
                actions[last].Stop();
            base.Stop();
        }

        public override void Update(float t)
        {
            int found = 0;
            float new_t = 0.0f;

            if( t < split )
            {
                // action[0]
                found = 0;
                if( split != 0 )
                    new_t = t / split;
                else
                    new_t = 1;

            }
            else
            {
                // action[1]
                found = 1;
                if ( split == 1 )
                    new_t = 1;
                else
                    new_t = (t-split) / (1 - split );
            }

            if ( found==1 )
            {
                if( last == -1 )
                {
                    // action[0] was skipped, execute it.
                    actions[0].StartWithTarget(target);
                    actions[0].Update(1.0f);
                    actions[0].Stop();
                }
                else if( last == 0 )
                {
                    // switching to action 1. stop action 0.
                    actions[0].Update(1.0f);
                    actions[0].Stop();
                }
            }
            else if(found==0 && last==1 )
            {
                // Reverse mode ?
                // FIXME: Bug. this case doesn't contemplate when last==-1, found=0 and in "reverse mode"
                // since it will require a hack to know if an action is on reverse mode or not.
                // "step" should be overridden, and the "reverseMode" value propagated to inner Sequences.
                actions[1].Update(0);
                actions[1].Stop();
            }
            // Last action found and it is done.
            if( found == last && actions[found].IsDone() )
            {
                return;
            }

            // Last action found and it is done
            if( found != last )
            {
                actions[found].StartWithTarget(target);
            }
            actions[found].Update(new_t);
            last = found;
        }

        public override Action Reverse()
        {
            if (actions[0] != null && actions[1] != null)
                return Sequence.CreateWithTwoActions(actions[1].Reverse() as FiniteTimeAction, actions[0].Reverse() as FiniteTimeAction);
            return null;
        }
    }


    //
    // Spawn
    //
    public class Spawn : ActionInterval
    {
        FiniteTimeAction one;
        FiniteTimeAction two;
        public static Spawn Create(FiniteTimeAction action1, params FiniteTimeAction[] args)
        {
            FiniteTimeAction now;
            FiniteTimeAction prev = action1;
            bool bOneAction = true;
            for (int i = 0; i < args.Length; i++)
            {
                now = args[i];
                prev = CreateWithTwoActions(prev, now);
                bOneAction = false;
            }
            if (bOneAction)
                prev = CreateWithTwoActions(prev, ExtraAction.Create());
            return prev as Spawn;
        }

        public static Spawn CreateWithTwoActions(FiniteTimeAction actionOne, FiniteTimeAction actionTwo)
        {
            Spawn spawn = new Spawn();
            if (spawn != null && spawn.InitWithTwoActions(actionOne, actionTwo))
                return spawn;
            return null;
        }

        public static Spawn CreateWithList(List<FiniteTimeAction> arrayOfActions)
        {
            Spawn seq = new Spawn();
            if (seq != null && seq.Init(arrayOfActions))
                return seq;
            return null;
        }

        public bool Init(List<FiniteTimeAction> arrayOfActions)
        {
            var count = arrayOfActions.Count;
            if (count == 0)
                return false;
            if (count == 1)
                return InitWithTwoActions(arrayOfActions[0], ExtraAction.Create());
            
            var prev = arrayOfActions[0];
            for (int i = 1; i < count-1; i++)
            {
                prev = CreateWithTwoActions(prev, arrayOfActions[i]);
            }
            return InitWithTwoActions(prev, arrayOfActions[count-1]);
        }

        public bool InitWithTwoActions(FiniteTimeAction action1, FiniteTimeAction action2)
        {
            if (action1 == null || action2 == null)
            {
                Debug.LogError("Sequence::initWithTwoActions error: action is null!!");
                return false;
            }
            bool ret = false;
            var d1 = action1.Duration;
            var d2 = action2.Duration;
            if (base.InitWithDuration(Math.Max(d1, d2)))
            {
                one = action1;
                two = action2;
                if (d1 > d2)
                {
                    two = Sequence.CreateWithTwoActions(action2, DelayTime.Create(d1 - d2));
                } 
                else if (d1 < d2)
                {
                    one = Sequence.CreateWithTwoActions(action1, DelayTime.Create(d2 - d1));
                }
                ret = true;
            }
            return ret;
        }

        public override Action Clone()
        {
            if (one != null && two != null)
                return Spawn.CreateWithTwoActions(one.Clone() as FiniteTimeAction, two.Clone() as FiniteTimeAction);
            return null;
        }

        protected Spawn()
        {
            one = null;
            two = null;
        }

        public override void StartWithTarget(Transform target)
        {
            if (target == null)
            {
                Debug.Log("Spawn::startWithTarget error: target is nullptr!");
                return;
            }
            if (one == null || two == null)
            {
                Debug.Log("Spawn::startWithTarget error: _one or _two is nullptr!");
                return;
            }
            base.StartWithTarget(target);
            one.StartWithTarget(target);
            two.StartWithTarget(target);
        }

        public override void Stop()
        {
            if (one != null)
                one.Stop();
            if (two != null)
                two.Stop();
            base.Stop();
        }

        public override void Update(float time)
        {
            if (one != null)
                one.Update(time);
            if (two != null)
                two.Update(time);
        }

        public override Action Reverse()
        {
            if (one != null && two != null)
                return Spawn.CreateWithTwoActions(one.Reverse() as FiniteTimeAction, two.Reverse() as FiniteTimeAction);
            return null;
        }
    }

}
