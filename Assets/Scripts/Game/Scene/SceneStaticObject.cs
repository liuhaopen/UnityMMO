
using System.Runtime.Serialization;
using UnityEngine;
using UnityMMO;

[System.Serializable]
[DataContract]
public class SceneStaticObject : ISceneObject
{
    [SerializeField]
    [DataMember]
    private Bounds m_Bounds;
    [SerializeField]
    [DataMember]
    private int m_ResID;
    [SerializeField]
    [DataMember]
    private Vector3 m_Position;
    [SerializeField]
    [DataMember]
    private Vector3 m_Rotation;
    [SerializeField]
    [DataMember]
    private Vector3 m_Size;
    [DataMember]
    public int m_LightmapIndex;
    [DataMember]
    public Vector4 m_LightmapScaleOffset;

    private GameObject m_LoadStaticObj;

    public Bounds Bounds
    {
        get { return m_Bounds; }
    }

    public void OnHide()
    {
        if (m_LoadStaticObj)
        {
            // Object.Destroy(m_LoadStaticObj);
            ResMgr.GetInstance().UnuseSceneObject(m_ResID, m_LoadStaticObj);
            m_LoadStaticObj = null;
        }
    }

    public bool OnShow(Transform parent)
    {
        if (m_LoadStaticObj == null)
        {
            GameObject obj = ResMgr.GetInstance().GetSceneRes(m_ResID);
            if (obj == null)
                return false;
            m_LoadStaticObj = obj as GameObject;
            // Debug.Log("LoadScene obj "+(obj!=null).ToString() +" m_LoadStaticObj : "+(m_LoadStaticObj!=null).ToString());
            m_LoadStaticObj.transform.SetParent(parent);
            m_LoadStaticObj.transform.position = m_Position;
            m_LoadStaticObj.transform.eulerAngles = m_Rotation;
            m_LoadStaticObj.transform.localScale = m_Size;
            if (m_LightmapIndex != -1)
            {
                Renderer renderer = m_LoadStaticObj.GetComponent<Renderer>();
                renderer.lightmapIndex = m_LightmapIndex;
                renderer.lightmapScaleOffset = m_LightmapScaleOffset;
            }
            return true;
        }
        return false;
    }

    public SceneStaticObject(Bounds bounds, Vector3 position, Vector3 rotation, Vector3 size, int resID, int lightmapIndex, Vector4 lightmapScaleOffset)
    {
        m_Bounds = bounds;
        m_Position = position;
        m_Rotation = rotation;
        m_Size = size;
        m_ResID = resID;
        m_LightmapIndex = lightmapIndex;
        m_LightmapScaleOffset = lightmapScaleOffset;
    }
}
