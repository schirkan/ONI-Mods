// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Model.Line
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Model
{
  public class Line : AbstractModel
  {
    protected bool _attenuate = true;
    protected Line.Position _startPosition = new Line.Position(0.0f, 0.0f, 0.0f);
    protected Line.Position _endPosition = new Line.Position(0.0f, 0.0f, 0.0f);

    public bool Attenuate
    {
      get => this._attenuate;
      set => this._attenuate = value;
    }

    public Line()
    {
    }

    public Line(IModule module)
      : base(module)
    {
    }

    public void SetStartPoint(float x, float y, float z)
    {
      this._startPosition.x = x;
      this._startPosition.y = y;
      this._startPosition.z = z;
    }

    public void SetEndPoint(float x, float y, float z)
    {
      this._endPosition.x = x;
      this._endPosition.y = y;
      this._endPosition.z = z;
    }

    public float GetValue(float p)
    {
      float num = ((IModule3D) this._sourceModule).GetValue((this._endPosition.x - this._startPosition.x) * p + this._startPosition.x, (this._endPosition.y - this._startPosition.y) * p + this._startPosition.y, (this._endPosition.z - this._startPosition.z) * p + this._startPosition.z);
      return this._attenuate ? (float) ((double) p * (1.0 - (double) p) * 4.0) * num : num;
    }

    protected struct Position
    {
      public float x;
      public float y;
      public float z;

      public Position(float x, float y, float z)
      {
        this.x = x;
        this.y = y;
        this.z = z;
      }
    }
  }
}
