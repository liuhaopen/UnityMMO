using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO{
public class MainWorld
{
    private MainWorld(){}
    static MainWorld instance = null;

    public static MainWorld GetInstance()
    {
        if (instance == null)
            instance = new MainWorld();
        return instance;
    }

    public void Initialize() {
        SceneObjectCreator.Instance.InitArcheType();
    }

    public void StartGame() {
        SceneObjectCreator.Instance.AddMainRole();
    }

    
}
}