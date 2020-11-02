// Decompiled with JetBrains decompiler
// Type: Steamworks.ValidateAuthTicketResponse_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(143)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ValidateAuthTicketResponse_t
  {
    public const int k_iCallback = 143;
    public CSteamID m_SteamID;
    public EAuthSessionResponse m_eAuthSessionResponse;
    public CSteamID m_OwnerSteamID;
  }
}
