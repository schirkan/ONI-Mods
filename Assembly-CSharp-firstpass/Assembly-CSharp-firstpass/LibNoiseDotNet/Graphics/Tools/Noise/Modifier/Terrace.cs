// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Terrace
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Terrace : ModifierModule, IModule3D, IModule
  {
    protected List<float> _controlPoints = new List<float>(2);
    protected bool _invert;

    public bool Invert
    {
      get => this._invert;
      set => this._invert = value;
    }

    public Terrace()
    {
    }

    public Terrace(IModule source)
      : base(source)
    {
    }

    public Terrace(IModule source, bool invert)
      : base(source)
      => this._invert = invert;

    public void AddControlPoint(float input)
    {
      if (this._controlPoints.Contains(input))
        throw new ArgumentException(string.Format("Cannont insert ControlPoint({0}) : Each control point is required to contain a unique input value", (object) input));
      this._controlPoints.Add(input);
      this.SortControlPoints();
    }

    public int CountControlPoints() => this._controlPoints.Count;

    public IList<float> getControlPoints() => (IList<float>) this._controlPoints.AsReadOnly();

    public void ClearControlPoints() => this._controlPoints.Clear();

    public void MakeControlPoints(int controlPointCount)
    {
      if (controlPointCount < 2)
        throw new ArgumentException("Two or more control points must be specified.");
      this.ClearControlPoints();
      float num = (float) (2.0 / ((double) controlPointCount - 1.0));
      float input = -1f;
      for (int index = 0; index < controlPointCount; ++index)
      {
        this.AddControlPoint(input);
        input += num;
      }
    }

    public float GetValue(float x, float y, float z)
    {
      float num1 = ((IModule3D) this._sourceModule).GetValue(x, y, z);
      int index1 = 0;
      while (index1 < this._controlPoints.Count && (double) num1 >= (double) this._controlPoints[index1])
        ++index1;
      int index2 = Libnoise.Clamp(index1 - 1, 0, this._controlPoints.Count - 1);
      int index3 = Libnoise.Clamp(index1, 0, this._controlPoints.Count - 1);
      if (index2 == index3)
        return this._controlPoints[index3];
      float controlPoint1 = this._controlPoints[index2];
      float controlPoint2 = this._controlPoints[index3];
      float num2 = (float) (((double) num1 - (double) controlPoint1) / ((double) controlPoint2 - (double) controlPoint1));
      if (this._invert)
      {
        num2 = 1f - num2;
        Libnoise.SwapValues(ref controlPoint1, ref controlPoint2);
      }
      float a = num2 * num2;
      return Libnoise.Lerp(controlPoint1, controlPoint2, a);
    }

    protected void SortControlPoints() => this._controlPoints.Sort((Comparison<float>) ((p1, p2) =>
    {
      if ((double) p1 > (double) p2)
        return 1;
      return (double) p1 < (double) p2 ? -1 : 0;
    }));
  }
}
