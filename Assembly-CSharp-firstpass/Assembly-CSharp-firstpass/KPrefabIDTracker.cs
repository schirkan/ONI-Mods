// Decompiled with JetBrains decompiler
// Type: KPrefabIDTracker
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class KPrefabIDTracker
{
  private static KPrefabIDTracker Instance;
  private Dictionary<int, KPrefabID> prefabIdMap = new Dictionary<int, KPrefabID>();

  public static void DestroyInstance() => KPrefabIDTracker.Instance = (KPrefabIDTracker) null;

  public static KPrefabIDTracker Get()
  {
    if (KPrefabIDTracker.Instance == null)
      KPrefabIDTracker.Instance = new KPrefabIDTracker();
    return KPrefabIDTracker.Instance;
  }

  public void Register(KPrefabID instance)
  {
    if (instance.InstanceID == -1)
      return;
    if (this.prefabIdMap.ContainsKey(instance.InstanceID))
      Debug.LogWarningFormat((Object) instance.gameObject, "KPID instance id {0} was previously used by {1} but we're trying to add it from {2}. Conflict!", (object) instance.InstanceID, (object) this.prefabIdMap[instance.InstanceID].gameObject, (object) instance.name);
    this.prefabIdMap[instance.InstanceID] = instance;
  }

  public void Unregister(KPrefabID instance) => this.prefabIdMap.Remove(instance.InstanceID);

  public KPrefabID GetInstance(int instance_id)
  {
    KPrefabID kprefabId = (KPrefabID) null;
    this.prefabIdMap.TryGetValue(instance_id, out kprefabId);
    return kprefabId;
  }
}
