// Decompiled with JetBrains decompiler
// Type: UserNameable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/UserNameable")]
public class UserNameable : KMonoBehaviour
{
  [Serialize]
  public string savedName = "";

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (string.IsNullOrEmpty(this.savedName))
      this.SetName(this.gameObject.GetProperName());
    else
      this.SetName(this.savedName);
  }

  public void SetName(string name)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    this.name = name;
    if ((Object) component != (Object) null)
      component.SetName(name);
    this.gameObject.name = name;
    NameDisplayScreen.Instance.UpdateName(this.gameObject);
    this.savedName = name;
    this.Trigger(1102426921, (object) name);
  }
}
