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
        public const int MinLuaNetSessionID = int.MaxValue/2;
    }
}