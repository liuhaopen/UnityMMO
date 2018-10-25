using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;
using UnityEngine;
using System.IO;
#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

namespace XLuaFramework
{
    [Hotfix]
    [LuaCallCSharp]
    internal sealed class DefineConstantsCookieHandler
    {
        public const string COMMON_COOKIE_FILE = "common.cfg";
        public const string COOKIE_FILE_PATH = "cookies/";
        public const string COOKIE_VAR = "Cookies";
        public const string COOKIE_COMMON = "common";
        public const int CFGFILE_SEED = 0x001FF003;
    }

    [Hotfix]
    [LuaCallCSharp]
    public class CookiesManager
    {
        static CookiesManager instance;
        public static CookiesManager GetInstance()
        {
            if (instance != null)
                return instance;
            instance = new CookiesManager();
            instance.Init();
            return instance;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        internal static int CheckNewType(IntPtr L)
        {
            if (!LuaAPI.lua_istable(L, 1))
            {
                Debug.Log("Need Table Here!");
                return 0;
            }

            // key值只能是数字或者字符串
            if (LuaAPI.lua_isnumber(L, 2) && LuaAPI.lua_isstring(L, 2))
            {
                Debug.Log("Need Number or String Here!");
                return 0;
            }

            // value值只能是数字、字符串、非循环table
            if (LuaAPI.lua_isnumber(L, 3) && LuaAPI.lua_isstring(L, 3) && !LuaAPI.lua_istable(L, 3) && !LuaAPI.lua_isboolean(L, 3))
            {
                Debug.Log("This Type Cannot Add to Cookies!Need[Table, Number, String]!");
                return 0;
            }

            // 如果是table，检查是否有循环结构或者是其他类型
            if (LuaAPI.lua_istable(L, 3))
            {
                int cycle_time = 0;
                if (!CheckCircleAndSetMeta(L, ref cycle_time) || !SetMetaTable(L, 3))
                {
                    Debug.Log("This Type Cannot Add to Cookies!Need[Table, Number, String]!");
                    LuaAPI.lua_pop(L, 1);
                    return 0;
                }
            }

            LuaAPI.lua_rawset(L, 1);

            return 0;
        }

        internal static bool SetMetaTable(IntPtr L, int table_index)
        {
            LuaAPI.xlua_getglobal(L, DefineConstantsCookieHandler.COOKIE_VAR);
            if (LuaAPI.lua_getmetatable(L, -1) == 0)
            {
                LuaAPI.lua_pop(L, 1);
                Debug.Log("Need Table to SetMetatable!");

                return false;
            }

            LuaAPI.lua_setmetatable(L, table_index);
            LuaAPI.lua_pop(L, 1);
            return true;

        }

        internal static bool CheckCircleAndSetMeta(IntPtr L, ref int cycle_time)
        {
            int now_top = LuaAPI.lua_gettop(L); // 堆栈操作保护
            int nIndex = LuaAPI.lua_gettop(L); // 取table索引值

            if (!LuaAPI.lua_istable(L, nIndex))
            {
                LuaAPI.lua_settop(L, now_top);
                Debug.Log("CheckCircleAndSetMeta Need Table!");
                return false;
            }

            LuaAPI.lua_pushnil(L); // nil入栈作为初始key
            while (0 != LuaAPI.lua_next(L, nIndex))
            {
                // 现在栈顶（-1）是value，-2位置是对应的key 
                // 这里可以判断key是什么并且对value进行各种处理

                // key值只能是数字或者字符串
                if (LuaAPI.lua_isnumber(L, -2) && LuaAPI.lua_isstring(L, -2))
                {
                    LuaAPI.lua_settop(L, now_top);
                    Debug.Log("Need Number or String Here!");
                    return false;
                }

                if (LuaAPI.lua_isnumber(L, -1) && LuaAPI.lua_isstring(L, -1) && !LuaAPI.lua_istable(L, -1) && !LuaAPI.lua_isboolean(L, -1))
                {
                    LuaAPI.lua_settop(L, now_top);
                    Debug.Log("Table Contains Unsupported Value! Expected [Number, String, Table].");
                    return false;
                }

                if (LuaAPI.lua_istable(L, -1))
                {
                    cycle_time += 1;
                    if (cycle_time > 50)
                    {
                        Debug.Log("Table Has Cycle!");
                        LuaAPI.lua_settop(L, now_top);
                        return false;
                    }

                    if (!CheckCircleAndSetMeta(L, ref cycle_time) || !SetMetaTable(L, nIndex))
                    {
                        LuaAPI.lua_settop(L, now_top);
                        return false;
                    }

                }
                LuaAPI.lua_pop(L, 1);
            }

            LuaAPI.lua_settop(L, now_top);
            return true;
        }

        public CookiesManager()
        {
            m_cookie_path = DefineConstantsCookieHandler.COOKIE_FILE_PATH;

        }

        public void Dispose()
        {

        }

        public bool Init()
        {
            
            m_lua_state = XLuaManager.Instance.GetLuaEnv().L;

            // 给lua塞一个空表进去
            LuaAPI.lua_newtable(m_lua_state);
            int table_index = LuaAPI.lua_gettop(m_lua_state);

            // LuaAPI.lua_newtable(m_lua_state);
            // LuaAPI.lua_pushstring(m_lua_state, "__newindex");
            // LuaAPI.lua_pushstdcallcfunction(m_lua_state, CheckNewType);
            // LuaAPI.xlua_psettable(m_lua_state, -3);
            // LuaAPI.lua_setmetatable(m_lua_state, table_index);
            LuaAPI.xlua_setglobal(m_lua_state, DefineConstantsCookieHandler.COOKIE_VAR);

            return true;
        }

        public bool LoadCookie(string table_name)
        {
            return LoadCookie(table_name, "");
        }

        public bool LoadCookie(string table_name, string ext_path)
        {
            if (!ReadCookie(table_name, ext_path))
            {
                // 读取失败，创建一个空表
                LuaAPI.xlua_getglobal(m_lua_state, DefineConstantsCookieHandler.COOKIE_VAR);
                LuaAPI.lua_pushstring(m_lua_state, table_name);
                LuaAPI.lua_newtable(m_lua_state);
                LuaAPI.xlua_psettable(m_lua_state, -3);

                LuaAPI.lua_pop(m_lua_state, 1);
            }
            return true;
        }

        public bool LoadFileToLua(string table_name)
        {
            return LoadFileToLua(table_name, "");
        }

        public bool LoadFileToLua(string table_name, string ext_path)
        {
            if (!ReadFileToLua(table_name, ext_path))
            {
                // 读取失败，创建一个空表
                LuaAPI.xlua_getglobal(m_lua_state, DefineConstantsCookieHandler.COOKIE_VAR);
                LuaAPI.lua_pushstring(m_lua_state, table_name);
                LuaAPI.lua_newtable(m_lua_state);
                LuaAPI.xlua_psettable(m_lua_state, -3); 


                LuaAPI.lua_pop(m_lua_state, 1);
            }
            return true;
        }

        public void SetCookiePath(string cookie_path)
        {
            m_cookie_path = cookie_path;
        }

        public string GetCookiePath()
        {
            return m_cookie_path;
        }

        public void WriteCookieAll()
        {
            WriteCookieAll("");
        }

        public void WriteCookieAll(string ext_path)
        {
            List<string> table_list = new List<string>();

            LuaAPI.xlua_getglobal(m_lua_state, DefineConstantsCookieHandler.COOKIE_VAR);

            int nIndex = LuaAPI.lua_gettop(m_lua_state);

            LuaAPI.lua_pushnil(m_lua_state); // nil入栈作为初始key
            while (0 != LuaAPI.lua_next(m_lua_state, nIndex))
            {
                LuaAPI.lua_pushvalue(m_lua_state, -2);
                if (LuaAPI.lua_isstring(m_lua_state, -1))
                {
                    table_list.Add(LuaAPI.lua_tostring(m_lua_state, -1));
                }
                LuaAPI.lua_pop(m_lua_state, 2);
            }

            LuaAPI.lua_pop(m_lua_state, 1);

            List<string>.Enumerator itr = table_list.GetEnumerator();
            while (itr.MoveNext())
            {
                Debug.LogWarning("WriteCookieAll :" + itr.ToString());
                WriteCookie(itr.ToString(), itr.ToString(), ext_path);
            }
        }

        public bool WriteCookie(string file_name, string table_list)
        {
            return WriteCookie(file_name, table_list, "");
        }

        public bool WriteCookie(string file_name, string table_list, string ext_path)
        {
            ByteBuffer write_file = new ByteBuffer();
            // Debug.Log("WriteCookie file_name : "+file_name +" table_list :"+table_list+" ext_path:"+ext_path);
            string table_list_str = (table_list != "") ? table_list : DefineConstantsCookieHandler.COOKIE_COMMON;
            string parent_str;


            string[] r_table_list = table_list_str.Split('.');

            // 写入空表
            parent_str = DefineConstantsCookieHandler.COOKIE_VAR;

            // out_buff_len = (uint)sprintf(out_buff, "%s = %s or {}\n", parent_str.c_str(), parent_str.c_str());
            string out_buff = string.Format("{0} = {1} or {{}}\n", parent_str, parent_str);
            write_file.WriteText(out_buff);

            LuaAPI.xlua_getglobal(m_lua_state, parent_str);

            int deep_size = 1;

            foreach (var value in r_table_list)
            {
                //string encryptValue = DESHelper.DESEncrypt(value);
                out_buff = string.Format("{0}[\"{1}\"]", parent_str, value);
                parent_str = out_buff;

                //out_buff_len = (uint)sprintf(out_buff, "%s = %s or {}\n", parent_str.c_str(), parent_str.c_str());
                out_buff = string.Format("{0} = {1} or {{}}\n", parent_str, parent_str);
                write_file.WriteText(out_buff);

                if (!LuaAPI.lua_istable(m_lua_state, -1))
                {
                    LuaAPI.lua_pop(m_lua_state, deep_size);
                    return false;
                }

                LuaAPI.lua_pushstring(m_lua_state, value);
                LuaAPI.xlua_pgettable(m_lua_state, -2);

                deep_size++;
            }

            // 写入表中的数据
            bool rlt = SerializeTableType(ref write_file, parent_str);
            LuaAPI.lua_pop(m_lua_state, 1);
            // Debug.Log("write rlt:"+rlt.ToString());
            if (rlt)
            {
                SaveToFile(file_name, write_file, ext_path);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool ReadFileToLua(string table_name)
        {
            return ReadFileToLua(table_name, "");
        }

        protected bool ReadFileToLua(string table_name, string ext_path)
        {
            string file_name_str = table_name;
            string file_path = (ext_path == "" || ext_path == null) ? m_cookie_path : ext_path;

            string write_path = AppConfig.DataPath;
            file_name_str = file_path + file_name_str + ".cfg";
            file_name_str = file_name_str.Replace("\\", "/");

            write_path = write_path + file_name_str;
            // Debug.Log("Read Cookie Path :" + file_name_str);

            if (!File.Exists(write_path))
            {
                Debug.Log("Read Cookie Failed! : " + file_name_str);
                return false;
            }

            string content = File.ReadAllText(write_path, Encoding.UTF8);

            if (string.IsNullOrEmpty(content))
            {
                Debug.Log("Read Cookie Failed! : " + file_name_str);
                return false;
            }
            if (content.Substring(content.Length - 1) == "#")
            {
                byte[] byteArray = Convert.FromBase64String(content.Remove(content.Length - 1));
                XLuaManager.Instance.SafeDoString(Encoding.UTF8.GetString(byteArray), write_path);
            }
            else
            {
                XLuaManager.Instance.LoadOutsideFile(write_path);
            }
           
            return true;
        }

        protected bool ReadCookie(string table_name)
        {
            return ReadCookie(table_name, "");
        }

        protected bool ReadCookie(string table_name, string ext_info)
        {
            string file_name_str = table_name;
            string file_path = (ext_info == "" || ext_info == null) ? m_cookie_path : ext_info;

            string write_path = AppConfig.DataPath;
            file_name_str = file_path + file_name_str + ".cfg";
            file_name_str = file_name_str.Replace( "\\", "/");

            write_path = write_path + file_name_str;
            // Debug.Log("Read Cookie Path :" +  file_name_str+ " write_path:"+write_path);

            byte[] file_data = new byte[1024*100];
            Pathtool.GetDataToFile(write_path, out file_data);
            if (file_data == null || file_data.Length == 0)
            {
                Debug.Log("Read Cookie Failed! : " + file_name_str);
                return false;
            }

            //DataEncrypt.DecryptHashTableData(out file_data, (uint)file_data.Length, DefineConstantsCookieHandler.CFGFILE_SEED);
            //int rlt = Ogre.CCLuaEngine.defaultEngine().executeString((string)file_data.contents(), (uint)file_data.Length, file_name_str);

            //[修改] 解密后使用DoString执行lua代码
            //加密过的文件最后字符为#，删除#再进行解密
            if (file_data[file_data.Length - 1] == 35)
            {
                string str = Encoding.UTF8.GetString(file_data);
                byte[] byteArray = Convert.FromBase64String(str.Substring(0, str.Length - 1));
                string show_str = System.Text.Encoding.Default.GetString(byteArray);
                // Debug.Log("read cookie str : "+show_str);
                XLuaManager.Instance.SafeDoString(Encoding.UTF8.GetString(byteArray), write_path);
            }
            else
            {
                // Debug.Log("load outside file : "+write_path);
                XLuaManager.Instance.LoadOutsideFile(write_path);
            }

            return true;
        }

        protected bool SaveToFile(string file_name, ByteBuffer write_buff)
        {
            return SaveToFile(file_name, write_buff, "");
        }

        protected bool SaveToFile(string file_name, ByteBuffer write_buff, string ext_path)
        {
            string file_name_str = file_name;
            string file_path_str = (ext_path == "" || ext_path == null) ? m_cookie_path : ext_path;

            string write_path = AppConfig.DataPath;
            write_path = write_path + file_path_str;
            string full_path = write_path + file_name_str + ".cfg";
            full_path = full_path.Replace("\\", "/");
            //DataEncrypt.EncryptHashTableData(out write_buff, (uint)write_buff.size(), DefineConstantsCookieHandler.CFGFILE_SEED);

            if (File.Exists(full_path))
                File.Delete(full_path);

            //添加一个#作为标识，这里需要优化
            string str = Convert.ToBase64String(write_buff.ToBytes());
            str = str.Insert(str.Length, "#"); 
            bool ret = Pathtool.SaveDataToFile(full_path, Encoding.UTF8.GetBytes(str));
            // Debug.Log("Write Cookie Path :" + full_path + " str:"+str+" ret:"+ret.ToString());
            write_buff.Close();
            return ret;
        }

        protected int SerializeBasicType(ref string out_buff)
        {
            if (LuaAPI.lua_type(m_lua_state, -1) == LuaTypes.LUA_TSTRING)
            {
                out_buff = string.Format("\"{0}\"", LuaAPI.lua_tostring(m_lua_state, -1));
                return out_buff.Length;
                //return sprintf(out_buff, "\"%s\"", LuaAPI.lua_tostring(m_lua_state, -1));
            }
            else if (LuaAPI.lua_type(m_lua_state, -1) == LuaTypes.LUA_TNUMBER)
            {
                out_buff = string.Format("{0}", LuaAPI.lua_tonumber(m_lua_state, -1));
                return out_buff.Length;

                //return sprintf(out_buff, "%lf", LuaAPI.lua_tonumber(m_lua_state, -1));
            }
            else if (LuaAPI.lua_type(m_lua_state, -1) == LuaTypes.LUA_TBOOLEAN)
            {
                out_buff = string.Format("{0}", LuaAPI.lua_toboolean(m_lua_state, -1) ? "true" : "false");
                return out_buff.Length;
               // return sprintf(out_buff, "%s", LuaAPI.lua_toboolean(m_lua_state, -1) ? "true" : "false");
            }
            else
            {
                Debug.Log("Need Basic Type Here!");
                return -1;
            }
        }

        protected bool SerializeTableType(ref ByteBuffer write_file, string parent_str)
        {
            int now_top = LuaAPI.lua_gettop(m_lua_state); // 堆栈操作保护

            string field_str_buff = new string(new char[1024]);
            string write_buff = new string(new char[1024]);

            int nIndex = LuaAPI.lua_gettop(m_lua_state); // 取table索引值

            if (!LuaAPI.lua_istable(m_lua_state, nIndex))
            {
                LuaAPI.lua_settop(m_lua_state, now_top);
                Debug.Log("Need Table Here!");
                return false;
            }

            string my_name_str;
            LuaAPI.lua_pushnil(m_lua_state); // nil入栈作为初始key
            while (0 != LuaAPI.lua_next(m_lua_state, nIndex))
            {
                // 现在栈顶（-1）是value，-2位置是对应的key
                // 这里可以判断key是什么并且对value进行各种处理

                // 写key值
                LuaAPI.lua_pushvalue(m_lua_state, -2);
                if (SerializeBasicType(ref field_str_buff) != 0)
                {
                    write_buff = string.Format("{0}[{1}]", parent_str, field_str_buff);
                    my_name_str = write_buff;
                }
                else
                {
                    LuaAPI.lua_settop(m_lua_state, now_top);
                    return false;
                }
                LuaAPI.lua_pop(m_lua_state, 1);

                if (LuaAPI.lua_isnumber(m_lua_state, -1)|| LuaAPI.lua_isstring(m_lua_state, -1) || LuaAPI.lua_isboolean(m_lua_state, -1))
                {
                    if (SerializeBasicType(ref field_str_buff) != 0)
                    {
                        write_buff = string.Format("{0} = {1}\n", my_name_str, field_str_buff);
                        write_file.WriteText(write_buff);
                    }
                    else
                    {
                        LuaAPI.lua_settop(m_lua_state, now_top);
                        return false;
                    }
                }
                else if (LuaAPI.lua_istable(m_lua_state, -1))
                {
                    write_buff = string.Format("{0} = {1} or {{}}\n", my_name_str, my_name_str);
                    write_file.WriteText(write_buff);

                    if (!SerializeTableType(ref write_file, my_name_str))
                    {
                        LuaAPI.lua_settop(m_lua_state, now_top);
                        return false;
                    }

                }
                else
                {
                    Debug.Log("This Type Cannot Write to Cookies!Need[Table, Number, String]!");
                }

                LuaAPI.lua_pop(m_lua_state, 1); // 弹出value，让key留在栈顶
            }

            LuaAPI.lua_settop(m_lua_state, now_top);

            return true;
        }

        private IntPtr m_lua_state;
        private string m_cookie_path;
    }
}