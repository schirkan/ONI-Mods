// Decompiled with JetBrains decompiler
// Type: MIConvexHull.VoronoiMesh
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace MIConvexHull
{
  public static class VoronoiMesh
  {
    public static VoronoiMesh<TVertex, TCell, TEdge> Create<TVertex, TCell, TEdge>(
      IList<TVertex> data)
      where TVertex : IVertex
      where TCell : TriangulationCell<TVertex, TCell>, new()
      where TEdge : VoronoiEdge<TVertex, TCell>, new()
    {
      return VoronoiMesh<TVertex, TCell, TEdge>.Create(data);
    }

    public static VoronoiMesh<TVertex, DefaultTriangulationCell<TVertex>, VoronoiEdge<TVertex, DefaultTriangulationCell<TVertex>>> Create<TVertex>(
      IList<TVertex> data)
      where TVertex : IVertex
    {
      return VoronoiMesh<TVertex, DefaultTriangulationCell<TVertex>, VoronoiEdge<TVertex, DefaultTriangulationCell<TVertex>>>.Create(data);
    }

    public static VoronoiMesh<DefaultVertex, DefaultTriangulationCell<DefaultVertex>, VoronoiEdge<DefaultVertex, DefaultTriangulationCell<DefaultVertex>>> Create(
      IList<double[]> data)
    {
      return VoronoiMesh<DefaultVertex, DefaultTriangulationCell<DefaultVertex>, VoronoiEdge<DefaultVertex, DefaultTriangulationCell<DefaultVertex>>>.Create((IList<DefaultVertex>) data.Select<double[], DefaultVertex>((Func<double[], DefaultVertex>) (p => new DefaultVertex()
      {
        Position = ((IEnumerable<double>) p).ToArray<double>()
      })).ToList<DefaultVertex>());
    }

    public static VoronoiMesh<TVertex, TCell, VoronoiEdge<TVertex, TCell>> Create<TVertex, TCell>(
      IList<TVertex> data)
      where TVertex : IVertex
      where TCell : TriangulationCell<TVertex, TCell>, new()
    {
      return VoronoiMesh<TVertex, TCell, VoronoiEdge<TVertex, TCell>>.Create(data);
    }
  }
}
