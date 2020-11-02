// Decompiled with JetBrains decompiler
// Type: EventExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class EventExtensions
{
  public static int Subscribe<ComponentType>(
    this GameObject go,
    int hash,
    EventSystem.IntraObjectHandler<ComponentType> handler)
  {
    return KObjectManager.Instance.GetOrCreateObject(go).GetEventSystem().Subscribe<ComponentType>(hash, handler);
  }

  public static void Trigger(this GameObject go, int hash, object data = null)
  {
    KObject kobject = KObjectManager.Instance.Get(go);
    if (kobject == null || !kobject.hasEventSystem)
      return;
    kobject.GetEventSystem().Trigger(go, hash, data);
  }
}
