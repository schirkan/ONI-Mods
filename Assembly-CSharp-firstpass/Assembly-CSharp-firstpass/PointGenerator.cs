// Decompiled with JetBrains decompiler
// Type: PointGenerator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PointGenerator
{
  public static List<Vector2> GetRandomPoints(
    Polygon boundingArea,
    float density,
    float avoidRadius,
    List<Vector2> avoidPoints,
    PointGenerator.SampleBehaviour behaviour,
    bool testInsideBounds,
    SeededRandom rnd,
    bool doShuffle = true,
    bool testAvoidPoints = true)
  {
    float width = boundingArea.bounds.width;
    float height = boundingArea.bounds.height;
    float num1 = width / 2f;
    float num2 = height / 2f;
    int num3;
    uint num4 = (uint) Mathf.Sqrt((float) (num3 = (int) Mathf.Floor(width * height / density)));
    float minimumDistance = density;
    int pointsPerIteration = 10;
    uint num5 = (uint) ((double) num3 * 0.980000019073486);
    Vector2 min = boundingArea.bounds.min;
    Vector2 max = boundingArea.bounds.max;
    List<Vector2> vector2List = new List<Vector2>();
    switch (behaviour)
    {
      case PointGenerator.SampleBehaviour.UniformSquare:
        for (float y = -num2 + density; (double) y < (double) num2 - (double) density; y += density)
        {
          for (float x = -num1 + density; (double) x < (double) num1 - (double) density; x += density)
            vector2List.Add(boundingArea.Centroid() + new Vector2(x, y));
        }
        break;
      case PointGenerator.SampleBehaviour.UniformHex:
        for (uint index1 = 0; index1 < num4; ++index1)
        {
          for (uint index2 = 0; index2 < num4; ++index2)
            vector2List.Add(boundingArea.Centroid() + new Vector2((float) (-(double) num1 + (0.5 + (double) index1) / (double) num4 * (double) width), (float) (-(double) num2 + (0.25 + 0.5 * (double) (index1 % 2U) + (double) index2) / (double) num4 * (double) height)));
        }
        break;
      case PointGenerator.SampleBehaviour.UniformSpiral:
        for (uint index = 0; index < num5; ++index)
        {
          double d = (double) index / (32.0 * (double) density * 8.0);
          double num6 = Math.Sqrt(d * 512.0 * (double) density);
          double num7 = Math.Sqrt(d);
          double num8 = Math.Sin(num6) * num7;
          double num9 = Math.Cos(num6) * num7;
          vector2List.Add(boundingArea.bounds.center + new Vector2((float) num8 * boundingArea.bounds.width, (float) num9 * boundingArea.bounds.height));
        }
        break;
      case PointGenerator.SampleBehaviour.UniformCircle:
        float num10 = 6.283185f * avoidRadius / density;
        float num11 = rnd.RandomValue();
        for (uint index = 1; (double) index < (double) num10; ++index)
        {
          double a;
          double num6 = Math.Cos(a = (double) num11 + (double) index / (double) num10 * 3.14159274101257 * 2.0) * (double) avoidRadius;
          double num7 = Math.Sin(a) * (double) avoidRadius;
          vector2List.Add(boundingArea.bounds.center + new Vector2((float) num6, (float) num7));
        }
        break;
      case PointGenerator.SampleBehaviour.PoissonDisk:
        vector2List = new UniformPoissonDiskSampler(rnd).SampleRectangle(min, max, minimumDistance, pointsPerIteration);
        break;
      default:
        for (float num6 = (float) (-(double) num2 + (double) avoidRadius * 0.300000011920929 + (double) rnd.RandomValue() * 2.0); (double) num6 < (double) num2 - ((double) avoidRadius * 0.300000011920929 + (double) rnd.RandomValue() * 2.0); num6 += density + rnd.RandomValue())
        {
          for (float x = (float) (-(double) num1 + (double) avoidRadius * 0.300000011920929 + (double) rnd.RandomValue() * 2.0 + (double) rnd.RandomValue() * 2.0); (double) x < (double) num1 - ((double) avoidRadius * 0.300000011920929 + (double) rnd.RandomValue() * 2.0); x += density + rnd.RandomValue())
            vector2List.Add(boundingArea.Centroid() + new Vector2(x, (float) ((double) num6 + (double) rnd.RandomValue() - 0.5)));
        }
        break;
    }
    List<Vector2> list = new List<Vector2>();
    for (int index1 = 0; index1 < vector2List.Count; ++index1)
    {
      if (!testInsideBounds || boundingArea.Contains(vector2List[index1]))
      {
        bool flag = false;
        if (testAvoidPoints && avoidPoints != null)
        {
          for (int index2 = 0; index2 < avoidPoints.Count; ++index2)
          {
            if ((double) Mathf.Abs((avoidPoints[index2] - vector2List[index1]).magnitude) < (double) avoidRadius)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          list.Add(vector2List[index1]);
      }
    }
    if (doShuffle)
      list.ShuffleSeeded<Vector2>(rnd.RandomSource());
    return list;
  }

  public static List<Vector2> GetArchimedesSpiralPoints(
    int pointCount,
    Vector2 startPoint,
    double tetha,
    double alpha)
  {
    List<Vector2> vector2List = new List<Vector2>();
    for (int index = 0; index < pointCount; ++index)
    {
      double num1 = tetha / (double) pointCount * (double) index;
      double num2 = alpha / (double) pointCount * (double) index;
      vector2List.Add(new Vector2(startPoint.x + (float) (num2 * Math.Cos(num1)), startPoint.y + (float) (num2 * Math.Sin(num1))));
    }
    return vector2List;
  }

  public static List<Vector2> GetFilliedRectangle(Rect boundingArea, float density)
  {
    List<Vector2> vector2List = new List<Vector2>();
    for (float xMin = boundingArea.xMin; (double) xMin < (double) boundingArea.xMax; xMin += density)
    {
      for (float yMin = boundingArea.yMin; (double) yMin < (double) boundingArea.yMax; yMin += density)
        vector2List.Add(new Vector2(xMin, yMin));
    }
    return vector2List;
  }

  public static List<Vector2> GetSpaceFillingRandom(
    Rect boundingArea,
    float density,
    SeededRandom rnd)
  {
    List<Vector2> filliedRectangle = PointGenerator.GetFilliedRectangle(boundingArea, density);
    filliedRectangle.ShuffleSeeded<Vector2>(rnd.RandomSource());
    return filliedRectangle;
  }

  private static Vector2I PointOnRightHandSpiralOut(int index)
  {
    int num1 = (int) Mathf.Ceil((float) ((double) Mathf.Sqrt((float) (4 * index + 1)) * 0.5 - 1.0 + 0.5));
    int num2 = (num1 & 1) == 0 ? 1 : 0;
    int num3 = num1 * (num1 + 1);
    bool flag = num3 - index < num1;
    int num4 = 2 * (num2 ^ (flag ? 1 : 0)) - 1;
    Vector2I vector2I = new Vector2I(-num4, 2 * num2 - 1);
    return new Vector2I(-(num2 == 0 & flag ? 1 : 0), 0) + vector2I * (num1 / 2) + new Vector2I(flag ? 0 : 1, flag ? 1 : 0) * num4 * (index - num3 + 2 * num1 - (flag ? 1 : 0) * num1);
  }

  public static List<Vector2> GetSpaceFillingSpiral(Rect boundingArea, float density)
  {
    List<Vector2> vector2List = new List<Vector2>();
    float num1 = boundingArea.width / density;
    float num2 = boundingArea.height / density;
    for (int index = 0; (double) index < (double) num2 * (double) num1; ++index)
    {
      Vector2I vector2I = PointGenerator.PointOnRightHandSpiralOut(index);
      vector2List.Add(new Vector2(boundingArea.center.x + (float) vector2I.x, boundingArea.center.y + (float) vector2I.y - density));
    }
    return vector2List;
  }

  public static List<Vector2> GetSpaceFillingSpiral(Polygon boundingArea, float density)
  {
    List<Vector2> vector2List = new List<Vector2>();
    float num1 = boundingArea.bounds.width / density;
    float num2 = boundingArea.bounds.height / density;
    for (int index = 0; (double) index < (double) num2 * (double) num1; ++index)
    {
      Vector2I vector2I = PointGenerator.PointOnRightHandSpiralOut(index);
      Vector2 point;
      ref Vector2 local = ref point;
      Rect bounds = boundingArea.bounds;
      double num3 = (double) bounds.center.x + (double) vector2I.x;
      bounds = boundingArea.bounds;
      double num4 = (double) bounds.center.y + (double) vector2I.y - (double) density;
      local = new Vector2((float) num3, (float) num4);
      if (boundingArea.Contains(point))
        vector2List.Add(point);
    }
    return vector2List;
  }

  [SerializeField]
  public enum SampleBehaviour
  {
    UniformSquare,
    UniformHex,
    UniformScaledHex,
    UniformSpiral,
    UniformCircle,
    PoissonDisk,
    StdRand,
  }
}
