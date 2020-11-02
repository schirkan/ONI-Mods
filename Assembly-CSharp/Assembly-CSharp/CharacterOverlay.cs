// Decompiled with JetBrains decompiler
// Type: CharacterOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/CharacterOverlay")]
public class CharacterOverlay : KMonoBehaviour
{
  private bool registered;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Register();
  }

  public void Register()
  {
    if (this.registered)
      return;
    this.registered = true;
    NameDisplayScreen.Instance.AddNewEntry(this.gameObject);
  }
}
