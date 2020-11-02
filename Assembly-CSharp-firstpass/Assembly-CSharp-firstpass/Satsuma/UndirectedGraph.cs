// Decompiled with JetBrains decompiler
// Type: Satsuma.UndirectedGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public sealed class UndirectedGraph : IGraph, IArcLookup
  {
    private IGraph graph;

    public UndirectedGraph(IGraph graph) => this.graph = graph;

    public Node U(Arc arc) => this.graph.U(arc);

    public Node V(Arc arc) => this.graph.V(arc);

    public bool IsEdge(Arc arc) => true;

    public IEnumerable<Node> Nodes() => this.graph.Nodes();

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All) => this.graph.Arcs();

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All) => this.graph.Arcs(u);

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.graph.Arcs(u, v);

    public int NodeCount() => this.graph.NodeCount();

    public int ArcCount(ArcFilter filter = ArcFilter.All) => this.graph.ArcCount();

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.graph.ArcCount(u);

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.graph.ArcCount(u, v);

    public bool HasNode(Node node) => this.graph.HasNode(node);

    public bool HasArc(Arc arc) => this.graph.HasArc(arc);
  }
}
