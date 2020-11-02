﻿// Decompiled with JetBrains decompiler
// Type: Expectation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Expectation
{
  public string id { get; protected set; }

  public string name { get; protected set; }

  public string description { get; protected set; }

  public System.Action<MinionResume> OnApply { get; protected set; }

  public System.Action<MinionResume> OnRemove { get; protected set; }

  public Expectation(
    string id,
    string name,
    string description,
    System.Action<MinionResume> OnApply,
    System.Action<MinionResume> OnRemove)
  {
    this.id = id;
    this.name = name;
    this.description = description;
    this.OnApply = OnApply;
    this.OnRemove = OnRemove;
  }
}
