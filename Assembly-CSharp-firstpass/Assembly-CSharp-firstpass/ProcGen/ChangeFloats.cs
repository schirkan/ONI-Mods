// Decompiled with JetBrains decompiler
// Type: ProcGen.ChangeFloats
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;

namespace ProcGen
{
  public struct ChangeFloats
  {
    [StringEnumConverter]
    public ChangeFloats.ChangeType change { get; private set; }

    public MinMax value { get; private set; }

    public enum ChangeType
    {
      NoChange,
      OverrideRange,
      OverrideSet,
      TakeNoiseVal,
    }
  }
}
