﻿// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PAN_GETROLLOFFGAIN_FUNC
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace FMOD
{
  public delegate RESULT DSP_PAN_GETROLLOFFGAIN_FUNC(
    ref DSP_STATE dsp_state,
    DSP_PAN_3D_ROLLOFF_TYPE rolloff,
    float distance,
    float mindistance,
    float maxdistance,
    out float gain);
}