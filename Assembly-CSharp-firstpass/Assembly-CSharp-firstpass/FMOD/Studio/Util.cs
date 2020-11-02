// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.Util
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct Util
  {
    public static RESULT ParseID(string idString, out Guid id)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return Util.FMOD_Studio_ParseID(freeHelper.byteFromStringUTF8(idString), out id);
    }

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_ParseID(byte[] idString, out Guid id);
  }
}
