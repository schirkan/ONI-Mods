// Decompiled with JetBrains decompiler
// Type: MIConvexHull.DelaunayTriangulation`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace MIConvexHull
{
  public class DelaunayTriangulation<TVertex, TCell> : ITriangulation<TVertex, TCell>
    where TVertex : IVertex
    where TCell : TriangulationCell<TVertex, TCell>, new()
  {
    private DelaunayTriangulation()
    {
    }

    public IEnumerable<TCell> Cells { get; private set; }

    public static DelaunayTriangulation<TVertex, TCell> Create(
      IList<TVertex> data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (data.Count == 0)
        return new DelaunayTriangulation<TVertex, TCell>()
        {
          Cells = (IEnumerable<TCell>) new TCell[0]
        };
      TCell[] delaunayTriangulation = ConvexHullAlgorithm.GetDelaunayTriangulation<TVertex, TCell>(data);
      return new DelaunayTriangulation<TVertex, TCell>()
      {
        Cells = (IEnumerable<TCell>) delaunayTriangulation
      };
    }
  }
}
