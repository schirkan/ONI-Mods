// Decompiled with JetBrains decompiler
// Type: MIConvexHull.ConvexHull
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace MIConvexHull
{
  public static class ConvexHull
  {
    public static ConvexHull<TVertex, TFace> Create<TVertex, TFace>(
      IList<TVertex> data,
      double PlaneDistanceTolerance = 1E-10)
      where TVertex : IVertex
      where TFace : ConvexFace<TVertex, TFace>, new()
    {
      return ConvexHull<TVertex, TFace>.Create(data, PlaneDistanceTolerance);
    }

    public static ConvexHull<TVertex, DefaultConvexFace<TVertex>> Create<TVertex>(
      IList<TVertex> data,
      double PlaneDistanceTolerance = 1E-10)
      where TVertex : IVertex
    {
      return ConvexHull<TVertex, DefaultConvexFace<TVertex>>.Create(data, PlaneDistanceTolerance);
    }

    public static ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>> Create(
      IList<double[]> data,
      double PlaneDistanceTolerance = 1E-10)
    {
      return ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>>.Create((IList<DefaultVertex>) data.Select<double[], DefaultVertex>((Func<double[], DefaultVertex>) (p => new DefaultVertex()
      {
        Position = p
      })).ToList<DefaultVertex>(), PlaneDistanceTolerance);
    }
  }
}
