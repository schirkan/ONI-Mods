// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_STATE
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public struct DSP_STATE
  {
    public IntPtr instance;
    public IntPtr plugindata;
    public uint channelmask;
    public int source_speakermode;
    public IntPtr sidechaindata;
    public int sidechainchannels;
    public IntPtr functions;
    public int systemobject;
  }
}
