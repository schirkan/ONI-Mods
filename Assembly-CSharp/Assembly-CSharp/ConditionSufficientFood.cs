// Decompiled with JetBrains decompiler
// Type: ConditionSufficientFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ConditionSufficientFood : RocketLaunchCondition
{
  private CommandModule module;

  public ConditionSufficientFood(CommandModule module) => this.module = module;

  public override RocketLaunchCondition GetParentCondition() => (RocketLaunchCondition) null;

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition() => (double) this.module.storage.GetAmountAvailable(GameTags.Edible) <= 1.0 ? RocketLaunchCondition.LaunchStatus.Failure : RocketLaunchCondition.LaunchStatus.Ready;

  public override string GetLaunchStatusMessage(bool ready) => ready ? (string) UI.STARMAP.HASFOOD.NAME : (string) UI.STARMAP.NOFOOD.NAME;

  public override string GetLaunchStatusTooltip(bool ready) => ready ? (string) UI.STARMAP.HASFOOD.TOOLTIP : (string) UI.STARMAP.NOFOOD.TOOLTIP;
}
