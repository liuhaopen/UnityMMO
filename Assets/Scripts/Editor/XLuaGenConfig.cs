/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;
using TMPro;
using UnityEngine.UI;
using static XLuaManager;
using System.Reflection;
using UnityMMO;
using System.Linq;
using UnityEngine.EventSystems;
using Cinemachine;
//using System.Reflection;
//using System.Linq;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class XLuaGenConfig
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>() {
        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        // typeof(WWW),
        typeof(Light),
        typeof(Mathf),
        typeof(System.Collections.Generic.List<int>),
        typeof(Action<string>),
        typeof(InputField),
        typeof(InputField.ContentType),
        typeof(InputField.InputType),
        typeof(InputField.CharacterValidation),
        typeof(InputField.LineType),
        typeof(InputField.SubmitEvent),
        typeof(InputField.OnChangeEvent),
        typeof(TextAnchor),
        typeof(RaycastHit),
        typeof(Touch),
        typeof(TouchPhase),
        typeof(LayerMask),
        typeof(Plane),
        typeof(RectTransform),
        typeof(RectTransform.Axis),
        typeof(RectTransform.Edge),
        typeof(TextMesh),
        typeof(Graphic),
        typeof(UIBehaviour),
        typeof(MaskableGraphic),
        typeof(MaskableGraphic.CullStateChangedEvent),
        typeof(ScrollRect),
        typeof(ScrollRect.MovementType),
        typeof(ScrollRect.ScrollbarVisibility),
        typeof(Image),
        typeof(Image.Type),
        typeof(Image.FillMethod),
        typeof(Image.OriginHorizontal),
        typeof(Image.OriginVertical),
        typeof(Image.Origin90),
        typeof(Image.Origin180),
        typeof(Image.Origin360),
        typeof(Animator),
        typeof(CircleRawImage),
        typeof(RawImage),
        typeof(Camera),
        typeof(Camera.CameraCallback),
        typeof(Camera.FieldOfViewAxis),
        typeof(Camera.GateFitParameters),
        typeof(Camera.MonoOrStereoscopicEye),
        typeof(Camera.StereoscopicEye),
        typeof(Camera.GateFitMode),
        typeof(Physics),
        // typeof(System.RuntimeType),
        typeof(Input),
        typeof(Sprite),
        typeof(Texture),
        typeof(Texture2D),
        typeof(Texture2D.EXRFlags),
        typeof(Button),
        typeof(KeyCode),
        typeof(Screen),
        typeof(RenderTexture),
        typeof(CinemachineFreeLook),
        typeof(AxisState),
        typeof(UnityEngine.UI.ScrollRect.ScrollRectEvent),
        typeof(UnityEngine.Debug)
    };

    [LuaCallCSharp]
    public static List<Type> LuaCallCSharpEntities = new List<Type>() {
        typeof(Unity.Entities.World),
        typeof(Unity.Entities.Entity),
        typeof(Unity.Entities.GameObjectEntity),
        typeof(Unity.Entities.EntityManagerExtensions),
        typeof(Unity.Entities.EntityManager),
        typeof(Unity.Entities.EntityManager.EntityManagerDebug),
    };

    [LuaCallCSharp]
    [CSharpCallLua]
    public static List<Type> CSharpCallLuaUnityMMO
    {
        get
        {
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            List<Type> list = (from type in types
                               where (type.Namespace == "UnityMMO" || type.Namespace == "XLuaFramework" || type.Namespace == "Cocos")
                               select type).ToList();
            return list;
        }
    }

    [LuaCallCSharp]
    public static List<Type> LuaCallCSharpTextMeshPro = new List<Type>() {
        typeof(TextMeshProUGUI),
        typeof(TMP_InputField),
        typeof(TMP_Text),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    [LuaCallCSharp]
    public static List<Type> CSharpCallLua = new List<Type>() {
        typeof(Action),
        typeof(Func<double, double, double>),
        typeof(Func<long, long>),
        typeof(Func<long, string>),
        typeof(Func<string, long>),
        typeof(Action<string>),
        typeof(Action<double>),
        typeof(Action<bool>),
        typeof(Action<float, float>),
        typeof(UnityEngine.Events.UnityAction),
        typeof(UnityEngine.Events.UnityAction<long>),
        typeof(UnityEngine.Events.UnityAction<long,long>),
        typeof(UnityEngine.Events.UnityAction<string>),
        typeof(UnityEngine.Events.UnityAction<UnityEngine.Vector2>),
        typeof(UnityEngine.Events.UnityEvent<UnityEngine.Vector2>),
        typeof(System.Collections.IEnumerator),
    };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()  {
        new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
        new List<string>(){"UnityEngine.WWW", "movie"},
#if UNITY_WEBGL
        new List<string>(){"UnityEngine.WWW", "threadPriority"},
#endif
        new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
        new List<string>(){"UnityEngine.Texture", "imageContentsHash"},
        new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
        new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
        new List<string>(){"UnityEngine.Light", "areaSize"},
        new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
        new List<string>(){"UnityEngine.Light", "SetLightDirty"},
        new List<string>(){"UnityEngine.Light", "shadowRadius"},
        new List<string>(){"UnityEngine.Light", "shadowAngle"},
        new List<string>(){"UnityEngine.Input", "IsJoystickPreconfigured", "System.String"},
        
        new List<string>(){"UnityEngine.WWW", "MovieTexture"},
        new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
        new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
#if !UNITY_WEBPLAYER
        new List<string>(){"UnityEngine.Application", "ExternalEval"},
#endif
        new List<string>(){"UnityEngine.UI.Graphic", "OnRebuildRequested"}, 
        new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
        new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
        new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
        new List<string>(){"Unity.Entities.EntityManager", "GetName", "Unity.Entities.Entity"},
        new List<string>(){"Unity.Entities.EntityManager", "SetName", "Unity.Entities.Entity", "System.String"},
    };

    //static bool hasGenericParameter(Type type)
    //{
    //    if (type.IsGenericTypeDefinition) return true;
    //    if (type.IsGenericParameter) return true;
    //    if (type.IsByRef || type.IsArray)
    //    {
    //        return hasGenericParameter(type.GetElementType());
    //    }
    //    if (type.IsGenericType)
    //    {
    //        foreach (var typeArg in type.GetGenericArguments())
    //        {
    //            if (hasGenericParameter(typeArg))
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    //// 配置某Assembly下所有涉及到的delegate到CSharpCallLua下，Hotfix下拿不准那些delegate需要适配到lua function可以这么配置
    //[CSharpCallLua]
    //static IEnumerable<Type> AllDelegate
    //{
    //    get
    //    {
    //        BindingFlags flag = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
    //        List<Type> allTypes = new List<Type>();
    //        var allAssemblys = new Assembly[]
    //        {
    //            Assembly.Load("Assembly-CSharp")
    //        };
    //        foreach (var t in (from assembly in allAssemblys from type in assembly.GetTypes() select type))
    //        {
    //            var p = t;
    //            while (p != null)
    //            {
    //                allTypes.Add(p);
    //                p = p.BaseType;
    //            }
    //        }
    //        allTypes = allTypes.Distinct().ToList();
    //        var allMethods = from type in allTypes
    //                         from method in type.GetMethods(flag)
    //                         select method;
    //        var returnTypes = from method in allMethods
    //                          select method.ReturnType;
    //        var paramTypes = allMethods.SelectMany(m => m.GetParameters()).Select(pinfo => pinfo.ParameterType.IsByRef ? pinfo.ParameterType.GetElementType() : pinfo.ParameterType);
    //        var fieldTypes = from type in allTypes
    //                         from field in type.GetFields(flag)
    //                         select field.FieldType;
    //        return (returnTypes.Concat(paramTypes).Concat(fieldTypes)).Where(t => t.BaseType == typeof(MulticastDelegate) && !hasGenericParameter(t)).Distinct();
    //    }
    //}
}
