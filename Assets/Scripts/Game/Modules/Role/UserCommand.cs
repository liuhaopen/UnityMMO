using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct UserCommand : Unity.Entities.IComponentData
{
    public float moveYaw;
    public float moveMagnitude;
    public float lookYaw;
    public float lookPitch;
    public int jump;
    public int sprint;//冲刺
    public int skill;//使用的技能索引，普攻也是技能来的

    public static readonly UserCommand defaultCommand = new UserCommand(0); 

    private UserCommand(int i)    
    {
        moveYaw = 0;
        moveMagnitude = 0;
        lookYaw = 0;
        lookPitch = 90;
        jump = 0;
        sprint = 0;
        skill = 0;
    }
    
    public void ClearCommand()  
    {
        jump = 0;
        sprint = 0;
        skill = 0;
    }
    

    // public Vector3 lookDir
    // {
    //     get { return Quaternion.Euler(new Vector3(-lookPitch, lookYaw, 0)) * Vector3.down; }
    // }
    // public Quaternion lookRotation
    // {
    //     get { return Quaternion.Euler(new Vector3(90 - lookPitch, lookYaw, 0)); }
    // }

    public override string ToString()
    {
        System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
        strBuilder.AppendLine("moveYaw:" + moveYaw);
        strBuilder.AppendLine("moveMagnitude:" + moveMagnitude);
        strBuilder.AppendLine("lookYaw:" + lookYaw);
        strBuilder.AppendLine("lookPitch:" + lookPitch);
        strBuilder.AppendLine("jump:" + jump);
        strBuilder.AppendLine("sprint:" + sprint);
        strBuilder.AppendLine("skillID:" + skill);
        // strBuilder.AppendLine("emote:" + emote);
        return strBuilder.ToString();
    }
}
