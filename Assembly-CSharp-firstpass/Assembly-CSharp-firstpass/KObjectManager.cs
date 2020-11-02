// Decompiled with JetBrains decompiler
// Type: KObjectManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class KObjectManager : MonoBehaviour
{
  private Dictionary<int, KObject> objects = new Dictionary<int, KObject>();
  private List<int> pendingDestroys = new List<int>();

  public static KObjectManager Instance { get; private set; }

  public static void DestroyInstance() => KObjectManager.Instance = (KObjectManager) null;

  private void Awake()
  {
    Debug.Assert((Object) KObjectManager.Instance == (Object) null);
    KObjectManager.Instance = this;
  }

  private void OnDestroy()
  {
    Debug.Assert((Object) KObjectManager.Instance != (Object) null);
    Debug.Assert((Object) KObjectManager.Instance == (Object) this);
    this.Cleanup();
    KObjectManager.Instance = (KObjectManager) null;
  }

  public void Cleanup()
  {
    foreach (KeyValuePair<int, KObject> keyValuePair in this.objects)
      keyValuePair.Value.OnCleanUp();
    this.objects.Clear();
    this.pendingDestroys.Clear();
  }

  public KObject GetOrCreateObject(GameObject go)
  {
    int instanceId = go.GetInstanceID();
    KObject kobject = (KObject) null;
    if (!this.objects.TryGetValue(instanceId, out kobject))
    {
      kobject = new KObject(go);
      this.objects[instanceId] = kobject;
    }
    return kobject;
  }

  public KObject Get(GameObject go)
  {
    KObject kobject = (KObject) null;
    this.objects.TryGetValue(go.GetInstanceID(), out kobject);
    return kobject;
  }

  public void QueueDestroy(KObject obj)
  {
    int id = obj.id;
    if (this.pendingDestroys.Contains(id))
      return;
    this.pendingDestroys.Add(id);
  }

  private void LateUpdate()
  {
    for (int index = 0; index < this.pendingDestroys.Count; ++index)
    {
      int pendingDestroy = this.pendingDestroys[index];
      KObject kobject = (KObject) null;
      if (this.objects.TryGetValue(pendingDestroy, out kobject))
      {
        this.objects.Remove(pendingDestroy);
        kobject.OnCleanUp();
      }
    }
    this.pendingDestroys.Clear();
  }

  public void DumpEventData()
  {
  }
}
