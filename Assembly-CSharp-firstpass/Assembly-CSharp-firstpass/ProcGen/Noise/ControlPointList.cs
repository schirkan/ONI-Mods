// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.ControlPointList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Modifier;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen.Noise
{
  [Serializable]
  public class ControlPointList : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (ControlPointList);

    [SerializeField]
    public List<ControlPointList.Control> points { get; set; }

    public ControlPointList() => this.points = new List<ControlPointList.Control>();

    public List<ControlPoint> GetControls()
    {
      List<ControlPoint> controlPointList = new List<ControlPoint>();
      for (int index = 0; index < this.points.Count; ++index)
        controlPointList.Add(new ControlPoint(this.points[index].input, this.points[index].output));
      return controlPointList;
    }

    public class Control
    {
      public float input { get; set; }

      public float output { get; set; }

      public Control()
      {
        this.input = 0.0f;
        this.output = 0.0f;
      }

      public Control(float i, float o)
      {
        this.input = i;
        this.output = o;
      }
    }
  }
}
