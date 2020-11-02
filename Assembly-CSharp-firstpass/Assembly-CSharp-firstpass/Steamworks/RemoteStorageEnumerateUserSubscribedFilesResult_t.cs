// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageEnumerateUserSubscribedFilesResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1314)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageEnumerateUserSubscribedFilesResult_t
  {
    public const int k_iCallback = 1314;
    public EResult m_eResult;
    public int m_nResultsReturned;
    public int m_nTotalResultCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public PublishedFileId_t[] m_rgPublishedFileId;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public uint[] m_rgRTimeSubscribed;
  }
}
