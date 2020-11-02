// Decompiled with JetBrains decompiler
// Type: Satsuma.HamiltonianCycle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class HamiltonianCycle
  {
    public IGraph Graph { get; private set; }

    public IPath Cycle { get; private set; }

    public HamiltonianCycle(IGraph graph)
    {
      this.Graph = graph;
      this.Cycle = (IPath) null;
      this.Run();
    }

    private void Run()
    {
      Func<Node, Node, double> cost = (Func<Node, Node, double>) ((u, v) => this.Graph.Arcs(u, v, ArcFilter.Forward).Any<Arc>() ? 1.0 : 10.0);
      IEnumerable<Node> source = (IEnumerable<Node>) null;
      double num = (double) this.Graph.NodeCount();
      InsertionTsp<Node> insertionTsp = new InsertionTsp<Node>(this.Graph.Nodes(), cost);
      insertionTsp.Run();
      if (insertionTsp.TourCost == num)
      {
        source = insertionTsp.Tour;
      }
      else
      {
        Opt2Tsp<Node> opt2Tsp = new Opt2Tsp<Node>(cost, insertionTsp.Tour, new double?(insertionTsp.TourCost));
        opt2Tsp.Run();
        if (opt2Tsp.TourCost == num)
          source = opt2Tsp.Tour;
      }
      if (source == null)
      {
        this.Cycle = (IPath) null;
      }
      else
      {
        Path path = new Path(this.Graph);
        if (source.Any<Node>())
        {
          Node u = Node.Invalid;
          foreach (Node node in source)
          {
            if (u == Node.Invalid)
              path.Begin(node);
            else
              path.AddLast(this.Graph.Arcs(u, node, ArcFilter.Forward).First<Arc>());
            u = node;
          }
          path.AddLast(this.Graph.Arcs(u, source.First<Node>(), ArcFilter.Forward).First<Arc>());
        }
        this.Cycle = (IPath) path;
      }
    }
  }
}
