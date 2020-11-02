// Decompiled with JetBrains decompiler
// Type: Steamworks.UserAchievementIconFetched_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1109)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct UserAchievementIconFetched_t
  {
    public const int k_iCallback = 1109;
    public CGameID m_nGameID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string m_rgchAchievementName;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAchieved;
    public int m_nIconHandle;
  }
}
