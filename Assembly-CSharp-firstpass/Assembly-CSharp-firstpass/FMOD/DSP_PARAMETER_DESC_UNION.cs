// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PARAMETER_DESC_UNION
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace FMOD
{
  [StructLayout(LayoutKind.Explicit)]
  public struct DSP_PARAMETER_DESC_UNION
  {
    [FieldOffset(0)]
    public DSP_PARAMETER_DESC_FLOAT floatdesc;
    [FieldOffset(0)]
    public DSP_PARAMETER_DESC_INT intdesc;
    [FieldOffset(0)]
    public DSP_PARAMETER_DESC_BOOL booldesc;
    [FieldOffset(0)]
    public DSP_PARAMETER_DESC_DATA datadesc;
  }
}
