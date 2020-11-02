// Decompiled with JetBrains decompiler
// Type: Steamworks.SocketStatusCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1201)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SocketStatusCallback_t
  {
    public const int k_iCallback = 1201;
    public SNetSocket_t m_hSocket;
    public SNetListenSocket_t m_hListenSocket;
    public CSteamID m_steamIDRemote;
    public int m_eSNetSocketState;
  }
}
