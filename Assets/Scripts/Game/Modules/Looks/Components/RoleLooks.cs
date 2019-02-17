using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

//负责显示角色的外观模型，动画等
[DisallowMultipleComponent]
public class RoleLooks : MonoBehaviour
{
    [NonSerialized] public int career; 
    [NonSerialized] public Entity presentation;    // Main char presentation used updating animation state 
    // [NonSerialized] public List<CharPresentation> presentations = new List<CharPresentation>();
    [NonSerialized] public float altitude; 
    [NonSerialized] public Collider groundCollider; 
    [NonSerialized] public Vector3 groundNormal;
}