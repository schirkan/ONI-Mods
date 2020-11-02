// Decompiled with JetBrains decompiler
// Type: Steamworks.GetOPFSettingsResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4624)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GetOPFSettingsResult_t
  {
    public const int k_iCallback = 4624;
    public EResult m_eResult;
    public AppId_t m_unVideoAppID;
  }
}
