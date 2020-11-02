// Decompiled with JetBrains decompiler
// Type: MIConvexHull.ConvexHull`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace MIConvexHull
{
  public class ConvexHull<TVertex, TFace>
    where TVertex : IVertex
    where TFace : ConvexFace<TVertex, TFace>, new()
  {
    internal ConvexHull()
    {
    }

    public IEnumerable<TVertex> Points { get; internal set; }

    public IEnumerable<TFace> Faces { get; internal set; }

    public static ConvexHull<TVertex, TFace> Create(
      IList<TVertex> data,
      double PlaneDistanceTolerance)
    {
      return data != null ? ConvexHullAlgorithm.GetConvexHull<TVertex, TFace>(data, PlaneDistanceTolerance) : throw new ArgumentNullException("The supplied data is null.");
    }
  }
}
