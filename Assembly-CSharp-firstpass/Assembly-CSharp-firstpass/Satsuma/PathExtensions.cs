// Decompiled with JetBrains decompiler
// Type: Satsuma.PathExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public static class PathExtensions
  {
    public static bool IsCycle(this IPath path) => path.FirstNode == path.LastNode && path.ArcCount() > 0;

    public static Node NextNode(this IPath path, Node node)
    {
      Arc arc = path.NextArc(node);
      return arc == Arc.Invalid ? Node.Invalid : path.Other(arc, node);
    }

    public static Node PrevNode(this IPath path, Node node)
    {
      Arc arc = path.PrevArc(node);
      return arc == Arc.Invalid ? Node.Invalid : path.Other(arc, node);
    }

    internal static IEnumerable<Arc> ArcsHelper(
      this IPath path,
      Node u,
      ArcFilter filter)
    {
      Arc arc1 = path.PrevArc(u);
      Arc arc2 = path.NextArc(u);
      if (arc1 == arc2)
        arc2 = Arc.Invalid;
      for (int i = 0; i < 2; ++i)
      {
        Arc arc = i == 0 ? arc1 : arc2;
        if (!(arc == Arc.Invalid))
        {
          switch (filter)
          {
            case ArcFilter.All:
              yield return arc;
              continue;
            case ArcFilter.Edge:
              if (path.IsEdge(arc))
              {
                yield return arc;
                continue;
              }
              continue;
            case ArcFilter.Forward:
              if (path.IsEdge(arc) || path.U(arc) == u)
              {
                yield return arc;
                continue;
              }
              continue;
            case ArcFilter.Backward:
              if (path.IsEdge(arc) || path.V(arc) == u)
              {
                yield return arc;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
  }
}
