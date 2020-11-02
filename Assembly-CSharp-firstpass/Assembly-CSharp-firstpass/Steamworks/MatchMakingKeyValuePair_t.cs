// Decompiled with JetBrains decompiler
// Type: Steamworks.MatchMakingKeyValuePair_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  public struct MatchMakingKeyValuePair_t
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szKey;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szValue;

    private MatchMakingKeyValuePair_t(string strKey, string strValue)
    {
      this.m_szKey = strKey;
      this.m_szValue = strValue;
    }
  }
}
