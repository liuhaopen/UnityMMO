
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class MonsterInfo : MonoBehaviour
{
    [DataMember]
    public int monster_id;

    [DataMember]
    public float pos_x
    {
        get
        {
            return transform.position.x;
        }
        set
        {
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }
    }

    [DataMember]
    public float pos_y
    {
        get
        {
            return transform.position.y;
        }
        set
        {
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }
    }

    [DataMember]
    public float pos_z
    {
        get
        {
            return transform.position.z;
        }
        set
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }
    }

}
