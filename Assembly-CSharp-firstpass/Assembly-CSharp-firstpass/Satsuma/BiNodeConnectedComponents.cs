﻿// Decompiled with JetBrains decompiler
// Type: Satsuma.BiNodeConnectedComponents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public class BiNodeConnectedComponents
  {
    public IGraph Graph { get; private set; }

    public int Count { get; private set; }

    public List<HashSet<Node>> Components { get; private set; }

    public Dictionary<Node, int> Cutvertices { get; private set; }

    public BiNodeConnectedComponents(IGraph graph, BiNodeConnectedComponents.Flags flags = BiNodeConnectedComponents.Flags.None)
    {
      this.Graph = graph;
      if ((flags & BiNodeConnectedComponents.Flags.CreateComponents) != BiNodeConnectedComponents.Flags.None)
        this.Components = new List<HashSet<Node>>();
      if ((flags & BiNodeConnectedComponents.Flags.CreateCutvertices) != BiNodeConnectedComponents.Flags.None)
        this.Cutvertices = new Dictionary<Node, int>();
      new BiNodeConnectedComponents.BlockDfs()
      {
        Parent = this
      }.Run(graph);
    }

    [System.Flags]
    public enum Flags
    {
      None = 0,
      CreateComponents = 1,
      CreateCutvertices = 2,
    }

    private class BlockDfs : LowpointDfs
    {
      public BiNodeConnectedComponents Parent;
      private Stack<Node> blockStack;
      private bool oneNodeComponent;

      protected override void Start(out Dfs.Direction direction)
      {
        base.Start(out direction);
        if (this.Parent.Components == null)
          return;
        this.blockStack = new Stack<Node>();
      }

      protected override bool NodeEnter(Node node, Arc arc)
      {
        if (!base.NodeEnter(node, arc))
          return false;
        if (this.Parent.Cutvertices != null && arc == Arc.Invalid)
          this.Parent.Cutvertices[node] = -1;
        if (this.Parent.Components != null)
          this.blockStack.Push(node);
        this.oneNodeComponent = arc == Arc.Invalid;
        return true;
      }

      protected override bool NodeExit(Node node, Arc arc)
      {
        if (arc == Arc.Invalid)
        {
          if (this.oneNodeComponent)
          {
            ++this.Parent.Count;
            if (this.Parent.Components != null)
              this.Parent.Components.Add(new HashSet<Node>()
              {
                node
              });
          }
          if (this.Parent.Cutvertices != null && this.Parent.Cutvertices[node] == 0)
            this.Parent.Cutvertices.Remove(node);
          if (this.Parent.Components != null)
            this.blockStack.Clear();
        }
        else
        {
          Node key = this.Graph.Other(arc, node);
          if (this.lowpoint[node] >= this.Level - 1)
          {
            if (this.Parent.Cutvertices != null)
            {
              int num;
              this.Parent.Cutvertices[key] = (this.Parent.Cutvertices.TryGetValue(key, out num) ? num : 0) + 1;
            }
            ++this.Parent.Count;
            if (this.Parent.Components != null)
            {
              HashSet<Node> nodeSet = new HashSet<Node>();
              Node node1;
              do
              {
                node1 = this.blockStack.Pop();
                nodeSet.Add(node1);
              }
              while (!(node1 == node));
              nodeSet.Add(key);
              this.Parent.Components.Add(nodeSet);
            }
          }
        }
        return base.NodeExit(node, arc);
      }
    }
  }
}
