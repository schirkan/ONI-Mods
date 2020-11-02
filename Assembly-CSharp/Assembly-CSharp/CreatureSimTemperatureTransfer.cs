// Decompiled with JetBrains decompiler
// Type: CreatureSimTemperatureTransfer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System.Collections.Generic;

public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
  public AttributeModifier averageTemperatureTransferPerSecond;
  private PrimaryElement primaryElement;
  public RunningWeightedAverage average_kilowatts_exchanged;
  public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();

  public float deltaEnergy
  {
    get => this.deltaKJ;
    protected set => this.deltaKJ = value;
  }

  public float currentExchangeWattage => (float) ((double) this.deltaKJ * 5.0 * 1000.0);

  protected override void OnPrefabInit()
  {
    this.primaryElement = this.GetComponent<PrimaryElement>();
    this.average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f, 20);
    this.surfaceArea = 1f;
    this.thickness = 1f / 500f;
    this.groundTransferScale = 0.0f;
    this.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier).Add(new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, this.thickness, (string) DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME));
    this.averageTemperatureTransferPerSecond = new AttributeModifier("TemperatureDelta", 0.0f, (string) DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME, uiOnly: true, is_readonly: false);
    this.GetAttributes().Add(this.averageTemperatureTransferPerSecond);
    base.OnPrefabInit();
  }

  public void Sim200ms(float dt)
  {
    this.average_kilowatts_exchanged.AddSample(this.currentExchangeWattage * (1f / 1000f));
    this.averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(this.average_kilowatts_exchanged.GetWeightedAverage, this.primaryElement.Element.specificHeatCapacity, this.primaryElement.Mass));
    float num = 0.0f;
    foreach (AttributeModifier temperatureModifier in this.NonSimTemperatureModifiers)
      num += temperatureModifier.Value;
    if (!Sim.IsValidHandle(this.simHandle))
      return;
    SimMessages.ModifyElementChunkEnergy(this.simHandle, (float) ((double) num * (double) dt * ((double) this.primaryElement.Mass * 1000.0) * (double) this.primaryElement.Element.specificHeatCapacity * (1.0 / 1000.0)));
  }

  public void RefreshRegistration()
  {
    this.SimUnregister();
    this.thickness = this.gameObject.GetAttributes().Get("ThermalConductivityBarrier").GetTotalValue();
    this.simHandle = -1;
    this.SimRegister();
  }

  public static float PotentialEnergyFlowToCreature(
    int cell,
    PrimaryElement transfererPrimaryElement,
    SimTemperatureTransfer temperatureTransferer,
    float deltaTime = 1f)
  {
    return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
  }
}
