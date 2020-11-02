// Decompiled with JetBrains decompiler
// Type: IEnergyConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IEnergyConsumer
{
  float WattsUsed { get; }

  float WattsNeededWhenActive { get; }

  int PowerSortOrder { get; }

  void SetConnectionStatus(CircuitManager.ConnectionStatus status);

  string Name { get; }

  int PowerCell { get; }

  bool IsConnected { get; }

  bool IsPowered { get; }
}
