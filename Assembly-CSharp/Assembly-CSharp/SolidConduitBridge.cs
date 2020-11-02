// Decompiled with JetBrains decompiler
// Type: SolidConduitBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitBridge")]
public class SolidConduitBridge : KMonoBehaviour
{
  [MyCmpGet]
  private Operational operational;
  private int inputCell;
  private int outputCell;
  private bool dispensing;

  public bool IsDispensing => this.dispensing;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.inputCell = component.GetUtilityInputCell();
    this.outputCell = component.GetUtilityOutputCell();
    SolidConduit.GetFlowManager().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
  }

  protected override void OnCleanUp()
  {
    SolidConduit.GetFlowManager().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    base.OnCleanUp();
  }

  private void ConduitUpdate(float dt)
  {
    this.dispensing = false;
    if ((bool) (UnityEngine.Object) this.operational && !this.operational.IsOperational)
      return;
    SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
    if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell) || (!flowManager.IsConduitFull(this.inputCell) || !flowManager.IsConduitEmpty(this.outputCell)))
      return;
    Pickupable pickupable = flowManager.RemovePickupable(this.inputCell);
    if (!(bool) (UnityEngine.Object) pickupable)
      return;
    flowManager.AddPickupable(this.outputCell, pickupable);
    this.dispensing = true;
  }
}
