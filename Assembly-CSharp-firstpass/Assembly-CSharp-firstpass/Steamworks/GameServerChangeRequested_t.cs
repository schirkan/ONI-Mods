// Decompiled with JetBrains decompiler
// Type: Steamworks.GameServerChangeRequested_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(332)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GameServerChangeRequested_t
  {
    public const int k_iCallback = 332;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string m_rgchServer;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string m_rgchPassword;
  }
}
