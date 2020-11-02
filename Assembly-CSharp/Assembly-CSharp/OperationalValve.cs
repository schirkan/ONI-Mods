// Decompiled with JetBrains decompiler
// Type: OperationalValve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
  [MyCmpReq]
  private Operational operational;
  private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>((System.Action<OperationalValve, object>) ((component, data) => component.OnOperationalChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
  }

  protected override void OnSpawn()
  {
    this.OnOperationalChanged((object) this.operational.IsOperational);
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object data)
  {
    bool flag = (bool) data;
    if (flag)
      this.CurrentFlow = this.MaxFlow;
    else
      this.CurrentFlow = 0.0f;
    this.operational.SetActive(flag);
  }

  public override void UpdateAnim()
  {
    float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
    if (this.operational.IsOperational)
    {
      if ((double) averageRate > 0.0)
        this.controller.Queue((HashedString) "on_flow", KAnim.PlayMode.Loop);
      else
        this.controller.Queue((HashedString) "on");
    }
    else if ((double) averageRate > 0.0)
      this.controller.Queue((HashedString) "off_flow", KAnim.PlayMode.Loop);
    else
      this.controller.Queue((HashedString) "off");
  }
}
