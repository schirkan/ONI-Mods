// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.Union_IntBoolFloatString
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace FMOD.Studio
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct Union_IntBoolFloatString
  {
    [FieldOffset(0)]
    public int intvalue;
    [FieldOffset(0)]
    public bool boolvalue;
    [FieldOffset(0)]
    public float floatvalue;
    [FieldOffset(0)]
    public StringWrapper stringvalue;
  }
}
