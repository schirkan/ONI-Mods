// Decompiled with JetBrains decompiler
// Type: Satsuma.ReverseGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public sealed class ReverseGraph : IGraph, IArcLookup
  {
    private IGraph graph;

    public static ArcFilter Reverse(ArcFilter filter)
    {
      if (filter == ArcFilter.Forward)
        return ArcFilter.Backward;
      return filter == ArcFilter.Backward ? ArcFilter.Forward : filter;
    }

    public ReverseGraph(IGraph graph) => this.graph = graph;

    public Node U(Arc arc) => this.graph.V(arc);

    public Node V(Arc arc) => this.graph.U(arc);

    public bool IsEdge(Arc arc) => this.graph.IsEdge(arc);

    public IEnumerable<Node> Nodes() => this.graph.Nodes();

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All) => this.graph.Arcs(filter);

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All) => this.graph.Arcs(u, ReverseGraph.Reverse(filter));

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.graph.Arcs(u, v, ReverseGraph.Reverse(filter));

    public int NodeCount() => this.graph.NodeCount();

    public int ArcCount(ArcFilter filter = ArcFilter.All) => this.graph.ArcCount(filter);

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.graph.ArcCount(u, ReverseGraph.Reverse(filter));

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.graph.ArcCount(u, v, ReverseGraph.Reverse(filter));

    public bool HasNode(Node node) => this.graph.HasNode(node);

    public bool HasArc(Arc arc) => this.graph.HasArc(arc);
  }
}
