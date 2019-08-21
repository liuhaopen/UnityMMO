using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//如果你的项目没有用到TextMeshPro，那么可以直接把此文件删掉
namespace Cocos
{
    public class ColorAttrCatcherTextMeshPro : IColorAttrCatcher
    {
        public Func<Transform, Color> GetColor { get => GetColorFunc; }
        public System.Action<Transform, Color> SetColor { get => SetColorFunc; }
        public static ColorAttrCatcherTextMeshPro Ins = new ColorAttrCatcherTextMeshPro();

        public static Color GetColorFunc(Transform target)
        {
            if (target != null)
            {
                var text = target.GetComponent<TextMeshPro>();
                if (text != null)
                    return text.color;
                Debug.LogError("action target has no TextMeshPro component, please don't use ColorAttrCatcherTextMeshPro!" + new System.Diagnostics.StackTrace().ToString());
            }
            return Color.white;
        }
        public static void SetColorFunc(Transform target, Color color)
        {
            if (target != null)
            {
                var text = target.GetComponent<TextMeshPro>();
                if (text != null)
                    text.color = color;
                else
                    Debug.LogError("action target has no TextMeshPro component, please don't use ColorAttrCatcherTextMeshPro!" + new System.Diagnostics.StackTrace().ToString());
            }
        }
    }

    public class ColorAttrCatcherTextMeshProUI : IColorAttrCatcher
    {
        public Func<Transform, Color> GetColor { get => GetColorFunc; }
        public System.Action<Transform, Color> SetColor { get => SetColorFunc; }
        public static ColorAttrCatcherTextMeshProUI Ins = new ColorAttrCatcherTextMeshProUI();

        public static Color GetColorFunc(Transform target)
        {
            if (target != null)
            {
                var text = target.GetComponent<TextMeshProUGUI>();
                if (text != null)
                    return text.color;
                Debug.LogError("action target has no TextMeshProUGUI component, please don't use ColorAttrCatcherTextMeshProUI!" + new System.Diagnostics.StackTrace().ToString());
            }
            return Color.white;
        }
        public static void SetColorFunc(Transform target, Color color)
        {
            if (target != null)
            {
                var text = target.GetComponent<TextMeshProUGUI>();
                if (text != null)
                    text.color = color;
                else
                    Debug.LogError("action target has no TextMeshProUGUI component, please don't use ColorAttrCatcherTextMeshProUI!" + new System.Diagnostics.StackTrace().ToString());
            }
        }
    }

}