// Decompiled with JetBrains decompiler
// Type: ProcGen.FeatureConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;

namespace ProcGen
{
  public class FeatureConverter
  {
    [StringEnumConverter]
    public FeatureConverter.Shape shape { get; private set; }

    public MinMax size { get; private set; }

    public MinMax density { get; private set; }

    public ChangeFloats mass { get; private set; }

    public ChangeFloats temperature { get; private set; }

    public enum Shape
    {
      Circle,
      Oval,
      Blob,
      Square,
      Rectangle,
      Line,
    }
  }
}
