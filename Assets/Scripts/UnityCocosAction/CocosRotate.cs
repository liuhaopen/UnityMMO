using System;
using UnityEngine;

namespace Cocos
{
    //
    // RotateBy
    //
    public class RotateBy : ActionInterval
    {
        protected Vector3 positionDelta;
        protected Vector3 previousPosition;
        protected Vector3 startPosition;
        protected CoordinateStyle style;
        protected RectTransform rectTrans;

        public static RotateBy Create(float duration, Vector3 deltaPosition, CoordinateStyle style=CoordinateStyle.Local)
        {
            RotateBy ret = new RotateBy();
            if (ret != null && ret.InitWithStyle(duration, deltaPosition, style))
                return ret;
            return null;
        }

        public static RotateBy CreateLocal(float duration, Vector3 deltaPosition)
        {
            return Create(duration, deltaPosition, CoordinateStyle.Local);
        }
        public static RotateBy CreateAbs(float duration, Vector3 deltaPosition)
        {
            return Create(duration, deltaPosition, CoordinateStyle.Abs);
        }
        public static RotateBy CreateAnchored(float duration, Vector3 deltaPosition)
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
            Debug.Log("Rotate by clone");
            return RotateBy.Create(duration, positionDelta, style);
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
                    break;
            }
        }

        public override Action Reverse()
        {
            return RotateBy.Create(duration, -positionDelta, style);
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

    //
    // RotateTo
    //
    public class RotateTo : RotateBy
    {
        Vector3 endPosition;
        public static new RotateTo Create(float duration, Vector3 position, CoordinateStyle style=CoordinateStyle.Local)
        {
            RotateTo ret = new RotateTo();
            if (ret != null && ret.InitWithDuration(duration, position, style))
                return ret;
            return null;
        }

        public static new RotateTo CreateLocal(float duration, Vector3 position)
        {
            return Create(duration, position, CoordinateStyle.Local);
        }
        public static new RotateTo CreateAbs(float duration, Vector3 position)
        {
            return Create(duration, position, CoordinateStyle.Abs);
        }
        public static new RotateTo CreateAnchored(float duration, Vector3 position)
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
            return RotateTo.Create(duration, endPosition);
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
            Debug.LogError("reverse() not supported in RotateTo");
            return null;
        }
    }


}