// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardScoreUploaded_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1106)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardScoreUploaded_t
  {
    public const int k_iCallback = 1106;
    public byte m_bSuccess;
    public SteamLeaderboard_t m_hSteamLeaderboard;
    public int m_nScore;
    public byte m_bScoreChanged;
    public int m_nGlobalRankNew;
    public int m_nGlobalRankPrevious;
  }
}
