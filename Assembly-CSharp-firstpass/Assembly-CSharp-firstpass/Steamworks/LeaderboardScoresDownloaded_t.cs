﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardScoresDownloaded_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1105)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardScoresDownloaded_t
  {
    public const int k_iCallback = 1105;
    public SteamLeaderboard_t m_hSteamLeaderboard;
    public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;
    public int m_cEntryCount;
  }
}
