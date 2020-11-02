// Decompiled with JetBrains decompiler
// Type: Satsuma.ContractedGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class ContractedGraph : IGraph, IArcLookup
  {
    private IGraph graph;
    private DisjointSet<Node> nodeGroups;
    private int unionCount;

    public ContractedGraph(IGraph graph)
    {
      this.graph = graph;
      this.nodeGroups = new DisjointSet<Node>();
      this.Reset();
    }

    public void Reset()
    {
      this.nodeGroups.Clear();
      this.unionCount = 0;
    }

    public Node Merge(Node u, Node v)
    {
      DisjointSetSet<Node> a = this.nodeGroups.WhereIs(u);
      DisjointSetSet<Node> disjointSetSet = this.nodeGroups.WhereIs(v);
      if (a.Equals(disjointSetSet))
        return a.Representative;
      ++this.unionCount;
      return this.nodeGroups.Union(a, disjointSetSet).Representative;
    }

    public Node Contract(Arc arc) => this.Merge(this.graph.U(arc), this.graph.V(arc));

    public Node U(Arc arc) => this.nodeGroups.WhereIs(this.graph.U(arc)).Representative;

    public Node V(Arc arc) => this.nodeGroups.WhereIs(this.graph.V(arc)).Representative;

    public bool IsEdge(Arc arc) => this.graph.IsEdge(arc);

    public IEnumerable<Node> Nodes()
    {
      foreach (Node node in this.graph.Nodes())
      {
        if (this.nodeGroups.WhereIs(node).Representative == node)
          yield return node;
      }
    }

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All) => this.graph.Arcs(filter);

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All)
    {
      foreach (Node element in this.nodeGroups.Elements(this.nodeGroups.WhereIs(u)))
      {
        foreach (Arc arc in this.graph.Arcs(element, filter))
        {
          if (!(this.U(arc) == this.V(arc)) || filter != ArcFilter.All && !this.IsEdge(arc) || this.graph.U(arc) == element)
            yield return arc;
        }
      }
    }

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      ContractedGraph graph = this;
      foreach (Arc arc in graph.Arcs(u, filter))
      {
        if (graph.Other(arc, u) == v)
          yield return arc;
      }
    }

    public int NodeCount() => this.graph.NodeCount() - this.unionCount;

    public int ArcCount(ArcFilter filter = ArcFilter.All) => this.graph.ArcCount(filter);

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.Arcs(u, filter).Count<Arc>();

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.Arcs(u, v, filter).Count<Arc>();

    public bool HasNode(Node node) => node == this.nodeGroups.WhereIs(node).Representative;

    public bool HasArc(Arc arc) => this.graph.HasArc(arc);
  }
}
