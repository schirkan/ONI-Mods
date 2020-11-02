// Decompiled with JetBrains decompiler
// Type: Blur
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class Blur
{
  private static Material blurMaterial;

  public static RenderTexture Run(Texture2D image)
  {
    if ((Object) Blur.blurMaterial == (Object) null)
      Blur.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
    return (RenderTexture) null;
  }
}
