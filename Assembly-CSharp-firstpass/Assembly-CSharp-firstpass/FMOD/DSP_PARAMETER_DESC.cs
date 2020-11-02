// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PARAMETER_DESC
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace FMOD
{
  public struct DSP_PARAMETER_DESC
  {
    public DSP_PARAMETER_TYPE type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public char[] name;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public char[] label;
    public string description;
    public DSP_PARAMETER_DESC_UNION desc;
  }
}
