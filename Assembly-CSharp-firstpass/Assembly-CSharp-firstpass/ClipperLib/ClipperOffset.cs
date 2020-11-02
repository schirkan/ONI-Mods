﻿// Decompiled with JetBrains decompiler
// Type: ClipperLib.ClipperOffset
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ClipperLib
{
  public class ClipperOffset
  {
    private List<List<IntPoint>> m_destPolys;
    private List<IntPoint> m_srcPoly;
    private List<IntPoint> m_destPoly;
    private List<DoublePoint> m_normals = new List<DoublePoint>();
    private double m_delta;
    private double m_sinA;
    private double m_sin;
    private double m_cos;
    private double m_miterLim;
    private double m_StepsPerRad;
    private IntPoint m_lowest;
    private PolyNode m_polyNodes = new PolyNode();
    private const double two_pi = 6.28318530717959;
    private const double def_arc_tolerance = 0.25;

    public double ArcTolerance { get; set; }

    public double MiterLimit { get; set; }

    public ClipperOffset(double miterLimit = 2.0, double arcTolerance = 0.25)
    {
      this.MiterLimit = miterLimit;
      this.ArcTolerance = arcTolerance;
      this.m_lowest.X = -1L;
    }

    public void Clear()
    {
      this.m_polyNodes.Childs.Clear();
      this.m_lowest.X = -1L;
    }

    internal static long Round(double value) => value >= 0.0 ? (long) (value + 0.5) : (long) (value - 0.5);

    public void AddPath(List<IntPoint> path, JoinType joinType, EndType endType)
    {
      int index1 = path.Count - 1;
      if (index1 < 0)
        return;
      PolyNode Child = new PolyNode();
      Child.m_jointype = joinType;
      Child.m_endtype = endType;
      if (endType == EndType.etClosedLine || endType == EndType.etClosedPolygon)
      {
        while (index1 > 0 && path[0] == path[index1])
          --index1;
      }
      Child.m_polygon.Capacity = index1 + 1;
      Child.m_polygon.Add(path[0]);
      int index2 = 0;
      int index3 = 0;
      for (int index4 = 1; index4 <= index1; ++index4)
      {
        if (Child.m_polygon[index2] != path[index4])
        {
          ++index2;
          Child.m_polygon.Add(path[index4]);
          if (path[index4].Y > Child.m_polygon[index3].Y || path[index4].Y == Child.m_polygon[index3].Y && path[index4].X < Child.m_polygon[index3].X)
            index3 = index2;
        }
      }
      if (endType == EndType.etClosedPolygon && index2 < 2)
        return;
      this.m_polyNodes.AddChild(Child);
      if (endType != EndType.etClosedPolygon)
        return;
      if (this.m_lowest.X < 0L)
      {
        this.m_lowest = new IntPoint((long) (this.m_polyNodes.ChildCount - 1), (long) index3);
      }
      else
      {
        IntPoint intPoint = this.m_polyNodes.Childs[(int) this.m_lowest.X].m_polygon[(int) this.m_lowest.Y];
        if (Child.m_polygon[index3].Y <= intPoint.Y && (Child.m_polygon[index3].Y != intPoint.Y || Child.m_polygon[index3].X >= intPoint.X))
          return;
        this.m_lowest = new IntPoint((long) (this.m_polyNodes.ChildCount - 1), (long) index3);
      }
    }

    public void AddPaths(List<List<IntPoint>> paths, JoinType joinType, EndType endType)
    {
      foreach (List<IntPoint> path in paths)
        this.AddPath(path, joinType, endType);
    }

    private void FixOrientations()
    {
      if (this.m_lowest.X >= 0L && !Clipper.Orientation(this.m_polyNodes.Childs[(int) this.m_lowest.X].m_polygon))
      {
        for (int index = 0; index < this.m_polyNodes.ChildCount; ++index)
        {
          PolyNode child = this.m_polyNodes.Childs[index];
          if (child.m_endtype == EndType.etClosedPolygon || child.m_endtype == EndType.etClosedLine && Clipper.Orientation(child.m_polygon))
            child.m_polygon.Reverse();
        }
      }
      else
      {
        for (int index = 0; index < this.m_polyNodes.ChildCount; ++index)
        {
          PolyNode child = this.m_polyNodes.Childs[index];
          if (child.m_endtype == EndType.etClosedLine && !Clipper.Orientation(child.m_polygon))
            child.m_polygon.Reverse();
        }
      }
    }

    internal static DoublePoint GetUnitNormal(IntPoint pt1, IntPoint pt2)
    {
      double num1 = (double) (pt2.X - pt1.X);
      double num2 = (double) (pt2.Y - pt1.Y);
      if (num1 == 0.0 && num2 == 0.0)
        return new DoublePoint();
      double num3 = 1.0 / Math.Sqrt(num1 * num1 + num2 * num2);
      double num4 = num1 * num3;
      return new DoublePoint(num2 * num3, -num4);
    }

    private void DoOffset(double delta)
    {
      this.m_destPolys = new List<List<IntPoint>>();
      this.m_delta = delta;
      if (ClipperBase.near_zero(delta))
      {
        this.m_destPolys.Capacity = this.m_polyNodes.ChildCount;
        for (int index = 0; index < this.m_polyNodes.ChildCount; ++index)
        {
          PolyNode child = this.m_polyNodes.Childs[index];
          if (child.m_endtype == EndType.etClosedPolygon)
            this.m_destPolys.Add(child.m_polygon);
        }
      }
      else
      {
        this.m_miterLim = this.MiterLimit <= 2.0 ? 0.5 : 2.0 / (this.MiterLimit * this.MiterLimit);
        double num1 = Math.PI / Math.Acos(1.0 - (this.ArcTolerance > 0.0 ? (this.ArcTolerance <= Math.Abs(delta) * 0.25 ? this.ArcTolerance : Math.Abs(delta) * 0.25) : 0.25) / Math.Abs(delta));
        this.m_sin = Math.Sin(2.0 * Math.PI / num1);
        this.m_cos = Math.Cos(2.0 * Math.PI / num1);
        this.m_StepsPerRad = num1 / (2.0 * Math.PI);
        if (delta < 0.0)
          this.m_sin = -this.m_sin;
        this.m_destPolys.Capacity = this.m_polyNodes.ChildCount * 2;
        for (int index1 = 0; index1 < this.m_polyNodes.ChildCount; ++index1)
        {
          PolyNode child = this.m_polyNodes.Childs[index1];
          this.m_srcPoly = child.m_polygon;
          int count = this.m_srcPoly.Count;
          if (count != 0 && (delta > 0.0 || count >= 3 && child.m_endtype == EndType.etClosedPolygon))
          {
            this.m_destPoly = new List<IntPoint>();
            if (count == 1)
            {
              if (child.m_jointype == JoinType.jtRound)
              {
                double num2 = 1.0;
                double num3 = 0.0;
                for (int index2 = 1; (double) index2 <= num1; ++index2)
                {
                  this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[0].X + num2 * delta), ClipperOffset.Round((double) this.m_srcPoly[0].Y + num3 * delta)));
                  double num4 = num2;
                  num2 = num2 * this.m_cos - this.m_sin * num3;
                  double sin = this.m_sin;
                  num3 = num4 * sin + num3 * this.m_cos;
                }
              }
              else
              {
                double num2 = -1.0;
                double num3 = -1.0;
                for (int index2 = 0; index2 < 4; ++index2)
                {
                  this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[0].X + num2 * delta), ClipperOffset.Round((double) this.m_srcPoly[0].Y + num3 * delta)));
                  if (num2 < 0.0)
                    num2 = 1.0;
                  else if (num3 < 0.0)
                    num3 = 1.0;
                  else
                    num2 = -1.0;
                }
              }
              this.m_destPolys.Add(this.m_destPoly);
            }
            else
            {
              this.m_normals.Clear();
              this.m_normals.Capacity = count;
              for (int index2 = 0; index2 < count - 1; ++index2)
                this.m_normals.Add(ClipperOffset.GetUnitNormal(this.m_srcPoly[index2], this.m_srcPoly[index2 + 1]));
              if (child.m_endtype == EndType.etClosedLine || child.m_endtype == EndType.etClosedPolygon)
                this.m_normals.Add(ClipperOffset.GetUnitNormal(this.m_srcPoly[count - 1], this.m_srcPoly[0]));
              else
                this.m_normals.Add(new DoublePoint(this.m_normals[count - 2]));
              if (child.m_endtype == EndType.etClosedPolygon)
              {
                int k = count - 1;
                for (int j = 0; j < count; ++j)
                  this.OffsetPoint(j, ref k, child.m_jointype);
                this.m_destPolys.Add(this.m_destPoly);
              }
              else if (child.m_endtype == EndType.etClosedLine)
              {
                int k = count - 1;
                for (int j = 0; j < count; ++j)
                  this.OffsetPoint(j, ref k, child.m_jointype);
                this.m_destPolys.Add(this.m_destPoly);
                this.m_destPoly = new List<IntPoint>();
                DoublePoint normal = this.m_normals[count - 1];
                for (int index2 = count - 1; index2 > 0; --index2)
                  this.m_normals[index2] = new DoublePoint(-this.m_normals[index2 - 1].X, -this.m_normals[index2 - 1].Y);
                this.m_normals[0] = new DoublePoint(-normal.X, -normal.Y);
                k = 0;
                for (int j = count - 1; j >= 0; --j)
                  this.OffsetPoint(j, ref k, child.m_jointype);
                this.m_destPolys.Add(this.m_destPoly);
              }
              else
              {
                int k = 0;
                for (int j = 1; j < count - 1; ++j)
                  this.OffsetPoint(j, ref k, child.m_jointype);
                IntPoint intPoint;
                if (child.m_endtype == EndType.etOpenButt)
                {
                  int index2 = count - 1;
                  intPoint = new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[index2].X + this.m_normals[index2].X * delta), ClipperOffset.Round((double) this.m_srcPoly[index2].Y + this.m_normals[index2].Y * delta));
                  this.m_destPoly.Add(intPoint);
                  intPoint = new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[index2].X - this.m_normals[index2].X * delta), ClipperOffset.Round((double) this.m_srcPoly[index2].Y - this.m_normals[index2].Y * delta));
                  this.m_destPoly.Add(intPoint);
                }
                else
                {
                  int num2 = count - 1;
                  k = count - 2;
                  this.m_sinA = 0.0;
                  this.m_normals[num2] = new DoublePoint(-this.m_normals[num2].X, -this.m_normals[num2].Y);
                  if (child.m_endtype == EndType.etOpenSquare)
                    this.DoSquare(num2, k);
                  else
                    this.DoRound(num2, k);
                }
                for (int index2 = count - 1; index2 > 0; --index2)
                  this.m_normals[index2] = new DoublePoint(-this.m_normals[index2 - 1].X, -this.m_normals[index2 - 1].Y);
                this.m_normals[0] = new DoublePoint(-this.m_normals[1].X, -this.m_normals[1].Y);
                k = count - 1;
                for (int j = k - 1; j > 0; --j)
                  this.OffsetPoint(j, ref k, child.m_jointype);
                if (child.m_endtype == EndType.etOpenButt)
                {
                  intPoint = new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[0].X - this.m_normals[0].X * delta), ClipperOffset.Round((double) this.m_srcPoly[0].Y - this.m_normals[0].Y * delta));
                  this.m_destPoly.Add(intPoint);
                  intPoint = new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[0].X + this.m_normals[0].X * delta), ClipperOffset.Round((double) this.m_srcPoly[0].Y + this.m_normals[0].Y * delta));
                  this.m_destPoly.Add(intPoint);
                }
                else
                {
                  k = 1;
                  this.m_sinA = 0.0;
                  if (child.m_endtype == EndType.etOpenSquare)
                    this.DoSquare(0, 1);
                  else
                    this.DoRound(0, 1);
                }
                this.m_destPolys.Add(this.m_destPoly);
              }
            }
          }
        }
      }
    }

    public void Execute(ref List<List<IntPoint>> solution, double delta)
    {
      solution.Clear();
      this.FixOrientations();
      this.DoOffset(delta);
      Clipper clipper = new Clipper();
      clipper.AddPaths(this.m_destPolys, PolyType.ptSubject, true);
      if (delta > 0.0)
      {
        clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
      }
      else
      {
        IntRect bounds = ClipperBase.GetBounds(this.m_destPolys);
        clipper.AddPath(new List<IntPoint>(4)
        {
          new IntPoint(bounds.left - 10L, bounds.bottom + 10L),
          new IntPoint(bounds.right + 10L, bounds.bottom + 10L),
          new IntPoint(bounds.right + 10L, bounds.top - 10L),
          new IntPoint(bounds.left - 10L, bounds.top - 10L)
        }, PolyType.ptSubject, true);
        clipper.ReverseSolution = true;
        clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNegative, PolyFillType.pftNegative);
        if (solution.Count <= 0)
          return;
        solution.RemoveAt(0);
      }
    }

    public void Execute(ref PolyTree solution, double delta)
    {
      solution.Clear();
      this.FixOrientations();
      this.DoOffset(delta);
      Clipper clipper = new Clipper();
      clipper.AddPaths(this.m_destPolys, PolyType.ptSubject, true);
      if (delta > 0.0)
      {
        clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
      }
      else
      {
        IntRect bounds = ClipperBase.GetBounds(this.m_destPolys);
        clipper.AddPath(new List<IntPoint>(4)
        {
          new IntPoint(bounds.left - 10L, bounds.bottom + 10L),
          new IntPoint(bounds.right + 10L, bounds.bottom + 10L),
          new IntPoint(bounds.right + 10L, bounds.top - 10L),
          new IntPoint(bounds.left - 10L, bounds.top - 10L)
        }, PolyType.ptSubject, true);
        clipper.ReverseSolution = true;
        clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNegative, PolyFillType.pftNegative);
        if (solution.ChildCount == 1 && solution.Childs[0].ChildCount > 0)
        {
          PolyNode child = solution.Childs[0];
          solution.Childs.Capacity = child.ChildCount;
          solution.Childs[0] = child.Childs[0];
          solution.Childs[0].m_Parent = (PolyNode) solution;
          for (int index = 1; index < child.ChildCount; ++index)
            solution.AddChild(child.Childs[index]);
        }
        else
          solution.Clear();
      }
    }

    private void OffsetPoint(int j, ref int k, JoinType jointype)
    {
      this.m_sinA = this.m_normals[k].X * this.m_normals[j].Y - this.m_normals[j].X * this.m_normals[k].Y;
      if (Math.Abs(this.m_sinA * this.m_delta) < 1.0)
      {
        if (this.m_normals[k].X * this.m_normals[j].X + this.m_normals[j].Y * this.m_normals[k].Y > 0.0)
        {
          this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + this.m_normals[k].X * this.m_delta), ClipperOffset.Round((double) this.m_srcPoly[j].Y + this.m_normals[k].Y * this.m_delta)));
          return;
        }
      }
      else if (this.m_sinA > 1.0)
        this.m_sinA = 1.0;
      else if (this.m_sinA < -1.0)
        this.m_sinA = -1.0;
      if (this.m_sinA * this.m_delta < 0.0)
      {
        this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + this.m_normals[k].X * this.m_delta), ClipperOffset.Round((double) this.m_srcPoly[j].Y + this.m_normals[k].Y * this.m_delta)));
        this.m_destPoly.Add(this.m_srcPoly[j]);
        this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + this.m_normals[j].X * this.m_delta), ClipperOffset.Round((double) this.m_srcPoly[j].Y + this.m_normals[j].Y * this.m_delta)));
      }
      else
      {
        switch (jointype)
        {
          case JoinType.jtSquare:
            this.DoSquare(j, k);
            break;
          case JoinType.jtRound:
            this.DoRound(j, k);
            break;
          case JoinType.jtMiter:
            double r = 1.0 + (this.m_normals[j].X * this.m_normals[k].X + this.m_normals[j].Y * this.m_normals[k].Y);
            if (r >= this.m_miterLim)
            {
              this.DoMiter(j, k, r);
              break;
            }
            this.DoSquare(j, k);
            break;
        }
      }
      k = j;
    }

    internal void DoSquare(int j, int k)
    {
      double num = Math.Tan(Math.Atan2(this.m_sinA, this.m_normals[k].X * this.m_normals[j].X + this.m_normals[k].Y * this.m_normals[j].Y) / 4.0);
      this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + this.m_delta * (this.m_normals[k].X - this.m_normals[k].Y * num)), ClipperOffset.Round((double) this.m_srcPoly[j].Y + this.m_delta * (this.m_normals[k].Y + this.m_normals[k].X * num))));
      this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + this.m_delta * (this.m_normals[j].X + this.m_normals[j].Y * num)), ClipperOffset.Round((double) this.m_srcPoly[j].Y + this.m_delta * (this.m_normals[j].Y - this.m_normals[j].X * num))));
    }

    internal void DoMiter(int j, int k, double r)
    {
      double num = this.m_delta / r;
      this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + (this.m_normals[k].X + this.m_normals[j].X) * num), ClipperOffset.Round((double) this.m_srcPoly[j].Y + (this.m_normals[k].Y + this.m_normals[j].Y) * num)));
    }

    internal void DoRound(int j, int k)
    {
      int num1 = Math.Max((int) ClipperOffset.Round(this.m_StepsPerRad * Math.Abs(Math.Atan2(this.m_sinA, this.m_normals[k].X * this.m_normals[j].X + this.m_normals[k].Y * this.m_normals[j].Y))), 1);
      double num2 = this.m_normals[k].X;
      double num3 = this.m_normals[k].Y;
      for (int index = 0; index < num1; ++index)
      {
        this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + num2 * this.m_delta), ClipperOffset.Round((double) this.m_srcPoly[j].Y + num3 * this.m_delta)));
        double num4 = num2;
        num2 = num2 * this.m_cos - this.m_sin * num3;
        double sin = this.m_sin;
        num3 = num4 * sin + num3 * this.m_cos;
      }
      this.m_destPoly.Add(new IntPoint(ClipperOffset.Round((double) this.m_srcPoly[j].X + this.m_normals[j].X * this.m_delta), ClipperOffset.Round((double) this.m_srcPoly[j].Y + this.m_normals[j].Y * this.m_delta)));
    }
  }
}
