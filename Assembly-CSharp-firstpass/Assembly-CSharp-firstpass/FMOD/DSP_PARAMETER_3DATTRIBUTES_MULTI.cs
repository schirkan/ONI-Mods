// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PARAMETER_3DATTRIBUTES_MULTI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace FMOD
{
  public struct DSP_PARAMETER_3DATTRIBUTES_MULTI
  {
    public int numlisteners;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public ATTRIBUTES_3D[] relative;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public float[] weight;
    public ATTRIBUTES_3D absolute;
  }
}
