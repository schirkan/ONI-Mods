// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_METERING_INFO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace FMOD
{
  public struct DSP_METERING_INFO
  {
    public int numsamples;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public float[] peaklevel;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public float[] rmslevel;
    public short numchannels;
  }
}
