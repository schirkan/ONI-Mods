// Decompiled with JetBrains decompiler
// Type: ProcGen.Segment
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
  [SerializationConfig(MemberSerialization.OptOut)]
  public struct Segment
  {
    public Vector2 e0;
    public Vector2 e1;

    public Segment(Vector2 e0, Vector2 e1)
    {
      this.e0 = e0;
      this.e1 = e1;
    }

    public List<Segment> Stagger(
      SeededRandom rnd,
      float maxDistance = 10f,
      float staggerRange = 3f)
    {
      List<Segment> segmentList = new List<Segment>();
      Vector2 vector2_1 = this.e1 - this.e0;
      Vector2 e0 = this.e0;
      Vector2 vector2_2 = this.e1;
      float num = vector2_1.magnitude / maxDistance;
      Vector2 normalized = new Vector2(-vector2_1.y, vector2_1.x).normalized;
      for (int index = 0; (double) index < (double) num; ++index)
      {
        vector2_2 = this.e0 + vector2_1 * (1f / num) * (float) index + normalized * rnd.RandomRange(-staggerRange, staggerRange);
        segmentList.Add(new Segment(e0, vector2_2));
        e0 = vector2_2;
      }
      segmentList.Add(new Segment(vector2_2, this.e1));
      return segmentList;
    }
  }
}
