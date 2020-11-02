// Decompiled with JetBrains decompiler
// Type: SimDebugViewCompositor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SimDebugViewCompositor : MonoBehaviour
{
  public Material material;
  public static SimDebugViewCompositor Instance;

  private void Awake() => SimDebugViewCompositor.Instance = this;

  private void OnDestroy() => SimDebugViewCompositor.Instance = (SimDebugViewCompositor) null;

  private void Start() => this.material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));

  private void OnRenderImage(RenderTexture src, RenderTexture dest) => Graphics.Blit((Texture) src, dest, this.material);

  public void Toggle(bool is_on) => this.enabled = is_on;
}
