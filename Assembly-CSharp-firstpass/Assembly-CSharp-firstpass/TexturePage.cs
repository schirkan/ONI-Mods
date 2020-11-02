// Decompiled with JetBrains decompiler
// Type: TexturePage
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class TexturePage
{
  public int width;
  public int height;
  public TextureFormat format;
  public TexturePagePool pool;
  public Texture2D texture;
  public byte[] bytes;

  public TexturePage(string name, int width, int height, TextureFormat format)
  {
    this.width = width;
    this.height = height;
    this.format = format;
    this.texture = new Texture2D(width, height, format, false);
    this.texture.name = name;
    this.texture.filterMode = FilterMode.Point;
    this.texture.wrapMode = TextureWrapMode.Clamp;
    this.bytes = new byte[width * height * TextureUtil.GetBytesPerPixel(format)];
    this.SetName(name);
  }

  public void SetName(string name) => this.texture.name = name;
}
