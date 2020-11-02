// Decompiled with JetBrains decompiler
// Type: ProcGen.River
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
  [SerializationConfig(MemberSerialization.OptOut)]
  public class River : Path
  {
    public string element { get; set; }

    public string backgroundElement { get; set; }

    public float widthCenter { get; set; }

    public float widthBorder { get; set; }

    public float temperature { get; set; }

    public float maxMass { get; set; }

    public float flowIn { get; set; }

    public float flowOut { get; set; }

    public River() => this.pathElements = new List<Segment>();

    public River(
      Node t0,
      Node t1,
      string element = "Water",
      string backgroundElement = "Granite",
      float temperature = 373f,
      float maxMass = 2000f,
      float flowIn = 1000f,
      float flowOut = 100f,
      float widthCenter = 1.5f,
      float widthBorder = 1.5f)
      : this()
    {
      this.element = element;
      this.backgroundElement = backgroundElement;
      this.AddSection(t0, t1);
      this.temperature = temperature;
      this.maxMass = maxMass;
      this.flowIn = flowIn;
      this.flowOut = flowOut;
      this.widthCenter = widthCenter;
      this.widthBorder = widthBorder;
    }

    public River(River other, bool copySections = true)
    {
      if (copySections)
        this.pathElements = new List<Segment>((IEnumerable<Segment>) other.pathElements);
      this.element = this.element;
      this.backgroundElement = this.backgroundElement;
      this.temperature = other.temperature;
      this.maxMass = other.maxMass;
      this.flowIn = other.flowIn;
      this.flowOut = other.flowOut;
      this.widthCenter = other.widthCenter;
      this.widthBorder = other.widthBorder;
    }

    public void AddSection(Node t0, Node t1) => this.pathElements.Add(new Segment(t0.position, t1.position));

    public Vector2 SourcePosition() => this.pathElements[0].e0;

    public Vector2 SinkPosition() => this.pathElements[this.pathElements.Count - 1].e1;
  }
}
