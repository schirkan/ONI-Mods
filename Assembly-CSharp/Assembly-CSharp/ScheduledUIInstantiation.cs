// Decompiled with JetBrains decompiler
// Type: ScheduledUIInstantiation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ScheduledUIInstantiation")]
public class ScheduledUIInstantiation : KMonoBehaviour
{
  public ScheduledUIInstantiation.Instantiation[] UIElements;
  public bool InstantiateOnAwake;
  public GameHashes InstantiationEvent = GameHashes.StartGameUser;
  private bool completed;
  private List<GameObject> instantiatedObjects = new List<GameObject>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.InstantiateOnAwake)
      this.InstantiateElements((object) null);
    else
      Game.Instance.Subscribe((int) this.InstantiationEvent, new System.Action<object>(this.InstantiateElements));
  }

  public void InstantiateElements(object data)
  {
    if (this.completed)
      return;
    this.completed = true;
    foreach (ScheduledUIInstantiation.Instantiation uiElement in this.UIElements)
    {
      foreach (GameObject prefab in uiElement.prefabs)
      {
        Vector3 anchoredPosition = (Vector3) prefab.rectTransform().anchoredPosition;
        GameObject go = Util.KInstantiateUI(prefab, uiElement.parent.gameObject);
        go.rectTransform().anchoredPosition = (Vector2) anchoredPosition;
        go.rectTransform().localScale = Vector3.one;
        this.instantiatedObjects.Add(go);
      }
    }
    if (this.InstantiateOnAwake)
      return;
    this.Unsubscribe((int) this.InstantiationEvent, new System.Action<object>(this.InstantiateElements));
  }

  public T GetInstantiatedObject<T>() where T : Component
  {
    for (int index = 0; index < this.instantiatedObjects.Count; ++index)
    {
      if ((UnityEngine.Object) this.instantiatedObjects[index].GetComponent(typeof (T)) != (UnityEngine.Object) null)
        return this.instantiatedObjects[index].GetComponent(typeof (T)) as T;
    }
    return default (T);
  }

  [Serializable]
  public struct Instantiation
  {
    public string Name;
    public string Comment;
    public GameObject[] prefabs;
    public Transform parent;
  }
}
