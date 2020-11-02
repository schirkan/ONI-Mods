// Decompiled with JetBrains decompiler
// Type: AsteroidDescriptor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public struct AsteroidDescriptor
{
  public string text;
  public string tooltip;
  public List<Tuple<string, Color, float>> bands;

  public AsteroidDescriptor(string text, string tooltip, List<Tuple<string, Color, float>> bands = null)
  {
    this.text = text;
    this.tooltip = tooltip;
    this.bands = bands;
  }
}
