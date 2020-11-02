// Decompiled with JetBrains decompiler
// Type: Steamworks.GameConnectedClanChatMsg_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(338)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct GameConnectedClanChatMsg_t
  {
    public const int k_iCallback = 338;
    public CSteamID m_steamIDClanChat;
    public CSteamID m_steamIDUser;
    public int m_iMessageID;
  }
}
