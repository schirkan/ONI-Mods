﻿// Decompiled with JetBrains decompiler
// Type: BuildingElementEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/BuildingElementEmitter")]
public class BuildingElementEmitter : KMonoBehaviour, IGameObjectEffectDescriptor, IElementEmitter, ISim200ms
{
  [SerializeField]
  public float emitRate = 0.3f;
  [SerializeField]
  [Serialize]
  public float temperature = 293f;
  [SerializeField]
  [HashedEnum]
  public SimHashes element = SimHashes.Oxygen;
  [SerializeField]
  public Vector2 modifierOffset;
  [SerializeField]
  public byte emitRange = 1;
  [SerializeField]
  public byte emitDiseaseIdx = byte.MaxValue;
  [SerializeField]
  public int emitDiseaseCount;
  private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
  private int simHandle = -1;
  private bool simActive;
  private bool dirty = true;
  private Guid statusHandle;
  private static readonly EventSystem.IntraObjectHandler<BuildingElementEmitter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<BuildingElementEmitter>((System.Action<BuildingElementEmitter, object>) ((component, data) => component.OnActiveChanged(data)));

  public float AverageEmitRate => Game.Instance.accumulators.GetAverageRate(this.accumulator);

  public float EmitRate => this.emitRate;

  public SimHashes Element => this.element;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.accumulator = Game.Instance.accumulators.Add("Element", (KMonoBehaviour) this);
    this.Subscribe<BuildingElementEmitter>(824508782, BuildingElementEmitter.OnActiveChangedDelegate);
    this.SimRegister();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.accumulator);
    this.SimUnregister();
    base.OnCleanUp();
  }

  private void OnActiveChanged(object data)
  {
    this.simActive = ((Operational) data).IsActive;
    this.dirty = true;
  }

  public void Sim200ms(float dt) => this.UnsafeUpdate(dt);

  private unsafe void UnsafeUpdate(float dt)
  {
    if (!Sim.IsValidHandle(this.simHandle))
      return;
    this.UpdateSimState();
    Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[Sim.GetHandleIndex(this.simHandle)];
    if ((double) emittedMassInfo.mass <= 0.0)
      return;
    Game.Instance.accumulators.Accumulate(this.accumulator, emittedMassInfo.mass);
    if (this.element != SimHashes.Oxygen)
      return;
    ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, emittedMassInfo.mass, this.gameObject.GetProperName());
  }

  private void UpdateSimState()
  {
    if (!this.dirty)
      return;
    this.dirty = false;
    if (this.simActive)
    {
      if (this.element != (SimHashes) 0 && (double) this.emitRate > 0.0)
        SimMessages.ModifyElementEmitter(this.simHandle, Grid.PosToCell(new Vector3(this.transform.GetPosition().x + this.modifierOffset.x, this.transform.GetPosition().y + this.modifierOffset.y, 0.0f)), (int) this.emitRange, this.element, 0.2f, this.emitRate * 0.2f, this.temperature, float.MaxValue, this.emitDiseaseIdx, this.emitDiseaseCount);
      this.statusHandle = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (object) this);
    }
    else
    {
      SimMessages.ModifyElementEmitter(this.simHandle, 0, 0, SimHashes.Vacuum, 0.0f, 0.0f, 0.0f, 0.0f, byte.MaxValue, 0);
      this.statusHandle = this.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, (bool) (UnityEngine.Object) this);
    }
  }

  private void SimRegister()
  {
    if (!this.isSpawned || this.simHandle != -1)
      return;
    this.simHandle = -2;
    SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new System.Action<int, object>(BuildingElementEmitter.OnSimRegisteredCallback), (object) this, nameof (BuildingElementEmitter)).index);
  }

  private void SimUnregister()
  {
    if (this.simHandle == -1)
      return;
    if (Sim.IsValidHandle(this.simHandle))
      SimMessages.RemoveElementEmitter(-1, this.simHandle);
    this.simHandle = -1;
  }

  private static void OnSimRegisteredCallback(int handle, object data) => ((BuildingElementEmitter) data).OnSimRegistered(handle);

  private void OnSimRegistered(int handle)
  {
    if ((UnityEngine.Object) this != (UnityEngine.Object) null)
      this.simHandle = handle;
    else
      SimMessages.RemoveElementEmitter(-1, handle);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string str = ElementLoader.FindElementByHash(this.element).tag.ProperName();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_FIXEDTEMP, (object) str, (object) GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond), (object) GameUtil.GetFormattedTemperature(this.temperature)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_FIXEDTEMP, (object) str, (object) GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond), (object) GameUtil.GetFormattedTemperature(this.temperature)));
    descriptorList.Add(descriptor);
    return descriptorList;
  }
}