// Decompiled with JetBrains decompiler
// Type: IStateMachineTarget
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public interface IStateMachineTarget
{
  int Subscribe(int hash, System.Action<object> handler);

  void Unsubscribe(int hash, System.Action<object> handler);

  void Unsubscribe(int id);

  void Trigger(int hash, object data = null);

  ComponentType GetComponent<ComponentType>();

  GameObject gameObject { get; }

  Transform transform { get; }

  string name { get; }

  bool isNull { get; }
}
