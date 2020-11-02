// Decompiled with JetBrains decompiler
// Type: SimTemperatureTransfer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimTemperatureTransfer")]
public class SimTemperatureTransfer : KMonoBehaviour
{
  private const float MIN_MASS_FOR_TEMPERATURE_TRANSFER = 0.01f;
  public float deltaKJ;
  public System.Action<SimTemperatureTransfer> onSimRegistered;
  protected int simHandle = -1;
  private float pendingEnergyModifications;
  [SerializeField]
  protected float surfaceArea = 10f;
  [SerializeField]
  protected float thickness = 0.01f;
  [SerializeField]
  protected float groundTransferScale = 1f / 16f;
  private static Dictionary<int, SimTemperatureTransfer> handleInstanceMap = new Dictionary<int, SimTemperatureTransfer>();

  public float SurfaceArea
  {
    get => this.surfaceArea;
    set => this.surfaceArea = value;
  }

  public float Thickness
  {
    get => this.thickness;
    set => this.thickness = value;
  }

  public float GroundTransferScale
  {
    get => this.GroundTransferScale;
    set => this.groundTransferScale = value;
  }

  public int SimHandle => this.simHandle;

  public static void ClearInstanceMap() => SimTemperatureTransfer.handleInstanceMap.Clear();

  public static void DoStateTransition(int sim_handle)
  {
    SimTemperatureTransfer cmp = (SimTemperatureTransfer) null;
    if (!SimTemperatureTransfer.handleInstanceMap.TryGetValue(sim_handle, out cmp) || (UnityEngine.Object) cmp == (UnityEngine.Object) null || cmp.HasTag(GameTags.Sealed))
      return;
    PrimaryElement component = cmp.GetComponent<PrimaryElement>();
    Element element = component.Element;
    if (element.highTempTransitionTarget == SimHashes.Unobtanium)
      return;
    if ((double) component.Mass > 0.0)
      SimMessages.AddRemoveSubstance(Grid.PosToCell(cmp.transform.GetPosition()), element.highTempTransitionTarget, CellEventLogger.Instance.OreMelted, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount);
    cmp.OnCleanUp();
    Util.KDestroyGameObject(cmp.gameObject);
  }

