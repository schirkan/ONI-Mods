// Decompiled with JetBrains decompiler
// Type: MIConvexHull.VoronoiEdge`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace MIConvexHull
{
  public class VoronoiEdge<TVertex, TCell>
    where TVertex : IVertex
    where TCell : TriangulationCell<TVertex, TCell>
  {
    public VoronoiEdge()
    {
    }

    public VoronoiEdge(TCell source, TCell target)
    {
      this.Source = source;
      this.Target = target;
    }

    public TCell Source { get; internal set; }

    public TCell Target { get; internal set; }

    public override bool Equals(object obj)
    {
      if (!(obj is VoronoiEdge<TVertex, TCell> voronoiEdge))
        return false;
      if (this == voronoiEdge || (object) this.Source == (object) voronoiEdge.Source && (object) this.Target == (object) voronoiEdge.Target)
        return true;
      return (object) this.Source == (object) voronoiEdge.Target && (object) this.Target == (object) voronoiEdge.Source;
    }

    public override int GetHashCode() => (23 * 31 + this.Source.GetHashCode()) * 31 + this.Target.GetHashCode();
  }
}
