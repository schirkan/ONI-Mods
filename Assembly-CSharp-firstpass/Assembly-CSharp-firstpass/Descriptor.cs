// Decompiled with JetBrains decompiler
// Type: Descriptor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{text}")]
public struct Descriptor
{
  public string text;
  public string tooltipText;
  public int indent;
  public Descriptor.DescriptorType type;
  public bool onlyForSimpleInfoScreen;

  public Descriptor(
    string txt,
    string tooltip,
    Descriptor.DescriptorType descriptorType = Descriptor.DescriptorType.Effect,
    bool only_for_simple_info_screen = false)
  {
    this.indent = 0;
    this.text = txt;
    this.tooltipText = tooltip;
    this.type = descriptorType;
    this.onlyForSimpleInfoScreen = only_for_simple_info_screen;
  }

  public void SetupDescriptor(string txt, string tooltip, Descriptor.DescriptorType descriptorType = Descriptor.DescriptorType.Effect)
  {
    this.text = txt;
    this.tooltipText = tooltip;
    this.type = descriptorType;
  }

  public Descriptor IncreaseIndent()
  {
    ++this.indent;
    return this;
  }

  public Descriptor DecreaseIndent()
  {
    this.indent = Mathf.Max(this.indent - 1, 0);
    return this;
  }

  public string IndentedText()
  {
    string str = this.text;
    for (int index = 0; index < this.indent; ++index)
      str = "    " + str;
    return str;
  }

  public enum DescriptorType
  {
    Requirement,
    Effect,
    Lifecycle,
    Information,
    DiseaseSource,
    Detail,
    Symptom,
    SymptomAidable,
  }
}
