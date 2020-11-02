﻿// Decompiled with JetBrains decompiler
// Type: EquippableWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/EquippableWorkable")]
public class EquippableWorkable : Workable, ISaveLoadable
{
  [MyCmpReq]
  private Equippable equippable;
  private Chore chore;
  private QualityLevel quality;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_equip_clothing_kanim")
    };
    this.synchronizeAnims = false;
  }

  public QualityLevel GetQuality() => this.quality;

  public void SetQuality(QualityLevel level) => this.quality = level;

  protected override void OnSpawn()
  {
    this.SetWorkTime(1.5f);
    this.equippable.OnAssign += new System.Action<IAssignableIdentity>(this.RefreshChore);
  }

  private void CreateChore()
  {
    Debug.Assert(this.chore == null, (object) "chore should be null");
    this.chore = (Chore) new EquipChore((IStateMachineTarget) this);
  }

  public void CancelChore()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("Manual equip");
    this.chore = (Chore) null;
  }

  private void RefreshChore(IAssignableIdentity target)
  {
    if (this.chore != null)
    {
      this.chore.Cancel("Equipment Reassigned");
      this.chore = (Chore) null;
    }
    if (target == null || target.GetSoleOwner().GetComponent<Equipment>().IsEquipped(this.equippable))
      return;
    this.CreateChore();
  }

  protected override void OnCompleteWork(Worker worker)
  {
    if (this.equippable.assignee == null)
      return;
    Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
    if (!(bool) (UnityEngine.Object) soleOwner)
      return;
    soleOwner.GetComponent<Equipment>().Equip(this.equippable);
  }

  protected override void OnStopWork(Worker worker)
  {
    this.workTimeRemaining = this.GetWorkTime();
    base.OnStopWork(worker);
  }
}
