﻿// Decompiled with JetBrains decompiler
// Type: SubmersionMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SubmersionMonitor")]
public class SubmersionMonitor : KMonoBehaviour, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
  private int position;
  private bool dry;
  protected float cellLiquidThreshold = 0.2f;
  private Extents extents;
  private HandleVector<int>.Handle partitionerEntry;

  public bool Dry => this.dry;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnMove();
    this.CheckDry();
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnMove), "SubmersionMonitor.OnSpawn");
  }

  private void OnMove()
  {
    this.position = Grid.PosToCell(this.gameObject);
    if (this.partitionerEntry.IsValid())
    {
      GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.position);
    }
    else
    {
      Vector2I xy = Grid.PosToXY(this.transform.GetPosition());
      this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", (object) this.gameObject, new Extents(xy.x, xy.y, 1, 2), GameScenePartitioner.Instance.liquidChangedLayer, new System.Action<object>(this.OnLiquidChanged));
    }
    this.CheckDry();
  }

  private void OnDrawGizmosSelected()
  {
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnMove));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  public void Configure(float _maxStamina, float _staminaRegenRate, float _cellLiquidThreshold = 0.95f) => this.cellLiquidThreshold = _cellLiquidThreshold;

  public void Sim1000ms(float dt) => this.CheckDry();

  private void CheckDry()
  {
    if (!this.IsCellSafe())
    {
      if (this.dry)
        return;
      this.dry = true;
      this.Trigger(-2057657673, (object) null);
    }
    else
    {
      if (!this.dry)
        return;
      this.dry = false;
      this.Trigger(1555379996, (object) null);
    }
  }

  public bool IsCellSafe()
  {
    int cell = Grid.PosToCell(this.gameObject);
    return Grid.IsValidCell(cell) && Grid.IsSubstantialLiquid(cell, this.cellLiquidThreshold);
  }

  private void OnLiquidChanged(object data) => this.CheckDry();

  WiltCondition.Condition[] IWiltCause.Conditions => new WiltCondition.Condition[1]
  {
    WiltCondition.Condition.DryingOut
  };

  public string WiltStateString => this.Dry ? Db.Get().CreatureStatusItems.DryingOut.resolveStringCallback((string) CREATURES.STATUSITEMS.DRYINGOUT.NAME, (object) this) : "";

  public void SetIncapacitated(bool state)
  {
  }

  public List<Descriptor> GetDescriptors(GameObject go) => new List<Descriptor>()
  {
    new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_SUBMERSION, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_SUBMERSION, Descriptor.DescriptorType.Requirement)
  };
}
