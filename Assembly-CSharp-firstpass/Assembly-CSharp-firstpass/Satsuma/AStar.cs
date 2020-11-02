// Decompiled with JetBrains decompiler
// Type: Satsuma.AStar
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Satsuma
{
  public sealed class AStar
  {
    private Dijkstra dijkstra;

    public IGraph Graph { get; private set; }

    public Func<Arc, double> Cost { get; private set; }

    public Func<Node, double> Heuristic { get; private set; }

    public AStar(IGraph graph, Func<Arc, double> cost, Func<Node, double> heuristic)
    {
      this.Graph = graph;
      this.Cost = cost;
      this.Heuristic = heuristic;
      this.dijkstra = new Dijkstra(this.Graph, (Func<Arc, double>) (arc => this.Cost(arc) - this.Heuristic(this.Graph.U(arc)) + this.Heuristic(this.Graph.V(arc))), DijkstraMode.Sum);
    }

    private Node CheckTarget(Node node)
    {
      if (node != Node.Invalid && this.Heuristic(node) != 0.0)
        throw new ArgumentException("Heuristic is nonzero for a target");
      return node;
    }

    public void AddSource(Node node) => this.dijkstra.AddSource(node, this.Heuristic(node));

    public Node RunUntilReached(Node target) => this.CheckTarget(this.dijkstra.RunUntilFixed(target));

    public Node RunUntilReached(Func<Node, bool> isTarget) => this.CheckTarget(this.dijkstra.RunUntilFixed(isTarget));

    public double GetDistance(Node node)
    {
      this.CheckTarget(node);
      return !this.dijkstra.Fixed(node) ? double.PositiveInfinity : this.dijkstra.GetDistance(node);
    }

    public IPath GetPath(Node node)
    {
      this.CheckTarget(node);
      return !this.dijkstra.Fixed(node) ? (IPath) null : this.dijkstra.GetPath(node);
    }
  }
}
