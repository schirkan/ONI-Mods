// Decompiled with JetBrains decompiler
// Type: ScheduleBlockType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
  public Color color { get; private set; }

  public string description { get; private set; }

  public ScheduleBlockType(
    string id,
    ResourceSet parent,
    string name,
    string description,
    Color color)
    : base(id, parent, name)
  {
    this.color = color;
    this.description = description;
  }
}
