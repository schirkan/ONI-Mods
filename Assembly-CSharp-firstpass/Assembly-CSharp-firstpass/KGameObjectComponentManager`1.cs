// Decompiled with JetBrains decompiler
// Type: KGameObjectComponentManager`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public abstract class KGameObjectComponentManager<T> : KComponentManager<T> where T : new()
{
  public HandleVector<int>.Handle Add(GameObject go, T cmp) => this.InternalAddComponent((object) go, cmp);

  public virtual void Remove(GameObject go)
  {
    HandleVector<int>.Handle handle = this.GetHandle(go);
    KComponentManager<T>.CleanupInfo info = new KComponentManager<T>.CleanupInfo((object) go, handle);
    if (!KComponentCleanUp.InCleanUpPhase)
    {
      this.cleanupList.Add(info);
    }
    else
    {
      this.RemoveFromCleanupList((object) go);
      this.OnCleanUp(handle);
      this.InternalRemoveComponent(info);
    }
  }

  public HandleVector<int>.Handle GetHandle(GameObject obj) => this.GetHandle((object) obj);

  public HandleVector<int>.Handle GetHandle(MonoBehaviour obj) => this.GetHandle((object) obj.gameObject);
}
