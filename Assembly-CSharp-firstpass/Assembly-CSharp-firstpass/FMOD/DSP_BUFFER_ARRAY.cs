// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_BUFFER_ARRAY
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public struct DSP_BUFFER_ARRAY
  {
    public int numbuffers;
    public int[] buffernumchannels;
    public CHANNELMASK[] bufferchannelmask;
    public IntPtr[] buffers;
    public SPEAKERMODE speakermode;
  }
}
