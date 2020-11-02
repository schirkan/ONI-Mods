// Decompiled with JetBrains decompiler
// Type: Steamworks.JoinPartyCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(5301)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct JoinPartyCallback_t
  {
    public const int k_iCallback = 5301;
    public EResult m_eResult;
    public PartyBeaconID_t m_ulBeaconID;
    public CSteamID m_SteamIDBeaconOwner;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_rgchConnectString;
  }
}
