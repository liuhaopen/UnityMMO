using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 场景物体接口：需要插入到场景四叉树并实现动态显示与隐藏的物体实现该接口
/// </summary>
public interface ISceneObject
{
    /// <summary>
    /// 该物体的包围盒
    /// </summary>
    Bounds Bounds { get; }

    /// <summary>
    /// 该物体进入显示区域时调用（在这里处理物体的加载或显示）
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    bool OnShow(Transform parent);

    /// <summary>
    /// 该物体离开显示区域时调用（在这里处理物体的卸载或隐藏）
    /// </summary>
    void OnHide();

    
}

public interface ISOLinkedListNode
{
    LinkedListNode<T> GetLinkedListNode<T>() where T : ISceneObject;

    void SetLinkedListNode<T>(LinkedListNode<T> node);
}