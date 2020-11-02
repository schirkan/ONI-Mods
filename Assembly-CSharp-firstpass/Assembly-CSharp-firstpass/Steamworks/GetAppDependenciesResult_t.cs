// Decompiled with JetBrains decompiler
// Type: Steamworks.GetAppDependenciesResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(3416)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GetAppDependenciesResult_t
  {
    public const int k_iCallback = 3416;
    public EResult m_eResult;
    public PublishedFileId_t m_nPublishedFileId;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public AppId_t[] m_rgAppIDs;
    public uint m_nNumAppDependencies;
    public uint m_nTotalNumAppDependencies;
  }
}
