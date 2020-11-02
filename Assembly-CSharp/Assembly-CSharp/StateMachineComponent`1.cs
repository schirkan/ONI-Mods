﻿// Decompiled with JetBrains decompiler
// Type: StateMachineComponent`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

[SerializationConfig(MemberSerialization.OptIn)]
public class StateMachineComponent<StateMachineInstanceType> : StateMachineComponent, ISaveLoadable
  where StateMachineInstanceType : StateMachine.Instance
{
  private StateMachineInstanceType _smi;

  public StateMachineInstanceType smi
  {
    get
    {
      if ((object) this._smi == null)
        this._smi = (StateMachineInstanceType) Activator.CreateInstance(typeof (StateMachineInstanceType), (object) this);
      return this._smi;
    }
  }

  public override StateMachine.Instance GetSMI() => (StateMachine.Instance) this._smi;

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if ((object) this._smi == null)
      return;
    this._smi.StopSM("StateMachineComponent.OnCleanUp");
    this._smi = default (StateMachineInstanceType);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!this.isSpawned)
      return;
    this.smi.StartSM();
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if ((object) this._smi == null)
      return;
    this._smi.StopSM("StateMachineComponent.OnDisable");
  }
}
