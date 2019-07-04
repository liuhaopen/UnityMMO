


using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace XLuaFramework
{
    public class Pathtool
    {
        public static string CombinePath(string path1, string path2)
        {
            string[] paths1 = path1.Split('/');
            string[] paths2 = path2.Split('/');
            List<string> paths1_list = new List<string>();

            foreach (string value in paths1)
            {
                paths1_list.Add(value);
            }

            for (int i = 0; i < paths2.Length; i++)
            {
                if (paths2[i] == "..")
                {
                    paths1_list.RemoveAt(paths1_list.Count - 1);
                }
                else if (paths2[i] != ".")
                {
                    paths1_list.Add(paths2[i]);
                }
            }

            string out_path = "";
            for (int i = 0; i < paths1_list.Count; i++)
            {
                if (i == 0)
                {
                    out_path = paths1_list[0];
                }
                else
                {

                    if (out_path.EndsWith("/"))
                    {
                        out_path += paths1_list[i];
                    }
                    else
                    {
                        out_path += "/" + paths1_list[i];
                    }
                }
            }

            return out_path;
        }

        public static void CreatePath(string path)
        {
            string NewPath = path.Replace("\\", "/");

            string[] strs = NewPath.Split('/');
            string p = "";

            for (int i = 0; i < strs.Length; ++i)
            {
                p += strs[i];

                if (i != strs.Length - 1)
                {
                    p += "/";
                }

                if (!Path.HasExtension(p))
                {
                    if (!Directory.Exists(p))
                        Directory.CreateDirectory(p);
                }
            }
        }

        public static bool SaveDataToFile(string path, byte[] buffer)
        {
            // Debug.Log("path tool save data to file : "+path);
            if(path.IndexOf(AppConfig.AssetDir) != -1 && AppConfig.UpdateMode)
            {
                if(File.Exists(path))
                {
                    File.Delete(path);
                }

                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                }
            }

            if (!File.Exists(path) && path.IndexOf( AppConfig.AssetDir) == -1 || !AppConfig.UpdateMode)
            {
                CreatePath(path);
            }

            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
            catch (System.Exception ex)
            {
                LogManager.LogError("Can't create local resource" + path + " e:"+ex.Message);
                return false;
            }

            return true;
        }

        public static void DeleteToFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path); CreatePath(path);
            }
        }

        public static bool GetDataToFile(string path, out byte[] buffer)
        {
            buffer = null;
            if (File.Exists(path))
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.Open);
                    int nLength = (int)fs.Length;
                    byte[] buffs = new byte[nLength];
                    fs.Read(buffs, 0, nLength);
                    fs.Close();
                    buffer = buffs;
                    return true;
                }
                catch (System.Exception ex)
                {
                    LogManager.LogError("Can't create local resource" + path + " msg:"+ex.Message);
                    return false;
                }
            }
            return false;
        }
    }
}