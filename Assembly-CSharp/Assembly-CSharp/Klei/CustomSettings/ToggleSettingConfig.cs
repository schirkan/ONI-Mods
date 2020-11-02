﻿// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.ToggleSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.CustomSettings
{
  public class ToggleSettingConfig : SettingConfig
  {
    public SettingLevel on_level { get; private set; }

    public SettingLevel off_level { get; private set; }

    public ToggleSettingConfig(
      string id,
      string label,
      string tooltip,
      SettingLevel off_level,
      SettingLevel on_level,
      string default_level_id,
      string nosweat_default_level_id,
      int coordinate_dimension = -1,
      int coordinate_dimension_width = -1,
      bool debug_only = false)
      : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_dimension, coordinate_dimension_width, debug_only)
    {
      this.off_level = off_level;
      this.on_level = on_level;
    }

    public override SettingLevel GetLevel(string level_id)
    {
      if (this.on_level.id == level_id)
        return this.on_level;
      if (this.off_level.id == level_id)
        return this.off_level;
      if (this.default_level_id == this.on_level.id)
      {
        Debug.LogWarning((object) ("Unable to find level for setting:" + this.id + "(" + level_id + ") Using default level."));
        return this.on_level;
      }
      if (this.default_level_id == this.off_level.id)
      {
        Debug.LogWarning((object) ("Unable to find level for setting:" + this.id + "(" + level_id + ") Using default level."));
        return this.off_level;
      }
      Debug.LogError((object) ("Unable to find setting level for setting:" + this.id + " level: " + level_id));
      return (SettingLevel) null;
    }

    public override List<SettingLevel> GetLevels() => new List<SettingLevel>()
    {
      this.off_level,
      this.on_level
    };

    public string ToggleSettingLevelID(string current_id) => this.on_level.id == current_id ? this.off_level.id : this.on_level.id;

    public bool IsOnLevel(string level_id) => level_id == this.on_level.id;
  }
}
