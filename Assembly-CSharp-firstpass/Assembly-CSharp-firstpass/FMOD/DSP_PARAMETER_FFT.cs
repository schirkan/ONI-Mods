// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PARAMETER_FFT
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD
{
  public struct DSP_PARAMETER_FFT
  {
    public int length;
    public int numchannels;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    private IntPtr[] spectrum_internal;

    public float[][] spectrum
    {
      get
      {
        float[][] numArray = new float[this.numchannels][];
        for (int index = 0; index < this.numchannels; ++index)
        {
          numArray[index] = new float[this.length];
          Marshal.Copy(this.spectrum_internal[index], numArray[index], 0, this.length);
        }
        return numArray;
      }
    }
  }
}
