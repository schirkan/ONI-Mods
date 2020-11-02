// Decompiled with JetBrains decompiler
// Type: CmpFns
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using UnityEngine;

public class CmpFns
{
  public Func<KMonoBehaviour, Component> mFindOrAddFn;
  public Func<KMonoBehaviour, Component> mFindFn;
  public Func<KMonoBehaviour, Component> mRequireFn;

  public static Component FindComponent<T>(MonoBehaviour c) where T : Component => (Component) c.FindComponent<T>();

  public static Component RequireComponent<T>(MonoBehaviour c) where T : Component => (Component) c.RequireComponent<T>();

  public static Component FindOrAddComponent<T>(MonoBehaviour c) where T : Component => (Component) c.FindOrAddComponent<T>();

  public CmpFns(System.Type type)
  {
    System.Type[] type_array = new System.Type[1]{ type };
    this.mFindOrAddFn = this.GetMethod("FindOrAddComponent", type_array);
    this.mFindFn = this.GetMethod("FindComponent", type_array);
    this.mRequireFn = this.GetMethod("RequireComponent", type_array);
  }

  private Func<KMonoBehaviour, Component> GetMethod(
    string name,
    System.Type[] type_array)
  {
    MethodInfo method1 = typeof (CmpFns).GetMethod(name);
    MethodInfo method2 = (MethodInfo) null;
    try
    {
      method2 = method1.MakeGenericMethod(type_array);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
      foreach (object type in type_array)
        Debug.Log(type);
    }
    return (Func<KMonoBehaviour, Component>) Delegate.CreateDelegate(typeof (Func<KMonoBehaviour, Component>), method2);
  }
}
