// Decompiled with JetBrains decompiler
// Type: TexturePagePool
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class TexturePagePool
{
  private List<TexturePage>[] activePages = new List<TexturePage>[2];
  private List<TexturePage> freePages = new List<TexturePage>();

  public TexturePagePool()
  {
    this.activePages[0] = new List<TexturePage>();
    this.activePages[1] = new List<TexturePage>();
  }

  private int Clamp(int value)
  {
    if (value == 0)
      return 32;
    return value % 32 == 0 ? value : 32 + value / 32 * 32;
  }

  public TexturePage Alloc(string name, int width, int height, TextureFormat format)
  {
    int width1 = this.Clamp(width);
    int height1 = this.Clamp(height);
    int index1 = Time.frameCount % 2;
    foreach (TexturePage texturePage in this.activePages[index1])
      this.freePages.Add(texturePage);
    this.activePages[index1].Clear();
    for (int index2 = 0; index2 < this.freePages.Count; ++index2)
    {
      TexturePage freePage = this.freePages[index2];
      if (freePage.width == width1 && freePage.height == height1 && freePage.format == format)
      {
        this.freePages.RemoveAt(index2);
        freePage.SetName(name);
        return freePage;
      }
    }
    return new TexturePage(name, width1, height1, format);
  }

  public void Release(TexturePage page) => this.activePages[(Time.frameCount + 1) % 2].Add(page);
}
