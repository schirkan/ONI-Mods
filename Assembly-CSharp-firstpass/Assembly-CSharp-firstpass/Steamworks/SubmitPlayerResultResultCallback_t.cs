// Decompiled with JetBrains decompiler
// Type: Steamworks.SubmitPlayerResultResultCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(5214)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SubmitPlayerResultResultCallback_t
  {
    public const int k_iCallback = 5214;
    public EResult m_eResult;
    public ulong ullUniqueGameID;
    public CSteamID steamIDPlayer;
  }
}
