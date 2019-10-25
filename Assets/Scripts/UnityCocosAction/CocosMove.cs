using System;
using UnityEngine;

namespace Cocos
{
    public enum CoordinateStyle
    {
        Local,
        Abs,
        Anchored,
    } 

    public class MoveBy : ActionInterval
    {
        protected Vector3 positionDelta;
        protected Vector3 previousPosition;
        protected Vector3 startPosition;
        protected CoordinateStyle style;
        protected RectTransform rectTrans;

        public static MoveBy Create(float duration, Vector3 deltaPosition, CoordinateStyle style=CoordinateStyle.Local)
        {
            MoveBy ret = new MoveBy();
            if (ret != null && ret.InitWithStyle(duration, deltaPosition, style))
                return ret;
            return null;
        }

        public static MoveBy CreateLocal(float duration, Vector3 deltaPosition)
        {
            return Create(duration, deltaPosition, CoordinateStyle.Local);
        }

        public static MoveBy CreateAbs(float duration, Vector3 deltaPosition)
        {
            return Create(duration, deltaPosition, CoordinateStyle.Abs);
        }

        public static MoveBy CreateAnchored(float duration, Vector3 deltaPosition)
        {
            return Create(duration, deltaPosition, CoordinateStyle.Anchored);
        }

        private bool InitWithStyle(float duration, Vector3 deltaPosition, CoordinateStyle style)
        {
            bool ret = false;
            if (base.InitWithDuration(duration))
            {
                this.positionDelta = deltaPosition;
                this.style = style;
                ret = true;
            }
            return ret;
        }

        public override Action Clone()
        {
            Debug.Log("move by clone");
            return MoveBy.Create(duration, positionDelta, style);
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            switch (style)
            {
                case CoordinateStyle.Local:
                {
                    previousPosition = startPosition = target.localPosition;
                    break;
                }
                case CoordinateStyle.Abs:
                {
                    previousPosition = startPosition = target.position;
                    break;
                }
                case CoordinateStyle.Anchored:
                {
                    rectTrans = target as RectTransform;
                    previousPosition = startPosition = rectTrans.anchoredPosition3D;
                    break;
                }
                default:
                    Debug.LogError("cannot support coordinate style for move action : "+style);
                    break;
            }
        }

        public override Action Reverse()
        {
            return MoveBy.Create(duration, -positionDelta, style);
        }

        public override void Update(float t) 
        {
            switch (style)
            {
                case CoordinateStyle.Local:
                {
                    var currentPos = target.localPosition;
                    var diff = currentPos - previousPosition;
                    startPosition = startPosition + diff;
                    var newPos = startPosition + (positionDelta*t);
                    target.localPosition = newPos;
                    previousPosition = newPos;
                    break;
                }
                case CoordinateStyle.Abs:
                {
                    var currentPos = target.position;
                    var diff = currentPos - previousPosition;
                    startPosition = startPosition + diff;
                    var newPos = startPosition + (positionDelta*t);
                    target.position = newPos;
                    previousPosition = newPos;
                    break;
                }
                case CoordinateStyle.Anchored:
                {
                    var currentPos = rectTrans.anchoredPosition3D;
                    var diff = currentPos - previousPosition;
                    startPosition = startPosition + diff;
                    var newPos = startPosition + (positionDelta*t);
                    rectTrans.anchoredPosition3D = newPos;
                    previousPosition = newPos;
                    break;
                }
                default:
                    break;
            }
        }
    }

    public class MoveTo : MoveBy
    {
        Vector3 endPosition;
        public static new MoveTo Create(float duration, Vector3 position, CoordinateStyle style=CoordinateStyle.Local)
        {
            MoveTo ret = new MoveTo();
            if (ret != null && ret.InitWithDuration(duration, position, style))
                return ret;
            return null;
        }

        public static new MoveTo CreateLocal(float duration, Vector3 position)
        {
            return Create(duration, position, CoordinateStyle.Local);
        }

        public static new MoveTo CreateAbs(float duration, Vector3 position)
        {
            return Create(duration, position, CoordinateStyle.Abs);
        }

        public static new MoveTo CreateAnchored(float duration, Vector3 position)
        {
            return Create(duration, position, CoordinateStyle.Anchored);
        }

        private bool InitWithDuration(float duration, Vector3 position, CoordinateStyle style)
        {
            bool ret = false;
            if (base.InitWithDuration(duration))
            {
                this.style = style;
                this.endPosition = position;
                ret = true;
            }
            return ret;
        }

        public override Action Clone()
        {
            return MoveTo.Create(duration, endPosition);
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            switch (style)
            {
                case CoordinateStyle.Local:
                {
                    positionDelta = endPosition - target.localPosition;
                    break;
                }
                case CoordinateStyle.Abs:
                {
                    positionDelta = endPosition - target.position;
                    break;
                }
                case CoordinateStyle.Anchored:
                {
                    positionDelta = endPosition - rectTrans.anchoredPosition3D;
                    break;
                }
                default:
                    break;
            }
        }

        public override Action Reverse()
        {
            Debug.LogError("reverse() not supported in MoveTo");
            return null;
        }
    }


}