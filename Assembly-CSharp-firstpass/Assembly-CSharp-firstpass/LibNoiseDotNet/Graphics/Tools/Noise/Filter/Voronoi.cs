// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Filter.Voronoi
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Filter
{
  public class Voronoi : FilterModule, IModule3D, IModule
  {
    public const float DEFAULT_DISPLACEMENT = 1f;
    protected float _displacement = 1f;
    protected bool _distance;

    public float Displacement
    {
      get => this._displacement;
      set => this._displacement = value;
    }

    public bool Distance
    {
      get => this._distance;
      set => this._distance = value;
    }

    public float GetValue(float x, float y, float z)
    {
      x *= this._frequency;
      y *= this._frequency;
      z *= this._frequency;
      int num1 = (double) x > 0.0 ? (int) x : (int) x - 1;
      int num2 = (double) y > 0.0 ? (int) y : (int) y - 1;
      int num3 = (double) z > 0.0 ? (int) z : (int) z - 1;
      float num4 = (float) int.MaxValue;
      float num5 = 0.0f;
      float num6 = 0.0f;
      float num7 = 0.0f;
      for (int index1 = num3 - 2; index1 <= num3 + 2; ++index1)
      {
        for (int index2 = num2 - 2; index2 <= num2 + 2; ++index2)
        {
          for (int index3 = num1 - 2; index3 <= num1 + 2; ++index3)
          {
            float num8 = (float) index3 + this._source3D.GetValue((float) index3, (float) index2, (float) index1);
            float num9 = (float) index2 + this._source3D.GetValue((float) index3, (float) index2, (float) index1);
            float num10 = (float) index1 + this._source3D.GetValue((float) index3, (float) index2, (float) index1);
            float num11 = num8 - x;
            float num12 = num9 - y;
            float num13 = num10 - z;
            float num14 = (float) ((double) num11 * (double) num11 + (double) num12 * (double) num12 + (double) num13 * (double) num13);
            if ((double) num14 < (double) num4)
            {
              num4 = num14;
              num5 = num8;
              num6 = num9;
              num7 = num10;
            }
          }
        }
      }
      float num15;
      if (this._distance)
      {
        double num8 = (double) num5 - (double) x;
        float num9 = num6 - y;
        float num10 = num7 - z;
        num15 = (float) (Math.Sqrt(num8 * num8 + (double) num9 * (double) num9 + (double) num10 * (double) num10) * 1.73205077648163 - 1.0);
      }
      else
        num15 = 0.0f;
      return num15 + this._displacement * this._source3D.GetValue((float) (int) Math.Floor((double) num5), (float) (int) Math.Floor((double) num6), (float) (int) Math.Floor((double) num7));
    }
  }
}
