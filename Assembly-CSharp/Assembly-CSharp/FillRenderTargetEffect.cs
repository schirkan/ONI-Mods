// Decompiled with JetBrains decompiler
// Type: FillRenderTargetEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FillRenderTargetEffect : MonoBehaviour
{
  private Texture fillTexture;

  public void SetFillTexture(Texture tex) => this.fillTexture = tex;

  private void OnRenderImage(RenderTexture source, RenderTexture destination) => Graphics.Blit(this.fillTexture, (RenderTexture) null);
}
