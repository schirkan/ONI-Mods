// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PAN_SUMSURROUNDMATRIX_FUNC
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public delegate RESULT DSP_PAN_SUMSURROUNDMATRIX_FUNC(
    ref DSP_STATE dsp_state,
    int sourceSpeakerMode,
    int targetSpeakerMode,
    float direction,
    float extent,
    float rotation,
    float lowFrequencyGain,
    float overallGain,
    int matrixHop,
    IntPtr matrix,
    DSP_PAN_SURROUND_FLAGS flags);
}
