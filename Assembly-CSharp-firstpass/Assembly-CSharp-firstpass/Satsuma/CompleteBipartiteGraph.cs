// Decompiled with JetBrains decompiler
// Type: Satsuma.CompleteBipartiteGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public sealed class CompleteBipartiteGraph : IGraph, IArcLookup
  {
    public int RedNodeCount { get; private set; }

    public int BlueNodeCount { get; private set; }

    public bool Directed { get; private set; }

    public CompleteBipartiteGraph(int redNodeCount, int blueNodeCount, Directedness directedness)
    {
      if (redNodeCount < 0 || blueNodeCount < 0)
        throw new ArgumentException("Invalid node count: " + (object) redNodeCount + ";" + (object) blueNodeCount);
      this.RedNodeCount = (long) redNodeCount + (long) blueNodeCount <= (long) int.MaxValue && (long) redNodeCount * (long) blueNodeCount <= (long) int.MaxValue ? redNodeCount : throw new ArgumentException("Too many nodes: " + (object) redNodeCount + ";" + (object) blueNodeCount);
      this.BlueNodeCount = blueNodeCount;
      this.Directed = directedness == Directedness.Directed;
    }

    public Node GetRedNode(int index) => new Node(1L + (long) index);

    public Node GetBlueNode(int index) => new Node(1L + (long) this.RedNodeCount + (long) index);

    public bool IsRed(Node node) => node.Id <= (long) this.RedNodeCount;

    public Arc GetArc(Node u, Node v)
    {
      int num1 = this.IsRed(u) ? 1 : 0;
      bool flag = this.IsRed(v);
      int num2 = flag ? 1 : 0;
      if (num1 == num2)
        return Arc.Invalid;
      if (flag)
      {
        Node node = u;
        u = v;
        v = node;
      }
      int num3 = (int) (u.Id - 1L);
      return new Arc(1L + (long) (int) (v.Id - (long) this.RedNodeCount - 1L) * (long) this.RedNodeCount + (long) num3);
    }

    public Node U(Arc arc) => new Node(1L + (arc.Id - 1L) % (long) this.RedNodeCount);

    public Node V(Arc arc) => new Node(1L + (long) this.RedNodeCount + (arc.Id - 1L) / (long) this.RedNodeCount);

    public bool IsEdge(Arc arc) => !this.Directed;

    public IEnumerable<Node> Nodes(CompleteBipartiteGraph.Color color)
    {
      int i;
      switch (color)
      {
        case CompleteBipartiteGraph.Color.Red:
          for (i = 0; i < this.RedNodeCount; ++i)
            yield return this.GetRedNode(i);
          break;
        case CompleteBipartiteGraph.Color.Blue:
          for (i = 0; i < this.BlueNodeCount; ++i)
            yield return this.GetBlueNode(i);
          break;
      }
    }

    public IEnumerable<Node> Nodes()
    {
      int i;
      for (i = 0; i < this.RedNodeCount; ++i)
        yield return this.GetRedNode(i);
      for (i = 0; i < this.BlueNodeCount; ++i)
        yield return this.GetBlueNode(i);
    }

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
    {
      if (!this.Directed || filter != ArcFilter.Edge)
      {
        for (int i = 0; i < this.RedNodeCount; ++i)
        {
          for (int j = 0; j < this.BlueNodeCount; ++j)
            yield return this.GetArc(this.GetRedNode(i), this.GetBlueNode(j));
        }
      }
    }

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All)
    {
      bool flag = this.IsRed(u);
      int i;
      if (!this.Directed || filter != ArcFilter.Edge && (filter != ArcFilter.Forward || flag) && !(filter == ArcFilter.Backward & flag))
      {
        if (flag)
        {
          for (i = 0; i < this.BlueNodeCount; ++i)
            yield return this.GetArc(u, this.GetBlueNode(i));
        }
        else
        {
          for (i = 0; i < this.RedNodeCount; ++i)
            yield return this.GetArc(this.GetRedNode(i), u);
        }
      }
    }

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      Arc arc = this.GetArc(u, v);
      if (arc != Arc.Invalid && this.ArcCount(u, filter) > 0)
        yield return arc;
    }

    public int NodeCount() => this.RedNodeCount + this.BlueNodeCount;

    public int ArcCount(ArcFilter filter = ArcFilter.All) => this.Directed && filter == ArcFilter.Edge ? 0 : this.RedNodeCount * this.BlueNodeCount;

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All)
    {
      bool flag = this.IsRed(u);
      if (this.Directed)
      {
        switch (filter)
        {
          case ArcFilter.Edge:
            return 0;
          case ArcFilter.Forward:
            if (!flag)
              goto case ArcFilter.Edge;
            else
              goto default;
          default:
            if (!(filter == ArcFilter.Backward & flag))
              break;
            goto case ArcFilter.Edge;
        }
      }
      return !flag ? this.RedNodeCount : this.BlueNodeCount;
    }

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.IsRed(u) == this.IsRed(v) || this.ArcCount(u, filter) <= 0 ? 0 : 1;

    public bool HasNode(Node node) => node.Id >= 1L && node.Id <= (long) (this.RedNodeCount + this.BlueNodeCount);

    public bool HasArc(Arc arc) => arc.Id >= 1L && arc.Id <= (long) (this.RedNodeCount * this.BlueNodeCount);

    public enum Color
    {
      Red,
      Blue,
    }
  }
}
