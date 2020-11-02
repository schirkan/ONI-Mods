// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifierInstance`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class ModifierInstance<ModifierType> : IStateMachineTarget
  {
    public ModifierType modifier;

    public GameObject gameObject { get; private set; }

    public ModifierInstance(GameObject game_object, ModifierType modifier)
    {
      this.gameObject = game_object;
      this.modifier = modifier;
    }

    public ComponentType GetComponent<ComponentType>() => this.gameObject.GetComponent<ComponentType>();

    public int Subscribe(int hash, System.Action<object> handler) => this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);

    public void Unsubscribe(int hash, System.Action<object> handler) => this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);

    public void Unsubscribe(int id) => this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);

    public void Trigger(int hash, object data = null) => this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);

    public Transform transform => this.gameObject.transform;

    public bool isNull => (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null;

    public string name => this.gameObject.name;

    public virtual void OnCleanUp()
    {
    }
  }
}
