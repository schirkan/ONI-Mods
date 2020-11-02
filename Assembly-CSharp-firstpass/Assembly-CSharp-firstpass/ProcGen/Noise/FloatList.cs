// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.FloatList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen.Noise
{
  [Serializable]
  public class FloatList : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (FloatList);

    [SerializeField]
    public List<float> points { get; set; }

    public FloatList() => this.points = new List<float>();
  }
}
