﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendRichPresenceUpdate_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(336)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct FriendRichPresenceUpdate_t
  {
    public const int k_iCallback = 336;
    public CSteamID m_steamIDFriend;
    public AppId_t m_nAppID;
  }
}