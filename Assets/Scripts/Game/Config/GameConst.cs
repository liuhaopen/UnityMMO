namespace UnityMMO
{
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
        public static readonly float[] JumpAscentDuration = new float[]{0.8333f*3/4.0f*1000, 0.8333f*3/4.0f*1000, 1.1666f*3/4.0f*1000};
        public static readonly float[] JumpAscentHeight = new float[]{3000, 3000, 3000};
        public const float MaxFallVelocity = 10;
    }
}