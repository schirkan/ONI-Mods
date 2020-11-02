// Decompiled with JetBrains decompiler
// Type: Steamworks.GameConnectedFriendChatMsg_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(343)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct GameConnectedFriendChatMsg_t
  {
    public const int k_iCallback = 343;
    public CSteamID m_steamIDUser;
    public int m_iMessageID;
  }
}
