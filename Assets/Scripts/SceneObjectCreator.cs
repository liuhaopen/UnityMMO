using Unity.Entities;
using UnityEngine;

namespace UnityMMO
{
public class SceneObjectCreator : MonoBehaviour
{
	public static SceneObjectCreator Instance;
	EntityManager entityManager;
    public void Awake()
	{
		Instance = this; // worst singleton ever but it works
		entityManager = World.Active.GetExistingManager<EntityManager>();
	}

	public void OnDestroy()
	{
		Instance = null;
	}

    public void AddSceneObject(SceneObjectData data)
    {
        var entity = entityManager.CreateEntity(typeof(SceneObjectData));
		entityManager.SetComponentData(entity, data);
    }
}
}}