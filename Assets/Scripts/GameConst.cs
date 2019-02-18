using XLua;

namespace UnityMMO
{
    [Hotfix]
    [LuaCallCSharp]
    public class GameConst
    {
        public const int RealToLogic = 100;
        public const int LogicToReal = 1/RealToLogic;

        //session id split two part:c# side(GamePlay) and lua side(UI)
        public const int MinLuaNetSessionID = System.Int32.MaxValue/2;
        public const int MaxLuaNetSessionID = System.Int32.MaxValue;
        public const int NetResultOk = 0;

        public string GetRoleResPath()
        {
            return "Assets/AssetBundleRes/role";
        }

        public static string GetRoleCareerResPath(int career)
        {
            return "Assets/AssetBundleRes/role/career_"+career;
        }
    }
}