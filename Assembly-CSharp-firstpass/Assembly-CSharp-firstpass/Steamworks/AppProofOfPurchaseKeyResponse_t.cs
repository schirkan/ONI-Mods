// Decompiled with JetBrains decompiler
// Type: Steamworks.AppProofOfPurchaseKeyResponse_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1021)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct AppProofOfPurchaseKeyResponse_t
  {
    public const int k_iCallback = 1021;
    public EResult m_eResult;
    public uint m_nAppID;
    public uint m_cchKeyLength;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 240)]
    public string m_rgchKey;
  }
}
