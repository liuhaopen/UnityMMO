
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

public class LogInfo
{
    public string message = null;
    public LogType type = 0;
    public LogInfo(string message, LogType type)
    {
        this.message = message;
        this.type = type;
    }
}
namespace XLuaFramework
{
    public class LogHandler : MonoBehaviour
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    //    ConsoleWindows.ConsoleWindow console = new ConsoleWindows.ConsoleWindow();
    //    ConsoleWindows.ConsoleInput input = new ConsoleWindows.ConsoleInput();
#endif
        private Thread thread;
        static readonly object m_lockObject = new object();
        static readonly object m_lockObject2 = new object();
        static Queue<LogInfo> log_list = new Queue<LogInfo>();

        //日志输出路径
        private string output_path = null;
        void Awake()
        {
            LogManager.RegisterLogCallback(delegate (string message, LogType type)
            {
                AddMessage(message, type);
            });

            string log_dir = Path.Combine(AppConfig.DataPath, "log");
            if (!Directory.Exists(log_dir))
                Directory.CreateDirectory(log_dir);

            if (Application.isEditor)
                output_path = Path.Combine(log_dir, "out_put.txt");
            else
                output_path = Path.Combine(log_dir, "out_put_uneditor.txt");
            Debug.Log("LogHandler.cs log_dir : " + log_dir+ "  Directory.Exists(log_dir)"+ Directory.Exists(log_dir).ToString());
            Debug.Log("LogHandler.cs output_path : " + output_path+ "  Directory.Exists(output_path)"+ Directory.Exists(output_path).ToString());
            //每次启动先删除旧的
            File.WriteAllText(output_path, "");
            // if (File.Exists(output_path))
            //     File.Delete(output_path);
            // if (!File.Exists(output_path))
                // File.Create(output_path);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            // console.Initialize();
            // console.SetTitle("game log");
#endif

            thread = new Thread(OnUpdateThread);

            Application.logMessageReceived += HandleLog;
        }
        // Use this for initialization
        void Start()
        {
            thread.Start();
        }

        public void AddMessage(string message, LogType type)
        {
            lock (m_lockObject2)
            {
                LogInfo li = new LogInfo(message, type);
                string day_str = System.DateTime.Now.Day.ToString()+"/";
                string hour_str = System.DateTime.Now.Hour.ToString()+":";
                string minute_str = System.DateTime.Now.Minute.ToString()+":";
                string second_str = System.DateTime.Now.Second.ToString()+":";
                li.message = day_str+hour_str+minute_str+second_str+li.message;
                if (li.type == LogType.Error || li.type == LogType.Exception)
                    li.message += " trace back : " + new System.Diagnostics.StackTrace().ToString();
                log_list.Enqueue(li);
            }
        }

        //日志回调
        void HandleLog(string message, string stackTrace, LogType type)
        {
            AddMessage(message, type);
        }
        //主线程update
        void Update()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            // input.Update();
#endif
        }

        //多线程update
        void OnUpdateThread()
        {
            while (true)
            {
                lock (m_lockObject)
                {
                    if (log_list.Count > 0)
                    {

                        LogInfo li = log_list.Dequeue();

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                        if (li.type == LogType.Warning)
                            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                        else if (li.type == LogType.Error || li.type == LogType.Exception)
                            System.Console.ForegroundColor = System.ConsoleColor.Red;
                        else
                            System.Console.ForegroundColor = System.ConsoleColor.White;

                        // if (System.Console.CursorLeft != 0)
                        //     input.ClearLine();

                        System.Console.WriteLine(li.message);
#endif

                        OnWriteFile(li.message);

                        //input.RedrawInputLine();
                    }
                }
                Thread.Sleep(1);
            }
        }

        void OnWriteFile(string message)
        {
            try
            {
                StreamWriter writer = new StreamWriter(output_path, true, System.Text.Encoding.UTF8);
                writer.WriteLine(message);
                writer.Close();
            }
            catch(Exception e)
            {
                Debug.Log("write game log error :" + e.Message);
            }
        }
        void OnDestroy()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            // console.Shutdown();
#endif
        }
    }
}
