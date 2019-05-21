using System.Collections.Generic;

public class ConfigNPC
{
    public class Item
    {
        public long TypeID;
        public string Name;
        public int BodyResID;
        public int WeaponResID;
        public float MoveSpeed;
        public long MaxHP;
    }

    Dictionary<long, Item> Items;
    static ConfigNPC instance;
    public static ConfigNPC GetInstance()
    {
        if (instance != null)
            return instance;
        instance = new ConfigNPC();
        return instance;
    }

    public Item GetCfg(long typeID)
    {
        if (Items.ContainsKey(typeID))
            return Items[typeID];
        return null;
    }

    public string GetName(long typeID)
    {
        if (Items.ContainsKey(typeID))
            return Items[typeID].Name;
        return "Unknow";
    }

    public int GetBodyResID(long typeID)
    {
        if (Items.ContainsKey(typeID))
            return Items[typeID].BodyResID;
        return 0;
    }
    
    private ConfigNPC()
    {
        //Cat_TODO: 改成用excel导出自动生成前后端数据
        Items = new Dictionary<long, Item>{
            {3000, new Item{
                TypeID = 3000, Name = "流浪剑客", BodyResID=3000 
            }},
            {3001, new Item{
                TypeID = 3001, Name = "风车男孩", BodyResID=3001 
            }},
            {3002, new Item{
                TypeID = 3002, Name = "猫女", BodyResID=3002 
            }},
            // {3003, new Item{
            //     TypeID = 3003, Name = "金钱猫", BodyResID=3003, 
             
            // }},
            // {3003, new Item{
            //     TypeID = 3003, Name = "红袍妖女", BodyResID=3003, 
             
            // }},
            // {3004, new Item{
            //     TypeID = 3004, Name = "断头鬼", BodyResID=3004, 
             
            // }},
            // {3005, new Item{
            //     TypeID = 3005, Name = "唱大戏的", BodyResID=3005, 
             
            // }}
        };
    }
}