// Decompiled with JetBrains decompiler
// Type: Satsuma.Dijkstra
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Dijkstra
  {
    private readonly Dictionary<Node, double> distance;
    private readonly Dictionary<Node, Arc> parentArc;
    private readonly PriorityQueue<Node, double> priorityQueue;

    public IGraph Graph { get; private set; }

    public Func<Arc, double> Cost { get; private set; }

    public DijkstraMode Mode { get; private set; }

    public double NullCost { get; private set; }

    public Dijkstra(IGraph graph, Func<Arc, double> cost, DijkstraMode mode)
    {
      this.Graph = graph;
      this.Cost = cost;
      this.Mode = mode;
      this.NullCost = mode == DijkstraMode.Sum ? 0.0 : double.NegativeInfinity;
      this.distance = new Dictionary<Node, double>();
      this.parentArc = new Dictionary<Node, Arc>();
      this.priorityQueue = new PriorityQueue<Node, double>();
    }

    private void ValidateCost(double c)
    {
      if (this.Mode == DijkstraMode.Sum && c < 0.0)
        throw new InvalidOperationException("Invalid cost: " + (object) c);
    }

    public void AddSource(Node node) => this.AddSource(node, this.NullCost);

    public void AddSource(Node node, double nodeCost)
    {
      if (this.Reached(node))
        throw new InvalidOperationException("Cannot add a reached node as a source.");
      this.ValidateCost(nodeCost);
      this.parentArc[node] = Arc.Invalid;
      this.priorityQueue[node] = nodeCost;
    }

    public Node Step()
    {
      if (this.priorityQueue.Count == 0)
        return Node.Invalid;
      double priority1;
      Node node1 = this.priorityQueue.Peek(out priority1);
      this.priorityQueue.Pop();
      if (double.IsPositiveInfinity(priority1))
        return Node.Invalid;
      this.distance[node1] = priority1;
      foreach (Arc arc in this.Graph.Arcs(node1, ArcFilter.Forward))
      {
        Node node2 = this.Graph.Other(arc, node1);
        if (!this.Fixed(node2))
        {
          double num1 = this.Cost(arc);
          this.ValidateCost(num1);
          double num2 = this.Mode == DijkstraMode.Sum ? priority1 + num1 : Math.Max(priority1, num1);
          double priority2;
          if (!this.priorityQueue.TryGetPriority(node2, out priority2))
            priority2 = double.PositiveInfinity;
          if (num2 < priority2)
          {
            this.priorityQueue[node2] = num2;
            this.parentArc[node2] = arc;
          }
        }
      }
      return node1;
    }

    public void Run()
    {
      do
        ;
      while (this.Step() != Node.Invalid);
    }

    public Node RunUntilFixed(Node target)
    {
      if (this.Fixed(target))
        return target;
      Node node;
      do
      {
        node = this.Step();
      }
      while (!(node == Node.Invalid) && !(node == target));
      return node;
    }

    public Node RunUntilFixed(Func<Node, bool> isTarget)
    {
      Node node1 = this.FixedNodes.FirstOrDefault<Node>(isTarget);
      if (node1 != Node.Invalid)
        return node1;
      Node node2;
      do
      {
        node2 = this.Step();
      }
      while (!(node2 == Node.Invalid) && !isTarget(node2));
      return node2;
    }

    public bool Reached(Node node) => this.parentArc.ContainsKey(node);

    public IEnumerable<Node> ReachedNodes => (IEnumerable<Node>) this.parentArc.Keys;

    public bool Fixed(Node node) => this.distance.ContainsKey(node);

    public IEnumerable<Node> FixedNodes => (IEnumerable<Node>) this.distance.Keys;

    public double GetDistance(Node node)
    {
      double num;
      return !this.distance.TryGetValue(node, out num) ? double.PositiveInfinity : num;
    }

    public Arc GetParentArc(Node node)
    {
      Arc arc;
      return !this.parentArc.TryGetValue(node, out arc) ? Arc.Invalid : arc;
    }

    public IPath GetPath(Node node)
    {
      if (!this.Reached(node))
        return (IPath) null;
      Path path = new Path(this.Graph);
      path.Begin(node);
      while (true)
      {
        Arc parentArc = this.GetParentArc(node);
        if (!(parentArc == Arc.Invalid))
        {
          path.AddFirst(parentArc);
          node = this.Graph.Other(parentArc, node);
        }
        else
          break;
      }
      return (IPath) path;
    }
  }
}
