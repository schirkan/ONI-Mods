// Decompiled with JetBrains decompiler
// Type: Steamworks.EUserRestriction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum EUserRestriction
  {
    k_nUserRestrictionNone = 0,
    k_nUserRestrictionUnknown = 1,
    k_nUserRestrictionAnyChat = 2,
    k_nUserRestrictionVoiceChat = 4,
    k_nUserRestrictionGroupChat = 8,
    k_nUserRestrictionRating = 16, // 0x00000010
    k_nUserRestrictionGameInvites = 32, // 0x00000020
    k_nUserRestrictionTrading = 64, // 0x00000040
  }
}
