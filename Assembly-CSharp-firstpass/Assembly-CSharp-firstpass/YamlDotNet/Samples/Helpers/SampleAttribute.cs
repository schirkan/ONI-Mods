// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.Helpers.SampleAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Samples.Helpers
{
  internal class SampleAttribute : Attribute
  {
    private string title;

    public string DisplayName { get; private set; }

    public string Title
    {
      get => this.title;
      set
      {
        this.title = value;
        this.DisplayName = "Sample: " + value;
      }
    }

    public string Description { get; set; }
  }
}
