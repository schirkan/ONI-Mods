// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PROCESS_CALLBACK
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace FMOD
{
  public delegate RESULT DSP_PROCESS_CALLBACK(
    ref DSP_STATE dsp_state,
    uint length,
    ref DSP_BUFFER_ARRAY inbufferarray,
    ref DSP_BUFFER_ARRAY outbufferarray,
    bool inputsidle,
    DSP_PROCESS_OPERATION op);
}
