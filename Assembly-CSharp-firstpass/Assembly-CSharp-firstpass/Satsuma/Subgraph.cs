// Decompiled with JetBrains decompiler
// Type: Satsuma.Subgraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Subgraph : IGraph, IArcLookup
  {
    private IGraph graph;
    private bool defaultNodeEnabled;
    private HashSet<Node> nodeExceptions = new HashSet<Node>();
    private bool defaultArcEnabled;
    private HashSet<Arc> arcExceptions = new HashSet<Arc>();

    public Subgraph(IGraph graph)
    {
      this.graph = graph;
      this.EnableAllNodes(true);
      this.EnableAllArcs(true);
    }

    public void EnableAllNodes(bool enabled)
    {
      this.defaultNodeEnabled = enabled;
      this.nodeExceptions.Clear();
    }

    public void EnableAllArcs(bool enabled)
    {
      this.defaultArcEnabled = enabled;
      this.arcExceptions.Clear();
    }

    public void Enable(Node node, bool enabled)
    {
      if (this.defaultNodeEnabled != enabled)
        this.nodeExceptions.Add(node);
      else
        this.nodeExceptions.Remove(node);
    }

    public void Enable(Arc arc, bool enabled)
    {
      if (this.defaultArcEnabled != enabled)
        this.arcExceptions.Add(arc);
      else
        this.arcExceptions.Remove(arc);
    }

    public bool IsEnabled(Node node) => this.defaultNodeEnabled ^ this.nodeExceptions.Contains(node);

    public bool IsEnabled(Arc arc) => this.defaultArcEnabled ^ this.arcExceptions.Contains(arc);

    public Node U(Arc arc) => this.graph.U(arc);

    public Node V(Arc arc) => this.graph.V(arc);

    public bool IsEdge(Arc arc) => this.graph.IsEdge(arc);

    private IEnumerable<Node> NodesInternal()
    {
      foreach (Node node in this.graph.Nodes())
      {
        if (this.IsEnabled(node))
          yield return node;
      }
    }

    public IEnumerable<Node> Nodes()
    {
      if (this.nodeExceptions.Count != 0)
        return this.NodesInternal();
      return this.defaultNodeEnabled ? this.graph.Nodes() : Enumerable.Empty<Node>();
    }

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
    {
      foreach (Arc arc in this.graph.Arcs(filter))
      {
        if (this.IsEnabled(arc) && this.IsEnabled(this.graph.U(arc)) && this.IsEnabled(this.graph.V(arc)))
          yield return arc;
      }
    }

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All)
    {
      if (this.IsEnabled(u))
      {
        foreach (Arc arc in this.graph.Arcs(u, filter))
        {
          if (this.IsEnabled(arc) && this.IsEnabled(this.graph.Other(arc, u)))
            yield return arc;
        }
      }
    }

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      if (this.IsEnabled(u) && this.IsEnabled(v))
      {
        foreach (Arc arc in this.graph.Arcs(u, v, filter))
        {
          if (this.IsEnabled(arc))
            yield return arc;
        }
      }
    }

    public int NodeCount() => !this.defaultNodeEnabled ? this.nodeExceptions.Count : this.graph.NodeCount() - this.nodeExceptions.Count;

    public int ArcCount(ArcFilter filter = ArcFilter.All)
    {
      if (this.nodeExceptions.Count != 0 || filter != ArcFilter.All)
        return this.Arcs(filter).Count<Arc>();
      if (!this.defaultNodeEnabled)
        return 0;
      return !this.defaultArcEnabled ? this.arcExceptions.Count : this.graph.ArcCount() - this.arcExceptions.Count;
    }

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.Arcs(u, filter).Count<Arc>();

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.Arcs(u, v, filter).Count<Arc>();

    public bool HasNode(Node node) => this.graph.HasNode(node) && this.IsEnabled(node);

    public bool HasArc(Arc arc) => this.graph.HasArc(arc) && this.IsEnabled(arc);
  }
}
