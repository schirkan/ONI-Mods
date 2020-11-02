// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryRequestPricesResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4705)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SteamInventoryRequestPricesResult_t
  {
    public const int k_iCallback = 4705;
    public EResult m_result;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
    public string m_rgchCurrency;
  }
}
