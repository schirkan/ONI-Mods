// Decompiled with JetBrains decompiler
// Type: Steamworks.RequestPlayersForGameResultCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(5212)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RequestPlayersForGameResultCallback_t
  {
    public const int k_iCallback = 5212;
    public EResult m_eResult;
    public ulong m_ullSearchID;
    public CSteamID m_SteamIDPlayerFound;
    public CSteamID m_SteamIDLobby;
    public PlayerAcceptState_t m_ePlayerAcceptState;
    public int m_nPlayerIndex;
    public int m_nTotalPlayersFound;
    public int m_nTotalPlayersAcceptedGame;
    public int m_nSuggestedTeamIndex;
    public ulong m_ullUniqueGameID;
  }
}
