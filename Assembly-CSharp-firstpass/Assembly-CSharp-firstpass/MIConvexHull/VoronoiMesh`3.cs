// Decompiled with JetBrains decompiler
// Type: MIConvexHull.VoronoiMesh`3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace MIConvexHull
{
  public class VoronoiMesh<TVertex, TCell, TEdge>
    where TVertex : IVertex
    where TCell : TriangulationCell<TVertex, TCell>, new()
    where TEdge : VoronoiEdge<TVertex, TCell>, new()
  {
    private VoronoiMesh()
    {
    }

    public IEnumerable<TCell> Vertices { get; private set; }

    public IEnumerable<TEdge> Edges { get; private set; }

    public static VoronoiMesh<TVertex, TCell, TEdge> Create(IList<TVertex> data)
    {
      List<TCell> cellList = data != null ? DelaunayTriangulation<TVertex, TCell>.Create(data).Cells.ToList<TCell>() : throw new ArgumentNullException(nameof (data));
      HashSet<TEdge> source = new HashSet<TEdge>((IEqualityComparer<TEdge>) new VoronoiMesh<TVertex, TCell, TEdge>.EdgeComparer());
      foreach (TCell cell1 in cellList)
      {
        for (int index = 0; index < cell1.Adjacency.Length; ++index)
        {
          TCell cell2 = cell1.Adjacency[index];
          if ((object) cell2 != null)
          {
            HashSet<TEdge> edgeSet = source;
            TEdge edge = new TEdge();
            edge.Source = cell1;
            edge.Target = cell2;
            edgeSet.Add(edge);
          }
        }
      }
      return new VoronoiMesh<TVertex, TCell, TEdge>()
      {
        Vertices = (IEnumerable<TCell>) cellList,
        Edges = (IEnumerable<TEdge>) source.ToList<TEdge>()
      };
    }

    private class EdgeComparer : IEqualityComparer<TEdge>
    {
      public bool Equals(TEdge x, TEdge y)
      {
        if ((object) x.Source == (object) y.Source && (object) x.Target == (object) y.Target)
          return true;
        return (object) x.Source == (object) y.Target && (object) x.Target == (object) y.Source;
      }

      public int GetHashCode(TEdge obj) => obj.Source.GetHashCode() ^ obj.Target.GetHashCode();
    }
  }
}
