// Decompiled with JetBrains decompiler
// Type: TextureUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public static class TextureUtil
{
  public static GraphicsFormat TextureFormatToGraphicsFormat(TextureFormat format)
  {
    switch (format)
    {
      case TextureFormat.Alpha8:
        return GraphicsFormat.R8_UNorm;
      case TextureFormat.RGB24:
        return GraphicsFormat.R8G8B8_SRGB;
      case TextureFormat.RGBA32:
        return GraphicsFormat.R8G8B8A8_SRGB;
      case TextureFormat.RGFloat:
        return GraphicsFormat.R32G32_SFloat;
      case TextureFormat.RGBAFloat:
        return GraphicsFormat.R32G32B32A32_SFloat;
      default:
        Debug.LogError((object) ("Unspecfied graphics format for texture format: " + format.ToString()));
        throw new ArgumentOutOfRangeException();
    }
  }

  public static int GetBytesPerPixel(TextureFormat format)
  {
    switch (format)
    {
      case TextureFormat.Alpha8:
        return 1;
      case TextureFormat.RGB24:
        return 3;
      case TextureFormat.RGBA32:
        return 4;
      case TextureFormat.ARGB32:
        return 4;
      case TextureFormat.RGFloat:
        return 8;
      case TextureFormat.RGBAFloat:
        return 16;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public static RenderTextureFormat GetRenderTextureFormat(TextureFormat format)
  {
    switch (format)
    {
      case TextureFormat.Alpha8:
        return RenderTextureFormat.ARGB32;
      case TextureFormat.RGB24:
        return RenderTextureFormat.ARGB32;
      case TextureFormat.RGBA32:
        return RenderTextureFormat.ARGB32;
      case TextureFormat.ARGB32:
        return RenderTextureFormat.ARGB32;
      case TextureFormat.RGFloat:
        return RenderTextureFormat.RGFloat;
      case TextureFormat.RGBAFloat:
        return RenderTextureFormat.ARGBHalf;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
