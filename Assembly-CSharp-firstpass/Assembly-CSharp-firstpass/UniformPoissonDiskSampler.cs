// Decompiled with JetBrains decompiler
// Type: UniformPoissonDiskSampler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UniformPoissonDiskSampler
{
  public const int DefaultPointsPerIteration = 30;
  private static readonly float SquareRootTwo = (float) Math.Sqrt(2.0);
  private SeededRandom myRandom;

  public UniformPoissonDiskSampler(SeededRandom seed) => this.myRandom = seed;

  public List<Vector2> SampleCircle(Vector2 center, float radius, float minimumDistance) => this.SampleCircle(center, radius, minimumDistance, 30);

  public List<Vector2> SampleCircle(
    Vector2 center,
    float radius,
    float minimumDistance,
    int pointsPerIteration)
  {
    return this.Sample(center - new Vector2(radius, radius), center + new Vector2(radius, radius), new float?(radius), minimumDistance, pointsPerIteration);
  }

  public List<Vector2> SampleRectangle(
    Vector2 topLeft,
    Vector2 lowerRight,
    float minimumDistance)
  {
    return this.SampleRectangle(topLeft, lowerRight, minimumDistance, 30);
  }

  public List<Vector2> SampleRectangle(
    Vector2 topLeft,
    Vector2 lowerRight,
    float minimumDistance,
    int pointsPerIteration)
  {
    return this.Sample(topLeft, lowerRight, new float?(), minimumDistance, pointsPerIteration);
  }

  private List<Vector2> Sample(
    Vector2 topLeft,
    Vector2 lowerRight,
    float? rejectionDistance,
    float minimumDistance,
    int pointsPerIteration)
  {
    UniformPoissonDiskSampler.Settings settings1 = new UniformPoissonDiskSampler.Settings();
    settings1.TopLeft = topLeft;
    settings1.LowerRight = lowerRight;
    settings1.Dimensions = lowerRight - topLeft;
    settings1.Center = (topLeft + lowerRight) / 2f;
    settings1.CellSize = minimumDistance / UniformPoissonDiskSampler.SquareRootTwo;
    settings1.MinimumDistance = minimumDistance;
    ref UniformPoissonDiskSampler.Settings local = ref settings1;
    float? nullable1;
    if (rejectionDistance.HasValue)
    {
      float? nullable2 = rejectionDistance;
      float? nullable3 = rejectionDistance;
      nullable1 = nullable2.HasValue & nullable3.HasValue ? new float?(nullable2.GetValueOrDefault() * nullable3.GetValueOrDefault()) : new float?();
    }
    else
      nullable1 = new float?();
    local.RejectionSqDistance = nullable1;
    UniformPoissonDiskSampler.Settings settings2 = settings1;
    settings2.GridWidth = (int) ((double) settings2.Dimensions.x / (double) settings2.CellSize) + 1;
    settings2.GridHeight = (int) ((double) settings2.Dimensions.y / (double) settings2.CellSize) + 1;
    UniformPoissonDiskSampler.State state = new UniformPoissonDiskSampler.State()
    {
      Grid = new Vector2?[settings2.GridWidth, settings2.GridHeight],
      ActivePoints = new List<Vector2>(),
      Points = new List<Vector2>()
    };
    this.AddFirstPoint(ref settings2, ref state);
    while (state.ActivePoints.Count != 0)
    {
      int index1 = this.myRandom.RandomRange(0, state.ActivePoints.Count - 1);
      Vector2 activePoint = state.ActivePoints[index1];
      bool flag = false;
      for (int index2 = 0; index2 < pointsPerIteration; ++index2)
        flag |= this.AddNextPoint(activePoint, ref settings2, ref state);
      if (!flag)
        state.ActivePoints.RemoveAt(index1);
    }
    return state.Points;
  }

  private void AddFirstPoint(
    ref UniformPoissonDiskSampler.Settings settings,
    ref UniformPoissonDiskSampler.State state)
  {
    bool flag = false;
    while (!flag)
    {
      float num1 = this.myRandom.RandomValue();
      float x = settings.TopLeft.x + settings.Dimensions.x * num1;
      float num2 = this.myRandom.RandomValue();
      float y = settings.TopLeft.y + settings.Dimensions.y * num2;
      Vector2 point = new Vector2(x, y);
      if (settings.RejectionSqDistance.HasValue)
      {
        double num3 = (double) Vector2.SqrMagnitude(settings.Center - point);
        float? rejectionSqDistance = settings.RejectionSqDistance;
        double valueOrDefault = (double) rejectionSqDistance.GetValueOrDefault();
        if (num3 > valueOrDefault & rejectionSqDistance.HasValue)
          continue;
      }
      flag = true;
      Vector2 vector2 = UniformPoissonDiskSampler.Denormalize(point, settings.TopLeft, (double) settings.CellSize);
      state.Grid[(int) vector2.x, (int) vector2.y] = new Vector2?(point);
      state.ActivePoints.Add(point);
      state.Points.Add(point);
    }
  }

  private bool AddNextPoint(
    Vector2 point,
    ref UniformPoissonDiskSampler.Settings settings,
    ref UniformPoissonDiskSampler.State state)
  {
    bool flag1 = false;
    Vector2 randomAround = this.GenerateRandomAround(point, settings.MinimumDistance);
    if ((double) randomAround.x >= (double) settings.TopLeft.x && (double) randomAround.x < (double) settings.LowerRight.x && ((double) randomAround.y > (double) settings.TopLeft.y && (double) randomAround.y < (double) settings.LowerRight.y))
    {
      if (settings.RejectionSqDistance.HasValue)
      {
        double num = (double) Vector2.SqrMagnitude(settings.Center - randomAround);
        float? rejectionSqDistance = settings.RejectionSqDistance;
        double valueOrDefault = (double) rejectionSqDistance.GetValueOrDefault();
        if (!(num <= valueOrDefault & rejectionSqDistance.HasValue))
          goto label_13;
      }
      Vector2 vector2 = UniformPoissonDiskSampler.Denormalize(randomAround, settings.TopLeft, (double) settings.CellSize);
      bool flag2 = false;
      for (int index1 = (int) Math.Max(0.0f, vector2.x - 2f); (double) index1 < (double) Math.Min((float) settings.GridWidth, vector2.x + 3f) && !flag2; ++index1)
      {
        for (int index2 = (int) Math.Max(0.0f, vector2.y - 2f); (double) index2 < (double) Math.Min((float) settings.GridHeight, vector2.y + 3f) && !flag2; ++index2)
        {
          if (state.Grid[index1, index2].HasValue && (double) Vector2.Distance(state.Grid[index1, index2].Value, randomAround) < (double) settings.MinimumDistance)
            flag2 = true;
        }
      }
      if (!flag2)
      {
        flag1 = true;
        state.ActivePoints.Add(randomAround);
        state.Points.Add(randomAround);
        state.Grid[(int) vector2.x, (int) vector2.y] = new Vector2?(randomAround);
      }
    }
label_13:
    return flag1;
  }

  private Vector2 GenerateRandomAround(Vector2 center, float minimumDistance)
  {
    float num1 = this.myRandom.RandomValue();
    double num2 = (double) minimumDistance + (double) minimumDistance * (double) num1;
    float num3 = 6.283185f * this.myRandom.RandomValue();
    float num4 = (float) num2 * (float) Math.Sin((double) num3);
    float num5 = (float) num2 * (float) Math.Cos((double) num3);
    return new Vector2(center.x + num4, center.y + num5);
  }

  private static Vector2 Denormalize(Vector2 point, Vector2 origin, double cellSize) => new Vector2((float) (int) (((double) point.x - (double) origin.x) / cellSize), (float) (int) (((double) point.y - (double) origin.y) / cellSize));

  private struct Settings
  {
    public Vector2 TopLeft;
    public Vector2 LowerRight;
    public Vector2 Center;
    public Vector2 Dimensions;
    public float? RejectionSqDistance;
    public float MinimumDistance;
    public float CellSize;
    public int GridWidth;
    public int GridHeight;
  }

  private struct State
  {
    public Vector2?[,] Grid;
    public List<Vector2> ActivePoints;
    public List<Vector2> Points;
  }
}
