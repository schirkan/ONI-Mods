// Decompiled with JetBrains decompiler
// Type: ClipperLib.PolyNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace ClipperLib
{
  public class PolyNode
  {
    internal PolyNode m_Parent;
    internal List<IntPoint> m_polygon = new List<IntPoint>();
    internal int m_Index;
    internal JoinType m_jointype;
    internal EndType m_endtype;
    internal List<PolyNode> m_Childs = new List<PolyNode>();

    private bool IsHoleNode()
    {
      bool flag = true;
      for (PolyNode parent = this.m_Parent; parent != null; parent = parent.m_Parent)
        flag = !flag;
      return flag;
    }

    public int ChildCount => this.m_Childs.Count;

    public List<IntPoint> Contour => this.m_polygon;

    internal void AddChild(PolyNode Child)
    {
      int count = this.m_Childs.Count;
      this.m_Childs.Add(Child);
      Child.m_Parent = this;
      Child.m_Index = count;
    }

    public PolyNode GetNext() => this.m_Childs.Count > 0 ? this.m_Childs[0] : this.GetNextSiblingUp();

    internal PolyNode GetNextSiblingUp()
    {
      if (this.m_Parent == null)
        return (PolyNode) null;
      return this.m_Index == this.m_Parent.m_Childs.Count - 1 ? this.m_Parent.GetNextSiblingUp() : this.m_Parent.m_Childs[this.m_Index + 1];
    }

    public List<PolyNode> Childs => this.m_Childs;

    public PolyNode Parent => this.m_Parent;

    public bool IsHole => this.IsHoleNode();

    public bool IsOpen { get; set; }
  }
}
