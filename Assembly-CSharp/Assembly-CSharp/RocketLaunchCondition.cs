// Decompiled with JetBrains decompiler
// Type: RocketLaunchCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class RocketLaunchCondition
{
  public abstract RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition();

  public abstract string GetLaunchStatusMessage(bool ready);

  public abstract string GetLaunchStatusTooltip(bool ready);

  public virtual RocketLaunchCondition GetParentCondition() => (RocketLaunchCondition) null;

  public enum LaunchStatus
  {
    Ready,
    Warning,
    Failure,
  }
}
