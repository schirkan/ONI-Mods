// Decompiled with JetBrains decompiler
// Type: Steamworks.FileDetailsResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1023)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct FileDetailsResult_t
  {
    public const int k_iCallback = 1023;
    public EResult m_eResult;
    public ulong m_ulFileSize;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] m_FileSHA;
    public uint m_unFlags;
  }
}
