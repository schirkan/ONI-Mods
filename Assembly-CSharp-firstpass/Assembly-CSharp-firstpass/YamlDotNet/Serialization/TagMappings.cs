// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.TagMappings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization
{
  public sealed class TagMappings
  {
    private readonly IDictionary<string, Type> mappings;

    public TagMappings() => this.mappings = (IDictionary<string, Type>) new Dictionary<string, Type>();

    public TagMappings(IDictionary<string, Type> mappings) => this.mappings = (IDictionary<string, Type>) new Dictionary<string, Type>(mappings);

    public void Add(string tag, Type mapping) => this.mappings.Add(tag, mapping);

    internal Type GetMapping(string tag)
    {
      Type type;
      return this.mappings.TryGetValue(tag, out type) ? type : (Type) null;
    }
  }
}
