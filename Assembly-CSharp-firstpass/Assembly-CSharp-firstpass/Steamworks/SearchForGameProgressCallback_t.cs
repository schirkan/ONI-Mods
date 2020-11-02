// Decompiled with JetBrains decompiler
// Type: Steamworks.SearchForGameProgressCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(5201)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SearchForGameProgressCallback_t
  {
    public const int k_iCallback = 5201;
    public ulong m_ullSearchID;
    public EResult m_eResult;
    public CSteamID m_lobbyID;
    public CSteamID m_steamIDEndedSearch;
    public int m_nSecondsRemainingEstimate;
    public int m_cPlayersSearching;
  }
}
