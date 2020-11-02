// Decompiled with JetBrains decompiler
// Type: Satsuma.NetworkSimplex
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class NetworkSimplex : IClearable
  {
    private double Epsilon;
    private Supergraph MyGraph;
    private Node ArtificialNode;
    private HashSet<Arc> ArtificialArcs;
    private Dictionary<Arc, long> Tree;
    private Subgraph TreeSubgraph;
    private HashSet<Arc> Saturated;
    private Dictionary<Node, double> Potential;
    private IEnumerator<Arc> EnteringArcEnumerator;

    public IGraph Graph { get; private set; }

    public Func<Arc, long> LowerBound { get; private set; }

    public Func<Arc, long> UpperBound { get; private set; }

    public Func<Node, long> Supply { get; private set; }

    public Func<Arc, double> Cost { get; private set; }

    public SimplexState State { get; private set; }

    public NetworkSimplex(
      IGraph graph,
      Func<Arc, long> lowerBound = null,
      Func<Arc, long> upperBound = null,
      Func<Node, long> supply = null,
      Func<Arc, double> cost = null)
    {
      this.Graph = graph;
      this.LowerBound = lowerBound ?? (Func<Arc, long>) (x => 0L);
      this.UpperBound = upperBound ?? (Func<Arc, long>) (x => long.MaxValue);
      this.Supply = supply ?? (Func<Node, long>) (x => 0L);
      this.Cost = cost ?? (Func<Arc, double>) (x => 1.0);
      this.Epsilon = 1.0;
      foreach (Arc arc in graph.Arcs())
      {
        double num = Math.Abs(this.Cost(arc));
        if (num > 0.0 && num < this.Epsilon)
          this.Epsilon = num;
      }
      this.Epsilon *= 1E-12;
      this.Clear();
    }

    public long Flow(Arc arc)
    {
      if (this.Saturated.Contains(arc))
        return this.UpperBound(arc);
      long num1;
      if (this.Tree.TryGetValue(arc, out num1))
        return num1;
      long num2 = this.LowerBound(arc);
      return num2 != long.MinValue ? num2 : 0L;
    }

    public IEnumerable<KeyValuePair<Arc, long>> Forest => this.Tree.Where<KeyValuePair<Arc, long>>((Func<KeyValuePair<Arc, long>, bool>) (kv => this.Graph.HasArc(kv.Key)));

    public IEnumerable<Arc> UpperBoundArcs => (IEnumerable<Arc>) this.Saturated;

    public void Clear()
    {
      Dictionary<Node, long> dictionary1 = new Dictionary<Node, long>();
      foreach (Node node in this.Graph.Nodes())
        dictionary1[node] = this.Supply(node);
      this.Saturated = new HashSet<Arc>();
      foreach (Arc arc in this.Graph.Arcs())
      {
        long num1 = this.LowerBound(arc);
        if (this.UpperBound(arc) < long.MaxValue)
          this.Saturated.Add(arc);
        long num2 = this.Flow(arc);
        dictionary1[this.Graph.U(arc)] -= num2;
        dictionary1[this.Graph.V(arc)] += num2;
      }
      this.Potential = new Dictionary<Node, double>();
      this.MyGraph = new Supergraph(this.Graph);
      this.ArtificialNode = this.MyGraph.AddNode();
      this.Potential[this.ArtificialNode] = 0.0;
      this.ArtificialArcs = new HashSet<Arc>();
      Dictionary<Node, Arc> dictionary2 = new Dictionary<Node, Arc>();
      foreach (Node node in this.Graph.Nodes())
      {
        long num = dictionary1[node];
        Arc arc = num > 0L ? this.MyGraph.AddArc(node, this.ArtificialNode, Directedness.Directed) : this.MyGraph.AddArc(this.ArtificialNode, node, Directedness.Directed);
        this.Potential[node] = num > 0L ? -1.0 : 1.0;
        this.ArtificialArcs.Add(arc);
        dictionary2[node] = arc;
      }
      this.Tree = new Dictionary<Arc, long>();
      this.TreeSubgraph = new Subgraph((IGraph) this.MyGraph);
      this.TreeSubgraph.EnableAllArcs(false);
      foreach (KeyValuePair<Node, Arc> keyValuePair in dictionary2)
      {
        this.Tree[keyValuePair.Value] = Math.Abs(dictionary1[keyValuePair.Key]);
        this.TreeSubgraph.Enable(keyValuePair.Value, true);
      }
      this.State = SimplexState.FirstPhase;
      this.EnteringArcEnumerator = this.MyGraph.Arcs(ArcFilter.All).GetEnumerator();
      this.EnteringArcEnumerator.MoveNext();
    }

    private long ActualLowerBound(Arc arc) => !this.ArtificialArcs.Contains(arc) ? this.LowerBound(arc) : 0L;

    private long ActualUpperBound(Arc arc)
    {
      if (!this.ArtificialArcs.Contains(arc))
        return this.UpperBound(arc);
      return this.State != SimplexState.FirstPhase ? 0L : long.MaxValue;
    }

    private double ActualCost(Arc arc)
    {
      if (this.ArtificialArcs.Contains(arc))
        return 1.0;
      return this.State != SimplexState.FirstPhase ? this.Cost(arc) : 0.0;
    }

    private static ulong MySubtract(long a, long b) => a == long.MaxValue || b == long.MinValue ? ulong.MaxValue : (ulong) (a - b);

    public void Step()
    {
      if (this.State != SimplexState.FirstPhase && this.State != SimplexState.SecondPhase)
        return;
      Arc current1 = this.EnteringArcEnumerator.Current;
      Arc arc1 = Arc.Invalid;
      double num1 = double.NaN;
      bool flag1 = false;
      do
      {
        Arc current2 = this.EnteringArcEnumerator.Current;
        if (!this.Tree.ContainsKey(current2))
        {
          bool flag2 = this.Saturated.Contains(current2);
          double num2 = this.ActualCost(current2) - (this.Potential[this.MyGraph.V(current2)] - this.Potential[this.MyGraph.U(current2)]);
          if (num2 < -this.Epsilon && !flag2 || num2 > this.Epsilon && (flag2 || this.ActualLowerBound(current2) == long.MinValue))
          {
            arc1 = current2;
            num1 = num2;
            flag1 = flag2;
            break;
          }
        }
        if (!this.EnteringArcEnumerator.MoveNext())
        {
          this.EnteringArcEnumerator = this.MyGraph.Arcs(ArcFilter.All).GetEnumerator();
          this.EnteringArcEnumerator.MoveNext();
        }
      }
      while (!(this.EnteringArcEnumerator.Current == current1));
      if (arc1 == Arc.Invalid)
      {
        if (this.State == SimplexState.FirstPhase)
        {
          this.State = SimplexState.SecondPhase;
          foreach (Arc artificialArc in this.ArtificialArcs)
          {
            if (this.Flow(artificialArc) > 0L)
            {
              this.State = SimplexState.Infeasible;
              break;
            }
          }
          if (this.State != SimplexState.SecondPhase)
            return;
          new NetworkSimplex.RecalculatePotentialDfs()
          {
            Parent = this
          }.Run((IGraph) this.TreeSubgraph);
        }
        else
          this.State = SimplexState.Optimal;
      }
      else
      {
        Node node1 = this.MyGraph.U(arc1);
        Node node2 = this.MyGraph.V(arc1);
        List<Arc> arcList1 = new List<Arc>();
        List<Arc> arcList2 = new List<Arc>();
        IPath path = this.TreeSubgraph.FindPath(node2, node1, Dfs.Direction.Undirected);
        foreach (Node node3 in path.Nodes())
        {
          Arc arc2 = path.NextArc(node3);
          (this.MyGraph.U(arc2) == node3 ? arcList1 : arcList2).Add(arc2);
        }
        ulong num2 = num1 < 0.0 ? NetworkSimplex.MySubtract(this.ActualUpperBound(arc1), this.Flow(arc1)) : NetworkSimplex.MySubtract(this.Flow(arc1), this.ActualLowerBound(arc1));
        Arc arc3 = arc1;
        bool flag2 = !flag1;
        foreach (Arc arc2 in arcList1)
        {
          ulong num3 = num1 < 0.0 ? NetworkSimplex.MySubtract(this.ActualUpperBound(arc2), this.Tree[arc2]) : NetworkSimplex.MySubtract(this.Tree[arc2], this.ActualLowerBound(arc2));
          if (num3 < num2)
          {
            num2 = num3;
            arc3 = arc2;
            flag2 = num1 < 0.0;
          }
        }
        foreach (Arc arc2 in arcList2)
        {
          ulong num3 = num1 > 0.0 ? NetworkSimplex.MySubtract(this.ActualUpperBound(arc2), this.Tree[arc2]) : NetworkSimplex.MySubtract(this.Tree[arc2], this.ActualLowerBound(arc2));
          if (num3 < num2)
          {
            num2 = num3;
            arc3 = arc2;
            flag2 = num1 > 0.0;
          }
        }
        long num4 = 0;
        switch (num2)
        {
          case 0:
            if (arc3 == arc1)
            {
              if (flag1)
              {
                this.Saturated.Remove(arc1);
                break;
              }
              this.Saturated.Add(arc1);
              break;
            }
            this.Tree.Remove(arc3);
            this.TreeSubgraph.Enable(arc3, false);
            if (flag2)
              this.Saturated.Add(arc3);
            double num5 = this.ActualCost(arc1) - (this.Potential[node2] - this.Potential[node1]);
            if (num5 != 0.0)
              new NetworkSimplex.UpdatePotentialDfs()
              {
                Parent = this,
                Diff = num5
              }.Run((IGraph) this.TreeSubgraph, (IEnumerable<Node>) new Node[1]
              {
                node2
              });
            this.Tree[arc1] = this.Flow(arc1) + num4;
            if (flag1)
              this.Saturated.Remove(arc1);
            this.TreeSubgraph.Enable(arc1, true);
            break;
          case ulong.MaxValue:
            this.State = SimplexState.Unbounded;
            break;
          default:
            num4 = num1 < 0.0 ? (long) num2 : -(long) num2;
            foreach (Arc key in arcList1)
              this.Tree[key] += num4;
            using (List<Arc>.Enumerator enumerator = arcList2.GetEnumerator())
            {
              while (enumerator.MoveNext())
                this.Tree[enumerator.Current] -= num4;
              goto case 0;
            }
        }
      }
    }

    public void Run()
    {
      while (this.State == SimplexState.FirstPhase || this.State == SimplexState.SecondPhase)
        this.Step();
    }

    private class RecalculatePotentialDfs : Dfs
    {
      public NetworkSimplex Parent;

      protected override void Start(out Dfs.Direction direction) => direction = Dfs.Direction.Undirected;

      protected override bool NodeEnter(Node node, Arc arc)
      {
        if (arc == Arc.Invalid)
        {
          this.Parent.Potential[node] = 0.0;
        }
        else
        {
          Node key = this.Parent.MyGraph.Other(arc, node);
          this.Parent.Potential[node] = this.Parent.Potential[key] + (node == this.Parent.MyGraph.V(arc) ? this.Parent.ActualCost(arc) : -this.Parent.ActualCost(arc));
        }
        return true;
      }
    }

    private class UpdatePotentialDfs : Dfs
    {
      public NetworkSimplex Parent;
      public double Diff;

      protected override void Start(out Dfs.Direction direction) => direction = Dfs.Direction.Undirected;

      protected override bool NodeEnter(Node node, Arc arc)
      {
        this.Parent.Potential[node] += this.Diff;
        return true;
      }
    }
  }
}
