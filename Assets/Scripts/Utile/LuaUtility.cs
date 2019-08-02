using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

public class LuaUtility
{
    static string TabStr = "\t";
    public static string ToLua(object obj, int level=0)
    {
        string content = "";
        Type property_type = obj.GetType();
        Type list_type = typeof(IList);
        Type dic_type = typeof(IDictionary);
        
        if (property_type.IsPrimitive)
        {
            content += obj.ToString();
        }
        else if (property_type == typeof(string))
        {
            content += "\"" + obj.ToString() + "\"";
        }
        else if (list_type.IsAssignableFrom(property_type))
        {
            content += "{";
            IEnumerable list_info = obj as IEnumerable;
            foreach (var list_item in list_info)
            {
                content += ToLua(list_item, level+1) + ",\n";
            }
            content.Substring(content.Length-1);
            content += "}";
        }
        else if (dic_type.IsAssignableFrom(property_type))
        {
            content += "{\n";
            IDictionary dic_info = obj as IDictionary;
            foreach (var item in dic_info)
            {
                var itemKey = item.GetType().GetProperty("Key").GetValue(item, null);
                var itemValue = item.GetType().GetProperty("Value").GetValue(item, null);
                // GetContentFromDic(item);
                content += "["+itemKey.ToString()+"]" + " = " +ToLua(itemValue, level+1) + ", ";
            }
            content.Substring(content.Length-1);
            content += "}";
        }
        else
        {
            content += "{\n";
            Type type = obj.GetType();
            MemberInfo[] members = type.GetMembers();
            string tab_str = GetStrMutiple(TabStr, level);
            // UnityEngine.Debug.Log("members.Length : "+members.Length.ToString());
            if (members != null && members.Length > 0)
            {
                foreach (MemberInfo p in members)
                {
                    object[] objAttrs = p.GetCustomAttributes(typeof(DataMemberAttribute), true);
                    // UnityEngine.Debug.Log("objAttrs.Length : "+objAttrs.Length.ToString()+ " field!=null:"+(field!=null).ToString());
                    if (objAttrs != null && objAttrs.Length > 0)
                    {
                        object obj_value = null;
                        FieldInfo field = p as FieldInfo;
                        if(field!=null)
                        {
                            obj_value = field.GetValue(obj);
                        }
                        else
                        {
                            PropertyInfo pro = p as PropertyInfo;
                            if (pro != null)
                                obj_value = pro.GetValue(obj);
                        }
                        if (obj_value != null)
                            content += tab_str + p.Name + " = " + ToLua(obj_value, level+1) + ",\n";
                    }
                };
            }
            content += "}";
        }
        return content;
    }

    public static string GetStrMutiple(string str, int num)
    {
        string result = "";
        for (int i = 0; i < num; i++)
        {
            result += str;
        }
        return result;
    }

    // public static T FromLua<T>(string json)
    // {

    // }
}
