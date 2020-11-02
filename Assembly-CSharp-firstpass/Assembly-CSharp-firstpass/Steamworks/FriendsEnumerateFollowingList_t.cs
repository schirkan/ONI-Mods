// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsEnumerateFollowingList_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(346)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct FriendsEnumerateFollowingList_t
  {
    public const int k_iCallback = 346;
    public EResult m_eResult;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public CSteamID[] m_rgSteamID;
    public int m_nResultsReturned;
    public int m_nTotalResultCount;
  }
}
