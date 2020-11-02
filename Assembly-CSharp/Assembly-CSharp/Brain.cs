// Decompiled with JetBrains decompiler
// Type: Brain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
  private bool running;
  private bool suspend;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    this.running = true;
    Components.Brains.Add(this);
  }

  public event System.Action onPreUpdate;

  public virtual void UpdateBrain()
  {
    if (this.onPreUpdate != null)
      this.onPreUpdate();
    if (!this.IsRunning())
      return;
    this.UpdateChores();
  }

  private bool FindBetterChore(ref Chore.Precondition.Context context) => this.GetComponent<ChoreConsumer>().FindNextChore(ref context);

  private void UpdateChores()
  {
    if (this.GetComponent<KPrefabID>().HasTag(GameTags.PreventChoreInterruption))
      return;
    Chore.Precondition.Context context = new Chore.Precondition.Context();
    if (!this.FindBetterChore(ref context))
      return;
    if (this.HasTag(GameTags.PerformingWorkRequest))
      this.Trigger(1485595942, (object) null);
    else
      this.GetComponent<ChoreDriver>().SetChore(context);
  }

  public bool IsRunning() => this.running && !this.suspend;

  public void Reset(string reason)
  {
    this.Stop(nameof (Reset));
    this.running = true;
  }

  public void Stop(string reason)
  {
    this.GetComponent<ChoreDriver>().StopChore();
    this.running = false;
  }

  public void Resume(string caller) => this.suspend = false;

  public void Suspend(string caller) => this.suspend = true;

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.Stop(nameof (OnCmpDisable));
  }

  protected override void OnCleanUp()
  {
    this.Stop(nameof (OnCleanUp));
    Components.Brains.Remove(this);
  }
}
