// Decompiled with JetBrains decompiler
// Type: MIConvexHull.TriangulationCell`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace MIConvexHull
{
  public abstract class TriangulationCell<TVertex, TCell> : ConvexFace<TVertex, TCell>
    where TVertex : IVertex
    where TCell : ConvexFace<TVertex, TCell>
  {
  }
}
