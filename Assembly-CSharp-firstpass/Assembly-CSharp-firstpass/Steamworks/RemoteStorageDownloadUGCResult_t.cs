// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageDownloadUGCResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1317)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageDownloadUGCResult_t
  {
    public const int k_iCallback = 1317;
    public EResult m_eResult;
    public UGCHandle_t m_hFile;
    public AppId_t m_nAppID;
    public int m_nSizeInBytes;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string m_pchFileName;
    public ulong m_ulSteamIDOwner;
  }
}
