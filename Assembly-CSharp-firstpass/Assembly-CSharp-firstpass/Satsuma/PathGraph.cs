// Decompiled with JetBrains decompiler
// Type: Satsuma.PathGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class PathGraph : IPath, IGraph, IArcLookup
  {
    private readonly int nodeCount;
    private readonly bool isCycle;
    private readonly bool directed;

    public Node FirstNode => this.nodeCount <= 0 ? Node.Invalid : new Node(1L);

    public Node LastNode => this.nodeCount <= 0 ? Node.Invalid : new Node(this.isCycle ? 1L : (long) this.nodeCount);

    public PathGraph(int nodeCount, PathGraph.Topology topology, Directedness directedness)
    {
      this.nodeCount = nodeCount;
      this.isCycle = topology == PathGraph.Topology.Cycle;
      this.directed = directedness == Directedness.Directed;
    }

    public Node GetNode(int index) => new Node(1L + (long) index);

    public int GetNodeIndex(Node node) => (int) (node.Id - 1L);

    public Arc NextArc(Node node) => !this.isCycle && node.Id == (long) this.nodeCount ? Arc.Invalid : new Arc(node.Id);

    public Arc PrevArc(Node node)
    {
      if (node.Id != 1L)
        return new Arc(node.Id - 1L);
      return !this.isCycle ? Arc.Invalid : new Arc((long) this.nodeCount);
    }

    public Node U(Arc arc) => new Node(arc.Id);

    public Node V(Arc arc) => new Node(arc.Id == (long) this.nodeCount ? 1L : arc.Id + 1L);

    public bool IsEdge(Arc arc) => !this.directed;

    public IEnumerable<Node> Nodes()
    {
      for (int i = 1; i <= this.nodeCount; ++i)
        yield return new Node((long) i);
    }

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
    {
      if (!this.directed || filter != ArcFilter.Edge)
      {
        int i = 1;
        for (int n = this.ArcCountInternal(); i <= n; ++i)
          yield return new Arc((long) i);
      }
    }

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All) => this.ArcsHelper(u, filter);

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.Arcs(u, filter).Where<Arc>((Func<Arc, bool>) (arc => this.Other(arc, u) == v));

    public int NodeCount() => this.nodeCount;

    private int ArcCountInternal()
    {
      if (this.nodeCount == 0)
        return 0;
      return !this.isCycle ? this.nodeCount - 1 : this.nodeCount;
    }

    public int ArcCount(ArcFilter filter = ArcFilter.All) => !this.directed || filter != ArcFilter.Edge ? this.ArcCountInternal() : 0;

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.Arcs(u, filter).Count<Arc>();

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.Arcs(u, v, filter).Count<Arc>();

    public bool HasNode(Node node) => node.Id >= 1L && node.Id <= (long) this.nodeCount;

    public bool HasArc(Arc arc) => arc.Id >= 1L && arc.Id <= (long) this.ArcCountInternal();

    public enum Topology
    {
      Path,
      Cycle,
    }
  }
}
