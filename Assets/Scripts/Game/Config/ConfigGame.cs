using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using XLuaFramework;

namespace UnityMMO
{
    [DataContract]
    public class ConfigGameData
    {
        [DataMember]
        public string FileServerURL;
    }

    public class ConfigGame
    {
        public static string FilePath = string.Empty;
        public ConfigGameData Data;
        private static ConfigGame instance;
        private bool isLoaded;

        public static ConfigGame GetInstance()
        {
            if (instance != null)
                return instance;
            instance = new ConfigGame();
            return instance;
        }

        private ConfigGame()
        {
            FilePath = (AppConfig.DataPath+"../config_"+(AppConfig.AppName).ToLower()+".json").Trim();
            Debug.Log("init config game : "+FilePath);
        }

        public bool Load()
        {
            isLoaded = true;
            Debug.Log("Config Game FilePath : "+FilePath+" isexist:"+File.Exists(FilePath));
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                Data = JsonUtility.FromJson<ConfigGameData>(json);
                Debug.Log("Data : "+Data.FileServerURL+"  json:"+json);
            }
            else
            {
                Data = new ConfigGameData{
                    FileServerURL = "http://192.168.43.130"
                };
                Save();
            }
            return isLoaded;
        }

        public void Save()
        {
            if (isLoaded)
            {
                string json = JsonUtility.ToJson(Data, true);
                Debug.Log("Config Game Save : "+json);
                File.WriteAllText(FilePath, json);
            }
            else
            {
                Debug.LogError("has not loaded!");
            }
        }
    }
}
