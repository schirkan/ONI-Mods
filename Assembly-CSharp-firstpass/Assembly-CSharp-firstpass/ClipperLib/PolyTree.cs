// Decompiled with JetBrains decompiler
// Type: ClipperLib.PolyTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace ClipperLib
{
  public class PolyTree : PolyNode
  {
    internal List<PolyNode> m_AllPolys = new List<PolyNode>();

    ~PolyTree() => this.Clear();

    public void Clear()
    {
      for (int index = 0; index < this.m_AllPolys.Count; ++index)
        this.m_AllPolys[index] = (PolyNode) null;
      this.m_AllPolys.Clear();
      this.m_Childs.Clear();
    }

    public PolyNode GetFirst() => this.m_Childs.Count > 0 ? this.m_Childs[0] : (PolyNode) null;

    public int Total
    {
      get
      {
        int count = this.m_AllPolys.Count;
        if (count > 0 && this.m_Childs[0] != this.m_AllPolys[0])
          --count;
        return count;
      }
    }
  }
}
