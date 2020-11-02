﻿// Decompiled with JetBrains decompiler
// Type: CellAddRemoveSubstanceEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

public class CellAddRemoveSubstanceEvent : CellEvent
{
  public CellAddRemoveSubstanceEvent(string id, string reason, bool enable_logging = false)
    : base(id, reason, true, enable_logging)
  {
  }

  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void Log(int cell, SimHashes element, float amount, int callback_id)
  {
    if (!this.enableLogging)
      return;
    CellEventInstance ev = new CellEventInstance(cell, (int) element, (int) ((double) amount * 1000.0), (CellEvent) this);
    CellEventLogger.Instance.Add(ev);
  }

  public override string GetDescription(EventInstanceBase ev)
  {
    CellEventInstance cellEventInstance = ev as CellEventInstance;
    SimHashes data = (SimHashes) cellEventInstance.data;
    return this.GetMessagePrefix() + "Element=" + data.ToString() + ", Mass=" + (object) (float) ((double) cellEventInstance.data2 / 1000.0) + " (" + this.reason + ")";
  }
}