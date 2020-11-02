// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PARAMETER_DESC_INT
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public struct DSP_PARAMETER_DESC_INT
  {
    public int min;
    public int max;
    public int defaultval;
    public bool goestoinf;
    public IntPtr valuenames;
  }
}
