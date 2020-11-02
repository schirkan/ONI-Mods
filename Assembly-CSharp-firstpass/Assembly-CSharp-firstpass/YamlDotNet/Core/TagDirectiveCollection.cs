// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.TagDirectiveCollection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
  public class TagDirectiveCollection : KeyedCollection<string, TagDirective>
  {
    public TagDirectiveCollection()
    {
    }

    public TagDirectiveCollection(IEnumerable<TagDirective> tagDirectives)
    {
      foreach (TagDirective tagDirective in tagDirectives)
        this.Add(tagDirective);
    }

    protected override string GetKeyForItem(TagDirective item) => item.Handle;

    public new bool Contains(TagDirective directive) => this.Contains(this.GetKeyForItem(directive));
  }
}
