using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityMMO {

public class SceneInfoExporterForServer : Editor
{
    const string SavePath = "Assets/ConfigServer/";

    [MenuItem("Terrain/Export Scene Info For Server")]
    private static void Export()
    {
        SceneInfoForServer export_info = new SceneInfoForServer();
        export_info.scene_id = 1001;
        export_info.scene_name = "scene_name";
        export_info.door_list = new List<DoorInfo>();
        DoorInfo door = new DoorInfo();
        door.door_id = 1;
        door.pos_x = 1.1f;
        door.pos_y = 2.2f;
        door.pos_z = 3.123456f;
        door.target_scene_id = 1002;
        door.target_x = 11.1f;
        door.target_y = 22.2f;
        door.target_z = 33.123456f;
        export_info.door_list.Add(door);
        export_info.door_list.Add(door);

        export_info.npc_list = new List<NPCInfo>();
        NPCInfo npc = new NPCInfo();
        npc.npc_id = 1;
        npc.pos_x = 1.1f;
        npc.pos_y = 2.2f;
        npc.pos_z = 3.123456f;
        export_info.npc_list.Add(npc);

        export_info.monster_list = new List<MonsterInfo>();
        MonsterInfo monster = new MonsterInfo();
        monster.monster_id = 1;
        monster.pos_x = 1.1f;
        monster.pos_y = 2.2f;
        monster.pos_z = 3.123456f;
        export_info.monster_list.Add(monster);

        // export_info.test_dic = new Dictionary<int, string>();
        // export_info.test_dic.Add(1, "one");
        // export_info.test_dic.Add(3, "three");
        // export_info.test_dic.Add(6, "haha");
        
        string content = LuaUtility.ToLua(export_info);
        Debug.Log("SceneInfoExporterForServer:Export content : "+content);
    }

}
}