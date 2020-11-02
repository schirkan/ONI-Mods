// Decompiled with JetBrains decompiler
// Type: StateMachineControllerExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class StateMachineControllerExtensions
{
  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(
    this StateMachine.Instance smi)
    where StateMachineInstanceType : StateMachine.Instance
  {
    return smi.gameObject.GetSMI<StateMachineInstanceType>();
  }

  public static DefType GetDef<DefType>(this Component cmp) where DefType : StateMachine.BaseDef => cmp.gameObject.GetDef<DefType>();

  public static DefType GetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    return (Object) component == (Object) null ? default (DefType) : component.GetDef<DefType>();
  }

  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class => cmp.gameObject.GetSMI<StateMachineInstanceType>();

  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    return (Object) component != (Object) null ? component.GetSMI<StateMachineInstanceType>() : default (StateMachineInstanceType);
  }

  public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(
    this Component cmp)
    where StateMachineInstanceType : class
  {
    return cmp.gameObject.GetAllSMI<StateMachineInstanceType>();
  }

  public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(
    this GameObject go)
    where StateMachineInstanceType : class
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    return (Object) component != (Object) null ? component.GetAllSMI<StateMachineInstanceType>() : new List<StateMachineInstanceType>();
  }
}
