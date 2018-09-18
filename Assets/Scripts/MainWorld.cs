using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO{
public class MainWorld : MonoBehaviour
{
    private MainWorld(){}
    public static MainWorld Instance = null;

    private void Awake() {
        Instance = this;
        Initialize();
    }

    // public static MainWorld GetInstance()
    // {
    //     if (instance == null)
    //         instance = new MainWorld();
    //     return instance;
    // }

    public void Initialize() {
        SceneObjectCreator.Instance.InitArcheType();
        SynchFromNet.Instance.Init();
    }

    public void StartGame() {
        SceneObjectCreator.Instance.AddMainRole();

            SynchFromNet.Instance.ReqSceneObjInfoChange();
        Debug.Log("GameVariable.IsNeedSynchSceneInfo : "+GameVariable.IsNeedSynchSceneInfo.ToString());
        if (GameVariable.IsNeedSynchSceneInfo)
        {
        }
        else
        {
            Timer.Register(0.5f, () => SynchFromNet.Instance.ReqSceneObjInfoChange());
        }
    }

    

    // private void Update() {
    //     SynchFromNet.Instance.Update();
    // }
    
}
}