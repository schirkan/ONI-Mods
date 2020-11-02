// Decompiled with JetBrains decompiler
// Type: CreatureFeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
  public string effectId;
  private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>((System.Action<CreatureFeeder, object>) ((component, data) => component.OnAteFromStorage(data)));

  protected override void OnSpawn()
  {
    Components.CreatureFeeders.Add(this);
    this.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
  }

  protected override void OnCleanUp() => Components.CreatureFeeders.Remove(this);

  private void OnAteFromStorage(object data)
  {
    if (string.IsNullOrEmpty(this.effectId))
      return;
    (data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
  }
}
