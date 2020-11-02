// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardEntry_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardEntry_t
  {
    public CSteamID m_steamIDUser;
    public int m_nGlobalRank;
    public int m_nScore;
    public int m_cDetails;
    public UGCHandle_t m_hUGC;
  }
}
