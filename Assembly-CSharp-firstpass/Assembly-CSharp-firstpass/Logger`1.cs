// Decompiled with JetBrains decompiler
// Type: Logger`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;

public class Logger<EntryType> : Logger
{
  private List<EntryType> entries;
  public System.Action<EntryType> OnLog;

  public IEnumerator<EntryType> GetEnumerator()
  {
    if (this.entries == null)
      this.entries = new List<EntryType>();
    return (IEnumerator<EntryType>) this.entries.GetEnumerator();
  }

  public override int Count => this.entries == null ? 0 : this.entries.Count;

  public void SetMaxEntries(int new_max)
  {
  }

  public Logger(string name, int new_max = 35)
    : base(name)
    => this.SetMaxEntries(new_max);

  [Conditional("UNITY_EDITOR")]
  public void Log(EntryType entry)
  {
  }
}
