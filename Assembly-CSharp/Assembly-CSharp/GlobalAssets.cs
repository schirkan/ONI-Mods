// Decompiled with JetBrains decompiler
// Type: GlobalAssets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalAssets : KMonoBehaviour
{
  private static Dictionary<string, string> SoundTable = new Dictionary<string, string>();
  private static HashSet<string> LowPrioritySounds = new HashSet<string>();
  private static HashSet<string> HighPrioritySounds = new HashSet<string>();
  public ColorSet colorSet;
  public ColorSet[] colorSetOptions;

  public static GlobalAssets Instance { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GlobalAssets.Instance = this;
    if (GlobalAssets.SoundTable.Count == 0)
    {
      Bank[] array1 = (Bank[]) null;
      try
      {
        if (RuntimeManager.StudioSystem.getBankList(out array1) != RESULT.OK)
          array1 = (Bank[]) null;
      }
      catch
      {
        array1 = (Bank[]) null;
      }
      if (array1 != null)
      {
        foreach (Bank bank in array1)
        {
          EventDescription[] array2;
          RESULT eventList = bank.getEventList(out array2);
          string path1;
          if (eventList != RESULT.OK)
          {
            int path2 = (int) bank.getPath(out path1);
            Debug.LogError((object) string.Format("ERROR [{0}] loading FMOD events for bank [{1}]", (object) eventList, (object) path1));
          }
          else
          {
            for (int index = 0; index < array2.Length; ++index)
            {
              int path2 = (int) array2[index].getPath(out path1);
              string lowerInvariant = Assets.GetSimpleSoundEventName(path1).ToLowerInvariant();
              if (lowerInvariant.Length > 0 && !GlobalAssets.SoundTable.ContainsKey(lowerInvariant))
              {
                GlobalAssets.SoundTable[lowerInvariant] = path1;
                if (path1.ToLower().Contains("lowpriority") || lowerInvariant.Contains("lowpriority"))
                  GlobalAssets.LowPrioritySounds.Add(path1);
                else if (path1.ToLower().Contains("highpriority") || lowerInvariant.Contains("highpriority"))
                  GlobalAssets.HighPrioritySounds.Add(path1);
              }
            }
          }
        }
      }
    }
    SetDefaults.Initialize();
    GraphicsOptionsScreen.SetColorModeFromPrefs();
    this.AddColorModeStyles();
    LocString.CreateLocStringKeys(typeof (DUPLICANTS));
    LocString.CreateLocStringKeys(typeof (MISC));
    LocString.CreateLocStringKeys(typeof (UI));
    LocString.CreateLocStringKeys(typeof (ELEMENTS));
    LocString.CreateLocStringKeys(typeof (CREATURES));
    LocString.CreateLocStringKeys(typeof (SETITEMS));
    LocString.CreateLocStringKeys(typeof (RESEARCH));
    LocString.CreateLocStringKeys(typeof (ITEMS));
    LocString.CreateLocStringKeys(typeof (INPUT));
    LocString.CreateLocStringKeys(typeof (INPUT_BINDINGS));
    LocString.CreateLocStringKeys(typeof (BUILDING.STATUSITEMS), "STRINGS.BUILDING.");
    LocString.CreateLocStringKeys(typeof (BUILDING.DETAILS), "STRINGS.BUILDING.");
    LocString.CreateLocStringKeys(typeof (ROBOTS));
    LocString.CreateLocStringKeys(typeof (LORE));
    LocString.CreateLocStringKeys(typeof (CODEX));
    LocString.CreateLocStringKeys(typeof (WORLDS));
    LocString.CreateLocStringKeys(typeof (WORLD_TRAITS));
    LocString.CreateLocStringKeys(typeof (COLONY_ACHIEVEMENTS));
    LocString.CreateLocStringKeys(typeof (VIDEOS));
  }

  private void AddColorModeStyles()
  {
    TMP_StyleSheet.instance.AddStyle(new TMP_Style("logic_on", string.Format("<color=#{0}>", (object) ColorUtility.ToHtmlStringRGB((Color) this.colorSet.logicOn)), "</color>"));
    TMP_StyleSheet.instance.AddStyle(new TMP_Style("logic_off", string.Format("<color=#{0}>", (object) ColorUtility.ToHtmlStringRGB((Color) this.colorSet.logicOff)), "</color>"));
    TMP_StyleSheet.RefreshStyles();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GlobalAssets.Instance = (GlobalAssets) null;
  }

  public static string GetSound(string name, bool force_no_warning = false)
  {
    if (name == null)
      return (string) null;
    name = name.ToLowerInvariant();
    string str = (string) null;
    GlobalAssets.SoundTable.TryGetValue(name, out str);
    return str;
  }

  public static bool IsLowPriority(string path) => GlobalAssets.LowPrioritySounds.Contains(path);

  public static bool IsHighPriority(string path) => GlobalAssets.HighPrioritySounds.Contains(path);
}
