// Decompiled with JetBrains decompiler
// Type: Satsuma.LowpointDfs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  internal class LowpointDfs : Dfs
  {
    protected Dictionary<Node, int> level;
    protected Dictionary<Node, int> lowpoint;

    private void UpdateLowpoint(Node node, int newLowpoint)
    {
      if (this.lowpoint[node] <= newLowpoint)
        return;
      this.lowpoint[node] = newLowpoint;
    }

    protected override void Start(out Dfs.Direction direction)
    {
      direction = Dfs.Direction.Undirected;
      this.level = new Dictionary<Node, int>();
      this.lowpoint = new Dictionary<Node, int>();
    }

    protected override bool NodeEnter(Node node, Arc arc)
    {
      this.level[node] = this.Level;
      this.lowpoint[node] = this.Level;
      return true;
    }

    protected override bool NodeExit(Node node, Arc arc)
    {
      if (arc != Arc.Invalid)
        this.UpdateLowpoint(this.Graph.Other(arc, node), this.lowpoint[node]);
      return true;
    }

    protected override bool BackArc(Node node, Arc arc)
    {
      Node key = this.Graph.Other(arc, node);
      this.UpdateLowpoint(node, this.level[key]);
      return true;
    }

    protected override void StopSearch()
    {
      this.level = (Dictionary<Node, int>) null;
      this.lowpoint = (Dictionary<Node, int>) null;
    }
  }
}
