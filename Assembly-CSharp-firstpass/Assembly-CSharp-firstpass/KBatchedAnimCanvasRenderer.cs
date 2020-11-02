// Decompiled with JetBrains decompiler
// Type: KBatchedAnimCanvasRenderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class KBatchedAnimCanvasRenderer : MonoBehaviour, IMaskable
{
  private RectTransform rootRectTransform;
  public KAnimBatch batch;
  public Material uiMat;
  private KAnimConverter.IAnimConverter converter;
  private CompareFunction _cmp = CompareFunction.Never;
  private StencilOp _op = StencilOp.Zero;
  private static KBatchedAnimCanvasRenderer.TextureToCopyEntry[] texturesToCopy;
  private Vector4 _ClipRect = new Vector4(0.0f, 0.0f, 0.0f, 1f);

  public CanvasRenderer canvass { get; private set; }

  public CompareFunction compare
  {
    get => this._cmp;
    set
    {
      if (this._cmp == value)
        return;
      if ((Object) this.uiMat != (Object) null)
        this.uiMat.SetInt("_StencilComp", (int) value);
      this._cmp = value;
    }
  }

  public StencilOp stencilOp
  {
    get => this._op;
    set
    {
      if (this._op == value)
        return;
      if ((Object) this.uiMat != (Object) null)
        this.uiMat.SetInt("_StencilOp", (int) this._op);
      this._op = value;
    }
  }

  void IMaskable.RecalculateMasking()
  {
    Mask componentInParent = this.GetComponentInParent<Mask>();
    if ((Object) componentInParent != (Object) null && componentInParent.enabled)
    {
      this.compare = CompareFunction.Equal;
      this.stencilOp = StencilOp.Keep;
    }
    else
    {
      this.compare = CompareFunction.Disabled;
      this.stencilOp = StencilOp.Keep;
    }
    if (!((Object) this.uiMat != (Object) null))
      return;
    this.uiMat.SetInt("_StencilComp", (int) this.compare);
    this.uiMat.SetInt("_StencilOp", (int) this.stencilOp);
  }

  public void SetBatch(KAnimConverter.IAnimConverter conv)
  {
    this.converter = conv;
    if (conv != null)
    {
      this.batch = conv.GetBatch();
    }
    else
    {
      this.batch = (KAnimBatch) null;
      if ((Object) this.uiMat != (Object) null)
      {
        Object.Destroy((Object) this.uiMat);
        this.uiMat = (Material) null;
      }
    }
    if (this.batch == null)
      return;
    this.canvass = this.GetComponent<CanvasRenderer>();
    if ((Object) this.canvass == (Object) null)
      this.canvass = this.gameObject.AddComponent<CanvasRenderer>();
    this.rootRectTransform = this.GetComponent<RectTransform>();
    if ((Object) this.rootRectTransform == (Object) null)
      this.rootRectTransform = this.gameObject.AddComponent<RectTransform>();
    if (!this.batch.group.InitOK)
      return;
    if ((Object) this.uiMat != (Object) null)
    {
      Object.Destroy((Object) this.uiMat);
      this.uiMat = (Material) null;
    }
    this.uiMat = new Material(this.batch.group.GetMaterial(this.batch.materialType));
    ((IMaskable) this).RecalculateMasking();
  }

  private void UpdateCanvas()
  {
    this.canvass.Clear();
    this.canvass.SetMesh(this.batch.group.mesh);
    this.canvass.materialCount = 1;
    this.canvass.SetMaterial(this.uiMat, 0);
  }

  private void CopyPropertyBlockToMaterial()
  {
    if (KBatchedAnimCanvasRenderer.texturesToCopy == null)
      KBatchedAnimCanvasRenderer.texturesToCopy = new KBatchedAnimCanvasRenderer.TextureToCopyEntry[4]
      {
        new KBatchedAnimCanvasRenderer.TextureToCopyEntry()
        {
          textureId = Shader.PropertyToID("instanceTex"),
          sizeId = Shader.PropertyToID("INSTANCE_TEXTURE_SIZE")
        },
        new KBatchedAnimCanvasRenderer.TextureToCopyEntry()
        {
          textureId = Shader.PropertyToID("buildAndAnimTex"),
          sizeId = Shader.PropertyToID("BUILD_AND_ANIM_TEXTURE_SIZE")
        },
        new KBatchedAnimCanvasRenderer.TextureToCopyEntry()
        {
          textureId = Shader.PropertyToID("symbolInstanceTex"),
          sizeId = Shader.PropertyToID("SYMBOL_INSTANCE_TEXTURE_SIZE")
        },
        new KBatchedAnimCanvasRenderer.TextureToCopyEntry()
        {
          textureId = Shader.PropertyToID("symbolOverrideInfoTex"),
          sizeId = Shader.PropertyToID("SYMBOL_OVERRIDE_INFO_TEXTURE_SIZE")
        }
      };
    foreach (KBatchedAnimCanvasRenderer.TextureToCopyEntry textureToCopyEntry in KBatchedAnimCanvasRenderer.texturesToCopy)
    {
      this.uiMat.SetTexture(textureToCopyEntry.textureId, this.batch.matProperties.GetTexture(textureToCopyEntry.textureId));
      this.uiMat.SetVector(textureToCopyEntry.sizeId, this.batch.matProperties.GetVector(textureToCopyEntry.sizeId));
    }
    for (int index = 0; index < KAnimBatchManager.instance.atlasNames.Length; ++index)
    {
      Texture texture = this.batch.matProperties.GetTexture(KAnimBatchManager.instance.atlasNames[index]);
      if ((Object) texture != (Object) null)
        this.uiMat.SetTexture(KAnimBatchManager.instance.atlasNames[index], texture);
    }
    foreach (KBatchedAnimCanvasRenderer.TextureToCopyEntry textureToCopyEntry in KBatchedAnimCanvasRenderer.texturesToCopy)
    {
      this.uiMat.SetTexture(textureToCopyEntry.textureId, this.batch.matProperties.GetTexture(textureToCopyEntry.textureId));
      this.uiMat.SetVector(textureToCopyEntry.sizeId, this.batch.matProperties.GetVector(textureToCopyEntry.sizeId));
    }
    for (int index = 0; index < KAnimBatchManager.instance.atlasNames.Length; ++index)
    {
      Texture texture = this.batch.matProperties.GetTexture(KAnimBatchManager.instance.atlasNames[index]);
      if ((Object) texture != (Object) null)
        this.uiMat.SetTexture(KAnimBatchManager.instance.atlasNames[index], texture);
    }
    this.uiMat.SetFloat(KAnimBatch.ShaderProperty_SUPPORTS_SYMBOL_OVERRIDING, this.batch.matProperties.GetFloat(KAnimBatch.ShaderProperty_SUPPORTS_SYMBOL_OVERRIDING));
    this.uiMat.SetFloat(KAnimBatch.ShaderProperty_ANIM_TEXTURE_START_OFFSET, this.batch.matProperties.GetFloat(KAnimBatch.ShaderProperty_ANIM_TEXTURE_START_OFFSET));
  }

  private void LateUpdate()
  {
    if (this.batch == null)
      return;
    if (this.transform.hasChanged)
    {
      this.batch.SetDirty(this.converter);
      this.transform.hasChanged = false;
    }
    ref Vector4 local1 = ref this._ClipRect;
    Rect rect = this.rootRectTransform.rect;
    double xMin = (double) rect.xMin;
    local1.x = (float) xMin;
    ref Vector4 local2 = ref this._ClipRect;
    rect = this.rootRectTransform.rect;
    double yMin = (double) rect.yMin;
    local2.y = (float) yMin;
    ref Vector4 local3 = ref this._ClipRect;
    rect = this.rootRectTransform.rect;
    double xMax = (double) rect.xMax;
    local3.z = (float) xMax;
    ref Vector4 local4 = ref this._ClipRect;
    rect = this.rootRectTransform.rect;
    double yMax = (double) rect.yMax;
    local4.w = (float) yMax;
    this.UpdateCanvas();
    this.CopyPropertyBlockToMaterial();
  }

  private struct TextureToCopyEntry
  {
    public int textureId;
    public int sizeId;
  }
}
