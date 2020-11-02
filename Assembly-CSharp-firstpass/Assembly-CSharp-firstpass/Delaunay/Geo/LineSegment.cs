// Decompiled with JetBrains decompiler
// Type: Delaunay.Geo.LineSegment
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay.Geo
{
  public class LineSegment
  {
    public Vector2? p0;
    public Vector2? p1;

    public static int CompareLengths_MAX(LineSegment segment0, LineSegment segment1)
    {
      float num1 = Vector2.Distance(segment0.p0.Value, segment0.p1.Value);
      float num2 = Vector2.Distance(segment1.p0.Value, segment1.p1.Value);
      if ((double) num1 < (double) num2)
        return 1;
      return (double) num1 > (double) num2 ? -1 : 0;
    }

    public static int CompareLengths(LineSegment edge0, LineSegment edge1) => -LineSegment.CompareLengths_MAX(edge0, edge1);

    public LineSegment(Vector2? p0, Vector2? p1)
    {
      this.p0 = p0;
      this.p1 = p1;
    }

    public Vector2? Center()
    {
      if (!this.p0.HasValue)
        return this.p1;
      return !this.p1.HasValue ? this.p0 : new Vector2?(this.p0.Value + 0.5f * this.Direction());
    }

    public Vector2 Direction() => !this.p0.HasValue || !this.p1.HasValue ? Vector2.zero : this.p1.Value - this.p0.Value;

    private static float[] OverlapIntervals(float ub1, float ub2)
    {
      float val2_1 = Math.Min(ub1, ub2);
      float val2_2 = Math.Max(ub1, ub2);
      float num1 = Math.Max(0.0f, val2_1);
      float num2 = Math.Min(1f, val2_2);
      if ((double) num1 > (double) num2)
        return new float[0];
      return (double) num1 == (double) num2 ? new float[1]
      {
        num1
      } : new float[2]{ num1, num2 };
    }

    private static Vector2[] OneD_Intersection(
      Vector2 a1,
      Vector2 a2,
      Vector2 b1,
      Vector2 b2)
    {
      float num1 = a2.x - a1.x;
      float num2 = a2.y - a1.y;
      float ub1;
      float ub2;
      if ((double) Math.Abs(num1) > (double) Math.Abs(num2))
      {
        ub1 = (b1.x - a1.x) / num1;
        ub2 = (b2.x - a1.x) / num1;
      }
      else
      {
        ub1 = (b1.y - a1.y) / num2;
        ub2 = (b2.y - a1.y) / num2;
      }
      List<Vector2> vector2List = new List<Vector2>();
      foreach (float overlapInterval in LineSegment.OverlapIntervals(ub1, ub2))
      {
        Vector2 vector2 = new Vector2((float) ((double) a2.x * (double) overlapInterval + (double) a1.x * (1.0 - (double) overlapInterval)), (float) ((double) a2.y * (double) overlapInterval + (double) a1.y * (1.0 - (double) overlapInterval)));
        vector2List.Add(vector2);
      }
      return vector2List.ToArray();
    }

    private static bool PointOnLine(Vector2 p, Vector2 a1, Vector2 a2)
    {
      float u = 0.0f;
      return LineSegment.DistFromSeg(p, a1, a2, (double) Mathf.Epsilon, ref u) < (double) Mathf.Epsilon;
    }

    private static double DistFromSeg(
      Vector2 p,
      Vector2 q0,
      Vector2 q1,
      double radius,
      ref float u)
    {
      double num1 = (double) q1.x - (double) q0.x;
      double num2 = (double) q1.y - (double) q0.y;
      double num3 = (double) q0.x - (double) p.x;
      double num4 = (double) q0.y - (double) p.y;
      double num5 = Math.Sqrt(num1 * num1 + num2 * num2);
      if (num5 < (double) Mathf.Epsilon)
        throw new Exception("Expected line segment, not point.");
      return Math.Abs(num1 * num4 - num3 * num2) / num5;
    }

    public bool DoesIntersect(LineSegment other) => LineSegment.DoesIntersect(this, other);

    public static bool DoesIntersect(LineSegment a, LineSegment b) => LineSegment.Intersection(a.p0.Value, a.p1.Value, b.p0.Value, b.p1.Value).Length != 0;

    public static LineSegment Intersection(LineSegment a, LineSegment b)
    {
      Vector2[] vector2Array = LineSegment.Intersection(a.p0.Value, a.p1.Value, b.p0.Value, b.p1.Value);
      if (vector2Array.Length == 1)
        return new LineSegment(new Vector2?(vector2Array[0]), new Vector2?());
      return vector2Array.Length == 2 ? new LineSegment(new Vector2?(vector2Array[0]), new Vector2?(vector2Array[1])) : new LineSegment(new Vector2?(), new Vector2?());
    }

    public static Vector2[] Intersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
      if (a1.Equals(a2) && b1.Equals(b2))
      {
        if (!a1.Equals(b1))
          return new Vector2[0];
        return new Vector2[1]{ a1 };
      }
      if (b1.Equals(b2))
      {
        if (!LineSegment.PointOnLine(b1, a1, a2))
          return new Vector2[0];
        return new Vector2[1]{ b1 };
      }
      if (a1.Equals(a2))
      {
        if (!LineSegment.PointOnLine(a1, b1, b2))
          return new Vector2[0];
        return new Vector2[1]{ a1 };
      }
      float num1 = (float) (((double) b2.x - (double) b1.x) * ((double) a1.y - (double) b1.y) - ((double) b2.y - (double) b1.y) * ((double) a1.x - (double) b1.x));
      float num2 = (float) (((double) a2.x - (double) a1.x) * ((double) a1.y - (double) b1.y) - ((double) a2.y - (double) a1.y) * ((double) a1.x - (double) b1.x));
      float num3 = (float) (((double) b2.y - (double) b1.y) * ((double) a2.x - (double) a1.x) - ((double) b2.x - (double) b1.x) * ((double) a2.y - (double) a1.y));
      if (-(double) Mathf.Epsilon >= (double) num3 || (double) num3 >= (double) Mathf.Epsilon)
      {
        float num4 = num1 / num3;
        float num5 = num2 / num3;
        if (0.0 > (double) num4 || (double) num4 > 1.0 || (0.0 > (double) num5 || (double) num5 > 1.0))
          return new Vector2[0];
        return new Vector2[1]
        {
          new Vector2(a1.x + num4 * (a2.x - a1.x), a1.y + num4 * (a2.y - a1.y))
        };
      }
      if ((-(double) Mathf.Epsilon >= (double) num1 || (double) num1 >= (double) Mathf.Epsilon) && (-(double) Mathf.Epsilon >= (double) num2 || (double) num2 >= (double) Mathf.Epsilon))
        return new Vector2[0];
      return a1.Equals(a2) ? LineSegment.OneD_Intersection(b1, b2, a1, a2) : LineSegment.OneD_Intersection(a1, a2, b1, b2);
    }
  }
}
