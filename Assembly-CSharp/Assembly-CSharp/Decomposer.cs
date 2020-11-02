// Decompiled with JetBrains decompiler
// Type: Decomposer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Decomposer")]
public class Decomposer : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    StateMachineController component = this.GetComponent<StateMachineController>();
    if ((Object) component == (Object) null)
      return;
    DecompositionMonitor.Instance instance = new DecompositionMonitor.Instance((IStateMachineTarget) this, (Klei.AI.Disease) null, 1f, false);
    component.AddStateMachineInstance((StateMachine.Instance) instance);
    instance.StartSM();
    instance.dirtyWaterMaxRange = 3;
  }
}
