﻿// Decompiled with JetBrains decompiler
// Type: CustomGameSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CustomGameSettings")]
public class CustomGameSettings : KMonoBehaviour
{
  private static CustomGameSettings instance;
  [Serialize]
  public bool is_custom_game;
  [Serialize]
  public CustomGameSettings.CustomGameMode customGameMode;
  [Serialize]
  private Dictionary<string, string> CurrentQualityLevelsBySetting = new Dictionary<string, string>();
  public Dictionary<string, SettingConfig> QualitySettings = new Dictionary<string, SettingConfig>();
  private string hexChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

  public static CustomGameSettings Instance => CustomGameSettings.instance;

  public event System.Action<SettingConfig, SettingLevel> OnSettingChanged;

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 6))
      this.customGameMode = this.is_custom_game ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Survival;
    if (!this.CurrentQualityLevelsBySetting.ContainsKey("CarePackages "))
      return;
    if (!this.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.CarePackages.id))
      this.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.CarePackages.id, this.CurrentQualityLevelsBySetting["CarePackages "]);
    this.CurrentQualityLevelsBySetting.Remove("CarePackages ");
  }

  protected override void OnPrefabInit()
  {
    CustomGameSettings.instance = this;
    this.AddSettingConfig((SettingConfig) CustomGameSettingConfigs.World);
    this.AddSettingConfig((SettingConfig) CustomGameSettingConfigs.WorldgenSeed);
    this.AddSettingConfig(CustomGameSettingConfigs.ImmuneSystem);
    this.AddSettingConfig(CustomGameSettingConfigs.CalorieBurn);
    this.AddSettingConfig(CustomGameSettingConfigs.Morale);
    this.AddSettingConfig(CustomGameSettingConfigs.Stress);
    this.AddSettingConfig(CustomGameSettingConfigs.StressBreaks);
    this.AddSettingConfig(CustomGameSettingConfigs.CarePackages);
    this.AddSettingConfig(CustomGameSettingConfigs.SandboxMode);
    this.AddSettingConfig(CustomGameSettingConfigs.FastWorkersMode);
    this.VerifySettingCoordinates();
  }

  public void SetSurvivalDefaults()
  {
    this.customGameMode = CustomGameSettings.CustomGameMode.Survival;
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
      this.SetQualitySetting(qualitySetting.Value, qualitySetting.Value.default_level_id);
  }

  public void SetNosweatDefaults()
  {
    this.customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
      this.SetQualitySetting(qualitySetting.Value, qualitySetting.Value.nosweat_default_level_id);
  }

  public SettingLevel CycleSettingLevel(ListSettingConfig config, int direction)
  {
    this.SetQualitySetting((SettingConfig) config, config.CycleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id], direction));
    return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
  }

  public SettingLevel ToggleSettingLevel(ToggleSettingConfig config)
  {
    this.SetQualitySetting((SettingConfig) config, config.ToggleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id]));
    return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
  }

  public void SetQualitySetting(SettingConfig config, string value)
  {
    this.CurrentQualityLevelsBySetting[config.id] = value;
    bool flag1 = true;
    bool flag2 = true;
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
    {
      if (this.QualitySettings[keyValuePair.Key].triggers_custom_game)
      {
        if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].default_level_id)
          flag1 = false;
        if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].nosweat_default_level_id)
          flag2 = false;
        if (!flag1)
        {
          if (!flag2)
            break;
        }
      }
    }
    CustomGameSettings.CustomGameMode customGameMode = !flag1 ? (!flag2 ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Nosweat) : CustomGameSettings.CustomGameMode.Survival;
    if (customGameMode != this.customGameMode)
    {
      DebugUtil.LogArgs((object) "Game mode changed from", (object) this.customGameMode, (object) "to", (object) customGameMode);
      this.customGameMode = customGameMode;
    }
    if (this.OnSettingChanged == null)
      return;
    this.OnSettingChanged(config, this.GetCurrentQualitySetting(config));
  }

  public SettingLevel GetCurrentQualitySetting(SettingConfig setting) => this.GetCurrentQualitySetting(setting.id);

  public SettingLevel GetCurrentQualitySetting(string setting_id)
  {
    SettingConfig qualitySetting = this.QualitySettings[setting_id];
    if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && qualitySetting.triggers_custom_game)
      return qualitySetting.GetLevel(qualitySetting.default_level_id);
    if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && qualitySetting.triggers_custom_game)
      return qualitySetting.GetLevel(qualitySetting.nosweat_default_level_id);
    if (!this.CurrentQualityLevelsBySetting.ContainsKey(setting_id))
      this.CurrentQualityLevelsBySetting[setting_id] = this.QualitySettings[setting_id].default_level_id;
    string level_id = this.CurrentQualityLevelsBySetting[setting_id];
    return this.QualitySettings[setting_id].GetLevel(level_id);
  }

  public string GetCurrentQualitySettingLevelId(SettingConfig config) => this.CurrentQualityLevelsBySetting[config.id];

  public string GetSettingLevelLabel(string setting_id, string level_id)
  {
    SettingConfig qualitySetting = this.QualitySettings[setting_id];
    if (qualitySetting != null)
    {
      SettingLevel level = qualitySetting.GetLevel(level_id);
      if (level != null)
        return level.label;
    }
    Debug.LogWarning((object) ("No label string for setting: " + setting_id + " level: " + level_id));
    return "";
  }

  public string GetSettingLevelTooltip(string setting_id, string level_id)
  {
    SettingConfig qualitySetting = this.QualitySettings[setting_id];
    if (qualitySetting != null)
    {
      SettingLevel level = qualitySetting.GetLevel(level_id);
      if (level != null)
        return level.tooltip;
    }
    Debug.LogWarning((object) ("No tooltip string for setting: " + setting_id + " level: " + level_id));
    return "";
  }

  public void AddSettingConfig(SettingConfig config)
  {
    this.QualitySettings.Add(config.id, config);
    if (this.CurrentQualityLevelsBySetting.ContainsKey(config.id) && !string.IsNullOrEmpty(this.CurrentQualityLevelsBySetting[config.id]))
      return;
    this.CurrentQualityLevelsBySetting[config.id] = config.default_level_id;
  }

  private static void AddWorldMods(object user_data, List<SettingLevel> levels)
  {
    string path = FileSystem.Normalize(System.IO.Path.Combine(SettingsCache.GetPath(), "worlds"));
    ListPool<string, CustomGameSettings>.PooledList pooledList1 = ListPool<string, CustomGameSettings>.Allocate();
    ListPool<string, CustomGameSettings>.PooledList pooledList2 = pooledList1;
    FileSystem.GetFiles(path, "*.yaml", (ICollection<string>) pooledList2);
    foreach (string str in (List<string>) pooledList1)
    {
      ProcGen.World world = YamlIO.LoadFile<ProcGen.World>(str);
      string worldName = Worlds.GetWorldName(str);
      levels.Add(new SettingLevel(worldName, world.name, world.description, userdata: user_data));
    }
    pooledList1.Recycle();
  }

  public void LoadWorlds()
  {
    Dictionary<string, ProcGen.World> worldCache = SettingsCache.worlds.worldCache;
    List<SettingLevel> levels = new List<SettingLevel>(worldCache.Count);
    foreach (KeyValuePair<string, ProcGen.World> keyValuePair in worldCache)
    {
      StringEntry result;
      string label = Strings.TryGet(new StringKey(keyValuePair.Value.name), out result) ? result.ToString() : keyValuePair.Value.name;
      string tooltip = Strings.TryGet(new StringKey(keyValuePair.Value.description), out result) ? result.ToString() : keyValuePair.Value.description;
      levels.Add(new SettingLevel(keyValuePair.Key, label, tooltip));
    }
    CustomGameSettingConfigs.World.StompLevels(levels, "worlds/SandstoneDefault", "worlds/SandstoneDefault");
  }

  public void Print()
  {
    string str = "Custom Settings: ";
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
      str = str + keyValuePair.Key + "=" + keyValuePair.Value + ",";
    Debug.Log((object) str);
  }

  private bool AllValuesMatch(
    Dictionary<string, string> data,
    CustomGameSettings.CustomGameMode mode)
  {
    bool flag = true;
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
    {
      if (!(qualitySetting.Key == CustomGameSettingConfigs.WorldgenSeed.id))
      {
        string str = (string) null;
        switch (mode)
        {
          case CustomGameSettings.CustomGameMode.Survival:
            str = qualitySetting.Value.default_level_id;
            break;
          case CustomGameSettings.CustomGameMode.Nosweat:
            str = qualitySetting.Value.nosweat_default_level_id;
            break;
        }
        if (data.ContainsKey(qualitySetting.Key) && data[qualitySetting.Key] != str)
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  public List<CustomGameSettings.MetricSettingsData> GetSettingsForMetrics()
  {
    List<CustomGameSettings.MetricSettingsData> metricSettingsDataList = new List<CustomGameSettings.MetricSettingsData>();
    metricSettingsDataList.Add(new CustomGameSettings.MetricSettingsData()
    {
      Name = "CustomGameMode",
      Value = this.customGameMode.ToString()
    });
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
      metricSettingsDataList.Add(new CustomGameSettings.MetricSettingsData()
      {
        Name = keyValuePair.Key,
        Value = keyValuePair.Value
      });
    CustomGameSettings.MetricSettingsData metricSettingsData = new CustomGameSettings.MetricSettingsData()
    {
      Name = "CustomGameModeActual",
      Value = CustomGameSettings.CustomGameMode.Custom.ToString()
    };
    foreach (CustomGameSettings.CustomGameMode mode in Enum.GetValues(typeof (CustomGameSettings.CustomGameMode)))
    {
      if (mode != CustomGameSettings.CustomGameMode.Custom && this.AllValuesMatch(this.CurrentQualityLevelsBySetting, mode))
      {
        metricSettingsData.Value = mode.ToString();
        break;
      }
    }
    metricSettingsDataList.Add(metricSettingsData);
    return metricSettingsDataList;
  }

  public bool VerifySettingCoordinates()
  {
    Dictionary<int, string> dictionary = new Dictionary<int, string>();
    bool flag1 = false;
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
    {
      if (qualitySetting.Value.coordinate_dimension < 0 || qualitySetting.Value.coordinate_dimension_width < 0)
      {
        if (qualitySetting.Value.coordinate_dimension >= 0 || qualitySetting.Value.coordinate_dimension_width >= 0)
        {
          flag1 = true;
          Debug.Assert(false, (object) (qualitySetting.Value.id + ": Both coordinate dimension props must be unset (-1) if either is unset."));
        }
      }
      else
      {
        List<SettingLevel> levels = qualitySetting.Value.GetLevels();
        if (qualitySetting.Value.coordinate_dimension_width < levels.Count)
        {
          flag1 = true;
          Debug.Assert(false, (object) (qualitySetting.Value.id + ": Range between coordinate min and max insufficient for all levels (" + (object) qualitySetting.Value.coordinate_dimension_width + "<" + (object) levels.Count + ")"));
        }
        foreach (SettingLevel settingLevel in levels)
        {
          int key = qualitySetting.Value.coordinate_dimension * settingLevel.coordinate_offset;
          string str1 = qualitySetting.Value.id + " > " + settingLevel.id;
          if (settingLevel.coordinate_offset < 0)
          {
            flag1 = true;
            Debug.Assert(false, (object) (str1 + ": Level coordinate offset must be >= 0"));
          }
          else if (settingLevel.coordinate_offset == 0)
          {
            if (settingLevel.id != qualitySetting.Value.default_level_id)
            {
              flag1 = true;
              Debug.Assert(false, (object) (str1 + ": Only the default level should have a coordinate offset of 0"));
            }
          }
          else if (settingLevel.coordinate_offset > qualitySetting.Value.coordinate_dimension_width)
          {
            flag1 = true;
            Debug.Assert(false, (object) (str1 + ": level coordinate must be <= dimension width"));
          }
          else
          {
            string str2;
            bool flag2 = !dictionary.TryGetValue(key, out str2);
            dictionary[key] = str1;
            if (settingLevel.id == qualitySetting.Value.default_level_id)
            {
              flag1 = true;
              Debug.Assert(false, (object) (str1 + ": Default level must be coordinate 0"));
            }
            if (!flag2)
            {
              flag1 = true;
              Debug.Assert(false, (object) (str1 + ": Combined coordinate conflicts with another coordinate (" + str2 + "). Ensure this SettingConfig's min and max don't overlap with another SettingConfig's"));
            }
          }
        }
      }
    }
    return flag1;
  }

  public string[] ParseSettingCoordinate(string coord)
  {
    Match match = new Regex("(.*)-(.*)-(.*)").Match(coord);
    string[] strArray = new string[match.Groups.Count];
    for (int groupnum = 0; groupnum < match.Groups.Count; ++groupnum)
      strArray[groupnum] = match.Groups[groupnum].Value;
    return strArray;
  }

  public string GetSettingsCoordinate()
  {
    ProcGen.World worldData = SettingsCache.worlds.GetWorldData(CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.World).id);
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed);
    string otherSettingsCode = this.GetOtherSettingsCode();
    return string.Format("{0}-{1}-{2}", (object) worldData.GetCoordinatePrefix(), (object) currentQualitySetting.id, (object) otherSettingsCode);
  }

  public void ParseAndApplySettingsCode(string code)
  {
    int num1 = this.Base36toBase10(code);
    Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
    {
      SettingConfig qualitySetting = this.QualitySettings[keyValuePair.Key];
      if (qualitySetting.coordinate_dimension >= 0 && qualitySetting.coordinate_dimension_width >= 0)
      {
        int num2 = 0;
        int num3 = qualitySetting.coordinate_dimension * qualitySetting.coordinate_dimension_width;
        int num4 = num1;
        if (num4 >= num3)
        {
          int num5 = num4 / num3 * num3;
          num4 -= num5;
        }
        if (num4 >= qualitySetting.coordinate_dimension)
          num2 = num4 / qualitySetting.coordinate_dimension;
        foreach (SettingLevel level in qualitySetting.GetLevels())
        {
          if (level.coordinate_offset == num2)
          {
            dictionary[qualitySetting] = level.id;
            break;
          }
        }
      }
    }
    foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
      this.SetQualitySetting(keyValuePair.Key, keyValuePair.Value);
  }

  private string GetOtherSettingsCode()
  {
    int input = 0;
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
    {
      SettingConfig settingConfig;
      this.QualitySettings.TryGetValue(keyValuePair.Key, out settingConfig);
      if (settingConfig != null && settingConfig.coordinate_dimension >= 0 && settingConfig.coordinate_dimension_width >= 0)
      {
        SettingLevel level = settingConfig.GetLevel(keyValuePair.Value);
        int num = settingConfig.coordinate_dimension * level.coordinate_offset;
        input += num;
      }
    }
    return this.Base10toBase36(input);
  }

  private int Base36toBase10(string input)
  {
    if (input == "0")
      return 0;
    int num = 0;
    for (int index = input.Length - 1; index >= 0; --index)
      num = num * 36 + this.hexChars.IndexOf(input[index]);
    return num;
  }

  private string Base10toBase36(int input)
  {
    if (input == 0)
      return "0";
    int num = input;
    string str = "";
    for (; num > 0; num /= 36)
      str += this.hexChars[num % 36].ToString();
    return str;
  }

  public enum CustomGameMode
  {
    Survival = 0,
    Nosweat = 1,
    Custom = 255, // 0x000000FF
  }

  public struct MetricSettingsData
  {
    public string Name;
    public string Value;
  }
}
