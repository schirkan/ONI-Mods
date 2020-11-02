// Decompiled with JetBrains decompiler
// Type: Steamworks.Packsize
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class Packsize
  {
    public const int value = 8;

    public static bool Test()
    {
      int num1 = Marshal.SizeOf(typeof (Packsize.ValvePackingSentinel_t));
      int num2 = Marshal.SizeOf(typeof (RemoteStorageEnumerateUserSubscribedFilesResult_t));
      return num1 == 32 && num2 == 616;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    private struct ValvePackingSentinel_t
    {
      private uint m_u32;
      private ulong m_u64;
      private ushort m_u16;
      private double m_d;
    }
  }
}
