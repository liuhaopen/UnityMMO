using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;

public class MoveQuery : MonoBehaviour
{
    // [Serializable]
    // public struct Settings
    // {
        public float slopeLimit;
        public float stepOffset;
        public float skinWidth;
        public float minMoveDistance;
        public float3 center;
        public float radius;
        public float height;
    // }

    [NonSerialized] public int collisionLayer;
    [NonSerialized] public float3 moveQueryStart;
    [NonSerialized] public float3 moveQueryEnd;
    [NonSerialized] public float3 moveQueryResult;
    [NonSerialized] public bool isGrounded;

    [NonSerialized] public CharacterController charController;
    // [NonSerialized] public Settings settings;
    
    // public void Initialize(Settings settings, Entity hitCollOwner)
    public void Initialize()
    {
        //GameDebug.Log("MoveQuery.Initialize");
        // this.settings = settings;
        // var go = new GameObject("MoveColl_" + name,typeof(CharacterController), typeof(HitCollision));
        var go = new GameObject("MoveColl_" + name,typeof(CharacterController));
        charController = go.GetComponent<CharacterController>();
        charController.transform.position = transform.position;
        charController.transform.SetParent(transform.parent);
        charController.slopeLimit = this.slopeLimit;
        charController.stepOffset = this.stepOffset;
        charController.skinWidth = this.skinWidth;
        charController.minMoveDistance = this.minMoveDistance;
        charController.center = this.center; 
        charController.radius = this.radius; 
        charController.height = this.height;

        // var hitCollision = go.GetComponent<HitCollision>();
        // hitCollision.owner = hitCollOwner;
    }

    public void Shutdown()
    {
        //GameDebug.Log("MoveQuery.Shutdown");
        GameObject.Destroy(charController.gameObject);
    }
}


[DisableAutoCreation]
class HandleMovementQueries : BaseComponentSystem
{
    ComponentGroup Group;
	
    public HandleMovementQueries(GameWorld world) : base(world) {}
	
    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        Group = GetComponentGroup(typeof(MoveQuery));
    }

    protected override void OnUpdate()
    {
        // Profiler.BeginSample("HandleMovementQueries");
        
        var queryArray = Group.GetComponentArray<MoveQuery>();

        // Debug.Log("queryArray.Length : "+queryArray.Length);
        for (var i = 0; i < queryArray.Length; i++)
        {
            var query = queryArray[i];
            var charController = query.charController;
            // if (charController.gameObject.layer != query.collisionLayer)
                // charController.gameObject.layer = query.collisionLayer;
            float3 currentControllerPos = charController.transform.position;
            if (math.distance(currentControllerPos, query.moveQueryStart) > 0.01f)
            {
                currentControllerPos = query.moveQueryStart;
                charController.transform.position = currentControllerPos;
            }

            var deltaPos = query.moveQueryEnd - currentControllerPos; 
            // Debug.Log("deltaPos : "+deltaPos.x+" "+deltaPos.y+" "+deltaPos.z);
            charController.Move(deltaPos);
            query.moveQueryResult = charController.transform.position;
            query.isGrounded = charController.isGrounded;
            // Debug.Log("res pos : "+query.moveQueryResult.x+" "+query.moveQueryResult.y+" "+query.moveQueryResult.z);
            query.transform.localPosition = query.moveQueryResult;
        }
        // Profiler.EndSample();
    }
}