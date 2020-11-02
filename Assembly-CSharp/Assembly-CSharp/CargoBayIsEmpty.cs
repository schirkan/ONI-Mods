// Decompiled with JetBrains decompiler
// Type: CargoBayIsEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class CargoBayIsEmpty : RocketLaunchCondition
{
  private CommandModule commandModule;

  public CargoBayIsEmpty(CommandModule module) => this.commandModule = module;

  public override RocketLaunchCondition GetParentCondition() => (RocketLaunchCondition) null;

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      CargoBay component = gameObject.GetComponent<CargoBay>();
      if ((Object) component != (Object) null && (double) component.storage.MassStored() != 0.0)
        return RocketLaunchCondition.LaunchStatus.Failure;
    }
    return RocketLaunchCondition.LaunchStatus.Ready;
  }

  public override string GetLaunchStatusMessage(bool ready)
  {
    int num = ready ? 1 : 0;
    return (string) UI.STARMAP.CARGOEMPTY.NAME;
  }

  public override string GetLaunchStatusTooltip(bool ready)
  {
    int num = ready ? 1 : 0;
    return (string) UI.STARMAP.CARGOEMPTY.TOOLTIP;
  }
}
