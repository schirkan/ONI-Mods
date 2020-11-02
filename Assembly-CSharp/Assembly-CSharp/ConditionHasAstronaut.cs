// Decompiled with JetBrains decompiler
// Type: ConditionHasAstronaut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

public class ConditionHasAstronaut : RocketLaunchCondition
{
  private CommandModule module;

  public ConditionHasAstronaut(CommandModule module) => this.module = module;

  public override RocketLaunchCondition GetParentCondition() => (RocketLaunchCondition) null;

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    List<MinionStorage.Info> storedMinionInfo = this.module.GetComponent<MinionStorage>().GetStoredMinionInfo();
    return storedMinionInfo.Count > 0 && storedMinionInfo[0].serializedMinion != null ? RocketLaunchCondition.LaunchStatus.Ready : RocketLaunchCondition.LaunchStatus.Failure;
  }

  public override string GetLaunchStatusMessage(bool ready) => ready ? (string) UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUT_TITLE : (string) UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;

  public override string GetLaunchStatusTooltip(bool ready) => ready ? (string) UI.STARMAP.LAUNCHCHECKLIST.HASASTRONAUT : (string) UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
}
