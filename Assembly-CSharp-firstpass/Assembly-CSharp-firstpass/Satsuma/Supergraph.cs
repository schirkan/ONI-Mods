// Decompiled with JetBrains decompiler
// Type: Satsuma.Supergraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public class Supergraph : IBuildableGraph, IClearable, IDestroyableGraph, IGraph, IArcLookup
  {
    private IGraph graph;
    private Supergraph.NodeAllocator nodeAllocator;
    private Supergraph.ArcAllocator arcAllocator;
    private HashSet<Node> nodes;
    private HashSet<Arc> arcs;
    private Dictionary<Arc, Supergraph.ArcProperties> arcProperties;
    private HashSet<Arc> edges;
    private Dictionary<Node, List<Arc>> nodeArcs_All;
    private Dictionary<Node, List<Arc>> nodeArcs_Edge;
    private Dictionary<Node, List<Arc>> nodeArcs_Forward;
    private Dictionary<Node, List<Arc>> nodeArcs_Backward;
    private static readonly List<Arc> EmptyArcList = new List<Arc>();

    public Supergraph(IGraph graph)
    {
      this.graph = graph;
      this.nodeAllocator = new Supergraph.NodeAllocator()
      {
        Parent = this
      };
      this.arcAllocator = new Supergraph.ArcAllocator()
      {
        Parent = this
      };
      this.nodes = new HashSet<Node>();
      this.arcs = new HashSet<Arc>();
      this.arcProperties = new Dictionary<Arc, Supergraph.ArcProperties>();
      this.edges = new HashSet<Arc>();
      this.nodeArcs_All = new Dictionary<Node, List<Arc>>();
      this.nodeArcs_Edge = new Dictionary<Node, List<Arc>>();
      this.nodeArcs_Forward = new Dictionary<Node, List<Arc>>();
      this.nodeArcs_Backward = new Dictionary<Node, List<Arc>>();
    }

    public void Clear()
    {
      this.nodeAllocator.Rewind();
      this.arcAllocator.Rewind();
      this.nodes.Clear();
      this.arcs.Clear();
      this.arcProperties.Clear();
      this.edges.Clear();
      this.nodeArcs_All.Clear();
      this.nodeArcs_Edge.Clear();
      this.nodeArcs_Forward.Clear();
      this.nodeArcs_Backward.Clear();
    }

    public Node AddNode()
    {
      if (this.NodeCount() == int.MaxValue)
        throw new InvalidOperationException("Error: too many nodes!");
      Node node = new Node(this.nodeAllocator.Allocate());
      this.nodes.Add(node);
      return node;
    }

    public Arc AddArc(Node u, Node v, Directedness directedness)
    {
      if (this.ArcCount(ArcFilter.All) == int.MaxValue)
        throw new InvalidOperationException("Error: too many arcs!");
      Arc key = new Arc(this.arcAllocator.Allocate());
      this.arcs.Add(key);
      bool isEdge = directedness == Directedness.Undirected;
      this.arcProperties[key] = new Supergraph.ArcProperties(u, v, isEdge);
      Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_All, u).Add(key);
      Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_Forward, u).Add(key);
      Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_Backward, v).Add(key);
      if (isEdge)
      {
        this.edges.Add(key);
        Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_Edge, u).Add(key);
      }
      if (v != u)
      {
        Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_All, v).Add(key);
        if (isEdge)
        {
          Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_Edge, v).Add(key);
          Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_Forward, v).Add(key);
          Utils.MakeEntry<Node, List<Arc>>(this.nodeArcs_Backward, u).Add(key);
        }
      }
      return key;
    }

    public bool DeleteNode(Node node)
    {
      if (!this.nodes.Remove(node))
        return false;
      Func<Arc, bool> condition = (Func<Arc, bool>) (a => this.U(a) == node || this.V(a) == node);
      Utils.RemoveAll<Arc>(this.arcs, condition);
      Utils.RemoveAll<Arc>(this.edges, condition);
      Utils.RemoveAll<Arc, Supergraph.ArcProperties>(this.arcProperties, condition);
      this.nodeArcs_All.Remove(node);
      this.nodeArcs_Edge.Remove(node);
      this.nodeArcs_Forward.Remove(node);
      this.nodeArcs_Backward.Remove(node);
      return true;
    }

    public bool DeleteArc(Arc arc)
    {
      if (!this.arcs.Remove(arc))
        return false;
      Supergraph.ArcProperties arcProperty = this.arcProperties[arc];
      this.arcProperties.Remove(arc);
      Utils.RemoveLast<Arc>(this.nodeArcs_All[arcProperty.U], arc);
      Utils.RemoveLast<Arc>(this.nodeArcs_Forward[arcProperty.U], arc);
      Utils.RemoveLast<Arc>(this.nodeArcs_Backward[arcProperty.V], arc);
      if (arcProperty.IsEdge)
      {
        this.edges.Remove(arc);
        Utils.RemoveLast<Arc>(this.nodeArcs_Edge[arcProperty.U], arc);
      }
      if (arcProperty.V != arcProperty.U)
      {
        Utils.RemoveLast<Arc>(this.nodeArcs_All[arcProperty.V], arc);
        if (arcProperty.IsEdge)
        {
          Utils.RemoveLast<Arc>(this.nodeArcs_Edge[arcProperty.V], arc);
          Utils.RemoveLast<Arc>(this.nodeArcs_Forward[arcProperty.V], arc);
          Utils.RemoveLast<Arc>(this.nodeArcs_Backward[arcProperty.U], arc);
        }
      }
      return true;
    }

    public Node U(Arc arc)
    {
      Supergraph.ArcProperties arcProperties;
      return this.arcProperties.TryGetValue(arc, out arcProperties) ? arcProperties.U : this.graph.U(arc);
    }

    public Node V(Arc arc)
    {
      Supergraph.ArcProperties arcProperties;
      return this.arcProperties.TryGetValue(arc, out arcProperties) ? arcProperties.V : this.graph.V(arc);
    }

    public bool IsEdge(Arc arc)
    {
      Supergraph.ArcProperties arcProperties;
      return this.arcProperties.TryGetValue(arc, out arcProperties) ? arcProperties.IsEdge : this.graph.IsEdge(arc);
    }

    private HashSet<Arc> ArcsInternal(ArcFilter filter) => filter != ArcFilter.All ? this.edges : this.arcs;

    private List<Arc> ArcsInternal(Node v, ArcFilter filter)
    {
      List<Arc> arcList;
      switch (filter)
      {
        case ArcFilter.All:
          this.nodeArcs_All.TryGetValue(v, out arcList);
          break;
        case ArcFilter.Edge:
          this.nodeArcs_Edge.TryGetValue(v, out arcList);
          break;
        case ArcFilter.Forward:
          this.nodeArcs_Forward.TryGetValue(v, out arcList);
          break;
        default:
          this.nodeArcs_Backward.TryGetValue(v, out arcList);
          break;
      }
      return arcList ?? Supergraph.EmptyArcList;
    }

    public IEnumerable<Node> Nodes() => this.graph != null ? this.nodes.Concat<Node>(this.graph.Nodes()) : (IEnumerable<Node>) this.nodes;

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All) => this.graph != null ? this.ArcsInternal(filter).Concat<Arc>(this.graph.Arcs(filter)) : (IEnumerable<Arc>) this.ArcsInternal(filter);

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All) => this.graph == null || this.nodes.Contains(u) ? (IEnumerable<Arc>) this.ArcsInternal(u, filter) : this.ArcsInternal(u, filter).Concat<Arc>(this.graph.Arcs(u, filter));

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      Supergraph graph = this;
      foreach (Arc arc in graph.ArcsInternal(u, filter))
      {
        if (graph.Other(arc, u) == v)
          yield return arc;
      }
      if (graph.graph != null && !graph.nodes.Contains(u) && !graph.nodes.Contains(v))
      {
        foreach (Arc arc in graph.graph.Arcs(u, v, filter))
          yield return arc;
      }
    }

    public int NodeCount() => this.nodes.Count + (this.graph == null ? 0 : this.graph.NodeCount());

    public int ArcCount(ArcFilter filter = ArcFilter.All) => this.ArcsInternal(filter).Count + (this.graph == null ? 0 : this.graph.ArcCount(filter));

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.ArcsInternal(u, filter).Count + (this.graph == null || this.nodes.Contains(u) ? 0 : this.graph.ArcCount(u, filter));

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      int num = 0;
      foreach (Arc arc in this.ArcsInternal(u, filter))
      {
        if (this.Other(arc, u) == v)
          ++num;
      }
      return num + (this.graph == null || this.nodes.Contains(u) || this.nodes.Contains(v) ? 0 : this.graph.ArcCount(u, v, filter));
    }

    public bool HasNode(Node node)
    {
      if (this.nodes.Contains(node))
        return true;
      return this.graph != null && this.graph.HasNode(node);
    }

    public bool HasArc(Arc arc)
    {
      if (this.arcs.Contains(arc))
        return true;
      return this.graph != null && this.graph.HasArc(arc);
    }

    private class NodeAllocator : IdAllocator
    {
      public Supergraph Parent;

      protected override bool IsAllocated(long id) => this.Parent.HasNode(new Node(id));
    }

    private class ArcAllocator : IdAllocator
    {
      public Supergraph Parent;

      protected override bool IsAllocated(long id) => this.Parent.HasArc(new Arc(id));
    }

    private class ArcProperties
    {
      public Node U { get; private set; }

      public Node V { get; private set; }

      public bool IsEdge { get; private set; }

      public ArcProperties(Node u, Node v, bool isEdge)
      {
        this.U = u;
        this.V = v;
        this.IsEdge = isEdge;
      }
    }
  }
}
