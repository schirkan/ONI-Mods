// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Extensions.UIPrimitiveBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UnityEngine.UI.Extensions
{
  public class UIPrimitiveBase : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
  {
    [SerializeField]
    private Sprite m_Sprite;
    [NonSerialized]
    private Sprite m_OverrideSprite;
    internal float m_EventAlphaThreshold = 1f;

    public Sprite sprite
    {
      get => this.m_Sprite;
      set
      {
        if (!SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
          return;
        this.SetAllDirty();
      }
    }

    public Sprite overrideSprite
    {
      get => !((UnityEngine.Object) this.m_OverrideSprite == (UnityEngine.Object) null) ? this.m_OverrideSprite : this.sprite;
      set
      {
        if (!SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
          return;
        this.SetAllDirty();
      }
    }

    public float eventAlphaThreshold
    {
      get => this.m_EventAlphaThreshold;
      set => this.m_EventAlphaThreshold = value;
    }

    public override Texture mainTexture
    {
      get
      {
        if (!((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null))
          return (Texture) this.overrideSprite.texture;
        return (UnityEngine.Object) this.material != (UnityEngine.Object) null && (UnityEngine.Object) this.material.mainTexture != (UnityEngine.Object) null ? this.material.mainTexture : (Texture) Graphic.s_WhiteTexture;
      }
    }

    public float pixelsPerUnit
    {
      get
      {
        float num1 = 100f;
        if ((bool) (UnityEngine.Object) this.sprite)
          num1 = this.sprite.pixelsPerUnit;
        float num2 = 100f;
        if ((bool) (UnityEngine.Object) this.canvas)
          num2 = this.canvas.referencePixelsPerUnit;
        return num1 / num2;
      }
    }

    protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
    {
      UIVertex[] uiVertexArray = new UIVertex[4];
      for (int index = 0; index < vertices.Length; ++index)
      {
        UIVertex simpleVert = UIVertex.simpleVert;
        simpleVert.color = (Color32) this.color;
        simpleVert.position = (Vector3) vertices[index];
        simpleVert.uv0 = uvs[index];
        uiVertexArray[index] = simpleVert;
      }
      return uiVertexArray;
    }

    public virtual void CalculateLayoutInputHorizontal()
    {
    }

    public virtual void CalculateLayoutInputVertical()
    {
    }

    public virtual float minWidth => 0.0f;

    public virtual float preferredWidth => (UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null ? 0.0f : this.overrideSprite.rect.size.x / this.pixelsPerUnit;

    public virtual float flexibleWidth => -1f;

    public virtual float minHeight => 0.0f;

    public virtual float preferredHeight => (UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null ? 0.0f : this.overrideSprite.rect.size.y / this.pixelsPerUnit;

    public virtual float flexibleHeight => -1f;

    public virtual int layoutPriority => 0;

    public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
      if ((double) this.m_EventAlphaThreshold >= 1.0)
        return true;
      Sprite overrideSprite = this.overrideSprite;
      if ((UnityEngine.Object) overrideSprite == (UnityEngine.Object) null)
        return true;
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPoint, eventCamera, out localPoint);
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      localPoint.x += this.rectTransform.pivot.x * pixelAdjustedRect.width;
      localPoint.y += this.rectTransform.pivot.y * pixelAdjustedRect.height;
      Vector2 vector2_1 = this.MapCoordinate(localPoint, pixelAdjustedRect);
      Rect textureRect = overrideSprite.textureRect;
      Vector2 vector2_2 = new Vector2(vector2_1.x / textureRect.width, vector2_1.y / textureRect.height);
      float x = Mathf.Lerp(textureRect.x, textureRect.xMax, vector2_2.x) / (float) overrideSprite.texture.width;
      float y = Mathf.Lerp(textureRect.y, textureRect.yMax, vector2_2.y) / (float) overrideSprite.texture.height;
      try
      {
        return (double) overrideSprite.texture.GetPixelBilinear(x, y).a >= (double) this.m_EventAlphaThreshold;
      }
      catch (UnityException ex)
      {
        Debug.LogError((object) ("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite."), (UnityEngine.Object) this);
        return true;
      }
    }

    private Vector2 MapCoordinate(Vector2 local, Rect rect)
    {
      Rect rect1 = this.sprite.rect;
      return new Vector2(local.x * rect1.width / rect.width, local.y * rect1.height / rect.height);
    }

    private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
    {
      for (int index = 0; index <= 1; ++index)
      {
        float num1 = border[index] + border[index + 2];
        if ((double) rect.size[index] < (double) num1 && (double) num1 != 0.0)
        {
          float num2 = rect.size[index] / num1;
          border[index] *= num2;
          border[index + 2] *= num2;
        }
      }
      return border;
    }
  }
}
