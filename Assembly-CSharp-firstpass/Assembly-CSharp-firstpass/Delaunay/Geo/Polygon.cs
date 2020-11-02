// Decompiled with JetBrains decompiler
// Type: Delaunay.Geo.Polygon
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using ClipperLib;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Delaunay.Geo
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public sealed class Polygon
  {
    [Serialize]
    private List<Vector2> vertices;
    private Vector2? centroid;
    private const int CLIPPER_INTEGER_SCALE = 10000;
    private const float CLIPPER_INVERSE_SCALE = 0.0001f;

    public Rect bounds { get; private set; }

    public List<Vector2> Vertices => this.vertices;

    public Polygon()
    {
    }

    [OnDeserializing]
    internal void OnDeserializingMethod() => this.vertices = new List<Vector2>();

    [OnDeserialized]
    internal void OnDeserializedMethod() => this.Initialize();

    public Polygon(List<Vector2> verts)
    {
      this.vertices = verts;
      this.Initialize();
    }

    public Polygon(Rect bounds)
    {
      this.vertices = new List<Vector2>();
      this.vertices.Add(new Vector2(bounds.x, bounds.y));
      this.vertices.Add(new Vector2(bounds.x + bounds.width, bounds.y));
      this.vertices.Add(new Vector2(bounds.x + bounds.width, bounds.y + bounds.height));
      this.vertices.Add(new Vector2(bounds.x, bounds.y + bounds.height));
      this.Initialize();
    }

    public void Add(Vector2 newVert)
    {
      if (this.vertices == null)
        this.vertices = new List<Vector2>();
      this.vertices.Add(newVert);
    }

    public void Initialize()
    {
      Debug.Assert(this.vertices != null, (object) "No verts added");
      Vector2 vector2_1 = new Vector2(float.MaxValue, float.MaxValue);
      Vector2 vector2_2 = new Vector2(float.MinValue, float.MinValue);
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        if ((double) this.vertices[index].y < (double) vector2_1.y)
          vector2_1.y = this.vertices[index].y;
        if ((double) this.vertices[index].x < (double) vector2_1.x)
          vector2_1.x = this.vertices[index].x;
        if ((double) this.vertices[index].y > (double) vector2_2.y)
          vector2_2.y = this.vertices[index].y;
        if ((double) this.vertices[index].x > (double) vector2_2.x)
          vector2_2.x = this.vertices[index].x;
      }
      this.bounds = Rect.MinMaxRect(vector2_1.x, vector2_1.y, vector2_2.x, vector2_2.y);
    }

    public float MinX => this.vertices.Min<Vector2>((Func<Vector2, float>) (point => point.x));

    public float MinY => this.vertices.Min<Vector2>((Func<Vector2, float>) (point => point.y));

    public float MaxX => this.vertices.Max<Vector2>((Func<Vector2, float>) (point => point.x));

    public float MaxY => this.vertices.Max<Vector2>((Func<Vector2, float>) (point => point.y));

    public float Area() => Mathf.Abs(this.SignedDoubleArea() * 0.5f);

    public Delaunay.Geo.Winding Winding()
    {
      float num = this.SignedDoubleArea();
      if ((double) num < 0.0)
        return Delaunay.Geo.Winding.CLOCKWISE;
      return (double) num > 0.0 ? Delaunay.Geo.Winding.COUNTERCLOCKWISE : Delaunay.Geo.Winding.NONE;
    }

    public void ForceWinding(Delaunay.Geo.Winding wind)
    {
      if (this.Winding() == wind)
        return;
      this.vertices.Reverse();
    }

    private float SignedDoubleArea()
    {
      int count = this.vertices.Count;
      float num = 0.0f;
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = (index1 + 1) % count;
        Vector2 vertex1 = this.vertices[index1];
        Vector2 vertex2 = this.vertices[index2];
        num += (float) ((double) vertex1.x * (double) vertex2.y - (double) vertex2.x * (double) vertex1.y);
      }
      return num;
    }

    public Vector2 Centroid()
    {
      if (!this.centroid.HasValue)
      {
        this.centroid = new Vector2?(Vector2.zero);
        if (this.vertices.Count > 1)
        {
          float num1 = this.Area();
          int index1 = 1;
          Vector2? centroid;
          for (int index2 = 0; index2 < this.vertices.Count; ++index2)
          {
            float num2 = (float) ((double) this.vertices[index2].x * (double) this.vertices[index1].y - (double) this.vertices[index1].x * (double) this.vertices[index2].y);
            centroid = this.centroid;
            Vector2 vector2 = new Vector2((this.vertices[index2].x + this.vertices[index1].x) * num2, (this.vertices[index2].y + this.vertices[index1].y) * num2);
            this.centroid = centroid.HasValue ? new Vector2?(centroid.GetValueOrDefault() + vector2) : new Vector2?();
            index1 = (index1 + 1) % this.vertices.Count;
          }
          centroid = this.centroid;
          float num3 = 6f * num1;
          this.centroid = centroid.HasValue ? new Vector2?(centroid.GetValueOrDefault() / num3) : new Vector2?();
        }
      }
      return this.centroid.Value;
    }

    public bool PointInPolygon(Vector2I point) => this.PointInPolygon(new Vector2((float) point.x, (float) point.y));

    public bool Contains(Vector2 point) => this.PointInPolygon(point);

    public bool PointInPolygon(Vector2 point)
    {
      if (!this.bounds.Contains(point))
        return false;
      int index1 = this.vertices.Count - 1;
      bool flag = false;
      for (int index2 = 0; index2 < this.vertices.Count; index1 = index2++)
      {
        if (((double) this.vertices[index2].y <= (double) point.y && (double) point.y < (double) this.vertices[index1].y || (double) this.vertices[index1].y <= (double) point.y && (double) point.y < (double) this.vertices[index2].y) && (double) point.x < ((double) this.vertices[index1].x - (double) this.vertices[index2].x) * ((double) point.y - (double) this.vertices[index2].y) / ((double) this.vertices[index1].y - (double) this.vertices[index2].y) + (double) this.vertices[index2].x)
          flag = !flag;
      }
      return flag;
    }

    public LineSegment GetEdge(int edgeIndex) => new LineSegment(new Vector2?(this.vertices[edgeIndex]), new Vector2?(this.vertices[(edgeIndex + 1) % this.vertices.Count]));

    public Polygon.Commonality SharesEdgeClosest(Polygon other)
    {
      Polygon.Commonality commonality = Polygon.Commonality.None;
      float timeOnEdge = 0.0f;
      MathUtil.Pair<Vector2, Vector2> closestEdge1 = this.GetClosestEdge(other.Centroid(), ref timeOnEdge);
      MathUtil.Pair<Vector2, Vector2> closestEdge2 = other.GetClosestEdge(this.Centroid(), ref timeOnEdge);
      if ((double) Vector2.Distance(closestEdge1.First, closestEdge2.First) >= 9.99999974737875E-06 && (double) Vector2.Distance(closestEdge1.First, closestEdge2.Second) >= 9.99999974737875E-06)
        return commonality;
      return (double) Vector2.Distance(closestEdge1.Second, closestEdge2.First) < 9.99999974737875E-06 || (double) Vector2.Distance(closestEdge1.Second, closestEdge2.Second) < 9.99999974737875E-06 ? Polygon.Commonality.Edge : Polygon.Commonality.Point;
    }

    public Polygon.Commonality SharesEdge(Polygon other, ref int edgeIdx)
    {
      Polygon.Commonality commonality = Polygon.Commonality.None;
      int index1 = this.vertices.Count - 1;
      for (int index2 = 0; index2 < this.vertices.Count; index1 = index2++)
      {
        Vector2 vertex1 = this.vertices[index1];
        Vector2 vertex2 = this.vertices[index2];
        int index3 = other.vertices.Count - 1;
        for (int index4 = 0; index4 < other.vertices.Count; index3 = index4++)
        {
          Vector2 vertex3 = other.vertices[index3];
          Vector2 vertex4 = other.vertices[index4];
          int num = 0 + ((double) Vector2.Distance(vertex4, vertex2) < 1.0 / 1000.0 ? 1 : 0) + ((double) Vector2.Distance(vertex4, vertex1) < 1.0 / 1000.0 ? 1 : 0) + ((double) Vector2.Distance(vertex3, vertex2) < 1.0 / 1000.0 ? 1 : 0) + ((double) Vector2.Distance(vertex3, vertex1) < 1.0 / 1000.0 ? 1 : 0);
          if (num == 1)
            commonality = Polygon.Commonality.Point;
          if (num > 1)
          {
            edgeIdx = index1;
            return Polygon.Commonality.Edge;
          }
        }
      }
      return commonality;
    }

    public float DistanceToClosestEdge(Vector2? point = null)
    {
      if (!point.HasValue)
        point = new Vector2?(this.Centroid());
      float timeOnEdge = 0.0f;
      MathUtil.Pair<Vector2, Vector2> closestEdge = this.GetClosestEdge(point.Value, ref timeOnEdge);
      Vector2 vector2 = closestEdge.Second - closestEdge.First;
      return Vector2.Distance(closestEdge.First + vector2 * timeOnEdge, point.Value);
    }

    public MathUtil.Pair<Vector2, Vector2> GetClosestEdge(
      Vector2 point,
      ref float timeOnEdge)
    {
      MathUtil.Pair<Vector2, Vector2> pair = (MathUtil.Pair<Vector2, Vector2>) null;
      float closest_point = 0.0f;
      timeOnEdge = 0.0f;
      float num1 = float.MaxValue;
      int index1 = this.vertices.Count - 1;
      for (int index2 = 0; index2 < this.vertices.Count; index1 = index2++)
      {
        MathUtil.Pair<Vector2, Vector2> segment = new MathUtil.Pair<Vector2, Vector2>(this.vertices[index1], this.vertices[index2]);
        float num2 = Mathf.Abs(MathUtil.GetClosestPointBetweenPointAndLineSegment(segment, point, ref closest_point));
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          pair = segment;
          timeOnEdge = closest_point;
        }
      }
      return pair;
    }

    public List<KeyValuePair<MathUtil.Pair<float, float>, MathUtil.Pair<Vector2, Vector2>>> GetEdgesWithinDistance(
      Vector2 point,
      float distance = 3.402823E+38f)
    {
      List<KeyValuePair<MathUtil.Pair<float, float>, MathUtil.Pair<Vector2, Vector2>>> keyValuePairList = new List<KeyValuePair<MathUtil.Pair<float, float>, MathUtil.Pair<Vector2, Vector2>>>();
      float closest_point = 0.0f;
      int index1 = this.vertices.Count - 1;
      for (int index2 = 0; index2 < this.vertices.Count; index1 = index2++)
      {
        MathUtil.Pair<Vector2, Vector2> segment = new MathUtil.Pair<Vector2, Vector2>(this.vertices[index1], this.vertices[index2]);
        MathUtil.Pair<float, float> key = new MathUtil.Pair<float, float>();
        float num = Mathf.Abs(MathUtil.GetClosestPointBetweenPointAndLineSegment(segment, point, ref closest_point));
        if ((double) num < (double) distance)
        {
          key.First = num;
          key.Second = closest_point;
          keyValuePairList.Add(new KeyValuePair<MathUtil.Pair<float, float>, MathUtil.Pair<Vector2, Vector2>>(key, segment));
        }
      }
      keyValuePairList.Sort((Comparison<KeyValuePair<MathUtil.Pair<float, float>, MathUtil.Pair<Vector2, Vector2>>>) ((a, b) => a.Key.First.CompareTo(b.Key.First)));
      return keyValuePairList;
    }

    public bool IsConvex()
    {
      if (this.vertices.Count < 4)
        return true;
      bool flag = false;
      int count = this.vertices.Count;
      for (int index = 0; index < count; ++index)
      {
        double num1 = (double) this.vertices[(index + 2) % count].x - (double) this.vertices[(index + 1) % count].x;
        double num2 = (double) this.vertices[(index + 2) % count].y - (double) this.vertices[(index + 1) % count].y;
        double num3 = (double) this.vertices[index].x - (double) this.vertices[(index + 1) % count].x;
        double num4 = (double) this.vertices[index].y - (double) this.vertices[(index + 1) % count].y;
        double num5 = num1 * num4 - num2 * num3;
        if (index == 0)
          flag = num5 > 0.0;
        else if (flag != num5 > 0.0)
          return false;
      }
      return true;
    }

    private List<IntPoint> GetPath()
    {
      List<IntPoint> intPointList = new List<IntPoint>();
      for (int index = 0; index < this.vertices.Count; ++index)
        intPointList.Add(new IntPoint((double) this.vertices[index].x * 10000.0, (double) this.vertices[index].y * 10000.0));
      return intPointList;
    }

    public Polygon Clip(Polygon clippingPoly, ClipType type = ClipType.ctIntersection)
    {
      List<List<IntPoint>> ppg1 = new List<List<IntPoint>>();
      ppg1.Add(this.GetPath());
      List<List<IntPoint>> ppg2 = new List<List<IntPoint>>();
      ppg2.Add(clippingPoly.GetPath());
      Clipper clipper = new Clipper();
      PolyTree polytree = new PolyTree();
      clipper.AddPaths(ppg1, PolyType.ptSubject, true);
      clipper.AddPaths(ppg2, PolyType.ptClip, true);
      clipper.Execute(type, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
      List<List<IntPoint>> paths = Clipper.PolyTreeToPaths(polytree);
      if (paths.Count <= 0)
        return (Polygon) null;
      List<Vector2> verts = new List<Vector2>();
      for (int index = 0; index < paths[0].Count; ++index)
        verts.Add(new Vector2((float) paths[0][index].X * 0.0001f, (float) paths[0][index].Y * 0.0001f));
      return new Polygon(verts);
    }

    private int CrossingNumber(Vector2 point)
    {
      int num1 = 0;
      for (int index1 = 0; index1 < this.vertices.Count; ++index1)
      {
        int index2 = index1;
        int index3 = index1 < this.vertices.Count - 1 ? index1 + 1 : 0;
        if ((double) this.vertices[index2].y <= (double) point.y && (double) this.vertices[index3].y > (double) point.y || (double) this.vertices[index2].y > (double) point.y && (double) this.vertices[index3].y <= (double) point.y)
        {
          float num2 = (point.y - this.vertices[index2].y) / (this.vertices[index3].y - this.vertices[index2].y);
          if ((double) point.x < (double) this.vertices[index2].x + (double) num2 * ((double) this.vertices[index3].x - (double) this.vertices[index2].x))
            ++num1;
        }
      }
      return num1 & 1;
    }

    private float perp(Vector2 u, Vector2 v) => (float) ((double) u.x * (double) v.y - (double) u.y * (double) v.x);

    public bool ClipSegment(LineSegment segment, ref LineSegment intersectingSegment)
    {
      Vector2 zero1 = Vector2.zero;
      Vector2 zero2 = Vector2.zero;
      return this.ClipSegment(segment, ref intersectingSegment, ref zero1, ref zero2);
    }

    public bool ClipSegment(
      LineSegment segment,
      ref LineSegment intersectingSegment,
      ref Vector2 normNear,
      ref Vector2 normFar)
    {
      normNear = Vector2.zero;
      normFar = Vector2.zero;
      Vector2? p0 = segment.p0;
      Vector2? nullable1 = segment.p1;
      if ((p0.HasValue == nullable1.HasValue ? (p0.HasValue ? (p0.GetValueOrDefault() == nullable1.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
      {
        intersectingSegment = segment;
        return this.CrossingNumber(segment.p0.Value) == 1;
      }
      float num1 = 0.0f;
      float num2 = 1f;
      Vector2 v = segment.Direction();
      for (int index1 = 0; index1 < this.vertices.Count; ++index1)
      {
        int index2 = index1;
        Vector2 u = this.vertices[index1 < this.vertices.Count - 1 ? index1 + 1 : 0] - this.vertices[index2];
        Vector2 vector2 = new Vector2(u.y, -u.x);
        float num3 = this.perp(u, segment.p0.Value - this.vertices[index2]);
        float f = -this.perp(u, v);
        if ((double) Mathf.Abs(f) < (double) Mathf.Epsilon)
        {
          if ((double) num3 < 0.0)
            return false;
        }
        else
        {
          float num4 = num3 / f;
          if ((double) f < 0.0)
          {
            if ((double) num4 > (double) num1)
            {
              num1 = num4;
              normNear = vector2;
              if ((double) num1 > (double) num2)
                return false;
            }
          }
          else if ((double) num4 < (double) num2)
          {
            num2 = num4;
            normFar = vector2;
            if ((double) num2 < (double) num1)
              return false;
          }
        }
      }
      LineSegment lineSegment1 = intersectingSegment;
      nullable1 = segment.p0;
      Vector2 vector2_1 = num1 * v;
      Vector2? nullable2 = nullable1.HasValue ? new Vector2?(nullable1.GetValueOrDefault() + vector2_1) : new Vector2?();
      lineSegment1.p0 = nullable2;
      LineSegment lineSegment2 = intersectingSegment;
      nullable1 = segment.p0;
      Vector2 vector2_2 = num2 * v;
      Vector2? nullable3 = nullable1.HasValue ? new Vector2?(nullable1.GetValueOrDefault() + vector2_2) : new Vector2?();
      lineSegment2.p1 = nullable3;
      normFar.Normalize();
      normNear.Normalize();
      return true;
    }

    public bool ClipSegmentSAT(
      LineSegment segment,
      ref LineSegment intersectingSegment,
      ref Vector2 normNear,
      ref Vector2 normFar)
    {
      normNear = Vector2.zero;
      normFar = Vector2.zero;
      float num1 = 0.0f;
      float num2 = 1f;
      Vector2 u = segment.Direction();
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        Vector2 vertex = this.vertices[index];
        Vector2 vector2 = this.vertices[index < this.vertices.Count - 1 ? index + 1 : 0] - vertex;
        Vector2 v = new Vector2(vector2.y, -vector2.x);
        float num3 = this.perp(vertex - segment.p0.Value, v);
        float f = this.perp(u, v);
        if ((double) Mathf.Abs(f) < (double) Mathf.Epsilon)
        {
          if ((double) num3 < 0.0)
            return false;
        }
        else
        {
          float num4 = num3 / f;
          if ((double) f < 0.0)
          {
            if ((double) num4 > (double) num2)
              return false;
            if ((double) num4 > (double) num1)
            {
              num1 = num4;
              normNear = v;
            }
          }
          else
          {
            if ((double) num4 < (double) num1)
              return false;
            if ((double) num4 < (double) num2)
            {
              num2 = num4;
              normFar = v;
            }
          }
        }
      }
      LineSegment lineSegment1 = intersectingSegment;
      Vector2? p0 = segment.p0;
      Vector2 vector2_1 = num1 * u;
      Vector2? nullable1 = p0.HasValue ? new Vector2?(p0.GetValueOrDefault() + vector2_1) : new Vector2?();
      lineSegment1.p0 = nullable1;
      LineSegment lineSegment2 = intersectingSegment;
      p0 = segment.p0;
      Vector2 vector2_2 = num2 * u;
      Vector2? nullable2 = p0.HasValue ? new Vector2?(p0.GetValueOrDefault() + vector2_2) : new Vector2?();
      lineSegment2.p1 = nullable2;
      normFar.Normalize();
      normNear.Normalize();
      return true;
    }

    public void DebugDraw(Color colour, bool drawCentroid = false, float duration = 1f, float inset = 0.0f)
    {
      Vector2 vector2_1 = this.Centroid();
      for (int index = 0; index < this.vertices.Count; ++index)
      {
        Vector2 vertex1 = this.vertices[index];
        Vector2 vertex2 = this.vertices[index < this.vertices.Count - 1 ? index + 1 : 0];
        if ((double) inset != 0.0)
        {
          Vector2 vector2_2 = vertex1 - vector2_1;
          Vector2 vector2_3 = vector2_2.normalized * -inset;
          vector2_2 = vertex2 - vector2_1;
          Vector2 vector2_4 = vector2_2.normalized * -inset;
        }
      }
      int num = drawCentroid ? 1 : 0;
    }

    public enum Commonality
    {
      None,
      Point,
      Edge,
    }
  }
}
