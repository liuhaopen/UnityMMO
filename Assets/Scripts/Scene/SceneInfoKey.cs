using XLua;

namespace UnityMMO
{
[Hotfix]
[LuaCallCSharp]
public enum SceneInfoKey
{
    None=0,
    EnterScene=1,//value is scene object type
    LeaveScene=2,
    PosChange=3,
    TargetPos=4,
    JumpState=5,
}

[Hotfix]
[LuaCallCSharp]
public enum SceneObjectType
{
    None=0,
    Role=1,
    Monster=2,
    NPC=3,
}
}