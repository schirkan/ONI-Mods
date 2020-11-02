// Decompiled with JetBrains decompiler
// Type: Satsuma.BiEdgeConnectedComponents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public sealed class BiEdgeConnectedComponents
  {
    public IGraph Graph { get; private set; }

    public int Count { get; private set; }

    public List<HashSet<Node>> Components { get; private set; }

    public HashSet<Arc> Bridges { get; private set; }

    public BiEdgeConnectedComponents(IGraph graph, BiEdgeConnectedComponents.Flags flags = BiEdgeConnectedComponents.Flags.None)
    {
      this.Graph = graph;
      BridgeDfs bridgeDfs = new BridgeDfs();
      bridgeDfs.Run(graph);
      this.Count = bridgeDfs.ComponentCount;
      if ((flags & BiEdgeConnectedComponents.Flags.CreateBridges) != BiEdgeConnectedComponents.Flags.None)
        this.Bridges = bridgeDfs.Bridges;
      if ((flags & BiEdgeConnectedComponents.Flags.CreateComponents) == BiEdgeConnectedComponents.Flags.None)
        return;
      Subgraph subgraph = new Subgraph(graph);
      foreach (Arc bridge in bridgeDfs.Bridges)
        subgraph.Enable(bridge, false);
      this.Components = new ConnectedComponents((IGraph) subgraph, ConnectedComponents.Flags.CreateComponents).Components;
    }

    [System.Flags]
    public enum Flags
    {
      None = 0,
      CreateComponents = 1,
      CreateBridges = 2,
    }
  }
}
