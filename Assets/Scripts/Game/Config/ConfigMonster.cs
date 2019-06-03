using System.Collections.Generic;

public class ConfigMonster
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
    static ConfigMonster instance;
    public static ConfigMonster GetInstance()
    {
        if (instance != null)
            return instance;
        instance = new ConfigMonster();
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

    public float GetMoveSpeed(long typeID)
    {
        if (Items.ContainsKey(typeID))
            return Items[typeID].MoveSpeed;
        return 500;
    }

    public int GetBodyResID(long typeID)
    {
        if (Items.ContainsKey(typeID))
            return Items[typeID].BodyResID;
        return 0;
    }
    
    private ConfigMonster()
    {
        //Cat_TODO: 改成用excel导出自动生成前后端数据
        Items = new Dictionary<long, Item>{
            {2000, new Item{
                TypeID = 2000, Name = "小灰狼", BodyResID=2000, WeaponResID=1, MaxHP=1000, 
                 MoveSpeed=500, 
            }},
            {2001, new Item{
                TypeID = 2001, Name = "大灰", BodyResID=2001, WeaponResID=1, MaxHP=1000, 
                 MoveSpeed=500, 
            }},
            {2002, new Item{
                TypeID = 2002, Name = "金钱猫", BodyResID=2002, WeaponResID=1, MaxHP=1000, 
                 MoveSpeed=500, 
            }},
            {2003, new Item{
                TypeID = 2003, Name = "红袍妖女", BodyResID=2003, WeaponResID=1, MaxHP=1000, 
                 MoveSpeed=500, 
            }},
            {2004, new Item{
                TypeID = 2004, Name = "断头鬼", BodyResID=2004, WeaponResID=1, MaxHP=1000, 
                 MoveSpeed=500, 
            }},
            {2005, new Item{
                TypeID = 2005, Name = "唱大戏的", BodyResID=2005, WeaponResID=1, MaxHP=1000, 
                 MoveSpeed=500, 
            }}
        };
    }
}