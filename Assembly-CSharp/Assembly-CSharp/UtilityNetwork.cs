﻿// Decompiled with JetBrains decompiler
// Type: UtilityNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class UtilityNetwork
{
  public int id;
  public ConduitType conduitType;

  public virtual void AddItem(int cell, object item)
  {
  }

  public virtual void RemoveItem(int cell, object item)
  {
  }

  public virtual void ConnectItem(int cell, object item)
  {
  }

  public virtual void DisconnectItem(int cell, object item)
  {
  }

  public virtual void Reset(UtilityNetworkGridNode[] grid)
  {
  }
}
