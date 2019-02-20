
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class BornInfo : MonoBehaviour
{
    [DataMember]
    public int born_id;

    [DataMember]
    public int pos_x
    {
        get
        {
            return (int)transform.position.x*EditorConst.LogicFactor;
        }
        set
        {
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }
    }

    [DataMember]
    public int pos_y
    {
        get
        {
            return (int)transform.position.y*EditorConst.LogicFactor;
        }
        set
        {
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }
    }

    [DataMember]
    public int pos_z
    {
        get
        {
            return (int)transform.position.z*EditorConst.LogicFactor;
        }
        set
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }
    }

}

