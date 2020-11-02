// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_GETPARAM_BOOL_CALLBACK
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public delegate RESULT DSP_GETPARAM_BOOL_CALLBACK(
    ref DSP_STATE dsp_state,
    int index,
    ref bool value,
    IntPtr valuestr);
}
