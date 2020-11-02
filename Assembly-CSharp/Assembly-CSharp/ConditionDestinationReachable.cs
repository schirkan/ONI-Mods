﻿// Decompiled with JetBrains decompiler
// Type: ConditionDestinationReachable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ConditionDestinationReachable : RocketLaunchCondition
{
  private CommandModule commandModule;

  public ConditionDestinationReachable(CommandModule module) => this.commandModule = module;

  public override RocketLaunchCondition GetParentCondition() => (RocketLaunchCondition) null;

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
    return spacecraftDestination != null && this.CanReachDestination(spacecraftDestination) && spacecraftDestination.GetDestinationType().visitable ? RocketLaunchCondition.LaunchStatus.Ready : RocketLaunchCondition.LaunchStatus.Failure;
  }

  public bool CanReachDestination(SpaceDestination destination)
  {
    float rocketMaxDistance = this.commandModule.rocketStats.GetRocketMaxDistance();
    return (double) destination.OneBasedDistance * 10000.0 <= (double) rocketMaxDistance;
  }

  public SpaceDestination GetDestination()
  {
    int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
    return SpacecraftManager.instance.GetSpacecraftDestination(id);
  }

  public override string GetLaunchStatusMessage(bool ready)
  {
    if (ready && this.GetDestination() != null)
      return (string) UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
    return this.GetDestination() != null ? (string) UI.STARMAP.DESTINATIONSELECTION.UNREACHABLE : (string) UI.STARMAP.DESTINATIONSELECTION.NOTSELECTED;
  }

  public override string GetLaunchStatusTooltip(bool ready)
  {
    if (ready && this.GetDestination() != null)
      return (string) UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
    return this.GetDestination() != null ? (string) UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.UNREACHABLE : (string) UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
  }
}
