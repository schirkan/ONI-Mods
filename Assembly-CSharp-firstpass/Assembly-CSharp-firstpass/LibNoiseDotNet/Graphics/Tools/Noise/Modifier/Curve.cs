// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Curve
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Curve : ModifierModule, IModule3D, IModule
  {
    protected List<ControlPoint> _controlPoints = new List<ControlPoint>(4);

    public Curve()
    {
    }

    public Curve(IModule source)
      : base(source)
    {
    }

    public void AddControlPoint(float input, float output) => this.AddControlPoint(new ControlPoint(input, output));

    public void AddControlPoint(ControlPoint point)
    {
      if (this._controlPoints.Contains(point))
        throw new ArgumentException(string.Format("Cannont insert ControlPoint({0}, {1}) : Each control point is required to contain a unique input value", (object) point.Input, (object) point.Output));
      this._controlPoints.Add(point);
      this.SortControlPoints();
    }

    public int CountControlPoints() => this._controlPoints.Count;

    public IList<ControlPoint> getControlPoints() => (IList<ControlPoint>) this._controlPoints.AsReadOnly();

    public void ClearControlPoints() => this._controlPoints.Clear();

    public float GetValue(float x, float y, float z)
    {
      float num = ((IModule3D) this._sourceModule).GetValue(x, y, z);
      int index1 = 0;
      while (index1 < this._controlPoints.Count && (double) num >= (double) this._controlPoints[index1].Input)
        ++index1;
      int index2 = Libnoise.Clamp(index1 - 2, 0, this._controlPoints.Count - 1);
      int index3 = Libnoise.Clamp(index1 - 1, 0, this._controlPoints.Count - 1);
      int index4 = Libnoise.Clamp(index1, 0, this._controlPoints.Count - 1);
      int index5 = Libnoise.Clamp(index1 + 1, 0, this._controlPoints.Count - 1);
      if (index3 == index4)
        return this._controlPoints[index3].Output;
      float input1 = this._controlPoints[index3].Input;
      float input2 = this._controlPoints[index4].Input;
      float a = (float) (((double) num - (double) input1) / ((double) input2 - (double) input1));
      return Libnoise.Cerp(this._controlPoints[index2].Output, this._controlPoints[index3].Output, this._controlPoints[index4].Output, this._controlPoints[index5].Output, a);
    }

    protected void SortControlPoints() => this._controlPoints.Sort((Comparison<ControlPoint>) ((p1, p2) =>
    {
      if ((double) p1.Input > (double) p2.Input)
        return 1;
      return (double) p1.Input < (double) p2.Input ? -1 : 0;
    }));
  }
}
