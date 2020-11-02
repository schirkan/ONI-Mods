// Decompiled with JetBrains decompiler
// Type: FMOD.Factory
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct Factory
  {
    public static RESULT System_Create(out FMOD.System system) => Factory.FMOD5_System_Create(out system.handle);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_System_Create(out IntPtr system);
  }
}
