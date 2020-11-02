// Decompiled with JetBrains decompiler
// Type: Satsuma.Path
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Path : IPath, IGraph, IArcLookup, IClearable
  {
    private int nodeCount;
    private Dictionary<Node, Arc> nextArc;
    private Dictionary<Node, Arc> prevArc;
    private HashSet<Arc> arcs;
    private int edgeCount;

    public IGraph Graph { get; private set; }

    public Node FirstNode { get; private set; }

    public Node LastNode { get; private set; }

    public Path(IGraph graph)
    {
      this.Graph = graph;
      this.nextArc = new Dictionary<Node, Arc>();
      this.prevArc = new Dictionary<Node, Arc>();
      this.arcs = new HashSet<Arc>();
      this.Clear();
    }

    public void Clear()
    {
      this.FirstNode = Node.Invalid;
      this.LastNode = Node.Invalid;
      this.nodeCount = 0;
      this.nextArc.Clear();
      this.prevArc.Clear();
      this.arcs.Clear();
      this.edgeCount = 0;
    }

    public void Begin(Node node)
    {
      this.nodeCount = this.nodeCount <= 0 ? 1 : throw new InvalidOperationException("Path not empty.");
      this.FirstNode = this.LastNode = node;
    }

    public void AddFirst(Arc arc)
    {
      Node node1 = this.U(arc);
      Node node2 = this.V(arc);
      Node key = node1 == this.FirstNode ? node2 : node1;
      if (node1 != this.FirstNode && node2 != this.FirstNode || (this.nextArc.ContainsKey(key) || this.prevArc.ContainsKey(this.FirstNode)))
        throw new ArgumentException("Arc not valid or path is a cycle.");
      if (key != this.LastNode)
        ++this.nodeCount;
      this.nextArc[key] = arc;
      this.prevArc[this.FirstNode] = arc;
      if (!this.arcs.Contains(arc))
      {
        this.arcs.Add(arc);
        if (this.IsEdge(arc))
          ++this.edgeCount;
      }
      this.FirstNode = key;
    }

    public void AddLast(Arc arc)
    {
      Node node1 = this.U(arc);
      Node node2 = this.V(arc);
      Node key = node1 == this.LastNode ? node2 : node1;
      if (node1 != this.LastNode && node2 != this.LastNode || (this.nextArc.ContainsKey(this.LastNode) || this.prevArc.ContainsKey(key)))
        throw new ArgumentException("Arc not valid or path is a cycle.");
      if (key != this.FirstNode)
        ++this.nodeCount;
      this.nextArc[this.LastNode] = arc;
      this.prevArc[key] = arc;
      if (!this.arcs.Contains(arc))
      {
        this.arcs.Add(arc);
        if (this.IsEdge(arc))
          ++this.edgeCount;
      }
      this.LastNode = key;
    }

    public void Reverse()
    {
      Node firstNode = this.FirstNode;
      this.FirstNode = this.LastNode;
      this.LastNode = firstNode;
      Dictionary<Node, Arc> nextArc = this.nextArc;
      this.nextArc = this.prevArc;
      this.prevArc = nextArc;
    }

    public Arc NextArc(Node node)
    {
      Arc arc;
      return !this.nextArc.TryGetValue(node, out arc) ? Arc.Invalid : arc;
    }

    public Arc PrevArc(Node node)
    {
      Arc arc;
      return !this.prevArc.TryGetValue(node, out arc) ? Arc.Invalid : arc;
    }

    public Node U(Arc arc) => this.Graph.U(arc);

    public Node V(Arc arc) => this.Graph.V(arc);

    public bool IsEdge(Arc arc) => this.Graph.IsEdge(arc);

    public IEnumerable<Node> Nodes()
    {
      Node n = this.FirstNode;
      if (!(n == Node.Invalid))
      {
        do
        {
          yield return n;
          Arc arc = this.NextArc(n);
          if (arc == Arc.Invalid)
            break;
          n = this.Graph.Other(arc, n);
        }
        while (!(n == this.FirstNode));
      }
    }

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
    {
      if (filter == ArcFilter.All)
        return (IEnumerable<Arc>) this.arcs;
      return this.edgeCount == 0 ? Enumerable.Empty<Arc>() : this.arcs.Where<Arc>((Func<Arc, bool>) (arc => this.IsEdge(arc)));
    }

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All) => this.ArcsHelper(u, filter);

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.Arcs(u, filter).Where<Arc>((Func<Arc, bool>) (arc => this.Other(arc, u) == v));

    public int NodeCount() => this.nodeCount;

    public int ArcCount(ArcFilter filter = ArcFilter.All) => filter != ArcFilter.All ? this.edgeCount : this.arcs.Count;

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All) => this.Arcs(u, filter).Count<Arc>();

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All) => this.Arcs(u, v, filter).Count<Arc>();

    public bool HasNode(Node node)
    {
      if (this.prevArc.ContainsKey(node))
        return true;
      return node != Node.Invalid && node == this.FirstNode;
    }

    public bool HasArc(Arc arc) => this.arcs.Contains(arc);
  }
}
