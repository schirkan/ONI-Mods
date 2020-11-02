// Decompiled with JetBrains decompiler
// Type: RotPile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RotPile : StateMachineComponent<RotPile.StatesInstance>
{
  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected void ConvertToElement()
  {
    PrimaryElement component = this.smi.master.GetComponent<PrimaryElement>();
    float mass = component.Mass;
    float temperature = component.Temperature;
    if ((double) mass <= 0.0)
    {
      Util.KDestroyGameObject(this.gameObject);
    }
    else
    {
      SimHashes hash = SimHashes.ToxicSand;
      GameObject gameObject = ElementLoader.FindElementByHash(hash).substance.SpawnResource(this.smi.master.transform.GetPosition(), mass, temperature, byte.MaxValue, 0);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(hash).name, gameObject.transform);
      Util.KDestroyGameObject(this.smi.gameObject);
    }
  }

  public class StatesInstance : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.GameInstance
  {
    public AttributeModifier baseDecomposeRate;

    private static string OnRottenTooltip(List<Notification> notifications, object data)
    {
      string str = "\n";
      foreach (Notification notification in notifications)
      {
        if (notification.tooltipData != null)
          str = str + "\n" + (string) notification.tooltipData;
      }
      return string.Format((string) MISC.NOTIFICATIONS.FOODROT.TOOLTIP, (object) str);
    }

    public StatesInstance(RotPile master)
      : base(master)
    {
      if (!WorldInventory.Instance.IsReachable(this.smi.master.gameObject.GetComponent<Pickupable>()))
        return;
      this.gameObject.AddOrGet<Notifier>().Add(new Notification((string) MISC.NOTIFICATIONS.FOODROT.NAME, NotificationType.BadMinor, HashedString.Invalid, new Func<List<Notification>, object, string>(RotPile.StatesInstance.OnRottenTooltip))
      {
        tooltipData = (object) master.gameObject.GetProperName()
      });
    }
  }

  public class States : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile>
  {
    public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State decomposing;
    public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State convertDestroy;
    public StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.FloatParameter decompositionAmount;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.decomposing;
      this.serializable = true;
      double num;
      this.decomposing.ParamTransition<float>((StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.Parameter<float>) this.decompositionAmount, this.convertDestroy, (StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.Parameter<float>.Callback) ((smi, p) => (double) p >= 600.0)).Update("Decomposing", (System.Action<RotPile.StatesInstance, float>) ((smi, dt) => num = (double) this.decompositionAmount.Delta(dt, smi)));
      this.convertDestroy.Enter((StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State.Callback) (smi => smi.master.ConvertToElement()));
    }
  }
}
