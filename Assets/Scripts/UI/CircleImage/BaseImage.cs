using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BaseImage : MaskableGraphic,ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter
{

	// Use this for initialization
	// void Start () {
	
	// }
	
	// // Update is called once per frame
	// void Update () {
	
	// }

    [FormerlySerializedAs("m_Frame")]
    [SerializeField]
    private Sprite m_Sprite;
    public Sprite sprite { get { return m_Sprite; } set { if (SetPropertyUtilityExt.SetClass(ref m_Sprite, value)) SetAllDirty(); } }

    [NonSerialized]
    private Sprite m_OverrideSprite;
    public Sprite overrideSprite { get { return m_OverrideSprite == null ? sprite : m_OverrideSprite; } set { if (SetPropertyUtilityExt.SetClass(ref m_OverrideSprite, value)) SetAllDirty(); } }


    /// <summary>
    /// Image's texture comes from the UnityEngine.Image.
    /// </summary>
    public override Texture mainTexture
    {
        get
        {
            return overrideSprite == null ? s_WhiteTexture : overrideSprite.texture;
        }
    }

    public float pixelsPerUnit
    {
        get
        {
            float spritePixelsPerUnit = 100;
            if (sprite)
                spritePixelsPerUnit = sprite.pixelsPerUnit;

            float referencePixelsPerUnit = 100;
            if (canvas)
                referencePixelsPerUnit = canvas.referencePixelsPerUnit;

            return spritePixelsPerUnit / referencePixelsPerUnit;
        }
    }

    /// <summary>
    /// 子类需要重写该方法来自定义Image形状
    /// </summary>
    /// <param name="vh"></param>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
    }

    #region ISerializationCallbackReceiver
    public void OnAfterDeserialize()
    {
    }

    //
    // 摘要: 
    //     Implement this method to receive a callback after unity serialized your object.
    public void OnBeforeSerialize()
    {
    }
    #endregion

    #region ILayoutElement
    public virtual void CalculateLayoutInputHorizontal() { }
    public virtual void CalculateLayoutInputVertical() { }

    public virtual float minWidth { get { return 0; } }

    public virtual float preferredWidth
    {
        get
        {
            if (overrideSprite == null)
                return 0;
            return overrideSprite.rect.size.x / pixelsPerUnit;
        }
    }

    public virtual float flexibleWidth { get { return -1; } }

    public virtual float minHeight { get { return 0; } }

    public virtual float preferredHeight
    {
        get
        {
            if (overrideSprite == null)
                return 0;
            return overrideSprite.rect.size.y / pixelsPerUnit;
        }
    }

    public virtual float flexibleHeight { get { return -1; } }

    public virtual int layoutPriority { get { return 0; } }
    #endregion

    #region ICanvasRaycastFilter
    public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return true;
    }
    #endregion

}
