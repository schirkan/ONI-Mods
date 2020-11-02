// Decompiled with JetBrains decompiler
// Type: Steamworks.SearchForGameResultCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(5202)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SearchForGameResultCallback_t
  {
    public const int k_iCallback = 5202;
    public ulong m_ullSearchID;
    public EResult m_eResult;
    public int m_nCountPlayersInGame;
    public int m_nCountAcceptedGame;
    public CSteamID m_steamIDHost;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bFinalCallback;
  }
}
