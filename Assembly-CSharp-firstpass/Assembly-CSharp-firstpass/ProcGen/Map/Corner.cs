// Decompiled with JetBrains decompiler
// Type: ProcGen.Map.Corner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using System.Collections.Generic;

namespace ProcGen.Map
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Corner : ProcGen.Node
  {
    public List<Edge> edges;
    public List<Cell> cells;

    public Corner()
      : base(WorldGenTags.Corner.Name)
      => this.Init();

    public Corner(Satsuma.Node node)
      : base(node, WorldGenTags.Corner.Name)
      => this.Init();

    private void Init()
    {
      this.edges = new List<Edge>();
      this.cells = new List<Cell>();
      this.tags = new TagSet();
    }

    public void Add(Edge e)
    {
      if (this.edges.Find((Predicate<Edge>) (edge =>
      {
        if (e.corner0 == edge.corner0 && e.corner1 == edge.corner1)
          return true;
        return e.corner1 == edge.corner0 && e.corner0 == edge.corner1;
      })) != null)
        return;
      this.edges.Add(e);
      if (e.site0 != null && e.site0.position == this.position && this.cells.Find((Predicate<Cell>) (site => e.site0 == site)) == null)
      {
        this.cells.Add(e.site0);
        e.site0.Add(this);
      }
      if (e.site1 == null || !(e.site1.position == this.position) || this.cells.Find((Predicate<Cell>) (site => e.site1 == site)) != null)
        return;
      this.cells.Add(e.site1);
      e.site1.Add(this);
    }
  }
}