  protected override void OnPrefabInit()
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    component.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(SimTemperatureTransfer.OnGetTemperature);
    component.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(SimTemperatureTransfer.OnSetTemperature);
    component.onDataChanged += new System.Action<PrimaryElement>(this.OnDataChanged);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    Element element = component.Element;
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged), "SimTemperatureTransfer.OnSpawn");
    if (component.Element.HasTag(GameTags.Special) || (double) element.specificHeatCapacity == 0.0)
      this.enabled = false;
    this.SimRegister();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!Sim.IsValidHandle(this.simHandle))
      return;
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    SimTemperatureTransfer.OnSetTemperature(component, component.Temperature);
  }

  protected override void OnCmpDisable()
  {
    if (Sim.IsValidHandle(this.simHandle))
    {
      PrimaryElement component = this.GetComponent<PrimaryElement>();
      float temperature = component.Temperature;
      component.InternalTemperature = component.Temperature;
      SimMessages.SetElementChunkData(this.simHandle, temperature, 0.0f);
    }
    base.OnCmpDisable();
  }

  private void OnCellChanged()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(cell))
    {
      this.enabled = false;
    }
    else
    {
      if (!Sim.IsValidHandle(this.simHandle))
        return;
      SimMessages.MoveElementChunk(this.simHandle, cell);
    }
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged));
    this.SimUnregister();
    this.OnForcedCleanUp();
  }

  public void ModifyEnergy(float delta_kilojoules)
  {
    if (Sim.IsValidHandle(this.simHandle))
      SimMessages.ModifyElementChunkEnergy(this.simHandle, delta_kilojoules);
    else
      this.pendingEnergyModifications += delta_kilojoules;
  }

  private static unsafe float OnGetTemperature(PrimaryElement primary_element)
  {
    SimTemperatureTransfer component = primary_element.GetComponent<SimTemperatureTransfer>();
    float num;
    if (Sim.IsValidHandle(component.simHandle))
    {
      int handleIndex = Sim.GetHandleIndex(component.simHandle);
      num = Game.Instance.simData.elementChunks[handleIndex].temperature;
      component.deltaKJ = Game.Instance.simData.elementChunks[handleIndex].deltaKJ;
    }
    else
      num = primary_element.InternalTemperature;
    return num;
  }

  private static unsafe void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    if ((double) temperature <= 0.0)
    {
      KCrashReporter.Assert(false, "STT.OnSetTemperature - Tried to set <= 0 degree temperature");
      temperature = 293f;
    }
    SimTemperatureTransfer component = primary_element.GetComponent<SimTemperatureTransfer>();
    if (Sim.IsValidHandle(component.simHandle))
    {
      float mass = primary_element.Mass;
      float heat_capacity = (double) mass >= 0.00999999977648258 ? mass * primary_element.Element.specificHeatCapacity : 0.0f;
      SimMessages.SetElementChunkData(component.simHandle, temperature, heat_capacity);
      Game.Instance.simData.elementChunks[Sim.GetHandleIndex(component.simHandle)].temperature = temperature;
    }
    else
      primary_element.InternalTemperature = temperature;
  }

  private void OnDataChanged(PrimaryElement primary_element)
  {
    if (!Sim.IsValidHandle(this.simHandle))
      return;
    float heat_capacity = (double) primary_element.Mass >= 0.00999999977648258 ? primary_element.Mass * primary_element.Element.specificHeatCapacity : 0.0f;
    SimMessages.SetElementChunkData(this.simHandle, primary_element.Temperature, heat_capacity);
  }

  protected void SimRegister()
  {
    if (!this.isSpawned || this.simHandle != -1 || !this.enabled)
      return;
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if ((double) component.Mass <= 0.0 || component.Element.IsTemperatureInsulated)
      return;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.simHandle = -2;
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add(new System.Action<int, object>(SimTemperatureTransfer.OnSimRegisteredCallback), (object) this, "SimTemperatureTransfer.SimRegister");
    float num1 = component.InternalTemperature;
    if ((double) num1 <= 0.0)
    {
      component.InternalTemperature = 293f;
      num1 = 293f;
    }
    int elementId = (int) component.ElementID;
    double mass = (double) component.Mass;
    double num2 = (double) num1;
    double surfaceArea = (double) this.surfaceArea;
    double thickness = (double) this.thickness;
    double groundTransferScale = (double) this.groundTransferScale;
    int index = handle.index;
    SimMessages.AddElementChunk(cell, (SimHashes) elementId, (float) mass, (float) num2, (float) surfaceArea, (float) thickness, (float) groundTransferScale, index);
  }

  protected unsafe void SimUnregister()
  {
    if (this.simHandle == -1 || KMonoBehaviour.isLoadingScene)
      return;
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if (Sim.IsValidHandle(this.simHandle))
    {
      int handleIndex = Sim.GetHandleIndex(this.simHandle);
      component.InternalTemperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
      SimMessages.RemoveElementChunk(this.simHandle, -1);
      SimTemperatureTransfer.handleInstanceMap.Remove(this.simHandle);
    }
    this.simHandle = -1;
  }

  private static void OnSimRegisteredCallback(int handle, object data) => ((SimTemperatureTransfer) data).OnSimRegistered(handle);

  private unsafe void OnSimRegistered(int handle)
  {
    if ((UnityEngine.Object) this != (UnityEngine.Object) null && this.simHandle == -2)
    {
      this.simHandle = handle;
      if ((double) Game.Instance.simData.elementChunks[Sim.GetHandleIndex(handle)].temperature <= 0.0)
        KCrashReporter.Assert(false, "Bad temperature");
      SimTemperatureTransfer.handleInstanceMap[this.simHandle] = this;
      if ((double) this.pendingEnergyModifications > 0.0)
      {
        this.ModifyEnergy(this.pendingEnergyModifications);
        this.pendingEnergyModifications = 0.0f;
      }
      if (this.onSimRegistered == null)
        return;
      this.onSimRegistered(this);
    }
    else
      SimMessages.RemoveElementChunk(handle, -1);
  }
}
