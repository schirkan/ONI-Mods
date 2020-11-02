// Decompiled with JetBrains decompiler
// Type: Steamworks.GameConnectedChatLeave_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(340)]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct GameConnectedChatLeave_t
  {
    public const int k_iCallback = 340;
    public CSteamID m_steamIDClanChat;
    public CSteamID m_steamIDUser;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bKicked;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bDropped;
  }
}
