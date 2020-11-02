// Decompiled with JetBrains decompiler
// Type: OneshotReactableHost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/OneshotReactableHost")]
public class OneshotReactableHost : KMonoBehaviour
{
  private Reactable reactable;
  public float lifetime = 1f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new System.Action<object>(this.OnExpire), (object) null, (SchedulerGroup) null);
  }

  public void SetReactable(Reactable reactable) => this.reactable = reactable;

  private void OnExpire(object obj)
  {
    if (!this.reactable.IsReacting)
    {
      this.reactable.Cleanup();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
      GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new System.Action<object>(this.OnExpire), (object) null, (SchedulerGroup) null);
  }
}
