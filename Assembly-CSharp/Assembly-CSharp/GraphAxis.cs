// Decompiled with JetBrains decompiler
// Type: GraphAxis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public struct GraphAxis
{
  public string name;
  public float min_value;
  public float max_value;
  public float guide_frequency;

  public float range => this.max_value - this.min_value;
}
