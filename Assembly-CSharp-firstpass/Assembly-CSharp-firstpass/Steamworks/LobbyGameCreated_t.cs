// Decompiled with JetBrains decompiler
// Type: Steamworks.LobbyGameCreated_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(509)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LobbyGameCreated_t
  {
    public const int k_iCallback = 509;
    public ulong m_ulSteamIDLobby;
    public ulong m_ulSteamIDGameServer;
    public uint m_unIP;
    public ushort m_usPort;
  }
}
