﻿// Decompiled with JetBrains decompiler
// Type: BaseNaming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("KMonoBehaviour/scripts/BaseNaming")]
public class BaseNaming : KMonoBehaviour
{
  [SerializeField]
  private TMP_InputField inputField;
  [SerializeField]
  private KButton shuffleBaseNameButton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GenerateBaseName();
    this.inputField.onValueChanged.AddListener((UnityAction<string>) (_param1 => Util.ScrubInputField(this.inputField)));
    this.shuffleBaseNameButton.onClick += new System.Action(this.GenerateBaseName);
    this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
  }

  private void OnEndEdit(string newName)
  {
    if (Localization.HasDirtyWords(newName))
    {
      this.inputField.text = this.GenerateBaseNameString();
      newName = this.inputField.text;
    }
    if (string.IsNullOrEmpty(newName))
      return;
    this.inputField.text = newName;
    SaveGame.Instance.SetBaseName(newName);
    string path = newName;
    if (!path.Contains(".sav"))
      path += ".sav";
    string prefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
    if (!path.Contains(prefixAndCreateFolder))
      path = prefixAndCreateFolder + path;
    SaveLoader.SetActiveSaveFilePath(path);
  }

  private void GenerateBaseName()
  {
    string baseNameString = this.GenerateBaseNameString();
    ((TMP_Text) this.inputField.placeholder).text = baseNameString;
    this.inputField.text = baseNameString;
    this.OnEndEdit(baseNameString);
  }

  private string GenerateBaseNameString()
  {
    string fullString = this.ReplaceStringWithRandom(LocString.GetStrings(typeof (NAMEGEN.COLONY.FORMATS)).GetRandom<string>(), "{noun}", LocString.GetStrings(typeof (NAMEGEN.COLONY.NOUN)));
    string[] strings = LocString.GetStrings(typeof (NAMEGEN.COLONY.ADJECTIVE));
    return this.ReplaceStringWithRandom(this.ReplaceStringWithRandom(this.ReplaceStringWithRandom(this.ReplaceStringWithRandom(fullString, "{adjective}", strings), "{adjective2}", strings), "{adjective3}", strings), "{adjective4}", strings);
  }

  private string ReplaceStringWithRandom(
    string fullString,
    string replacementKey,
    string[] replacementValues)
  {
    return !fullString.Contains(replacementKey) ? fullString : fullString.Replace(replacementKey, replacementValues.GetRandom<string>());
  }
}
