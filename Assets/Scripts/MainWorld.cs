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

    public void Initialize() {
        SceneMgr.Instance.InitArcheType();
        SynchFromNet.Instance.Init();
    }

    public void StartGame() {
        SceneMgr.Instance.LoadScene(1001);
        SynchFromNet.Instance.ReqSceneObjInfoChange();

    }

    void TestLoadMultipleNavMeshInRunTime()
    {
        XLuaFramework.ResourceManager.GetInstance().LoadNavMesh("Test1");
        XLuaFramework.ResourceManager.GetInstance().LoadNavMesh("Test2");
        AsyncOperation asy = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Test1", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        AsyncOperation asy2 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Test2", UnityEngine.SceneManagement.LoadSceneMode.Additive); 
    }

    // private void Update() {
    //     SynchFromNet.Instance.Update();
    // }
    
}
}