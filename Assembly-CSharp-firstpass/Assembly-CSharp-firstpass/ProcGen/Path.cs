// Decompiled with JetBrains decompiler
// Type: ProcGen.Path
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
  [SerializationConfig(MemberSerialization.OptOut)]
  public class Path
  {
    public List<Segment> pathElements;

    public Path() => this.pathElements = new List<Segment>();

    public void AddSegment(Segment segment) => this.pathElements.Add(segment);

    public void AddSegment(Vector2 e0, Vector2 e1) => this.pathElements.Add(new Segment(e0, e1));

    public void Stagger(SeededRandom rnd, float maxDistance = 10f, float staggerRange = 3f)
    {
      List<Segment> segmentList = new List<Segment>();
      for (int index = 0; index < this.pathElements.Count; ++index)
        segmentList.AddRange((IEnumerable<Segment>) this.pathElements[index].Stagger(rnd, maxDistance, staggerRange));
      this.pathElements = segmentList;
    }
  }
}
