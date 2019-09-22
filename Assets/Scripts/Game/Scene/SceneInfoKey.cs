using XLua;

namespace UnityMMO
{
public enum SceneInfoKey
{
    None=0,
    EnterView=1,//value is scene object type
    LeaveView=2,
    PosChange=3,
    TargetPos=4,
    JumpState=5,
    HPChange=6,
    NPCState=7,
    SceneChange=8,
    Buff=9,
    Speed=10,
    Exp=11,
}

public enum SceneObjectType
{
    None=0,
    Role=1,
    Monster=2,
    NPC=3,
    Collectable=4,
    DropItem=5,//掉落物
    Interactive=6,
}
}