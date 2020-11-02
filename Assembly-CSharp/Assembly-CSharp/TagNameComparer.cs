// Decompiled with JetBrains decompiler
// Type: TagNameComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class TagNameComparer : IComparer<Tag>
{
  private Tag firstTag;

  public TagNameComparer()
  {
  }

  public TagNameComparer(Tag firstTag) => this.firstTag = firstTag;

  public int Compare(Tag x, Tag y)
  {
    if (x == y)
      return 0;
    if (this.firstTag.IsValid)
    {
      if (x == this.firstTag && y != this.firstTag)
        return 1;
      if (x != this.firstTag && y == this.firstTag)
        return -1;
    }
    return x.ProperNameStripLink().CompareTo(y.ProperNameStripLink());
  }
}
