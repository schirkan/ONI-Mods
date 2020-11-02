// Decompiled with JetBrains decompiler
// Type: InfoDescription
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InfoDescription")]
public class InfoDescription : KMonoBehaviour
{
  public string nameLocString = "";
  public string descriptionLocString = "";
  public string description;
  public string displayName;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (!string.IsNullOrEmpty(this.nameLocString))
      this.displayName = (string) Strings.Get(this.nameLocString);
    if (string.IsNullOrEmpty(this.descriptionLocString))
      return;
    this.description = (string) Strings.Get(this.descriptionLocString);
  }
}
