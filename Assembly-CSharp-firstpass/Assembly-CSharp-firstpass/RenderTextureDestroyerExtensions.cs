// Decompiled with JetBrains decompiler
// Type: RenderTextureDestroyerExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class RenderTextureDestroyerExtensions
{
  public static void DestroyRenderTexture(this RenderTexture render_texture)
  {
    if ((Object) RenderTextureDestroyer.Instance != (Object) null)
      RenderTextureDestroyer.Instance.Add(render_texture);
    else
      Object.Destroy((Object) render_texture);
  }
}
