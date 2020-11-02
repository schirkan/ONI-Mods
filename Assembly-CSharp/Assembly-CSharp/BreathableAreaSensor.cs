// Decompiled with JetBrains decompiler
// Type: BreathableAreaSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BreathableAreaSensor : Sensor
{
  private bool isBreathable;
  private OxygenBreather breather;

  public BreathableAreaSensor(Sensors sensors)
    : base(sensors)
  {
  }

  public override void Update()
  {
    if ((Object) this.breather == (Object) null)
      this.breather = this.GetComponent<OxygenBreather>();
    bool isBreathable = this.isBreathable;
    this.isBreathable = this.breather.IsBreathableElement;
    if (this.isBreathable == isBreathable)
      return;
    if (this.isBreathable)
      this.Trigger(99949694);
    else
      this.Trigger(-1189351068);
  }

  public bool IsBreathable() => this.isBreathable;

  public bool IsUnderwater() => this.breather.IsUnderLiquid;
}
