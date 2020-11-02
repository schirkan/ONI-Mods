// Decompiled with JetBrains decompiler
// Type: Steamworks.EAuthSessionResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum EAuthSessionResponse
  {
    k_EAuthSessionResponseOK,
    k_EAuthSessionResponseUserNotConnectedToSteam,
    k_EAuthSessionResponseNoLicenseOrExpired,
    k_EAuthSessionResponseVACBanned,
    k_EAuthSessionResponseLoggedInElseWhere,
    k_EAuthSessionResponseVACCheckTimedOut,
    k_EAuthSessionResponseAuthTicketCanceled,
    k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
    k_EAuthSessionResponseAuthTicketInvalid,
    k_EAuthSessionResponsePublisherIssuedBan,
  }
}
