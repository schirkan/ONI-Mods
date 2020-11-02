// Decompiled with JetBrains decompiler
// Type: KMonoBehaviourExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class KMonoBehaviourExtensions
{
  public static int Subscribe(this GameObject go, int hash, System.Action<object> handler) => go.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);

  public static void Subscribe(
    this GameObject go,
    GameObject target,
    int hash,
    System.Action<object> handler)
  {
    go.GetComponent<KMonoBehaviour>().Subscribe(target, hash, handler);
  }

  public static void Unsubscribe(this GameObject go, int hash, System.Action<object> handler)
  {
    KMonoBehaviour component = go.GetComponent<KMonoBehaviour>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Unsubscribe(hash, handler);
  }

  public static void Unsubscribe(this GameObject go, int id)
  {
    KMonoBehaviour component = go.GetComponent<KMonoBehaviour>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Unsubscribe(id);
  }

  public static void Unsubscribe(
    this GameObject go,
    GameObject target,
    int hash,
    System.Action<object> handler)
  {
    KMonoBehaviour component = go.GetComponent<KMonoBehaviour>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Unsubscribe(target, hash, handler);
  }

  public static T GetComponentInChildrenOnly<T>(this GameObject go) where T : Component
  {
    foreach (T componentsInChild in go.GetComponentsInChildren<T>())
    {
      if ((UnityEngine.Object) componentsInChild.gameObject != (UnityEngine.Object) go)
        return componentsInChild;
    }
    return default (T);
  }

  public static T[] GetComponentsInChildrenOnly<T>(this GameObject go) where T : Component
  {
    List<T> objList = new List<T>();
    objList.AddRange((IEnumerable<T>) go.GetComponentsInChildren<T>());
    objList.RemoveAll((Predicate<T>) (t => (UnityEngine.Object) t.gameObject == (UnityEngine.Object) go));
    return objList.ToArray();
  }

  public static void SetAlpha(this Image img, float alpha)
  {
    Color color = img.color;
    color.a = alpha;
    img.color = color;
  }

  public static void SetAlpha(this Text txt, float alpha)
  {
    Color color = txt.color;
    color.a = alpha;
    txt.color = color;
  }
}
