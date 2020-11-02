// Decompiled with JetBrains decompiler
// Type: Satsuma.Matching
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Matching : IMatching, IGraph, IArcLookup, IClearable
  {
    private readonly Dictionary<Node, Arc> matchedArc;
    private readonly HashSet<Arc> arcs;
    private int edgeCount;

    public IGraph Graph { get; private set; }

    public Matching(IGraph graph)
    {
      this.Graph = graph;
      this.matchedArc = new Dictionary<Node, Arc>();
      this.arcs = new HashSet<Arc>();
      this.Clear();
    }

    public void Clear()
    {
      this.matchedArc.Clear();
      this.arcs.Clear();
      this.edgeCount = 0;
    }

    public void Enable(Arc arc, bool enabled)
    {
      if (enabled == this.arcs.Contains(arc))
        return;
      Node key1 = this.Graph.U(arc);
      Node key2 = this.Graph.V(arc);
      if (enabled)
      {
        if (key1 == key2)
          throw new ArgumentException("Matchings cannot have loop arcs.");
        if (this.matchedArc.ContainsKey(key1))
          throw new ArgumentException("Node is already matched: " + (object) key1);
        if (this.matchedArc.ContainsKey(key2))
          throw new ArgumentException("Node is already matched: " + (object) key2);
        this.matchedArc[key1] = arc;
        this.matchedArc[key2] = arc;
        this.arcs.Add(arc);
        if (!this.Graph.IsEdge(arc))
          return;
        ++this.edgeCount;
      }
      else
      {
        this.matchedArc.Remove(key1);
        this.matchedArc.Remove(key2);
        this.arcs.Remove(arc);
        if (!this.Graph.IsEdge(arc))
          return;
        --this.edgeCount;
      }
    }

    public Arc MatchedArc(Node node)
    {
      Arc arc;
      return !this.matchedArc.TryGetValue(node, out arc) ? Arc.Invalid : arc;
    }

    public Node U(Arc arc) => this.Graph.U(arc);

    public Node V(Arc arc) => this.Graph.V(arc);

    public bool IsEdge(Arc arc) => this.Graph.IsEdge(arc);

    public IEnumerable<Node> Nodes() => (IEnumerable<Node>) this.matchedArc.Keys;

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
    {
      if (filter == ArcFilter.All)
        return (IEnumerable<Arc>) this.arcs;
      return this.edgeCount == 0 ? Enumerable.Empty<Arc>() : this.arcs.Where<Arc>((Func<Arc, bool>) (arc => this.IsEdge(arc)));
    }

    private bool YieldArc(Node u, ArcFilter filter, Arc arc)
    {
      if (filter == ArcFilter.All || this.IsEdge(arc) || filter == ArcFilter.Forward && this.U(arc) == u)
        return true;
      return filter == ArcFilter.Backward && this.V(arc) == u;
    }

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All)
    {
      Arc arc = this.MatchedArc(u);
      if (arc != Arc.Invalid && this.YieldArc(u, filter, arc))
        yield return arc;
    }

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      if (u != v)
      {
        Arc arc = this.MatchedArc(u);
        if (arc != Arc.Invalid && arc == this.MatchedArc(v) && this.YieldArc(u, filter, arc))
          yield return arc;
      }
    }

    public int NodeCount() => this.matchedArc.Count;

    public int ArcCount(ArcFilter filter = ArcFilter.All) => filter != ArcFilter.All ? this.edgeCount : this.arcs.Count;

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All)
    {
      Arc arc = this.MatchedArc(u);
      return !(arc != Arc.Invalid) || !this.YieldArc(u, filter, arc) ? 0 : 1;
    }

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      if (!(u != v))
        return 0;
      Arc arc = this.MatchedArc(u);
      return !(arc != Arc.Invalid) || !(arc == this.MatchedArc(v)) || !this.YieldArc(u, filter, arc) ? 0 : 1;
    }

    public bool HasNode(Node node) => this.Graph.HasNode(node) && this.matchedArc.ContainsKey(node);

    public bool HasArc(Arc arc) => this.Graph.HasArc(arc) && this.arcs.Contains(arc);
  }
}
