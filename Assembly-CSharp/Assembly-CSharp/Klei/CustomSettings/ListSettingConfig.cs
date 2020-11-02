﻿// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.ListSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
  public class ListSettingConfig : SettingConfig
  {
    public List<SettingLevel> levels { get; private set; }

    public ListSettingConfig(
      string id,
      string label,
      string tooltip,
      List<SettingLevel> levels,
      string default_level_id,
      string nosweat_default_level_id,
      int coordinate_dimension = -1,
      int coordinate_dimension_width = -1,
      bool debug_only = false,
      bool triggers_custom_game = true)
      : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_dimension, coordinate_dimension_width, debug_only, triggers_custom_game)
    {
      this.levels = levels;
    }

    public void StompLevels(
      List<SettingLevel> levels,
      string default_level_id,
      string nosweat_default_level_id)
    {
      this.levels = levels;
      this.default_level_id = default_level_id;
      this.nosweat_default_level_id = nosweat_default_level_id;
    }

    public override SettingLevel GetLevel(string level_id)
    {
      for (int index = 0; index < this.levels.Count; ++index)
      {
        if (this.levels[index].id == level_id)
          return this.levels[index];
      }
      for (int index = 0; index < this.levels.Count; ++index)
      {
        if (this.levels[index].id == this.default_level_id)
          return this.levels[index];
      }
      Debug.LogError((object) ("Unable to find setting level for setting:" + this.id + " level: " + level_id));
      return (SettingLevel) null;
    }

    public override List<SettingLevel> GetLevels() => this.levels;

    public string CycleSettingLevelID(string current_id, int direction)
    {
      string str = "";
      if (current_id == "")
        current_id = this.levels[0].id;
      for (int index = 0; index < this.levels.Count; ++index)
      {
        if (this.levels[index].id == current_id)
        {
          str = this.levels[Mathf.Clamp(index + direction, 0, this.levels.Count - 1)].id;
          break;
        }
      }
      return str;
    }

    public bool IsFirstLevel(string level_id) => this.levels.FindIndex((Predicate<SettingLevel>) (l => l.id == level_id)) == 0;

    public bool IsLastLevel(string level_id) => this.levels.FindIndex((Predicate<SettingLevel>) (l => l.id == level_id)) == this.levels.Count - 1;
  }
}
