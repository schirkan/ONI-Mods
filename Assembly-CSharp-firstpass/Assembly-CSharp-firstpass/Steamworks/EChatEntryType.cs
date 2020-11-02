// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatEntryType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum EChatEntryType
  {
    k_EChatEntryTypeInvalid = 0,
    k_EChatEntryTypeChatMsg = 1,
    k_EChatEntryTypeTyping = 2,
    k_EChatEntryTypeInviteGame = 3,
    k_EChatEntryTypeEmote = 4,
    k_EChatEntryTypeLeftConversation = 6,
    k_EChatEntryTypeEntered = 7,
    k_EChatEntryTypeWasKicked = 8,
    k_EChatEntryTypeWasBanned = 9,
    k_EChatEntryTypeDisconnected = 10, // 0x0000000A
    k_EChatEntryTypeHistoricalChat = 11, // 0x0000000B
    k_EChatEntryTypeLinkBlocked = 14, // 0x0000000E
  }
}
