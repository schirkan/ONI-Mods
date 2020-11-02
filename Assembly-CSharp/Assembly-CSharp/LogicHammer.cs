// Decompiled with JetBrains decompiler
// Type: LogicHammer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHammer : Switch
{
  protected KBatchedAnimController animController;
  private static readonly EventSystem.IntraObjectHandler<LogicHammer> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicHammer>((System.Action<LogicHammer, object>) ((component, data) => component.OnLogicValueChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<LogicHammer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicHammer>((System.Action<LogicHammer, object>) ((component, data) => component.OnOperationalChanged(data)));
  public static readonly HashedString PORT_ID = new HashedString("LogicHammerInput");
  private static string PARAMETER_NAME = "hammerObjectCount";
  private static string SOUND_EVENT_PREFIX = "Hammer_strike_";
  private static string DEFAULT_NO_SOUND_EVENT = "Hammer_strike_default";
  [MyCmpGet]
  private Operational operational;
  private int resonator_cell;
  private CellOffset target_offset = new CellOffset(-1, 0);
  private Rotatable rotatable;
  private int logic_value;
  private bool wasOn;
  protected static readonly HashedString[] ON_HIT_ANIMS = new HashedString[1]
  {
    (HashedString) "on_hit"
  };
  protected static readonly HashedString[] ON_MISS_ANIMS = new HashedString[1]
  {
    (HashedString) "on_miss"
  };
  protected static readonly HashedString[] OFF_ANIMS = new HashedString[2]
  {
    (HashedString) "off_pre",
    (HashedString) "off"
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.switchedOn = false;
    this.UpdateVisualState(false);
    this.rotatable = this.GetComponent<Rotatable>();
    CellOffset rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.target_offset);
    this.resonator_cell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), rotatedCellOffset);
    this.Subscribe<LogicHammer>(-801688580, LogicHammer.OnLogicValueChangedDelegate);
    this.Subscribe<LogicHammer>(-592767678, LogicHammer.OnOperationalChangedDelegate);
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    bool connected = false;
    if (this.operational.IsOperational & toggled_on)
    {
      connected = this.TriggerAudio();
      this.operational.SetActive(true);
    }
    else
      this.operational.SetActive(false);
    this.UpdateVisualState(connected);
  }

  private void OnOperationalChanged(object data)
  {
    if (this.operational.IsOperational)
      this.SetState(LogicCircuitNetwork.IsBitActive(0, this.logic_value));
    else
      this.UpdateVisualState(false);
  }

  private bool TriggerAudio()
  {
    if (this.wasOn || !this.switchedOn)
      return false;
    string b = (string) null;
    if (!Grid.IsValidCell(this.resonator_cell))
      return false;
    float f = float.NaN;
    GameObject gameObject = Grid.Objects[this.resonator_cell, 1];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
    {
      gameObject = Grid.Objects[this.resonator_cell, 30];
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        gameObject = Grid.Objects[this.resonator_cell, 26];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Wire component = gameObject.GetComponent<Wire>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            ElectricalUtilityNetwork networkForCell = (ElectricalUtilityNetwork) Game.Instance.electricalConduitSystem.GetNetworkForCell(component.GetNetworkCell());
            if (networkForCell != null)
              f = (float) networkForCell.allWires.Count;
          }
        }
        else
        {
          gameObject = Grid.Objects[this.resonator_cell, 31];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            if ((UnityEngine.Object) gameObject.GetComponent<LogicWire>() != (UnityEngine.Object) null)
            {
              LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.resonator_cell);
              if (networkForCell != null)
                f = (float) networkForCell.WireCount;
            }
          }
          else
          {
            gameObject = Grid.Objects[this.resonator_cell, 12];
            if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
            {
              Conduit component = gameObject.GetComponent<Conduit>();
              FlowUtilityNetwork networkForCell = (FlowUtilityNetwork) Conduit.GetNetworkManager(ConduitType.Gas).GetNetworkForCell(component.GetNetworkCell());
              if (networkForCell != null)
                f = (float) networkForCell.conduitCount;
            }
            else
            {
              gameObject = Grid.Objects[this.resonator_cell, 16];
              if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
              {
                Conduit component = gameObject.GetComponent<Conduit>();
                FlowUtilityNetwork networkForCell = (FlowUtilityNetwork) Conduit.GetNetworkManager(ConduitType.Liquid).GetNetworkForCell(component.GetNetworkCell());
                if (networkForCell != null)
                  f = (float) networkForCell.conduitCount;
              }
              else
              {
                gameObject = Grid.Objects[this.resonator_cell, 20];
                int num = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? 1 : 0;
              }
            }
          }
        }
      }
    }
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      Building component = (Building) gameObject.GetComponent<BuildingComplete>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        b = component.Def.PrefabID;
    }
    if (b == null)
      return false;
    string sound = GlobalAssets.GetSound(StringFormatter.Combine(LogicHammer.SOUND_EVENT_PREFIX, b), true) ?? GlobalAssets.GetSound(LogicHammer.DEFAULT_NO_SOUND_EVENT);
    Vector3 position = this.transform.position;
    position.z = 0.0f;
    EventInstance instance = KFMOD.BeginOneShot(sound, position);
    if (!float.IsNaN(f))
    {
      int num1 = (int) instance.setParameterValue(LogicHammer.PARAMETER_NAME, f);
    }
    KFMOD.EndOneShot(instance);
    return true;
  }

  private void UpdateVisualState(bool connected, bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    if (this.switchedOn)
    {
      if (connected)
        this.animController.Play(LogicHammer.ON_HIT_ANIMS);
      else
        this.animController.Play(LogicHammer.ON_MISS_ANIMS);
    }
    else
      this.animController.Play(LogicHammer.OFF_ANIMS);
  }

  private void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (!(logicValueChanged.portID == LogicHammer.PORT_ID))
      return;
    this.SetState(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
    this.logic_value = logicValueChanged.newValue;
  }
}
