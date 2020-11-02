// Decompiled with JetBrains decompiler
// Type: CritterSensorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CritterSensorSideScreen : SideScreenContent
{
  public LogicCritterCountSensor targetSensor;
  public KToggle countCrittersToggle;
  public KToggle countEggsToggle;
  public KImage crittersCheckmark;
  public KImage eggsCheckmark;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.countCrittersToggle.onClick += new System.Action(this.ToggleCritters);
    this.countEggsToggle.onClick += new System.Action(this.ToggleEggs);
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<LogicCritterCountSensor>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetSensor = target.GetComponent<LogicCritterCountSensor>();
    this.crittersCheckmark.enabled = this.targetSensor.countCritters;
    this.eggsCheckmark.enabled = this.targetSensor.countEggs;
  }

  private void ToggleCritters()
  {
    this.targetSensor.countCritters = !this.targetSensor.countCritters;
    this.crittersCheckmark.enabled = this.targetSensor.countCritters;
  }

  private void ToggleEggs()
  {
    this.targetSensor.countEggs = !this.targetSensor.countEggs;
    this.eggsCheckmark.enabled = this.targetSensor.countEggs;
  }
}
