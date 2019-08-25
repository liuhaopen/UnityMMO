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
        // [DataMember]
        // public string LoginServerIP;
        // [DataMember]
        // public string LoginServerPort;
        // [DataMember]
        // public string GameServerIP;
        // [DataMember]
        // public string GameServerPort;
    }

    public class ConfigGame
    {
        public static string FilePath = Path.Combine(AppConfig.LuaAssetsDir, "Config/ConfigGame.json");
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
        public bool Load()
        {
            string json = File.ReadAllText(FilePath);
            Debug.Log("FilePath : "+FilePath+" json:"+json);
            Data = JsonUtility.FromJson<ConfigGameData>(json);
            Debug.Log("Data : "+Data.FileServerURL);
            isLoaded = true;
            return true;
        }

        public void Save()
        {
            if (isLoaded)
            {
                string json = JsonUtility.ToJson(Data, true);
                Debug.Log("save : "+json);
                File.WriteAllText(FilePath, json);
            }
            else
            {
                Debug.LogError("has not loaded!");
            }
        }
    }
}
