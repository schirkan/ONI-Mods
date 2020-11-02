// Decompiled with JetBrains decompiler
// Type: KGameObjectSplitComponentManager`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public abstract class KGameObjectSplitComponentManager<Header, Payload> : KSplitComponentManager<Header, Payload>
  where Header : new()
  where Payload : new()
{
  public HandleVector<int>.Handle Add(
    GameObject go,
    Header header,
    ref Payload payload)
  {
    return this.InternalAddComponent((object) go, header, ref payload);
  }

  public virtual void Remove(GameObject go)
  {
    HandleVector<int>.Handle handle = this.GetHandle(go);
    KSplitComponentManager<Header, Payload>.CleanupInfo info = new KSplitComponentManager<Header, Payload>.CleanupInfo((object) go, handle);
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
