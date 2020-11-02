// Decompiled with JetBrains decompiler
// Type: GameStateMachine`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class GameStateMachine<StateMachineType, StateMachineInstanceType> : GameStateMachine<StateMachineType, StateMachineInstanceType, IStateMachineTarget, object>
  where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, IStateMachineTarget, object>
  where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, IStateMachineTarget, object>.GameInstance
{
}
