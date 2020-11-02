// Decompiled with JetBrains decompiler
// Type: MIConvexHull.Triangulation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace MIConvexHull
{
  public static class Triangulation
  {
    public static ITriangulation<TVertex, DefaultTriangulationCell<TVertex>> CreateDelaunay<TVertex>(
      IList<TVertex> data)
      where TVertex : IVertex
    {
      return (ITriangulation<TVertex, DefaultTriangulationCell<TVertex>>) DelaunayTriangulation<TVertex, DefaultTriangulationCell<TVertex>>.Create(data);
    }

    public static ITriangulation<DefaultVertex, DefaultTriangulationCell<DefaultVertex>> CreateDelaunay(
      IList<double[]> data)
    {
      return (ITriangulation<DefaultVertex, DefaultTriangulationCell<DefaultVertex>>) DelaunayTriangulation<DefaultVertex, DefaultTriangulationCell<DefaultVertex>>.Create((IList<DefaultVertex>) data.Select<double[], DefaultVertex>((Func<double[], DefaultVertex>) (p => new DefaultVertex()
      {
        Position = p
      })).ToList<DefaultVertex>());
    }

    public static ITriangulation<TVertex, TFace> CreateDelaunay<TVertex, TFace>(
      IList<TVertex> data)
      where TVertex : IVertex
      where TFace : TriangulationCell<TVertex, TFace>, new()
    {
      return (ITriangulation<TVertex, TFace>) DelaunayTriangulation<TVertex, TFace>.Create(data);
    }

    public static VoronoiMesh<TVertex, TCell, TEdge> CreateVoronoi<TVertex, TCell, TEdge>(
      IList<TVertex> data)
      where TVertex : IVertex
      where TCell : TriangulationCell<TVertex, TCell>, new()
      where TEdge : VoronoiEdge<TVertex, TCell>, new()
    {
      return VoronoiMesh<TVertex, TCell, TEdge>.Create(data);
    }

    public static VoronoiMesh<TVertex, DefaultTriangulationCell<TVertex>, VoronoiEdge<TVertex, DefaultTriangulationCell<TVertex>>> CreateVoronoi<TVertex>(
      IList<TVertex> data)
      where TVertex : IVertex
    {
      return VoronoiMesh<TVertex, DefaultTriangulationCell<TVertex>, VoronoiEdge<TVertex, DefaultTriangulationCell<TVertex>>>.Create(data);
    }

    public static VoronoiMesh<DefaultVertex, DefaultTriangulationCell<DefaultVertex>, VoronoiEdge<DefaultVertex, DefaultTriangulationCell<DefaultVertex>>> CreateVoronoi(
      IList<double[]> data)
    {
      return VoronoiMesh<DefaultVertex, DefaultTriangulationCell<DefaultVertex>, VoronoiEdge<DefaultVertex, DefaultTriangulationCell<DefaultVertex>>>.Create((IList<DefaultVertex>) data.Select<double[], DefaultVertex>((Func<double[], DefaultVertex>) (p => new DefaultVertex()
      {
        Position = ((IEnumerable<double>) p).ToArray<double>()
      })).ToList<DefaultVertex>());
    }

    public static VoronoiMesh<TVertex, TCell, VoronoiEdge<TVertex, TCell>> CreateVoronoi<TVertex, TCell>(
      IList<TVertex> data)
      where TVertex : IVertex
      where TCell : TriangulationCell<TVertex, TCell>, new()
    {
      return VoronoiMesh<TVertex, TCell, VoronoiEdge<TVertex, TCell>>.Create(data);
    }
  }
}
