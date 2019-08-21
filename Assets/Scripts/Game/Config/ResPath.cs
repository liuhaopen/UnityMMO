using UnityEngine;

namespace UnityMMO
{
    public static class ResPath
    {
        public const string RoleResPath = "Assets/AssetBundleRes/role";
        public const string UIResPath = "Assets/AssetBundleRes/ui";
        public const string MonsterResPath = "Assets/AssetBundleRes/monster";
        public const string NPCResPath = "Assets/AssetBundleRes/npc";

        public static string GetRoleCareerResPath(int career)
        {
            return RoleResPath+"/career_"+career;
        }

        public static string GetMonsterResPath(long typeID)
        {
            return MonsterResPath+"/monster_"+typeID;
        }

        public static string GetMonsterBodyResPath(long typeID)
        {
            string resPath = MonsterResPath+"/monster_"+typeID;
            int bodyResID = ConfigMonster.GetInstance().GetBodyResID(typeID);
            if (bodyResID == 0)
            {
                Debug.LogWarning("ResPath:GetMonsterBodyResPath monster body res id 0, typeID:"+typeID);
                return string.Empty;
            }
            string bodyPath = resPath+"/model_clothe_"+bodyResID+".prefab";
            return bodyPath;
        }

        public static string GetNPCLooksPath(long typeID)
        {
            var bodyResID = ConfigNPC.GetInstance().GetBodyResID(typeID);
            if (bodyResID == 0)
            {
                Debug.LogError("npc body res id 0, typeID:"+typeID);
                return string.Empty;
            }
            return NPCResPath+"/npc_"+typeID+"/model_clothe_"+bodyResID+".prefab";
        }

        public static string GetRoleSkillResPath(int skillID)
        {
            return RoleResPath+"/timeline/skill_"+skillID+".playable";
        }

        public static string GetMonsterSkillResPath(int skillID)
        {
            return MonsterResPath+"/timeline/skill_"+skillID+".playable";
        }

        public static string GetRoleJumpResPath(int career, int jumpID)
        {
            return RoleResPath+"/timeline/jump_career"+career+"_"+jumpID+".playable";
        }

        public static string GetBloodResPath(Nameboard.ColorStyle style)
        {
            switch (style)
            {
                case Nameboard.ColorStyle.Red:
                {
                    return UIResPath+"/common/com_blood_red.png";
                }
                case Nameboard.ColorStyle.Green:
                {
                    return UIResPath+"/common/com_blood_green.png";
                }
                case Nameboard.ColorStyle.Blue:
                {
                    return UIResPath+"/common/com_blood_blue.png";
                }
                default:
                    return "";
            }
        }

        public static string GetRoleBodyResPath(int career, int bodyID)
        {
            int bodyResID = 1000+career*100+bodyID;
            return RoleResPath+"/body/body_"+bodyResID+"/model_body_"+bodyResID+".prefab";
        }

        public static string GetRoleHairResPath(int career, int hairID)
        {
            int hairResID = 1000+career*100+hairID;
            return RoleResPath+"/hair/hair_"+hairResID+"/model_hair_"+hairResID+".prefab";
        }

    }

}
