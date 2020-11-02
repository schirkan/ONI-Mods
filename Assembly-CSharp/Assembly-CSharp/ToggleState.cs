// Decompiled with JetBrains decompiler
// Type: ToggleState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public struct ToggleState
{
  public string Name;
  public string on_click_override_sound_path;
  public string on_release_override_sound_path;
  public Sprite sprite;
  public Color color;
  public Color color_on_hover;
  public bool use_color_on_hover;
  public bool use_rect_margins;
  public Vector2 rect_margins;
  public StatePresentationSetting[] additional_display_settings;
}
