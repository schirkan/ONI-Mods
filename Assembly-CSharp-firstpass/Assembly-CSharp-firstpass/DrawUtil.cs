// Decompiled with JetBrains decompiler
// Type: DrawUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class DrawUtil
{
  private static Vector3[] sphere_verts = new Vector3[18]
  {
    new Vector3(-1f, 0.0f, 0.0f),
    new Vector3(-0.7071f, -0.7071f, 0.0f),
    new Vector3(0.0f, -1f, 0.0f),
    new Vector3(0.7071f, -0.7071f, 0.0f),
    new Vector3(-0.7071f, 0.7071f, 0.0f),
    new Vector3(0.0f, 1f, 0.0f),
    new Vector3(0.7071f, 0.7071f, 0.0f),
    new Vector3(-0.7071f, 0.0f, -0.7071f),
    new Vector3(0.0f, 0.0f, -1f),
    new Vector3(0.7071f, 0.0f, -0.7071f),
    new Vector3(-0.7071f, 0.0f, 0.7071f),
    new Vector3(0.0f, 0.0f, 1f),
    new Vector3(0.7071f, 0.0f, 0.7071f),
    new Vector3(1f, 0.0f, 0.0f),
    new Vector3(0.0f, -0.7071f, -0.7071f),
    new Vector3(0.0f, 0.7071f, -0.7071f),
    new Vector3(0.0f, 0.7071f, 0.7071f),
    new Vector3(0.0f, -0.7071f, 0.7071f)
  };
  private static Vector3[] circlePointCache;

  public static void MultiColourGnomon(Vector2 pos, float size, float time = 0.0f) => size *= 0.5f;

  public static void Gnomon(Vector3 pos, float size) => size *= 0.5f;

  public static void Gnomon(Vector3 pos, float size, Color color, float time = 0.0f) => size *= 0.5f;

  public static void Arrow(Vector3 start, Vector3 end, float size, Color color, float time = 0.0f)
  {
    Vector3 forward = end - start;
    if ((double) forward.sqrMagnitude < 1.0 / 1000.0)
      return;
    Quaternion.LookRotation(forward, Vector3.up);
  }

  public static void Circle(Vector3 pos, float radius) => DrawUtil.Circle(pos, radius, Color.white);

  public static void Circle(Vector3 pos, float radius, Color color, Vector3? normal = null, float time = 0.0f)
  {
    Vector3 toDirection = normal ?? Vector3.up;
    int length = 40;
    if (DrawUtil.circlePointCache == null)
    {
      float num = 6.283185f / (float) length;
      DrawUtil.circlePointCache = new Vector3[length];
      for (int index = 0; index < length; ++index)
        DrawUtil.circlePointCache[index] = new Vector3(Mathf.Cos(num * (float) index), Mathf.Sin(num * (float) index), 0.0f);
    }
    Quaternion.FromToRotation(Vector3.forward, toDirection);
    int num1 = 0;
    while (num1 < length - 1)
      ++num1;
  }

  public static void Sphere(Vector3 pos, float radius) => DrawUtil.Sphere(pos, radius, Color.white);

  public static void Box(Vector3 pos, Color color, float size = 1f, float time = 1f)
  {
  }

  public static void Sphere(Vector3 pos, float radius, Color color, float time = 0.0f)
  {
  }
}
