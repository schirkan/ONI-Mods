// Decompiled with JetBrains decompiler
// Type: KFMODDebugger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Plugins/KFMODDebugger")]
public class KFMODDebugger : KMonoBehaviour
{
  public static KFMODDebugger instance;
  public List<KFMODDebugger.AudioDebugEntry> AudioDebugLog = new List<KFMODDebugger.AudioDebugEntry>();
  public Dictionary<KFMODDebugger.DebugSoundType, bool> allDebugSoundTypes = new Dictionary<KFMODDebugger.DebugSoundType, bool>();
  public bool debugEnabled;

  public static KFMODDebugger Get() => KFMODDebugger.instance;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    KFMODDebugger.instance = this;
    foreach (KFMODDebugger.DebugSoundType key in Enum.GetValues(typeof (KFMODDebugger.DebugSoundType)))
      this.allDebugSoundTypes.Add(key, false);
  }

  protected override void OnCleanUp() => KFMODDebugger.instance = (KFMODDebugger) null;

  [Conditional("ENABLE_KFMOD_LOGGER")]
  public void Log(string s)
  {
  }

  private KFMODDebugger.DebugSoundType GetDebugSoundType(string s)
  {
    if (s.Contains("Buildings"))
      return KFMODDebugger.DebugSoundType.Buildings;
    if (s.Contains("Notifications"))
      return KFMODDebugger.DebugSoundType.Notifications;
    if (s.Contains("UI"))
      return KFMODDebugger.DebugSoundType.UI;
    if (s.Contains("Creatures"))
      return KFMODDebugger.DebugSoundType.Creatures;
    if (s.Contains("Duplicant_voices"))
      return KFMODDebugger.DebugSoundType.DupeVoices;
    if (s.Contains("Ambience"))
      return KFMODDebugger.DebugSoundType.Ambience;
    if (s.Contains("Environment"))
      return KFMODDebugger.DebugSoundType.Environment;
    if (s.Contains("FX"))
      return KFMODDebugger.DebugSoundType.FX;
    if (s.Contains("Duplicant_actions/LowImportance/Movement"))
      return KFMODDebugger.DebugSoundType.DupeMovement;
    if (s.Contains("Duplicant_actions"))
      return KFMODDebugger.DebugSoundType.DupeActions;
    if (s.Contains("Plants"))
      return KFMODDebugger.DebugSoundType.Plants;
    return s.Contains("Music") ? KFMODDebugger.DebugSoundType.Music : KFMODDebugger.DebugSoundType.Uncategorized;
  }

  public struct AudioDebugEntry
  {
    public string log;
    public KFMODDebugger.DebugSoundType soundType;
    public float callTime;
  }

  public enum DebugSoundType
  {
    Uncategorized = -1, // 0xFFFFFFFF
    UI = 0,
    Notifications = 1,
    Buildings = 2,
    DupeVoices = 3,
    DupeMovement = 4,
    DupeActions = 5,
    Creatures = 6,
    Plants = 7,
    Ambience = 8,
    Environment = 9,
    FX = 10, // 0x0000000A
    Music = 11, // 0x0000000B
  }
}
