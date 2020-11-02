// Decompiled with JetBrains decompiler
// Type: ProcGen.Map.Cell
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using System.Collections.Generic;

namespace ProcGen.Map
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Cell : ProcGen.Node
  {
    public List<Cell> neighbors;
    public List<Edge> edges;
    public List<Corner> corners;

    public Cell()
    {
      this.SetType(WorldGenTags.Cell.Name);
      this.Init();
    }

    public Cell(Satsuma.Node node)
      : base(node, WorldGenTags.Cell.Name)
      => this.Init();

    public Cell(ProcGen.Node node)
      : base(node.node, WorldGenTags.Cell.Name)
      => this.Init();

    private void Init()
    {
      this.edges = new List<Edge>();
      this.corners = new List<Corner>();
      this.neighbors = new List<Cell>();
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
    }

    public void Remove(Edge e)
    {
      this.edges.Remove(e);
      e.site0.neighbors.Remove(e.site1);
      e.site1.neighbors.Remove(e.site0);
    }

    public void Add(Corner c)
    {
      if (this.corners.Contains(c))
        return;
      this.corners.Add(c);
    }

    public void Add(Cell c)
    {
      if (this.neighbors.Contains(c))
        return;
      this.neighbors.Add(c);
      c.Add(this);
    }
  }
}
