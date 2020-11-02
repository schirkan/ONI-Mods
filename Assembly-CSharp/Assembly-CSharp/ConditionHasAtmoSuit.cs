// Decompiled with JetBrains decompiler
// Type: ConditionHasAtmoSuit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ConditionHasAtmoSuit : RocketLaunchCondition
{
  private CommandModule module;

  public ConditionHasAtmoSuit(CommandModule module)
  {
    this.module = module;
    ManualDeliveryKG orAdd = this.module.FindOrAdd<ManualDeliveryKG>();
    orAdd.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    orAdd.SetStorage(module.storage);
    orAdd.requestedItemTag = GameTags.AtmoSuit;
    orAdd.minimumMass = 1f;
    orAdd.refillMass = 0.1f;
    orAdd.capacity = 1f;
  }

  public override RocketLaunchCondition GetParentCondition() => (RocketLaunchCondition) null;

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition() => (double) this.module.storage.GetAmountAvailable(GameTags.AtmoSuit) < 1.0 ? RocketLaunchCondition.LaunchStatus.Failure : RocketLaunchCondition.LaunchStatus.Ready;

  public override string GetLaunchStatusMessage(bool ready) => ready ? (string) UI.STARMAP.HASSUIT.NAME : (string) UI.STARMAP.NOSUIT.NAME;

  public override string GetLaunchStatusTooltip(bool ready) => ready ? (string) UI.STARMAP.HASSUIT.TOOLTIP : (string) UI.STARMAP.NOSUIT.TOOLTIP;
}
