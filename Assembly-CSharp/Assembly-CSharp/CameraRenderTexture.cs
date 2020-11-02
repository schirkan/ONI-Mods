// Decompiled with JetBrains decompiler
// Type: CameraRenderTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{
  public string TextureName;
  private RenderTexture resultTexture;
  private Material material;

  private void Awake() => this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));

  private void Start()
  {
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.OnResize();
  }

  private void OnResize()
  {
    if ((UnityEngine.Object) this.resultTexture != (UnityEngine.Object) null)
      this.resultTexture.DestroyRenderTexture();
    this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
    this.resultTexture.name = this.name;
    this.resultTexture.filterMode = FilterMode.Point;
    this.resultTexture.autoGenerateMips = false;
    if (!(this.TextureName != ""))
      return;
    Shader.SetGlobalTexture(this.TextureName, (Texture) this.resultTexture);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture dest) => Graphics.Blit((Texture) source, this.resultTexture, this.material);

  public RenderTexture GetTexture() => this.resultTexture;

  public bool ShouldFlip() => false;
}
