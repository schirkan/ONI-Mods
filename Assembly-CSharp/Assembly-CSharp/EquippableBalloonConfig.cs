// Decompiled with JetBrains decompiler
// Type: EquippableBalloonConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class EquippableBalloonConfig : IEquipmentConfig
{
  public const string ID = "EquippableBalloon";
  private BalloonFX.Instance fx;

  public EquipmentDef CreateEquipmentDef()
  {
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("EquippableBalloon", EQUIPMENT.TOYS.SLOT, SimHashes.Carbon, EQUIPMENT.TOYS.BALLOON_MASS, EQUIPMENT.VESTS.COOL_VEST_ICON0, (string) null, (string) null, 0, AttributeModifiers, CollisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.75f, height: 0.4f);
    equipmentDef.OnEquipCallBack = new System.Action<Equippable>(this.OnEquipBalloon);
    equipmentDef.OnUnequipCallBack = new System.Action<Equippable>(this.OnUnequipBalloon);
    return equipmentDef;
  }

  private void OnEquipBalloon(Equippable eq)
  {
    if (!((UnityEngine.Object) eq != (UnityEngine.Object) null) || eq.assignee == null)
      return;
    Ownables soleOwner = eq.assignee.GetSoleOwner();
    if ((UnityEngine.Object) soleOwner == (UnityEngine.Object) null)
      return;
    MinionAssignablesProxy component1 = soleOwner.GetComponent<MinionAssignablesProxy>();
    Effects component2 = (component1.target as KMonoBehaviour).GetComponent<Effects>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component2.Add("HasBalloon", false);
    this.fx = new BalloonFX.Instance((IStateMachineTarget) (component1.target as KMonoBehaviour).GetComponent<KMonoBehaviour>());
    this.fx.StartSM();
  }

  private void OnUnequipBalloon(Equippable eq)
  {
    if ((UnityEngine.Object) eq != (UnityEngine.Object) null && eq.assignee != null)
    {
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if ((UnityEngine.Object) soleOwner == (UnityEngine.Object) null)
        return;
      MinionAssignablesProxy component1 = soleOwner.GetComponent<MinionAssignablesProxy>();
      if (component1.target != null)
      {
        Effects component2 = (component1.target as KMonoBehaviour).GetComponent<Effects>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.Remove("HasBalloon");
      }
    }
    if (this.fx != null)
      this.fx.StopSM("Unequipped");
    Util.KDestroyGameObject(eq.gameObject);
  }

  public void DoPostConfigure(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
    Equippable equippable = go.GetComponent<Equippable>();
    if ((UnityEngine.Object) equippable == (UnityEngine.Object) null)
      equippable = go.AddComponent<Equippable>();
    equippable.hideInCodex = true;
    equippable.unequippable = false;
    go.AddOrGet<EquippableBalloon>();
  }
}
