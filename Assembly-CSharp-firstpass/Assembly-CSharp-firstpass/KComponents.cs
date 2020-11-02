// Decompiled with JetBrains decompiler
// Type: KComponents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public class KComponents
{
  private List<IComponentManager> managers = new List<IComponentManager>();
  private bool spawned;

  public virtual void Shutdown() => this.managers.Clear();

  protected T Add<T>(T manager) where T : IComponentManager
  {
    this.managers.Add((IComponentManager) manager);
    return manager;
  }

  public void Spawn()
  {
    if (this.spawned)
      return;
    this.spawned = true;
    foreach (IComponentManager manager in this.managers)
      manager.Spawn();
  }

  public void Sim33ms(float dt)
  {
    foreach (IComponentManager manager in this.managers)
      manager.FixedUpdate(dt);
  }

  public void RenderEveryTick(float dt)
  {
    foreach (IComponentManager manager in this.managers)
      manager.RenderEveryTick(dt);
  }

  public void Sim200ms(float dt)
  {
    foreach (IComponentManager manager in this.managers)
      manager.Sim200ms(dt);
  }

  public void CleanUp()
  {
    foreach (IComponentManager manager in this.managers)
      manager.CleanUp();
    this.spawned = false;
  }

  public void Clear()
  {
    foreach (IComponentManager manager in this.managers)
      manager.Clear();
    this.spawned = false;
  }
}
