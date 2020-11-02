// Decompiled with JetBrains decompiler
// Type: Steamworks.EPersonaChange
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EPersonaChange
  {
    k_EPersonaChangeName = 1,
    k_EPersonaChangeStatus = 2,
    k_EPersonaChangeComeOnline = 4,
    k_EPersonaChangeGoneOffline = 8,
    k_EPersonaChangeGamePlayed = 16, // 0x00000010
    k_EPersonaChangeGameServer = 32, // 0x00000020
    k_EPersonaChangeAvatar = 64, // 0x00000040
    k_EPersonaChangeJoinedSource = 128, // 0x00000080
    k_EPersonaChangeLeftSource = 256, // 0x00000100
    k_EPersonaChangeRelationshipChanged = 512, // 0x00000200
    k_EPersonaChangeNameFirstSet = 1024, // 0x00000400
    k_EPersonaChangeBroadcast = 2048, // 0x00000800
    k_EPersonaChangeNickname = 4096, // 0x00001000
    k_EPersonaChangeSteamLevel = 8192, // 0x00002000
    k_EPersonaChangeRichPresence = 16384, // 0x00004000
  }
}
