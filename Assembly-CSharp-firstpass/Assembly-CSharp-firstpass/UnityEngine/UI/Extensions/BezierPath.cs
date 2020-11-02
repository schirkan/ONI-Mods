// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Extensions.BezierPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
  public class BezierPath
  {
    public int SegmentsPerCurve = 10;
    public float MINIMUM_SQR_DISTANCE = 0.01f;
    public float DIVISION_THRESHOLD = -0.99f;
    private List<Vector2> controlPoints;
    private int curveCount;

    public BezierPath() => this.controlPoints = new List<Vector2>();

    public void SetControlPoints(List<Vector2> newControlPoints)
    {
      this.controlPoints.Clear();
      this.controlPoints.AddRange((IEnumerable<Vector2>) newControlPoints);
      this.curveCount = (this.controlPoints.Count - 1) / 3;
    }

    public void SetControlPoints(Vector2[] newControlPoints)
    {
      this.controlPoints.Clear();
      this.controlPoints.AddRange((IEnumerable<Vector2>) newControlPoints);
      this.curveCount = (this.controlPoints.Count - 1) / 3;
    }

    public List<Vector2> GetControlPoints() => this.controlPoints;

    public void Interpolate(List<Vector2> segmentPoints, float scale)
    {
      this.controlPoints.Clear();
      if (segmentPoints.Count < 2)
        return;
      for (int index = 0; index < segmentPoints.Count; ++index)
      {
        if (index == 0)
        {
          Vector2 segmentPoint = segmentPoints[index];
          Vector2 vector2_1 = segmentPoints[index + 1] - segmentPoint;
          Vector2 vector2_2 = segmentPoint + scale * vector2_1;
          this.controlPoints.Add(segmentPoint);
          this.controlPoints.Add(vector2_2);
        }
        else if (index == segmentPoints.Count - 1)
        {
          Vector2 segmentPoint1 = segmentPoints[index - 1];
          Vector2 segmentPoint2 = segmentPoints[index];
          Vector2 vector2 = segmentPoint2 - segmentPoint1;
          this.controlPoints.Add(segmentPoint2 - scale * vector2);
          this.controlPoints.Add(segmentPoint2);
        }
        else
        {
          Vector2 segmentPoint1 = segmentPoints[index - 1];
          Vector2 segmentPoint2 = segmentPoints[index];
          Vector2 segmentPoint3 = segmentPoints[index + 1];
          Vector2 normalized = (segmentPoint3 - segmentPoint1).normalized;
          Vector2 vector2_1 = segmentPoint2 - scale * normalized * (segmentPoint2 - segmentPoint1).magnitude;
          Vector2 vector2_2 = segmentPoint2 + scale * normalized * (segmentPoint3 - segmentPoint2).magnitude;
          this.controlPoints.Add(vector2_1);
          this.controlPoints.Add(segmentPoint2);
          this.controlPoints.Add(vector2_2);
        }
      }
      this.curveCount = (this.controlPoints.Count - 1) / 3;
    }

    public void SamplePoints(
      List<Vector2> sourcePoints,
      float minSqrDistance,
      float maxSqrDistance,
      float scale)
    {
      if (sourcePoints.Count < 2)
        return;
      Stack<Vector2> vector2Stack = new Stack<Vector2>();
      vector2Stack.Push(sourcePoints[0]);
      Vector2 sourcePoint = sourcePoints[1];
      Vector2 vector2_1;
      for (int index = 2; index < sourcePoints.Count; ++index)
      {
        vector2_1 = sourcePoint - sourcePoints[index];
        if ((double) vector2_1.sqrMagnitude > (double) minSqrDistance)
        {
          vector2_1 = vector2Stack.Peek() - sourcePoints[index];
          if ((double) vector2_1.sqrMagnitude > (double) maxSqrDistance)
            vector2Stack.Push(sourcePoint);
        }
        sourcePoint = sourcePoints[index];
      }
      Vector2 vector2_2 = vector2Stack.Pop();
      Vector2 vector2_3 = vector2Stack.Peek();
      vector2_1 = vector2_3 - sourcePoint;
      Vector2 normalized = vector2_1.normalized;
      vector2_1 = sourcePoint - vector2_2;
      float magnitude1 = vector2_1.magnitude;
      vector2_1 = vector2_2 - vector2_3;
      float magnitude2 = vector2_1.magnitude;
      Vector2 vector2_4 = vector2_2 + normalized * (float) (((double) magnitude2 - (double) magnitude1) / 2.0);
      vector2Stack.Push(vector2_4);
      vector2Stack.Push(sourcePoint);
      this.Interpolate(new List<Vector2>((IEnumerable<Vector2>) vector2Stack), scale);
    }

    public Vector2 CalculateBezierPoint(int curveIndex, float t)
    {
      int index = curveIndex * 3;
      Vector2 controlPoint1 = this.controlPoints[index];
      Vector2 controlPoint2 = this.controlPoints[index + 1];
      Vector2 controlPoint3 = this.controlPoints[index + 2];
      Vector2 controlPoint4 = this.controlPoints[index + 3];
      return this.CalculateBezierPoint(t, controlPoint1, controlPoint2, controlPoint3, controlPoint4);
    }

    public List<Vector2> GetDrawingPoints0()
    {
      List<Vector2> vector2List = new List<Vector2>();
      for (int curveIndex = 0; curveIndex < this.curveCount; ++curveIndex)
      {
        if (curveIndex == 0)
          vector2List.Add(this.CalculateBezierPoint(curveIndex, 0.0f));
        for (int index = 1; index <= this.SegmentsPerCurve; ++index)
        {
          float t = (float) index / (float) this.SegmentsPerCurve;
          vector2List.Add(this.CalculateBezierPoint(curveIndex, t));
        }
      }
      return vector2List;
    }

    public List<Vector2> GetDrawingPoints1()
    {
      List<Vector2> vector2List = new List<Vector2>();
      for (int index1 = 0; index1 < this.controlPoints.Count - 3; index1 += 3)
      {
        Vector2 controlPoint1 = this.controlPoints[index1];
        Vector2 controlPoint2 = this.controlPoints[index1 + 1];
        Vector2 controlPoint3 = this.controlPoints[index1 + 2];
        Vector2 controlPoint4 = this.controlPoints[index1 + 3];
        if (index1 == 0)
          vector2List.Add(this.CalculateBezierPoint(0.0f, controlPoint1, controlPoint2, controlPoint3, controlPoint4));
        for (int index2 = 1; index2 <= this.SegmentsPerCurve; ++index2)
        {
          float t = (float) index2 / (float) this.SegmentsPerCurve;
          vector2List.Add(this.CalculateBezierPoint(t, controlPoint1, controlPoint2, controlPoint3, controlPoint4));
        }
      }
      return vector2List;
    }

    public List<Vector2> GetDrawingPoints2()
    {
      List<Vector2> vector2List = new List<Vector2>();
      for (int curveIndex = 0; curveIndex < this.curveCount; ++curveIndex)
      {
        List<Vector2> drawingPoints = this.FindDrawingPoints(curveIndex);
        if (curveIndex != 0)
          drawingPoints.RemoveAt(0);
        vector2List.AddRange((IEnumerable<Vector2>) drawingPoints);
      }
      return vector2List;
    }

    private List<Vector2> FindDrawingPoints(int curveIndex)
    {
      List<Vector2> pointList = new List<Vector2>();
      Vector2 bezierPoint1 = this.CalculateBezierPoint(curveIndex, 0.0f);
      Vector2 bezierPoint2 = this.CalculateBezierPoint(curveIndex, 1f);
      pointList.Add(bezierPoint1);
      pointList.Add(bezierPoint2);
      this.FindDrawingPoints(curveIndex, 0.0f, 1f, pointList, 1);
      return pointList;
    }

    private int FindDrawingPoints(
      int curveIndex,
      float t0,
      float t1,
      List<Vector2> pointList,
      int insertionIndex)
    {
      Vector2 bezierPoint1 = this.CalculateBezierPoint(curveIndex, t0);
      Vector2 bezierPoint2 = this.CalculateBezierPoint(curveIndex, t1);
      if ((double) (bezierPoint1 - bezierPoint2).sqrMagnitude < (double) this.MINIMUM_SQR_DISTANCE)
        return 0;
      float num1 = (float) (((double) t0 + (double) t1) / 2.0);
      Vector2 bezierPoint3 = this.CalculateBezierPoint(curveIndex, num1);
      if ((double) Vector2.Dot((bezierPoint1 - bezierPoint3).normalized, (bezierPoint2 - bezierPoint3).normalized) <= (double) this.DIVISION_THRESHOLD && (double) Mathf.Abs(num1 - 0.5f) >= 9.99999974737875E-05)
        return 0;
      int num2 = 0 + this.FindDrawingPoints(curveIndex, t0, num1, pointList, insertionIndex);
      pointList.Insert(insertionIndex + num2, bezierPoint3);
      int num3 = num2 + 1;
      return num3 + this.FindDrawingPoints(curveIndex, num1, t1, pointList, insertionIndex + num3);
    }

    private Vector2 CalculateBezierPoint(
      float t,
      Vector2 p0,
      Vector2 p1,
      Vector2 p2,
      Vector2 p3)
    {
      float num1 = 1f - t;
      float num2 = t * t;
      float num3 = num1 * num1;
      double num4 = (double) num3 * (double) num1;
      float num5 = num2 * t;
      Vector2 vector2 = p0;
      return (float) num4 * vector2 + 3f * num3 * t * p1 + 3f * num1 * num2 * p2 + num5 * p3;
    }
  }
}
