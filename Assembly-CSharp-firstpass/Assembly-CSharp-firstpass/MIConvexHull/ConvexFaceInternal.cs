// Decompiled with JetBrains decompiler
// Type: MIConvexHull.ConvexFaceInternal
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace MIConvexHull
{
  internal sealed class ConvexFaceInternal
  {
    public int[] AdjacentFaces;
    public int FurthestVertex;
    public int Index;
    public bool InList;
    public bool IsNormalFlipped;
    public ConvexFaceInternal Next;
    public double[] Normal;
    public double Offset;
    public ConvexFaceInternal Previous;
    public int Tag;
    public int[] Vertices;
    public IndexBuffer VerticesBeyond;

    public ConvexFaceInternal(int dimension, int index, IndexBuffer beyondList)
    {
      this.Index = index;
      this.AdjacentFaces = new int[dimension];
      this.VerticesBeyond = beyondList;
      this.Normal = new double[dimension];
      this.Vertices = new int[dimension];
    }
  }
}
