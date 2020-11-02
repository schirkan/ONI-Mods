﻿// Decompiled with JetBrains decompiler
// Type: IlluminationVulnerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class IlluminationVulnerable : StateMachineComponent<IlluminationVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause
{
  public float lightIntensityThreshold;
  private OccupyArea _occupyArea;
  private SchedulerHandle handle;
  public bool prefersDarkness;

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Illumination, this.gameObject));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public void SetPrefersDarkness(bool prefersDarkness = false) => this.prefersDarkness = prefersDarkness;

  protected override void OnCleanUp()
  {
    this.handle.ClearScheduler();
    base.OnCleanUp();
  }

  public bool IsCellSafe(int cell) => this.prefersDarkness ? (double) Grid.LightIntensity[cell] <= (double) this.lightIntensityThreshold : (double) Grid.LightIntensity[cell] > (double) this.lightIntensityThreshold;

  WiltCondition.Condition[] IWiltCause.Conditions => new WiltCondition.Condition[2]
  {
    WiltCondition.Condition.Darkness,
    WiltCondition.Condition.IlluminationComfort
  };

  public string WiltStateString
  {
    get
    {
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.too_bright))
        return Db.Get().CreatureStatusItems.Crop_Too_Bright.resolveStringCallback((string) CREATURES.STATUSITEMS.CROP_TOO_BRIGHT.NAME, (object) this);
      return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.too_dark) ? Db.Get().CreatureStatusItems.Crop_Too_Dark.resolveStringCallback((string) CREATURES.STATUSITEMS.CROP_TOO_DARK.NAME, (object) this) : "";
    }
  }

  public bool IsComfortable() => this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.comfortable);

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    if (this.prefersDarkness)
      return new List<Descriptor>()
      {
        new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement)
      };
    return new List<Descriptor>()
    {
      new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT, Descriptor.DescriptorType.Requirement)
    };
  }

  public class StatesInstance : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.GameInstance
  {
    public bool hasMaturity;

    public StatesInstance(IlluminationVulnerable master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable>
  {
    public StateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.BoolParameter illuminated;
    public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State comfortable;
    public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_dark;
    public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_bright;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.comfortable;
      double num;
      this.root.Update("Illumination", (System.Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) => num = (double) smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue((float) Grid.LightCount[Grid.PosToCell(smi.master.gameObject)])), UpdateRate.SIM_1000ms);
      this.comfortable.Update("Illumination.Comfortable", (System.Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (smi.master.IsCellSafe(cell))
          return;
        GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State state = smi.master.prefersDarkness ? this.too_bright : this.too_dark;
        smi.GoTo((StateMachine.BaseState) state);
      }), UpdateRate.SIM_1000ms).Enter((StateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State.Callback) (smi => smi.master.Trigger(1113102781, (object) null)));
      this.too_dark.TriggerOnEnter(GameHashes.IlluminationDiscomfort).Update("Illumination.too_dark", (System.Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (!smi.master.IsCellSafe(cell))
          return;
        smi.GoTo((StateMachine.BaseState) this.comfortable);
      }), UpdateRate.SIM_1000ms);
      this.too_bright.TriggerOnEnter(GameHashes.IlluminationDiscomfort).Update("Illumination.too_bright", (System.Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (!smi.master.IsCellSafe(cell))
          return;
        smi.GoTo((StateMachine.BaseState) this.comfortable);
      }), UpdateRate.SIM_1000ms);
    }
  }
}
