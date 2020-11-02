// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Tranformer.RotatePoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Tranformer
{
  public class RotatePoint : TransformerModule, IModule3D, IModule
  {
    public const float DEFAULT_ROTATE_X = 0.0f;
    public const float DEFAULT_ROTATE_Y = 0.0f;
    public const float DEFAULT_ROTATE_Z = 0.0f;
    protected IModule _sourceModule;
    protected float _x1Matrix;
    protected float _x2Matrix;
    protected float _x3Matrix;
    protected float _xAngle;
    protected float _y1Matrix;
    protected float _y2Matrix;
    protected float _y3Matrix;
    protected float _yAngle;
    protected float _z1Matrix;
    protected float _z2Matrix;
    protected float _z3Matrix;
    protected float _zAngle;

    public IModule SourceModule
    {
      get => this._sourceModule;
      set => this._sourceModule = value;
    }

    public float XAngle
    {
      get => this._xAngle;
      set => this.SetAngles(value, this._yAngle, this._zAngle);
    }

    public float YAngle
    {
      get => this._yAngle;
      set => this.SetAngles(this._xAngle, value, this._zAngle);
    }

    public float ZAngle
    {
      get => this._zAngle;
      set => this.SetAngles(this._xAngle, this._yAngle, value);
    }

    public RotatePoint()
    {
    }

    public RotatePoint(IModule source) => this._sourceModule = source;

    public RotatePoint(IModule source, float xAngle, float yAngle, float zAngle)
    {
      this._sourceModule = source;
      this.SetAngles(xAngle, yAngle, zAngle);
    }

    public void SetAngles(float xAngle, float yAngle, float zAngle)
    {
      float num1 = (float) Math.Cos((double) xAngle * (Math.PI / 180.0));
      float num2 = (float) Math.Cos((double) yAngle * (Math.PI / 180.0));
      float num3 = (float) Math.Cos((double) zAngle * (Math.PI / 180.0));
      float num4 = (float) Math.Sin((double) xAngle * (Math.PI / 180.0));
      float num5 = (float) Math.Sin((double) yAngle * (Math.PI / 180.0));
      float num6 = (float) Math.Sin((double) zAngle * (Math.PI / 180.0));
      this._x1Matrix = (float) ((double) num5 * (double) num4 * (double) num6 + (double) num2 * (double) num3);
      this._y1Matrix = num1 * num6;
      this._z1Matrix = (float) ((double) num5 * (double) num3 - (double) num2 * (double) num4 * (double) num6);
      this._x2Matrix = (float) ((double) num5 * (double) num4 * (double) num3 - (double) num2 * (double) num6);
      this._y2Matrix = num1 * num3;
      this._z2Matrix = (float) (-(double) num2 * (double) num4 * (double) num3 - (double) num5 * (double) num6);
      this._x3Matrix = -num5 * num1;
      this._y3Matrix = num4;
      this._z3Matrix = num2 * num1;
      this._xAngle = xAngle;
      this._yAngle = yAngle;
      this._zAngle = zAngle;
    }

    public float GetValue(float x, float y, float z) => ((IModule3D) this._sourceModule).GetValue((float) ((double) this._x1Matrix * (double) x + (double) this._y1Matrix * (double) y + (double) this._z1Matrix * (double) z), (float) ((double) this._x2Matrix * (double) x + (double) this._y2Matrix * (double) y + (double) this._z2Matrix * (double) z), (float) ((double) this._x3Matrix * (double) x + (double) this._y3Matrix * (double) y + (double) this._z3Matrix * (double) z));
  }
}
