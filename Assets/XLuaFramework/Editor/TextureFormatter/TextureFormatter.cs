using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace AssetFormatter {
    public class TextureFormatter : EditorWindow
    {
        const string settingPath = "Assets/XLuaFramework/Editor/TextureFormatter/Setting/TFSetting.asset";
        const string terrainResPath = "terrain/";

        static List<string> reimportList = new List<string>();
        static TFormatterSetting tmp_setting;
        static string[] platforms = {
        TFormatterPlatform.Standalone.ToString(),
        TFormatterPlatform.Android.ToString(),
        TFormatterPlatform.iPhone.ToString(),
        };
        static string[] audioPlatforms = {
            TFormatterPlatform.Standalone.ToString(),
            TFormatterPlatform.Android.ToString(),
            "iOS",
        };
        TFormatterSetting setting;
        List<FormatPathPair> curtf;
        List<ItemState> stateList;
        List<string> editPaths = new List<string>();

        Vector2 scrollPos;
        int toolBar, editPlatform;
        delegate void ApplyImporter(ApplyData ad);

        [MenuItem("TextureFormatter/Setting")]
        static void OpenWindow()
        {
            TextureFormatter tf = GetWindow<TextureFormatter>();
            tf.setting = AssetDatabase.LoadAssetAtPath<TFormatterSetting>(settingPath);
            if (tmp_setting == null) tmp_setting = (TFormatterSetting)tf.setting.Clone();
            tf.curtf = tf.setting.formatList;
            tf.stateList = new List<ItemState>();
            for (int i = 0; i < tf.curtf.Count; i++) tf.stateList.Add(new ItemState());
            tf.minSize = new Vector2(731, 250);
            tf.toolBar = 0;
            tf.editPlatform = -1;
            tf.UpdateTarget();
        }

        [MenuItem("TextureFormatter/Apply All Platforms")]
        public static void ApplyAll()
        {
            FormatAll();
        }

        [MenuItem("TextureFormatter/Apply Standalone")]
        static void FormatStandalone()
        {
            Format(AssetDatabase.LoadAssetAtPath<TFormatterSetting>(settingPath).formatList, TFormatterPlatform.Standalone);
        }

        [MenuItem("TextureFormatter/Apply Android")]
        static void FormatAndroid()
        {
            Format(AssetDatabase.LoadAssetAtPath<TFormatterSetting>(settingPath).formatList, TFormatterPlatform.Android);
        }

        [MenuItem("TextureFormatter/Apply IOS")]
        static void FormatIOS()
        {
            Format(AssetDatabase.LoadAssetAtPath<TFormatterSetting>(settingPath).formatList, TFormatterPlatform.iPhone);
        }

        public static void PackagerFormat(BuildTarget target)
        {
            TFormatterPlatform platform = TFormatterPlatform.Standalone;

            if (target == BuildTarget.Android)
                platform = TFormatterPlatform.Android;

            if (target == BuildTarget.StandaloneWindows)
                platform = TFormatterPlatform.Standalone;

            if (target == BuildTarget.iOS)
                platform = TFormatterPlatform.iPhone;

            Format(AssetDatabase.LoadAssetAtPath<TFormatterSetting>(settingPath).formatList, platform, false);
        }

        public static void Format(List<FormatPathPair> ctf, TFormatterPlatform platform, bool showTip = true)
        {
            bool success = true;
            reimportList.Clear();
            ApplyFormat(ctf, platform);
            CombineParentPath(reimportList);
            for (int i = 0; i < reimportList.Count; i++)
            {
                string path = reimportList[i];
                if (Directory.Exists(path))
                {
                    if (EditorUtility.DisplayCancelableProgressBar("TextureFormatter", string.Format("Reimporting {0}", path), (i + 1) / reimportList.Count))
                    {
                        success = false;
                        break;
                    };
                    AssetDatabase.ImportAsset(reimportList[i], ImportAssetOptions.ImportRecursive);
                }
                else
                {
                    if (EditorUtility.DisplayCancelableProgressBar("TextureFormatter", string.Format("Reimporting {0}", path), (i + 1) / reimportList.Count))
                    {
                        success = false;
                        break;
                    };
                    AssetDatabase.ImportAsset(reimportList[i]);
                }
            }
            EditorUtility.ClearProgressBar();
            if (success)
            {
                if (showTip)
                {
                    EditorUtility.DisplayDialog("TextureFormatter", "资源格式化完成!", "确定");
                }
                else
                {
                    Debug.Log("资源格式化完成!");
                }
            }
            else
            {
                Debug.LogWarning("资源格式化中断");
            }
        }

        public static void FormatAll(bool showTip = true)
        {
            bool success = true;
            reimportList.Clear();
            TFormatterSetting tfs = AssetDatabase.LoadAssetAtPath<TFormatterSetting>(settingPath);
            List<FormatPathPair> ctf = tfs.formatList;
            AllApplyFormat(ctf);
            CombineParentPath(reimportList);
            for (int i = 0; i < reimportList.Count; i++)
            {
                string path = reimportList[i];
                if (Directory.Exists(path))
                {
                    if (EditorUtility.DisplayCancelableProgressBar("TextureFormatter", string.Format("Reimporting {0}", path), (i + 1) / reimportList.Count))
                    {
                        success = false;
                        break;
                    };
                    AssetDatabase.ImportAsset(reimportList[i], ImportAssetOptions.ImportRecursive);
                }
                else
                {
                    if (EditorUtility.DisplayCancelableProgressBar("TextureFormatter", string.Format("Reimporting {0}", path), (i + 1) / reimportList.Count))
                    {
                        success = false;
                        break;
                    };
                    AssetDatabase.ImportAsset(reimportList[i]);
                }
            }
            EditorUtility.ClearProgressBar();
            if (success)
            {
                if (showTip)
                {
                    EditorUtility.DisplayDialog("TextureFormatter", "资源格式化完成!", "确定");
                }
                else
                {
                    Debug.Log("资源格式化完成!");
                }
            }
            else
            {
                Debug.LogWarning("资源格式化中断");
            }
        }

        void OnGUI()
        {
            try
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                GUILayout.Label("默认格式:");
                setting.standaloneDefault = (TextureImporterFormat)EditorGUILayout.EnumPopup("StandAlone", (StandAloneCompress)setting.standaloneDefault);
                setting.androidDefault = (TextureImporterFormat)EditorGUILayout.EnumPopup("Android", (AndroidCompress)setting.androidDefault);
                setting.iosDefault = (TextureImporterFormat)EditorGUILayout.EnumPopup("IOS", (IosCompress)setting.iosDefault);
                GUILayout.BeginHorizontal();
                GUILayout.Label("平台设置选择：");
                toolBar = GUILayout.Toolbar(toolBar, platforms);
                GUILayout.EndHorizontal();
                if (toolBar != editPlatform)
                {
                    editPlatform = toolBar;
                    OnPlatformChange();
                }
                for (int i = 0; i < curtf.Count; i++)
                {
                    FormatPathPair fpp = curtf[i];
                    ItemState its = stateList[i];
                    TextureFormat tf = fpp.GetFormatByPlatform(editPlatform);
                    GUILayout.BeginVertical("box");
                    GUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                    {
                        curtf.RemoveAt(i);
                        stateList.RemoveAt(i);
                        return;
                    }
                    GUI.backgroundColor = Color.white;
                    if (its.rename)
                    {
                        fpp.name = GUILayout.TextField(fpp.name, GUILayout.MaxWidth(100));
                        GUI.backgroundColor = Color.yellow;
                        if (GUILayout.Button("R", GUILayout.MaxWidth(20))) its.rename = false;
                        GUI.backgroundColor = Color.white;
                    }
                    else
                    {
                        Rect rect = EditorGUILayout.BeginHorizontal("Button", GUILayout.MaxWidth(120));
                        if (GUI.Button(rect, GUIContent.none))
                        {
                            its.expand = !its.expand;
                        }
                        GUILayout.Label(its.expand ? "-" : "+");
                        GUILayout.Label(fpp.name);
                        GUILayout.Label("");
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.yellow;
                        if (GUILayout.Button("R", GUILayout.MaxWidth(20))) its.rename = true;
                        GUI.backgroundColor = Color.white;
                    }

                    fpp.iType = (ImporterType)EditorGUILayout.EnumPopup(fpp.iType);
                    if (fpp.iType == ImporterType.Texture) tf.format = (TextureImporterFormat)EditorGUILayout.EnumPopup(TextureFormat.GetPlatformEnum(editPlatform, (int)tf.format));
                    GUI.backgroundColor = Color.blue;
                    if (GUILayout.Button("Apply", GUILayout.MaxWidth(43)))
                    {
                        List<FormatPathPair> applyList = new List<FormatPathPair>();
                        applyList.Add(fpp);
                        Format(applyList, (TFormatterPlatform)editPlatform);
                        return;
                    }
                    GUI.backgroundColor = Color.white;
                    GUILayout.EndHorizontal();
                    if (its.expand)
                    {
                        switch (fpp.iType)
                        {
                            case ImporterType.Texture:
                                DrawTextureSettingItem(tf, fpp);
                                break;
                            case ImporterType.Model:
                                DrawModelSettingItem(tf, fpp);
                                break;
                            case ImporterType.Audio:
                                DrawAudioSettingItem(tf, fpp);
                                break;
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.Label("", GUILayout.Height(20));
                GUILayout.BeginArea(new Rect(0, position.height - 20 + scrollPos.y, position.width, 20));
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("添加自定义配置"))
                {
                    GUILayout.EndHorizontal();
                    if (scrollPos.y == 0) GUILayout.EndArea();
                    GUILayout.EndScrollView();
                    for (int i = 0; i < stateList.Count; i++)
                    {
                        stateList[i].expand = false;
                    }
                    int newIndex = 1;
                    while (!CheckIndexAvailable(newIndex, curtf)) newIndex++;
                    FormatPathPair fpp = new FormatPathPair(newIndex);
                    curtf.Add(fpp);
                    ItemState its = new ItemState();
                    its.expand = true;
                    stateList.Add(its);
                    return;
                }
                if (GUILayout.Button("保存配置到磁盘"))
                {
                    GUILayout.EndHorizontal();
                    if (scrollPos.y == 0) GUILayout.EndArea();
                    GUILayout.EndScrollView();
                    EditorUtility.SetDirty(setting);
                    AssetDatabase.SaveAssets();
                    tmp_setting = (TFormatterSetting)setting.Clone();
                    EditorUtility.DisplayDialog("TextureFormatter", "配置保存成功!", "确定");
                    return;
                }
                if (GUILayout.Button("回滚至磁盘版本"))
                {
                    GUILayout.EndHorizontal();
                    if (scrollPos.y == 0) GUILayout.EndArea();
                    GUILayout.EndScrollView();
                    setting.Copy(tmp_setting);
                    return;
                }
                if (GUILayout.Button("应用当前平台配置"))
                {
                    GUILayout.EndHorizontal();
                    if (scrollPos.y == 0) GUILayout.EndArea();
                    GUILayout.EndScrollView();
                    Format(setting.formatList, (TFormatterPlatform)editPlatform);
                    return;
                }
                if (GUILayout.Button("应用全平台配置"))
                {
                    GUILayout.EndHorizontal();
                    if (scrollPos.y == 0) GUILayout.EndArea();
                    GUILayout.EndScrollView();
                    FormatAll();
                    return;
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
                GUILayout.EndScrollView();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Close();
            }
        }

        bool CheckIndexAvailable(int index, List<FormatPathPair> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (index == list[i].index) return false;
            }
            return true;
        }

        void OnPlatformChange()
        {
            scrollPos = Vector2.zero;
        }

        void OnSelectionChange()
        {
            UpdateTarget();
            Repaint();
        }

        void UpdateTarget()
        {
            int[] sids = Selection.instanceIDs;
            editPaths.Clear();
            for (int i = 0; i < sids.Length; i++)
            {
                string path = AssetDatabase.GetAssetPath(sids[i]);
                if (!string.IsNullOrEmpty(path))
                    editPaths.Add(path);
            }
            FilterRepeatPath(editPaths);
        }

        void DrawTextureSettingItem(TextureFormat tf, FormatPathPair fpp)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("MaxSize", GUILayout.MaxWidth(56));
            tf.maxSize = EditorGUILayout.IntField(tf.maxSize, GUILayout.MaxWidth(60));
            GUILayout.Label("", GUILayout.MaxWidth(20));
            GUILayout.Label("Read/Write Enabled", GUILayout.MaxWidth(126));
            fpp.isReadable = EditorGUILayout.Toggle(fpp.isReadable, GUILayout.MaxWidth(20));
            GUILayout.Label("", GUILayout.MaxWidth(20));
            GUILayout.Label("useMipMaps", GUILayout.MaxWidth(80));
            fpp.useMipMaps = EditorGUILayout.Toggle(fpp.useMipMaps, GUILayout.MaxWidth(20));
            GUILayout.Label("", GUILayout.MaxWidth(20));
            GUILayout.Label("Non Power of 2", GUILayout.MaxWidth(105));
            fpp.nonPowerOf2 = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup(fpp.nonPowerOf2);
            GUILayout.Label("", GUILayout.MaxWidth(20));
            GUILayout.Label("compresssion Quality", GUILayout.MaxWidth(132));
            tf.compressionQuality = (int)(UnityEditor.TextureCompressionQuality)EditorGUILayout.EnumPopup((UnityEditor.TextureCompressionQuality)tf.compressionQuality);
            GUILayout.EndHorizontal();
            DrawPathAddition(fpp);
        }

        void DrawModelSettingItem(TextureFormat tf, FormatPathPair fpp)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Read/Write Enabled", GUILayout.MaxWidth(126));
            fpp.isReadable = EditorGUILayout.Toggle(fpp.isReadable, GUILayout.MaxWidth(20));
            GUILayout.EndHorizontal();
            DrawPathAddition(fpp);
        }

        void DrawAudioSettingItem(TextureFormat tf, FormatPathPair fpp) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Load Type", GUILayout.MaxWidth(81));
            tf.loadType = (AudioClipLoadType)EditorGUILayout.EnumPopup(tf.loadType);
            GUILayout.EndHorizontal();
            DrawPathAddition(fpp);
        }

        void DrawPathAddition(FormatPathPair fpp) {
            if (fpp.paths.Count > 0)
            {
                GUILayout.Label("应用路径:");
                for (int j = 0; j < fpp.paths.Count; j++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(fpp.paths[j]);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("delete", GUILayout.MaxWidth(80)))
                    {
                        fpp.paths.RemoveAt(j);
                    };
                    GUI.backgroundColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("应用路径:");
                GUILayout.Label("无");
                GUILayout.EndHorizontal();
            }
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("AddSelectedPaths"))
            {
                if (editPaths.Count == 0) Debug.LogWarning("未选中有效目录或文件");
                foreach (string path in editPaths)
                {
                    if (!fpp.paths.Contains(path))
                    {
                        AssetImporter ai = AssetImporter.GetAtPath(path);
                        bool typeCheck = ai as TextureImporter != null || ai as ModelImporter != null || ai as AudioImporter != null;
                        if (Directory.Exists(path) || typeCheck)
                        {
                            fpp.paths.Add(path);
                        }
                        else
                        {
                            Debug.LogWarningFormat("选中文件{0}不是有效文件", path);
                        }
                    }
                }
                FilterRepeatPath(fpp.paths);
            }
            GUI.backgroundColor = Color.white;
        }

        static void ApplyTextureImporter(ApplyData ad)
        {
            string assetPath = ad.assetPath;
            List<string> ignoreList = ad.ignoreList;
            TextureFormat tf = ad.tf;
            FormatPathPair fpp = ad.fpp;
            bool advanced = ad.advanced;
            string platformName = ad.platformName;
            bool needToCheck = ad.needToCheck;
            TextureImporter ti = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (ti != null)
            {
                if (needToCheck)
                {
                    for (int k = 0; k < ignoreList.Count; k++)
                    {
                        if (ignoreList[k] == assetPath)
                        {
                            return;
                        }
                    }
                }

                if (string.IsNullOrEmpty(platformName))
                {
                    for (int j = 0; j < 3; j++)
                    {
                        tf = fpp.GetFormatByPlatform(j);
                        platformName = platforms[j];
                        if (!fpp.Compare(ti, tf, platformName))
                        {
                            fpp.Apply(ti, tf, platformName);
                            if (!reimportList.Contains(assetPath)) reimportList.Add(assetPath);
                        }
                    }
                }
                else
                {
                    if (!fpp.Compare(ti, tf, platformName))
                    {
                        fpp.Apply(ti, tf, platformName);
                        if (!reimportList.Contains(assetPath)) reimportList.Add(assetPath);
                    }
                }
            }
        }

        static void ApplyModelImporter(ApplyData ad)
        {
            string assetPath = ad.assetPath;
            List<string> ignoreList = ad.ignoreList;
            TextureFormat tf = ad.tf;
            FormatPathPair fpp = ad.fpp;
            bool advanced = ad.advanced;
            string platformName = ad.platformName;
            bool needToCheck = ad.needToCheck;
            ModelImporter mi = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (mi != null)
            {
                if (needToCheck)
                {
                    for (int k = 0; k < ignoreList.Count; k++)
                    {
                        if (ignoreList[k] == assetPath)
                        {
                            return;
                        }
                    }
                }

                bool need_reimport = false;

                if (assetPath.Contains(terrainResPath))
                {
                    if (mi.isReadable != fpp.isReadable || mi.meshCompression != ModelImporterMeshCompression.Low || !mi.optimizeGameObjects || mi.animationCompression != ModelImporterAnimationCompression.Optimal || !mi.optimizeMesh || mi.indexFormat != ModelImporterIndexFormat.UInt16 || mi.importNormals != ModelImporterNormals.None)
                    {
                        mi.meshCompression = ModelImporterMeshCompression.Low;
                        need_reimport = true;
                        mi.importNormals = ModelImporterNormals.None;
                    }
                }
                else
                {
                    if (mi.isReadable != fpp.isReadable || mi.meshCompression != ModelImporterMeshCompression.High || !mi.optimizeGameObjects || mi.animationCompression != ModelImporterAnimationCompression.Optimal || mi.importLights || !mi.optimizeMesh || mi.indexFormat != ModelImporterIndexFormat.UInt16)
                    {
                        mi.meshCompression = ModelImporterMeshCompression.High;
                        mi.importLights = false;
                        need_reimport = true;
                    }
                }             
          
                if (need_reimport)
                {
                    mi.isReadable = fpp.isReadable;
                    mi.optimizeGameObjects = true;
                    mi.optimizeMesh = true;
                    mi.animationCompression = ModelImporterAnimationCompression.Optimal;
                    mi.indexFormat = ModelImporterIndexFormat.UInt16;
                    if (!reimportList.Contains(assetPath)) reimportList.Add(assetPath);
                }
            }
        }

        static void ApplyAudioImporter(ApplyData ad)
        {

            string assetPath = ad.assetPath;
            List<string> ignoreList = ad.ignoreList;
            TextureFormat tf = ad.tf;
            FormatPathPair fpp = ad.fpp;
            bool advanced = ad.advanced;
            string platformName = ad.platformName;
            bool needToCheck = ad.needToCheck;
            AudioImporter ai = AssetImporter.GetAtPath(assetPath) as AudioImporter;
            if (ai != null)
            {
                if (needToCheck)
                {
                    for (int k = 0; k < ignoreList.Count; k++)
                    {
                        if (ignoreList[k] == assetPath)
                        {
                            return;
                        }
                    }
                }

                AudioImporterSampleSettings aiss;
                if (string.IsNullOrEmpty(platformName))
                {
                    for (int j = 0; j < 3; j++)
                    {
                        tf = fpp.GetFormatByPlatform(j);
                        platformName = audioPlatforms[j];
                        if (ai.ContainsSampleSettingsOverride(platformName)) {
                            aiss = ai.GetOverrideSampleSettings(platformName);
                            if (aiss.loadType != tf.loadType || !ai.forceToMono)
                            {
                                aiss.loadType = tf.loadType;
                                ai.forceToMono = true;
                                if (!reimportList.Contains(assetPath)) reimportList.Add(assetPath);
                            }
                        }
                        else if(ai.defaultSampleSettings.loadType != tf.loadType || !ai.forceToMono)
                        {
                            ai.forceToMono = true;
                            aiss = ai.defaultSampleSettings;
                            aiss.loadType = tf.loadType;
                            ai.SetOverrideSampleSettings(platformName, aiss);
                            if (!reimportList.Contains(assetPath)) reimportList.Add(assetPath);
                        }
                    }
                }
                else
                {
                    if (platformName == platforms[2]) platformName = "iOS";
                    if (ai.ContainsSampleSettingsOverride(platformName))
                    {
                        aiss = ai.GetOverrideSampleSettings(platformName);
                        if (aiss.loadType != tf.loadType || !ai.forceToMono)
                        {
                            ai.forceToMono = true;
                            aiss.loadType = tf.loadType;
                            ai.SetOverrideSampleSettings(platformName, aiss);
                            if (!reimportList.Contains(assetPath)) reimportList.Add(assetPath);
                        }
                    }
                    else if (ai.defaultSampleSettings.loadType != tf.loadType || !ai.forceToMono)
                    {
                        ai.forceToMono = true;
                        aiss = ai.defaultSampleSettings;
                        aiss.loadType = tf.loadType;
                        ai.SetOverrideSampleSettings(platformName, aiss);
                        if (!reimportList.Contains(assetPath)) reimportList.Add(assetPath);
                    }
                }
            }
        }

        static void FilterRepeatPath(List<string> pathList)
        {
            for (int i = pathList.Count - 1; i >= 0; i--)
            {
                string checkStr = pathList[i];
                for (int j = 0; j < pathList.Count; j++)
                {
                    string path = pathList[j];
                    if (i != j && checkStr.Contains(path))
                    {
                        Debug.LogFormat("路径过滤:{0}已被包括", path);
                        pathList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        static void CombineParentPath(List<string> pathList, List<string> checkedList = null)
        {
            if (checkedList == null) checkedList = new List<string>();
            for (int i = pathList.Count - 1; i >= 0; i--)
            {
                string path = pathList[i];
                path = path.Substring(0, path.LastIndexOf('/'));
                if (!checkedList.Contains(path))
                {
                    checkedList.Add(path);
                    DirectoryInfo di = new DirectoryInfo(path);
                    if (CheckInclusiveRecursively(di, pathList))
                    {
                        for (int j = pathList.Count - 1; j >= 0; j--)
                        {
                            string jPath = pathList[j];
                            if (jPath.Contains(path))
                                pathList.RemoveAt(j);
                        }
                        pathList.Add(path);
                    }
                    CombineParentPath(pathList, checkedList);
                    break;
                }
            }

        }

        static bool CheckInclusiveRecursively(DirectoryInfo curDi, List<string> pathList)
        {
            FileInfo[] files = curDi.GetFiles();
            foreach (FileInfo fi in files)
            {
                string filePath = fi.FullName;
                int index = filePath.IndexOf("Assets");
                filePath = filePath.Substring(index, filePath.Length - index);
                filePath = filePath.Replace('\\', '/');
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                if (ai as TextureImporter != null || ai as ModelImporter != null || ai as AudioImporter != null)
                {
                    bool find = false;
                    for (int i = 0; i < pathList.Count; i++) if (filePath.Contains(pathList[i])) find = true;
                    if (!find) return false;
                }

            }
            DirectoryInfo[] dis = curDi.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                if (!CheckInclusiveRecursively(di, pathList)) return false;
            }
            return true;
        }

        static void ApplyFormat(List<FormatPathPair> list, TFormatterPlatform platform)
        {

            List<string> ignoreList = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                List<string> paths = list[i].paths;
                for (int j = 0; j < paths.Count; j++)
                {
                    ignoreList.Add(paths[j]);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                FormatPathPair fpp = list[i];
                for (int j = 0; j < fpp.paths.Count; j++)
                {
                    FormatPathRecursively(fpp.paths[j], ignoreList, fpp, platform, true);
                }
            }
        }

        /// <summary>
        /// 全平台应用请使用该接口(效率因素)
        /// </summary>
        /// <param name="list"></param>
        static void AllApplyFormat(List<FormatPathPair> list)
        {
            List<string> ignoreList = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                List<string> paths = list[i].paths;
                for (int j = 0; j < paths.Count; j++)
                {
                    ignoreList.Add(paths[j]);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                FormatPathPair fpp = list[i];
                for (int j = 0; j < fpp.paths.Count; j++)
                {
                    FormatPathRecursively(fpp.paths[j], ignoreList, fpp, true);
                }
            }
        }

        static void FormatPathRecursively(string path, List<string> ignoreList, FormatPathPair fpp, TFormatterPlatform platform, bool origin = false)
        {
            string platformName = TextureFormat.GetPlatformName(platform);
            TextureFormat tf = fpp.GetFormatByPlatform((int)platform);
            path = path.Replace('\\', '/');
            for (int k = 0; k < ignoreList.Count; k++)
            {
                if (ignoreList[k] == path && !origin) return;
            }
            TextureImporterFormat tif = tf.format;
            bool advanced = tif > 0;
            ApplyData ad = new ApplyData(path, ignoreList, fpp);
            ad.tf = tf;
            ad.advanced = advanced;
            ad.platformName = platformName;
            ApplyImporter importerHandler = null;
            switch (fpp.iType)
            {
                case ImporterType.Texture:
                    importerHandler = ApplyTextureImporter;
                    break;
                case ImporterType.Model:
                    importerHandler = ApplyModelImporter;
                    break;
                case ImporterType.Audio:
                    importerHandler = ApplyAudioImporter;
                    break;
            }
            if (Directory.Exists(path))
            {
                ad.needToCheck = true;
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles();
                DirectoryInfo[] dis = di.GetDirectories();
                foreach (FileInfo fi in files)
                {
                    int index = fi.FullName.IndexOf("Assets");
                    string assetPath = fi.FullName.Substring(index);
                    assetPath = assetPath.Replace('\\', '/');
                    ad.assetPath = assetPath;
                    importerHandler(ad);
                }
                foreach (DirectoryInfo idi in dis)
                {
                    FormatPathRecursively(idi.FullName.Substring(idi.FullName.IndexOf("Assets")), ignoreList, fpp, platform);
                }
            }
            else if (File.Exists(path))
            {
                importerHandler(ad);
            }
        }

        static void FormatPathRecursively(string path, List<string> ignoreList, FormatPathPair fpp, bool origin = false)
        {
            path = path.Replace('\\', '/');
            for (int k = 0; k < ignoreList.Count; k++)
            {
                if (ignoreList[k] == path && !origin) return;
            }
            ApplyData ad = new ApplyData(path, ignoreList, fpp);
            ApplyImporter importerHandler = null;
            switch (fpp.iType)
            {
                case ImporterType.Texture:
                    importerHandler = ApplyTextureImporter;
                    break;
                case ImporterType.Model:
                    importerHandler = ApplyModelImporter;
                    break;
                case ImporterType.Audio:
                    importerHandler = ApplyAudioImporter;
                    break;
            }
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles();
                DirectoryInfo[] dis = di.GetDirectories();
                foreach (FileInfo fi in files)
                {
                    int index = fi.FullName.IndexOf("Assets");
                    string assetPath = fi.FullName.Substring(index);
                    assetPath = assetPath.Replace('\\', '/');
                    ad.assetPath = assetPath;
                    importerHandler(ad);
                }
                foreach (DirectoryInfo idi in dis)
                {
                    FormatPathRecursively(idi.FullName.Substring(idi.FullName.IndexOf("Assets")), ignoreList, fpp);
                }
            }
            else if (File.Exists(path))
            {
                ad.needToCheck = false;
                importerHandler(ad);
            }
        }

        [MenuItem("TextureFormatter/Force Close Setting Window")]
        static void CloseWindow()
        {
            TextureFormatter tf = GetWindow<TextureFormatter>();
            tf.Close();
        }

        class ItemState
        {
            public bool expand = false;
            public bool rename = false;
        }

        class ApplyData
        {
            public string assetPath;
            public List<string> ignoreList;
            public FormatPathPair fpp;
            public TextureFormat tf;
            public bool advanced;
            public string platformName;
            public bool needToCheck = false;

            public ApplyData(string assetPath, List<string> ignoreList, FormatPathPair fpp)
            {
                this.assetPath = assetPath;
                this.ignoreList = ignoreList;
                this.fpp = fpp;
            }
        }
    }
}

