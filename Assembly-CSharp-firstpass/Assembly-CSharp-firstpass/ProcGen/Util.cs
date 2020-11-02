// Decompiled with JetBrains decompiler
// Type: ProcGen.Util
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
  public static class Util
  {
    public static HashSet<Vector2> GetPointsOnHermiteCurve(
      Vector2 p0,
      Vector2 p1,
      Vector2 t0,
      Vector2 t1,
      int numberOfPoints)
    {
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      Vector2 vector2_1 = t0 - p0;
      Vector2 vector2_2 = t1 - p1;
      float num1 = 1f / (float) numberOfPoints;
      for (int index = 0; index < numberOfPoints; ++index)
      {
        float num2 = (float) index * num1;
        Vector2 vector2_3 = (float) (2.0 * (double) num2 * (double) num2 * (double) num2 - 3.0 * (double) num2 * (double) num2 + 1.0) * p0 + ((float) ((double) num2 * (double) num2 * (double) num2 - 2.0 * (double) num2 * (double) num2) + num2) * vector2_1 + (float) (-2.0 * (double) num2 * (double) num2 * (double) num2 + 3.0 * (double) num2 * (double) num2) * p1 + (float) ((double) num2 * (double) num2 * (double) num2 - (double) num2 * (double) num2) * vector2_2;
        vector2Set.Add(vector2_3);
      }
      return vector2Set;
    }

    public static HashSet<Vector2> GetPointsOnCatmullRomSpline(
      List<Vector2> controlPoints,
      int numberOfPoints)
    {
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      float num1 = 1f / (float) numberOfPoints;
      for (int index1 = 0; index1 < controlPoints.Count - 1; ++index1)
      {
        Vector2 controlPoint1 = controlPoints[index1];
        Vector2 controlPoint2 = controlPoints[index1 + 1];
        Vector2 vector2_1 = index1 <= 0 ? controlPoints[index1 + 1] - controlPoints[index1] : 0.5f * (controlPoints[index1 + 1] - controlPoints[index1 - 1]);
        Vector2 vector2_2 = index1 >= controlPoints.Count - 2 ? controlPoints[index1 + 1] - controlPoints[index1] : 0.5f * (controlPoints[index1 + 2] - controlPoints[index1]);
        if (index1 == controlPoints.Count - 2)
          num1 = (float) (1.0 / ((double) numberOfPoints - 1.0));
        for (int index2 = 0; index2 < numberOfPoints; ++index2)
        {
          float num2 = (float) index2 * num1;
          Vector2 vector2_3 = (float) (2.0 * (double) num2 * (double) num2 * (double) num2 - 3.0 * (double) num2 * (double) num2 + 1.0) * controlPoint1 + ((float) ((double) num2 * (double) num2 * (double) num2 - 2.0 * (double) num2 * (double) num2) + num2) * vector2_1 + (float) (-2.0 * (double) num2 * (double) num2 * (double) num2 + 3.0 * (double) num2 * (double) num2) * controlPoint2 + (float) ((double) num2 * (double) num2 * (double) num2 - (double) num2 * (double) num2) * vector2_2;
          vector2Set.Add(vector2_3);
        }
      }
      return vector2Set;
    }

    public static List<Vector2I> StaggerLine(
      Vector2 p0,
      Vector2 p1,
      int numberOfBreaks,
      SeededRandom rand,
      float staggerRange = 3f)
    {
      List<Vector2I> vector2IList = new List<Vector2I>();
      if (numberOfBreaks == 0)
        return Util.GetLine(p0, p1);
      Vector2 vector2_1 = p1 - p0;
      Vector2 p0_1 = p0;
      Vector2 vector2_2 = p1;
      for (int index = 0; index < numberOfBreaks; ++index)
      {
        vector2_2 = p0 + vector2_1 * (1f / (float) numberOfBreaks) * (float) index + Vector2.one * rand.RandomRange(-staggerRange, staggerRange);
        vector2IList.AddRange((IEnumerable<Vector2I>) Util.GetLine(p0_1, vector2_2));
        p0_1 = vector2_2;
      }
      vector2IList.AddRange((IEnumerable<Vector2I>) Util.GetLine(vector2_2, p1));
      return vector2IList;
    }

    public static List<Vector2I> GetLine(Vector2 p0, Vector2 p1)
    {
      List<Vector2I> vector2IList = new List<Vector2I>();
      Vector2 vector2 = p1 - p0;
      float num1 = Mathf.Abs(vector2.x);
      float num2 = Mathf.Abs(vector2.y);
      int num3 = -1;
      if ((double) p0.x < (double) p1.x)
        num3 = 1;
      int num4 = -1;
      if ((double) p0.y < (double) p1.y)
        num4 = 1;
      float num5 = 0.0f;
      for (int index = 0; (double) index < (double) num1 + (double) num2; ++index)
      {
        vector2IList.Add(new Vector2I(Mathf.FloorToInt(p0.x), Mathf.FloorToInt(p0.y)));
        float f1 = num5 + num2;
        float f2 = num5 - num1;
        if ((double) Mathf.Abs(f1) < (double) Mathf.Abs(f2))
        {
          p0.x += (float) num3;
          num5 = f1;
        }
        else
        {
          p0.y += (float) num4;
          num5 = f2;
        }
      }
      return vector2IList;
    }

    public static List<Vector2> GetCircle(Vector2 center, int radius)
    {
      int num1 = radius;
      int num2 = 0;
      int num3 = 1 - num1;
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      while (num1 >= num2)
      {
        vector2Set.Add(new Vector2((float) num1 + center.x, (float) num2 + center.y));
        vector2Set.Add(new Vector2((float) num2 + center.x, (float) num1 + center.y));
        vector2Set.Add(new Vector2((float) -num1 + center.x, (float) num2 + center.y));
        vector2Set.Add(new Vector2((float) -num2 + center.x, (float) num1 + center.y));
        vector2Set.Add(new Vector2((float) -num1 + center.x, (float) -num2 + center.y));
        vector2Set.Add(new Vector2((float) -num2 + center.x, (float) -num1 + center.y));
        vector2Set.Add(new Vector2((float) num1 + center.x, (float) -num2 + center.y));
        vector2Set.Add(new Vector2((float) num2 + center.x, (float) -num1 + center.y));
        ++num2;
        if (num3 < 0)
        {
          num3 += 2 * num2 + 1;
        }
        else
        {
          --num1;
          num3 += 2 * (num2 - num1) + 1;
        }
      }
      return new List<Vector2>((IEnumerable<Vector2>) vector2Set);
    }

    private static void get8points(Vector2 c, float x, float y, List<Vector2I> points)
    {
      Vector2 p0_1 = new Vector2(c.x - x, c.y + y);
      Vector2 vector2_1 = new Vector2(c.x + x, c.y + y);
      Vector2 p1_1 = vector2_1;
      List<Vector2I> line1 = Util.GetLine(p0_1, p1_1);
      points.AddRange((IEnumerable<Vector2I>) line1);
      Vector2 p0_2 = new Vector2(c.x - x, c.y - y);
      Vector2 vector2_2 = new Vector2(c.x + x, c.y - y);
      Vector2 p1_2 = vector2_2;
      List<Vector2I> line2 = Util.GetLine(p0_2, p1_2);
      points.AddRange((IEnumerable<Vector2I>) line2);
      if ((double) x == (double) y)
        return;
      Vector2 p0_3 = new Vector2(c.x - y, c.y + x);
      vector2_1 = new Vector2(c.x + y, c.y + x);
      Vector2 p1_3 = vector2_1;
      List<Vector2I> line3 = Util.GetLine(p0_3, p1_3);
      points.AddRange((IEnumerable<Vector2I>) line3);
      Vector2 p0_4 = new Vector2(c.x - y, c.y - x);
      vector2_2 = new Vector2(c.x + y, c.y - x);
      Vector2 p1_4 = vector2_2;
      List<Vector2I> line4 = Util.GetLine(p0_4, p1_4);
      points.AddRange((IEnumerable<Vector2I>) line4);
    }

    public static List<Vector2I> GetFilledCircle(Vector2 center, float radius)
    {
      radius = Mathf.Floor(radius);
      List<Vector2I> points = new List<Vector2I>();
      float num1 = -radius;
      float x = radius;
      float y = 0.0f;
      while ((double) x >= (double) y)
      {
        Util.get8points(center, x, y, points);
        float num2 = num1 + y;
        ++y;
        num1 = num2 + y;
        if ((double) num1 >= 0.0)
        {
          float num3 = num1 - x;
          --x;
          num1 = num3 - x;
        }
      }
      return points;
    }

    public static Vector2 RandomInUnitCircle(System.Random rng = null)
    {
      if (rng == null)
        return UnityEngine.Random.insideUnitCircle;
      double d = rng.NextDouble();
      double num1 = rng.NextDouble();
      double num2 = Math.Sqrt(d);
      return new Vector2((float) (num2 * Math.Cos(num1)), (float) (num2 * Math.Sin(num1)));
    }

    public static List<Vector2I> GetBlob(Vector2 center, float radius, System.Random rng)
    {
      List<Vector2> circle = Util.GetCircle(center, (int) Mathf.Ceil(radius + 0.5f));
      circle.ShuffleSeeded<Vector2>(rng);
      for (int index = 0; index < circle.Count; ++index)
        circle[index] += Util.RandomInUnitCircle(rng) * radius;
      HashSet<Vector2> catmullRomSpline = Util.GetPointsOnCatmullRomSpline(circle, (int) (2.0 * (double) radius * (double) radius));
      HashSet<Vector2I> vector2ISet = new HashSet<Vector2I>();
      foreach (Vector2 vector2 in catmullRomSpline)
        vector2ISet.Add(new Vector2I((int) vector2.x, (int) vector2.y));
      return new List<Vector2I>((IEnumerable<Vector2I>) vector2ISet);
    }

    public static List<Vector2I> GetSplat(Vector2 center, float radius, System.Random rng)
    {
      HashSet<Vector2I> vector2ISet = new HashSet<Vector2I>();
      int num1 = Mathf.RoundToInt((float) (6.28318548202515 * (double) radius * 1.0));
      for (int index = 0; index < num1; ++index)
      {
        double num2 = rng.NextDouble();
        float num3 = (float) (num2 * num2) * radius;
        double num4 = 6.28318548202515 * ((double) index / (double) num1);
        float x = Mathf.Sin((float) num4) * num3;
        float y = Mathf.Cos((float) num4) * num3;
        foreach (Vector2I vector2I in Util.GetLine(center, new Vector2(x, y) + center))
          vector2ISet.Add(vector2I);
      }
      return new List<Vector2I>((IEnumerable<Vector2I>) vector2ISet);
    }

    public static List<Vector2I> GetBorder(HashSet<Vector2I> sourcePoints, int radius)
    {
      HashSet<Vector2I> vector2ISet = new HashSet<Vector2I>();
      IEnumerator<Vector2I> enumerator = (IEnumerator<Vector2I>) sourcePoints.GetEnumerator();
      int num = 0;
      while (enumerator.MoveNext())
      {
        int x = enumerator.Current.x;
        int y = enumerator.Current.y;
        for (int a = x - radius; a <= x + radius; ++a)
        {
          for (int b = y - radius; b <= y + radius; ++b)
          {
            if (a != x || b != y)
            {
              Vector2I vector2I = new Vector2I(a, b);
              if (!sourcePoints.Contains(vector2I))
                vector2ISet.Add(vector2I);
            }
          }
        }
        ++num;
      }
      return new List<Vector2I>((IEnumerable<Vector2I>) vector2ISet);
    }

    public static List<Vector2I> GetFilledRectangle(
      Vector2 center,
      float width,
      float height,
      SeededRandom rand,
      float jitterMaxStep = 2f,
      float jitterRange = 2f)
    {
      HashSet<Vector2I> vector2ISet = new HashSet<Vector2I>();
      if ((double) width < 1.0)
        width = 1f;
      if ((double) height < 1.0)
        height = 1f;
      float num1 = 0.0f;
      float num2 = 0.0f;
      int num3 = (int) ((double) center.x - (double) width / 2.0);
      int num4 = (int) ((double) center.x + (double) width / 2.0);
      int num5 = (int) ((double) center.y - (double) height / 2.0);
      int num6 = (int) ((double) center.y + (double) height / 2.0);
      for (int b = num5; b < num6; ++b)
      {
        num1 = Mathf.Max(-jitterRange, Mathf.Min(num1 + rand.RandomRange(-jitterMaxStep, jitterMaxStep), jitterRange));
        num2 = Mathf.Max(-jitterRange, Mathf.Min(num2 + rand.RandomRange(-jitterMaxStep, jitterMaxStep), jitterRange));
        for (int a = (int) ((double) num3 - (double) num1); (double) a < (double) num4 + (double) num2; ++a)
          vector2ISet.Add(new Vector2I(a, b));
      }
      float num7 = 0.0f;
      float num8 = 0.0f;
      for (int a = num3; a < num4; ++a)
      {
        num7 = Mathf.Max(-jitterRange, Mathf.Min(num7 + rand.RandomRange(-jitterMaxStep, jitterMaxStep), jitterRange));
        num8 = Mathf.Max(-jitterRange, Mathf.Min(num8 + rand.RandomRange(-jitterMaxStep, jitterMaxStep), jitterRange));
        for (int b = (int) ((double) num5 - (double) num7); b < num5; ++b)
          vector2ISet.Add(new Vector2I(a, b));
        for (int b = num6; (double) b < (double) num6 + (double) num8; ++b)
          vector2ISet.Add(new Vector2I(a, b));
      }
      return new List<Vector2I>((IEnumerable<Vector2I>) vector2ISet);
    }

    public static T GetRandom<T>(this T[] tArray, SeededRandom rand) => tArray[rand.RandomRange(0, tArray.Length)];

    public static T GetRandom<T>(this List<T> tList, SeededRandom rand) => tList[rand.RandomRange(0, tList.Count)];
  }
}
