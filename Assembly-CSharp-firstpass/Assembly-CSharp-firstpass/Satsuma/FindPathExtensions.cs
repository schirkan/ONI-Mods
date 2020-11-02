// Decompiled with JetBrains decompiler
// Type: Satsuma.FindPathExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public static class FindPathExtensions
  {
    public static IPath FindPath(
      this IGraph graph,
      IEnumerable<Node> source,
      Func<Node, bool> target,
      Dfs.Direction direction)
    {
      FindPathExtensions.PathDfs pathDfs = new FindPathExtensions.PathDfs()
      {
        PathDirection = direction,
        IsTarget = target
      };
      pathDfs.Run(graph, source);
      if (pathDfs.EndNode == Node.Invalid)
        return (IPath) null;
      Path path = new Path(graph);
      path.Begin(pathDfs.StartNode);
      foreach (Arc arc in pathDfs.Path)
        path.AddLast(arc);
      return (IPath) path;
    }

    public static IPath FindPath(
      this IGraph graph,
      Node source,
      Node target,
      Dfs.Direction direction)
    {
      return graph.FindPath((IEnumerable<Node>) new Node[1]
      {
        source
      }, (Func<Node, bool>) (x => x == target), direction);
    }

    private class PathDfs : Dfs
    {
      public Dfs.Direction PathDirection;
      public Func<Node, bool> IsTarget;
      public Node StartNode;
      public List<Arc> Path;
      public Node EndNode;

      protected override void Start(out Dfs.Direction direction)
      {
        direction = this.PathDirection;
        this.StartNode = Node.Invalid;
        this.Path = new List<Arc>();
        this.EndNode = Node.Invalid;
      }

      protected override bool NodeEnter(Node node, Arc arc)
      {
        if (arc == Arc.Invalid)
          this.StartNode = node;
        else
          this.Path.Add(arc);
        if (!this.IsTarget(node))
          return true;
        this.EndNode = node;
        return false;
      }

      protected override bool NodeExit(Node node, Arc arc)
      {
        if (arc != Arc.Invalid && this.EndNode == Node.Invalid)
          this.Path.RemoveAt(this.Path.Count - 1);
        return true;
      }
    }
  }
}
