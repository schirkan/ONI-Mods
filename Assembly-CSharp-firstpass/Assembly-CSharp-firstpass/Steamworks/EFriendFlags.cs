// Decompiled with JetBrains decompiler
// Type: Steamworks.EFriendFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EFriendFlags
  {
    k_EFriendFlagNone = 0,
    k_EFriendFlagBlocked = 1,
    k_EFriendFlagFriendshipRequested = 2,
    k_EFriendFlagImmediate = 4,
    k_EFriendFlagClanMember = 8,
    k_EFriendFlagOnGameServer = 16, // 0x00000010
    k_EFriendFlagRequestingFriendship = 128, // 0x00000080
    k_EFriendFlagRequestingInfo = 256, // 0x00000100
    k_EFriendFlagIgnored = 512, // 0x00000200
    k_EFriendFlagIgnoredFriend = 1024, // 0x00000400
    k_EFriendFlagChatMember = 4096, // 0x00001000
    k_EFriendFlagAll = 65535, // 0x0000FFFF
  }
}
