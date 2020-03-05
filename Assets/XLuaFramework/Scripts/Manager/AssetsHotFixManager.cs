//extract resource on game start and hot fix assets from cdn
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XLuaFramework
{
public class AssetsHotFixManager : MonoBehaviour
{
    private List<string> downloadFiles = new List<string>();
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    public static AssetsHotFixManager Instance;
    private void Awake() 
    {
        Instance = this;
    }

    public void CheckExtractResource(Action<float> on_update, Action on_ok) 
    {
        bool isExists = Directory.Exists(AppConfig.DataPath) &&
            Directory.Exists(AppConfig.DataPath + "lua/") && File.Exists(AppConfig.DataPath + "files.txt");
        Debug.Log("CheckExtraceResource AppConfig.DataPath:" + AppConfig.DataPath+ " isExists:"+ isExists.ToString()+" debugmode : "+ AppConfig.DebugMode.ToString());
        if (isExists || AppConfig.DebugMode)
        {
            Debug.Log("AssetsHotFixManager:CheckExtractResource ok");
            on_ok();
            return;   //文件已经解压过了，自己可添加检查文件列表逻辑
        }
        StartCoroutine(OnExtractResource(on_ok));    //启动释放协成 
    }

    public IEnumerator OnExtractResource(Action on_ok) {
        string dataPath = AppConfig.DataPath;  //数据目录
        string resPath = Util.AppContentPath(); //游戏包资源目录

        if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
        Directory.CreateDirectory(dataPath);

        string infile = resPath + "files.txt";
        string outfile = dataPath + "files.txt";
        if (File.Exists(outfile)) File.Delete(outfile);

        string message = "正在解包文件:>files.txt";
        Debug.Log(infile);
        Debug.Log(outfile);
        if (Application.platform == RuntimePlatform.Android) {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone) {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        } else File.Copy(infile, outfile, true);
        yield return new WaitForEndOfFrame();

        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        foreach (var file in files) {
            string[] fs = file.Split('|');
            infile = resPath + fs[0];  //
            outfile = dataPath + fs[0];

            message = "正在解包文件:>" + fs[0];
            Debug.Log("正在解包文件:>" + infile);
            // facade.SendMessageCommand(NotiData.UPDATE_MESSAGE, message);

            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android) {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone) {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            } else {
                if (File.Exists(outfile)) {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return new WaitForEndOfFrame();
        }
        message = "解包完成!!!";
        // facade.SendMessageCommand(NotiData.UPDATE_MESSAGE, message);
        yield return waitForSeconds;
        on_ok();
    }

    public void UpdateResource(Action<float,string> on_update, Action<string> on_ok) 
    {
        if (AppConfig.UpdateMode) 
        {
            StartCoroutine(OnUpdateResource(on_update, on_ok));
        }
        else
        {
            on_ok("");
        }
    }

    IEnumerator OnUpdateResource(Action<float,string> on_update, Action<string> on_ok) 
    {
        Debug.Log("OnUpdateResource() AppConfig.UpdateMode:"+ AppConfig.UpdateMode.ToString());
        string dataPath = AppConfig.DataPath;  //数据目录
        string url = AppConfig.WebUrl;
        string message = string.Empty;
        string random = DateTime.Now.ToString("yyyymmddhhmmss");
        string listUrl = url + "files.txt?v=" + random;
        Debug.LogWarning("OnUpdateResource() LoadUpdate---->>>" + listUrl);
        //TODO:不要直接返回中文，否则处理不了多语言版本，应该改成读取外部配置文件的
        on_update(0.01f, "下载最新资源列表文件...");
        WWW www = new WWW(listUrl); yield return www;
        if (www.error != null) {
            on_ok("下载最新资源列表文件失败!");
            yield break;
        }
        Debug.Log("is data path exist : "+Directory.Exists(dataPath)+" small:"+Directory.Exists(dataPath.Trim()));
        if (!Directory.Exists(dataPath)) {
            Directory.CreateDirectory(dataPath);
        }
        on_update(0.1f, "更新资源列表文件...");
        File.WriteAllBytes(dataPath + "files.txt", www.bytes);
        string filesText = www.text;
        string[] files = filesText.Split('\n');
        on_update(0.15f, "开始下载最新的资源文件...");
        float percent = 0.15f;
        for (int i = 0; i < files.Length; i++) {
            if (string.IsNullOrEmpty(files[i])) continue;
            string[] keyValue = files[i].Split('|');
            string f = keyValue[0];
            on_update(0.15f+0.85f*((i+1)/files.Length), "下载文件:"+f);
            string localfile = (dataPath + f).Trim();
            string path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            string fileUrl = url + f + "?v=" + random;
            bool canUpdate = !File.Exists(localfile);
            if (!canUpdate) {
                string remoteMd5 = keyValue[1].Trim();
                string localMd5 = Util.md5file(localfile);
                canUpdate = !remoteMd5.Equals(localMd5);
                if (canUpdate) File.Delete(localfile);
            }
            if (canUpdate) {   //本地缺少文件
                Debug.Log(fileUrl);
                //这里都是资源文件，用线程下载
                BeginDownload(fileUrl, localfile);
                while (!(IsDownOK(localfile))) { yield return new WaitForEndOfFrame(); }
            }
        }
        yield return new WaitForEndOfFrame();

        message = "更新完成!!";
        Debug.Log(message);
        on_ok("");
    }

    // void OnUpdateFailed(string file) 
    // {
    //     string message = "更新失败!>" + file;
    //     Debug.Log(message);
    //     // facade.SendMessageCommand(NotiData.UPDATE_MESSAGE, message);
    // }

    bool IsDownOK(string file) 
    {
        return downloadFiles.Contains(file);
    }

    void BeginDownload(string url, string file) 
    {     //线程下载
        object[] param = new object[2] { url, file };

        ThreadEvent ev = new ThreadEvent();
        ev.Key = NotiData.UPDATE_DOWNLOAD;
        ev.evParams.AddRange(param);
        ThreadManager.Instance.AddEvent(ev, OnThreadCompleted);   //线程下载
    }

    void OnThreadCompleted(NotiData data) 
    {
        Debug.Log("OnThreadCompleted "+data.evName + " data.evParam.ToString():"+data.evParam.ToString());
        switch (data.evName) 
        {
            case NotiData.UPDATE_EXTRACT:  //解压一个完成
            //
            break;
            case NotiData.UPDATE_DOWNLOAD: //下载一个完成
            downloadFiles.Add(data.evParam.ToString());
            break;
        }
    }

}
}