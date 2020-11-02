// Decompiled with JetBrains decompiler
// Type: MIConvexHull.FaceConnector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace MIConvexHull
{
  internal sealed class FaceConnector
  {
    public int EdgeIndex;
    public ConvexFaceInternal Face;
    public uint HashCode;
    public FaceConnector Next;
    public FaceConnector Previous;
    public int[] Vertices;

    public FaceConnector(int dimension) => this.Vertices = new int[dimension - 1];

    public void Update(ConvexFaceInternal face, int edgeIndex, int dim)
    {
      this.Face = face;
      this.EdgeIndex = edgeIndex;
      uint num1 = 23;
      int[] vertices = face.Vertices;
      int num2 = 0;
      for (int index = 0; index < edgeIndex; ++index)
      {
        this.Vertices[num2++] = vertices[index];
        num1 += (uint) (31 * (int) num1 + vertices[index]);
      }
      for (int index = edgeIndex + 1; index < vertices.Length; ++index)
      {
        this.Vertices[num2++] = vertices[index];
        num1 += (uint) (31 * (int) num1 + vertices[index]);
      }
      this.HashCode = num1;
    }

    public static bool AreConnectable(FaceConnector a, FaceConnector b, int dim)
    {
      if ((int) a.HashCode != (int) b.HashCode)
        return false;
      int[] vertices1 = a.Vertices;
      int[] vertices2 = b.Vertices;
      for (int index = 0; index < vertices1.Length; ++index)
      {
        if (vertices1[index] != vertices2[index])
          return false;
      }
      return true;
    }

    public static void Connect(FaceConnector a, FaceConnector b)
    {
      a.Face.AdjacentFaces[a.EdgeIndex] = b.Face.Index;
      b.Face.AdjacentFaces[b.EdgeIndex] = a.Face.Index;
    }
  }
}
