// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsIsFollowing_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(345)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct FriendsIsFollowing_t
  {
    public const int k_iCallback = 345;
    public EResult m_eResult;
    public CSteamID m_steamID;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bIsFollowing;
  }
}
