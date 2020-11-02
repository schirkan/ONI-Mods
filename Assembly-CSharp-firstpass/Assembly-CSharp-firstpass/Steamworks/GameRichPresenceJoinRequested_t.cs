// Decompiled with JetBrains decompiler
// Type: Steamworks.GameRichPresenceJoinRequested_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(337)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GameRichPresenceJoinRequested_t
  {
    public const int k_iCallback = 337;
    public CSteamID m_steamIDFriend;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_rgchConnect;
  }
}
