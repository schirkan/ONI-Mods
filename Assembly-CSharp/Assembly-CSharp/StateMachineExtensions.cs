// Decompiled with JetBrains decompiler
// Type: StateMachineExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public static class StateMachineExtensions
{
  public static bool IsNullOrStopped(this StateMachine.Instance smi) => smi == null || !smi.IsRunning();
}
