// Decompiled with JetBrains decompiler
// Type: Satsuma.BridgeDfs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  internal class BridgeDfs : LowpointDfs
  {
    public int ComponentCount;
    public HashSet<Arc> Bridges;

    protected override void Start(out Dfs.Direction direction)
    {
      base.Start(out direction);
      this.ComponentCount = 0;
      this.Bridges = new HashSet<Arc>();
    }

    protected override bool NodeExit(Node node, Arc arc)
    {
      if (arc == Arc.Invalid)
        ++this.ComponentCount;
      else if (this.lowpoint[node] == this.Level)
      {
        this.Bridges.Add(arc);
        ++this.ComponentCount;
      }
      return base.NodeExit(node, arc);
    }
  }
}
