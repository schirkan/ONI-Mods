// Decompiled with JetBrains decompiler
// Type: ClipperLib.MyIntersectNodeSort
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace ClipperLib
{
  public class MyIntersectNodeSort : IComparer<IntersectNode>
  {
    public int Compare(IntersectNode node1, IntersectNode node2)
    {
      long num = node2.Pt.Y - node1.Pt.Y;
      if (num > 0L)
        return 1;
      return num < 0L ? -1 : 0;
    }
  }
}
