// Decompiled with JetBrains decompiler
// Type: LoggerFSFSF
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct LoggerFSFSF
{
  public LoggerFSFSF(string name, int max_entries = 35)
  {
  }

  public string GetName() => "";

  public void SetName(string name)
  {
  }

  [Conditional("ENABLE_LOGGER")]
  public void Log(string name1, float val1, string name2, float val2)
  {
  }
}
