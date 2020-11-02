// Decompiled with JetBrains decompiler
// Type: KComponentManager`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public abstract class KComponentManager<T> : KCompactedVector<T>, IComponentManager
  where T : new()
{
  protected Dictionary<object, HandleVector<int>.Handle> instanceHandleMap = new Dictionary<object, HandleVector<int>.Handle>();
  private HashSet<HandleVector<int>.Handle> spawnList = new HashSet<HandleVector<int>.Handle>();
  private HashSet<HandleVector<int>.Handle> shadowSpawnList = new HashSet<HandleVector<int>.Handle>();
  protected List<KComponentManager<T>.CleanupInfo> cleanupList = new List<KComponentManager<T>.CleanupInfo>();
  private List<KComponentManager<T>.CleanupInfo> shadowCleanupList = new List<KComponentManager<T>.CleanupInfo>();

  public string Name { get; set; }

  public KComponentManager()
    : base()
    => this.Name = this.GetType().Name;

  public bool Has(object go) => !this.cleanupList.Exists((Predicate<KComponentManager<T>.CleanupInfo>) (x => x.instance == go)) && !(this.GetHandle(go) == HandleVector<int>.InvalidHandle);

  protected HandleVector<int>.Handle InternalAddComponent(object instance, T cmp_values)
  {
    HandleVector<int>.Handle handle = HandleVector<int>.InvalidHandle;
    this.RemoveFromCleanupList(instance);
    if (!this.instanceHandleMap.TryGetValue(instance, out handle))
    {
      handle = this.Allocate(cmp_values);
      this.instanceHandleMap[instance] = handle;
    }
    else
      this.SetData(handle, cmp_values);
    this.spawnList.Remove(handle);
    this.OnPrefabInit(handle);
    this.spawnList.Add(handle);
    return handle;
  }

  protected void InternalRemoveComponent(KComponentManager<T>.CleanupInfo info)
  {
    if (info.instance != null)
    {
      if (!this.instanceHandleMap.ContainsKey(info.instance))
      {
        DebugUtil.LogErrorArgs((object) "Tried to remove component of type", (object) typeof (T).ToString(), (object) "on instance", (object) info.instance.ToString(), (object) "but instance has not been registered yet. Handle:", (object) info.handle);
        return;
      }
      this.instanceHandleMap.Remove(info.instance);
    }
    else
    {
      foreach (KeyValuePair<object, HandleVector<int>.Handle> instanceHandle in this.instanceHandleMap)
      {
        if (instanceHandle.Value == info.handle)
          this.instanceHandleMap.Remove(instanceHandle.Key);
      }
    }
    this.Free(info.handle);
    this.spawnList.Remove(info.handle);
  }

  public HandleVector<int>.Handle GetHandle(object instance)
  {
    HandleVector<int>.Handle invalidHandle;
    if (!this.instanceHandleMap.TryGetValue(instance, out invalidHandle))
      invalidHandle = HandleVector<int>.InvalidHandle;
    return invalidHandle;
  }

  public void Spawn()
  {
    HashSet<HandleVector<int>.Handle> spawnList = this.spawnList;
    this.spawnList = this.shadowSpawnList;
    this.shadowSpawnList = spawnList;
    this.spawnList.Clear();
    foreach (KComponentManager<T>.CleanupInfo cleanup in this.cleanupList)
      this.shadowSpawnList.Remove(this.GetHandle((object) cleanup));
    foreach (HandleVector<int>.Handle shadowSpawn in this.shadowSpawnList)
      this.OnSpawn(shadowSpawn);
    this.shadowSpawnList.Clear();
  }

  public virtual void RenderEveryTick(float dt)
  {
  }

  public virtual void FixedUpdate(float dt)
  {
  }

  public virtual void Sim200ms(float dt)
  {
  }

  public void CleanUp()
  {
    this.shadowCleanupList.AddRange((IEnumerable<KComponentManager<T>.CleanupInfo>) this.cleanupList);
    this.cleanupList.Clear();
    foreach (KComponentManager<T>.CleanupInfo shadowCleanup in this.shadowCleanupList)
    {
      this.OnCleanUp(shadowCleanup.handle);
      this.InternalRemoveComponent(shadowCleanup);
    }
    this.shadowCleanupList.Clear();
  }

  protected void RemoveFromCleanupList(object instance)
  {
    for (int index = 0; index < this.cleanupList.Count; ++index)
    {
      if (this.cleanupList[index].instance == instance)
      {
        this.cleanupList[index] = this.cleanupList[this.cleanupList.Count - 1];
        this.cleanupList.RemoveAt(this.cleanupList.Count - 1);
        break;
      }
    }
  }

  public override void Clear()
  {
    base.Clear();
    this.spawnList.Clear();
    this.shadowSpawnList.Clear();
    this.cleanupList.Clear();
    this.shadowCleanupList.Clear();
    this.instanceHandleMap.Clear();
  }

  protected virtual void OnPrefabInit(HandleVector<int>.Handle h)
  {
  }

  protected virtual void OnSpawn(HandleVector<int>.Handle h)
  {
  }

  protected virtual void OnCleanUp(HandleVector<int>.Handle h)
  {
  }

  protected struct CleanupInfo
  {
    public object instance;
    public HandleVector<int>.Handle handle;

    public CleanupInfo(object instance, HandleVector<int>.Handle handle)
    {
      this.instance = instance;
      this.handle = handle;
    }
  }
}
