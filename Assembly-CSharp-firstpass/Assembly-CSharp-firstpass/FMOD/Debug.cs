// Decompiled with JetBrains decompiler
// Type: FMOD.Debug
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace FMOD
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct Debug
  {
    public static RESULT Initialize(
      DEBUG_FLAGS flags,
      DEBUG_MODE mode,
      DEBUG_CALLBACK callback,
      string filename)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return Debug.FMOD5_Debug_Initialize(flags, mode, callback, freeHelper.byteFromStringUTF8(filename));
    }

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Debug_Initialize(
      DEBUG_FLAGS flags,
      DEBUG_MODE mode,
      DEBUG_CALLBACK callback,
      byte[] filename);
  }
}
