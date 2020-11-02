// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Link
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ProcGen.Noise
{
  public class Link
  {
    public Link.Type type { get; set; }

    public string name { get; set; }

    public enum Type
    {
      None,
      Primitive,
      Filter,
      Transformer,
      Selector,
      Modifier,
      Combiner,
      FloatPoints,
      ControlPoints,
      Terminator,
    }
  }
}
