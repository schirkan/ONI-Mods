// Decompiled with JetBrains decompiler
// Type: Satsuma.MinimumCostMatching
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class MinimumCostMatching
  {
    public IGraph Graph { get; private set; }

    public Func<Node, bool> IsRed { get; private set; }

    public Func<Arc, double> Cost { get; private set; }

    public int MinimumMatchingSize { get; private set; }

    public int MaximumMatchingSize { get; private set; }

    public IMatching Matching { get; private set; }

    public MinimumCostMatching(
      IGraph graph,
      Func<Node, bool> isRed,
      Func<Arc, double> cost,
      int minimumMatchingSize = 0,
      int maximumMatchingSize = 2147483647)
    {
      this.Graph = graph;
      this.IsRed = isRed;
      this.Cost = cost;
      this.MinimumMatchingSize = minimumMatchingSize;
      this.MaximumMatchingSize = maximumMatchingSize;
      this.Run();
    }

    private void Run()
    {
      Supergraph supergraph = new Supergraph((IGraph) new RedirectedGraph(this.Graph, (Func<Arc, RedirectedGraph.Direction>) (x => !this.IsRed(this.Graph.U(x)) ? RedirectedGraph.Direction.Backward : RedirectedGraph.Direction.Forward)));
      Node node1 = supergraph.AddNode();
      Node node2 = supergraph.AddNode();
      foreach (Node node3 in this.Graph.Nodes())
      {
        if (this.IsRed(node3))
          supergraph.AddArc(node1, node3, Directedness.Directed);
        else
          supergraph.AddArc(node3, node2, Directedness.Directed);
      }
      Arc reflow = supergraph.AddArc(node2, node1, Directedness.Directed);
      NetworkSimplex networkSimplex = new NetworkSimplex((IGraph) supergraph, (Func<Arc, long>) (x => x == reflow ? (long) this.MinimumMatchingSize : 0L), (Func<Arc, long>) (x => x == reflow ? (long) this.MaximumMatchingSize : 1L), cost: ((Func<Arc, double>) (x => !this.Graph.HasArc(x) ? 0.0 : this.Cost(x))));
      networkSimplex.Run();
      if (networkSimplex.State != SimplexState.Optimal)
        return;
      Satsuma.Matching matching = new Satsuma.Matching(this.Graph);
      foreach (Arc arc in networkSimplex.UpperBoundArcs.Concat<Arc>(networkSimplex.Forest.Where<KeyValuePair<Arc, long>>((Func<KeyValuePair<Arc, long>, bool>) (kv => kv.Value == 1L)).Select<KeyValuePair<Arc, long>, Arc>((Func<KeyValuePair<Arc, long>, Arc>) (kv => kv.Key))))
      {
        if (this.Graph.HasArc(arc))
          matching.Enable(arc, true);
      }
      this.Matching = (IMatching) matching;
    }
  }
}
