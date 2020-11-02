// Decompiled with JetBrains decompiler
// Type: ElementGradient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using ProcGen;
using System;
using System.Diagnostics;

[DebuggerDisplay("{content} {bandSize} {maxValue}")]
[Serializable]
public class ElementGradient : Gradient<string>
{
  public ElementGradient()
    : base((string) null, 0.0f)
  {
  }

  public ElementGradient(string content, float bandSize, SampleDescriber.Override overrides)
    : base(content, bandSize)
    => this.overrides = overrides;

  public SampleDescriber.Override overrides { get; set; }

  public void Mod(WorldTrait.ElementBandModifier mod)
  {
    Debug.Assert(mod.element == this.content);
    this.bandSize *= mod.bandMultiplier;
    if (this.overrides == null)
      this.overrides = new SampleDescriber.Override();
    this.overrides.ModMultiplyMass(mod.massMultiplier);
  }
}
