// Decompiled with JetBrains decompiler
// Type: Satsuma.RedirectedGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class RedirectedGraph : IGraph, IArcLookup
  {
    private IGraph graph;
    private Func<Arc, RedirectedGraph.Direction> getDirection;

    public RedirectedGraph(IGraph graph, Func<Arc, RedirectedGraph.Direction> getDirection)
    {
      this.graph = graph;
      this.getDirection = getDirection;
    }

    public Node U(Arc arc) => this.getDirection(arc) != RedirectedGraph.Direction.Backward ? this.graph.U(arc) : this.graph.V(arc);

    public Node V(Arc arc) => this.getDirection(arc) != RedirectedGraph.Direction.Backward ? this.graph.V(arc) : this.graph.U(arc);

    public bool IsEdge(Arc arc) => this.getDirection(arc) == RedirectedGraph.Direction.Edge;

    public IEnumerable<Node> Nodes() => this.graph.Nodes();

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All) => filter != ArcFilter.All ? this.graph.Arcs().Where<Arc>((Func<Arc, bool>) (x => this.getDirection(x) == RedirectedGraph.Direction.Edge)) : this.graph.Arcs();

    private IEnumerable<Arc> FilterArcs(
      Node u,
      IEnumerable<Arc> arcs,
      ArcFilter filter)
    {
      switch (filter)
      {
        case ArcFilter.All:
          return arcs;
        case ArcFilter.Edge:
          return arcs.Where<Arc>((Func<Arc, bool>) (x => this.getDirection(x) == RedirectedGraph.Direction.Edge));
        case ArcFilter.Forward:
          return arcs.Where<Arc>((Func<Arc, bool>) (x =>
          {
            switch (this.getDirection(x))
            {
              case RedirectedGraph.Direction.Forward:
                return this.U(x) == u;
              case RedirectedGraph.Direction.Backward:
                return this.V(x) == u;
              default:
                return true;
            }
          }));
        default:
          return arcs.Where<Arc>((Func<Arc, bool>) (x =>
          {
            switch (this.getDirection(x))
            {
              case RedirectedGraph.Direction.Forward:
                return this.V(x) == u;
              case RedirectedGraph.Direction.Backward:
                return this.U(x) == u;
              default:
                return true;
            }
          }));
      }
    }

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All) => this.FilterArcs(u, this.graph.Arcs(u), filter);

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.FilterArcs(u, this.graph.Arcs(u, v), filter);

    public int NodeCount() => this.graph.NodeCount();

    public int ArcCount(ArcFilter filter = ArcFilter.All) => filter != ArcFilter.All ? this.Arcs(filter).Count<Arc>() : this.graph.ArcCount();

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.Arcs(u, filter).Count<Arc>();

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.Arcs(u, v, filter).Count<Arc>();

    public bool HasNode(Node node) => this.graph.HasNode(node);

    public bool HasArc(Arc arc) => this.graph.HasArc(arc);

    public enum Direction
    {
      Forward,
      Backward,
      Edge,
    }
  }
}
