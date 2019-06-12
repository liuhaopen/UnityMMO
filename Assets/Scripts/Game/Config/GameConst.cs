using XLua;

namespace UnityMMO
{
    [Hotfix]
    [LuaCallCSharp]
    public class GameConst
    {
        public const int RealToLogic = 100;
        // public const int LogicToReal = 1/RealToLogic;
        public const int SpeedFactor = 100;

        //session id split two part:c# side(GamePlay) and lua side(UI)
        public const int MinLuaNetSessionID = System.Int32.MaxValue/2;
        public const int MaxLuaNetSessionID = System.Int32.MaxValue;
        public const int NetResultOk = 0;
        public const int Gravity = -10;
        public const int MaxJumpCount = 3;
        public static readonly float[] JumpAscentDuration = new float[]{0.8333f*3/4.0f, 0.8333f*3/4.0f, 1.1666f*3/4.0f};
        public static readonly float[] JumpAscentHeight = new float[]{3, 3, 3};
        public const float MaxFallVelocity = 10;

        public static string GetRoleResPath()
        {
            return "Assets/AssetBundleRes/role";
        }

        public static string GetUIResPath()
        {
            return "Assets/AssetBundleRes/ui";
        }

        public static string GetRoleCareerResPath(int career)
        {
            return "Assets/AssetBundleRes/role/career_"+career;
        }

        public static string GetMonsterResPath(long typeID)
        {
            return "Assets/AssetBundleRes/monster/monster_"+typeID;
        }

        public static string GetNPCResPath(long typeID)
        {
            return "Assets/AssetBundleRes/npc/npc_"+typeID;
        }

        public static string GetRoleSkillResPath(int skillID)
        {
            return "Assets/AssetBundleRes/role/timeline/skill_"+skillID+".playable";
        }

        public static string GetMonsterSkillResPath(int skillID)
        {
            return "Assets/AssetBundleRes/monster/timeline/skill_"+skillID+".playable";
        }

        public static string GetRoleJumpResPath(int career, int jumpID)
        {
            return "Assets/AssetBundleRes/role/timeline/jump_career"+career+"_"+jumpID+".playable";
        }

        public static string GetBloodResPath(Nameboard.ColorStyle style)
        {
            switch (style)
            {
                case Nameboard.ColorStyle.Red:
                {
                    return "Assets/AssetBundleRes/ui/common/com_blood_red.png";
                }
                case Nameboard.ColorStyle.Green:
                {
                    return "Assets/AssetBundleRes/ui/common/com_blood_green.png";
                }
                case Nameboard.ColorStyle.Blue:
                {
                    return "Assets/AssetBundleRes/ui/common/com_blood_blue.png";
                }
                default:
                    return "";
            }
        }

    }
}