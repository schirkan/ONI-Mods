// Decompiled with JetBrains decompiler
// Type: StateMachineComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class StateMachineComponent : KMonoBehaviour, ISaveLoadable, IStateMachineTarget
{
  [MyCmpAdd]
  protected StateMachineController stateMachineController;

  public abstract StateMachine.Instance GetSMI();
}
