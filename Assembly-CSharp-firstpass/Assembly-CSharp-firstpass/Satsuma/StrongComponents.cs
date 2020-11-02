// Decompiled with JetBrains decompiler
// Type: Satsuma.StrongComponents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public sealed class StrongComponents
  {
    public IGraph Graph { get; private set; }

    public int Count { get; private set; }

    public List<HashSet<Node>> Components { get; private set; }

    public StrongComponents(IGraph graph, StrongComponents.Flags flags = StrongComponents.Flags.None)
    {
      this.Graph = graph;
      if ((flags & StrongComponents.Flags.CreateComponents) != StrongComponents.Flags.None)
        this.Components = new List<HashSet<Node>>();
      StrongComponents.ForwardDfs forwardDfs = new StrongComponents.ForwardDfs();
      forwardDfs.Run(graph);
      new StrongComponents.BackwardDfs() { Parent = this }.Run(graph, (IEnumerable<Node>) forwardDfs.ReverseExitOrder);
    }

    [System.Flags]
    public enum Flags
    {
      None = 0,
      CreateComponents = 1,
    }

    private class ForwardDfs : Dfs
    {
      public List<Node> ReverseExitOrder;

      protected override void Start(out Dfs.Direction direction)
      {
        direction = Dfs.Direction.Forward;
        this.ReverseExitOrder = new List<Node>();
      }

      protected override bool NodeExit(Node node, Arc arc)
      {
        this.ReverseExitOrder.Add(node);
        return true;
      }

      protected override void StopSearch() => this.ReverseExitOrder.Reverse();
    }

    private class BackwardDfs : Dfs
    {
      public StrongComponents Parent;

      protected override void Start(out Dfs.Direction direction) => direction = Dfs.Direction.Backward;

      protected override bool NodeEnter(Node node, Arc arc)
      {
        if (arc == Arc.Invalid)
        {
          ++this.Parent.Count;
          if (this.Parent.Components != null)
            this.Parent.Components.Add(new HashSet<Node>()
            {
              node
            });
        }
        else if (this.Parent.Components != null)
          this.Parent.Components[this.Parent.Components.Count - 1].Add(node);
        return true;
      }
    }
  }
}
