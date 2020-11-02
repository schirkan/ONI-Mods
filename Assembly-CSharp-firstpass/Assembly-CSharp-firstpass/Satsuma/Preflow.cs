// Decompiled with JetBrains decompiler
// Type: Satsuma.Preflow
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Preflow : IFlow<double>
  {
    private Dictionary<Arc, double> flow;
    private Arc artificialArc;
    private double U;
    private double CapacityMultiplier;

    public IGraph Graph { get; private set; }

    public Func<Arc, double> Capacity { get; private set; }

    public Node Source { get; private set; }

    public Node Target { get; private set; }

    public double FlowSize { get; private set; }

    public double Error { get; private set; }

    public Preflow(IGraph graph, Func<Arc, double> capacity, Node source, Node target)
    {
      this.Graph = graph;
      this.Capacity = capacity;
      this.Source = source;
      this.Target = target;
      this.flow = new Dictionary<Arc, double>();
      Dijkstra dijkstra = new Dijkstra(this.Graph, (Func<Arc, double>) (a => -this.Capacity(a)), DijkstraMode.Maximum);
      dijkstra.AddSource(this.Source);
      dijkstra.RunUntilFixed(this.Target);
      double d = -dijkstra.GetDistance(this.Target);
      if (double.IsPositiveInfinity(d))
      {
        this.FlowSize = double.PositiveInfinity;
        this.Error = 0.0;
        Node node = this.Target;
        Node invalid = Node.Invalid;
        Arc parentArc;
        for (; node != this.Source; node = this.Graph.Other(parentArc, node))
        {
          parentArc = dijkstra.GetParentArc(node);
          this.flow[parentArc] = double.PositiveInfinity;
        }
      }
      else
      {
        if (double.IsNegativeInfinity(d))
          d = 0.0;
        this.U = (double) this.Graph.ArcCount() * d;
        double val2_1 = 0.0;
        foreach (Arc arc in this.Graph.Arcs(this.Source, ArcFilter.Forward))
        {
          if (this.Graph.Other(arc, this.Source) != this.Source)
          {
            val2_1 += this.Capacity(arc);
            if (val2_1 > this.U)
              break;
          }
        }
        this.U = Math.Min(this.U, val2_1);
        double val2_2 = 0.0;
        foreach (Arc arc in this.Graph.Arcs(this.Target, ArcFilter.Backward))
        {
          if (this.Graph.Other(arc, this.Target) != this.Target)
          {
            val2_2 += this.Capacity(arc);
            if (val2_2 > this.U)
              break;
          }
        }
        this.U = Math.Min(this.U, val2_2);
        Supergraph supergraph = new Supergraph(this.Graph);
        Node node = supergraph.AddNode();
        this.artificialArc = supergraph.AddArc(node, this.Source, Directedness.Directed);
        this.CapacityMultiplier = Utils.LargestPowerOfTwo((double) long.MaxValue / this.U);
        if (this.CapacityMultiplier == 0.0)
          this.CapacityMultiplier = 1.0;
        IntegerPreflow integerPreflow = new IntegerPreflow((IGraph) supergraph, new Func<Arc, long>(this.IntegralCapacity), node, this.Target);
        this.FlowSize = (double) integerPreflow.FlowSize / this.CapacityMultiplier;
        this.Error = (double) this.Graph.ArcCount() / this.CapacityMultiplier;
        foreach (KeyValuePair<Arc, long> nonzeroArc in integerPreflow.NonzeroArcs)
          this.flow[nonzeroArc.Key] = (double) nonzeroArc.Value / this.CapacityMultiplier;
      }
    }

    private long IntegralCapacity(Arc arc) => (long) (this.CapacityMultiplier * (arc == this.artificialArc ? this.U : Math.Min(this.U, this.Capacity(arc))));

    public IEnumerable<KeyValuePair<Arc, double>> NonzeroArcs => this.flow.Where<KeyValuePair<Arc, double>>((Func<KeyValuePair<Arc, double>, bool>) (kv => kv.Value != 0.0));

    public double Flow(Arc arc)
    {
      double num;
      this.flow.TryGetValue(arc, out num);
      return num;
    }
  }
}
