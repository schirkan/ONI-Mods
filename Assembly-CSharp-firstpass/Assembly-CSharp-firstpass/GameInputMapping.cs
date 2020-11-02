// Decompiled with JetBrains decompiler
// Type: GameInputMapping
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameInputMapping
{
  public static BindingEntry[] KeyBindings;

  public static HashSet<KeyCode> GetKeyCodes()
  {
    HashSet<KeyCode> keyCodeSet = new HashSet<KeyCode>();
    foreach (BindingEntry bindingEntry in GameInputMapping.GetBindingEntries())
    {
      if (bindingEntry.mKeyCode < KKeyCode.KleiKeys)
        keyCodeSet.Add((KeyCode) bindingEntry.mKeyCode);
    }
    keyCodeSet.Add(KeyCode.LeftAlt);
    keyCodeSet.Add(KeyCode.LeftControl);
    keyCodeSet.Add(KeyCode.LeftShift);
    keyCodeSet.Add(KeyCode.CapsLock);
    return keyCodeSet;
  }

  public static HashSet<string> GetAxis() => new HashSet<string>()
  {
    "Mouse X",
    "Mouse Y",
    "Mouse ScrollWheel"
  };

  public static BindingEntry[] DefaultBindings { get; private set; }

  public static void SetDefaultKeyBindings(BindingEntry[] default_keybindings)
  {
    GameInputMapping.DefaultBindings = default_keybindings;
    GameInputMapping.KeyBindings = (BindingEntry[]) default_keybindings.Clone();
  }

  public static BindingEntry[] GetBindingEntries() => GameInputMapping.KeyBindings;

  public static BindingEntry FindEntry(Action mAction)
  {
    foreach (BindingEntry keyBinding in GameInputMapping.KeyBindings)
    {
      if (keyBinding.mAction == mAction)
        return keyBinding;
    }
    Debug.Assert(false, (object) ("Unbound action " + mAction.ToString()));
    return GameInputMapping.KeyBindings[0];
  }

  public static bool CompareActionKeyCodes(Action a, Action b)
  {
    BindingEntry entry1 = GameInputMapping.FindEntry(a);
    BindingEntry entry2 = GameInputMapping.FindEntry(b);
    return entry1.mKeyCode == entry2.mKeyCode && entry1.mModifier == entry2.mModifier;
  }

  public static BindingEntry[] FindEntriesByKeyCode(KKeyCode keycode)
  {
    List<BindingEntry> bindingEntryList = new List<BindingEntry>();
    foreach (BindingEntry keyBinding in GameInputMapping.KeyBindings)
    {
      if (keyBinding.mKeyCode == keycode)
        bindingEntryList.Add(keyBinding);
    }
    return bindingEntryList.ToArray();
  }

  private static string BindingsFilename => Path.Combine(Util.RootFolder(), "keybindings.json");

  public static void SaveBindings()
  {
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    List<BindingEntry> bindingEntryList = new List<BindingEntry>();
    foreach (BindingEntry keyBinding in GameInputMapping.KeyBindings)
    {
      bool flag = false;
      foreach (BindingEntry defaultBinding in GameInputMapping.DefaultBindings)
      {
        if (keyBinding == defaultBinding)
        {
          flag = true;
          break;
        }
      }
      if (!flag && keyBinding.mRebindable)
        bindingEntryList.Add(keyBinding);
    }
    if (bindingEntryList.Count > 0)
    {
      File.WriteAllText(GameInputMapping.BindingsFilename, JsonConvert.SerializeObject((object) bindingEntryList));
    }
    else
    {
      if (!File.Exists(GameInputMapping.BindingsFilename))
        return;
      File.Delete(GameInputMapping.BindingsFilename);
    }
  }

  public static void LoadBindings()
  {
    GameInputMapping.KeyBindings = (BindingEntry[]) GameInputMapping.DefaultBindings.Clone();
    if (!File.Exists(GameInputMapping.BindingsFilename))
      return;
    string str = File.ReadAllText(GameInputMapping.BindingsFilename);
    if (str == null || str == "")
      return;
    BindingEntry[] bindingEntryArray = (BindingEntry[]) null;
    try
    {
      bindingEntryArray = JsonConvert.DeserializeObject<BindingEntry[]>(str);
    }
    catch
    {
      DebugUtil.LogErrorArgs((object) "Error parsing", (object) GameInputMapping.BindingsFilename);
    }
    if (bindingEntryArray == null || bindingEntryArray.Length == 0)
      return;
    for (int index = 0; index < GameInputMapping.KeyBindings.Length; ++index)
    {
      BindingEntry keyBinding = GameInputMapping.KeyBindings[index];
      foreach (BindingEntry bindingEntry1 in bindingEntryArray)
      {
        if (bindingEntry1.mAction == keyBinding.mAction && keyBinding.mRebindable)
        {
          BindingEntry bindingEntry2 = keyBinding;
          bindingEntry2.mButton = bindingEntry1.mButton;
          bindingEntry2.mKeyCode = bindingEntry1.mKeyCode;
          bindingEntry2.mModifier = bindingEntry1.mModifier;
          GameInputMapping.KeyBindings[index] = bindingEntry2;
          break;
        }
      }
    }
  }
}
