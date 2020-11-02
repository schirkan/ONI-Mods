// Decompiled with JetBrains decompiler
// Type: Klei.MeshCreator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Klei
{
  public class MeshCreator
  {
    public static void MakePlane(GameObject target, int width, int height, bool hide = true)
    {
      int length = width * height * 2;
      Vector3[] vector3Array = new Vector3[length];
      Vector2[] vector2Array = new Vector2[length];
      int[] indices = new int[length];
      for (int index1 = 0; index1 < width; ++index1)
      {
        for (int index2 = 0; index2 < height; ++index2)
        {
          int index3 = index2 * width + index1;
          float x = 0.5f + (float) index1;
          float y = 0.5f + (float) index2;
          vector3Array[index3] = new Vector3(x, y, 0.0f);
          vector2Array[index3] = new Vector2(x / (float) width, y / (float) height);
          indices[index3] = index3;
        }
      }
      MeshFilter component = target.GetComponent<MeshFilter>();
      Mesh mesh;
      if ((Object) component.sharedMesh == (Object) null)
      {
        mesh = new Mesh();
        mesh.name = "Klei.MeshCreator.Plane";
        component.sharedMesh = mesh;
      }
      else
      {
        component.sharedMesh.Clear();
        mesh = component.sharedMesh;
      }
      mesh.vertices = vector3Array;
      mesh.uv = vector2Array;
      mesh.SetIndices(indices, MeshTopology.Points, 0);
      if (hide)
        mesh.hideFlags = HideFlags.HideInHierarchy;
      mesh.RecalculateBounds();
    }
  }
}
