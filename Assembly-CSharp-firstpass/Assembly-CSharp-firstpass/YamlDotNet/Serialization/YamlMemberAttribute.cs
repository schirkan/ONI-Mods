// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.YamlMemberAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public sealed class YamlMemberAttribute : Attribute
  {
    public Type SerializeAs { get; set; }

    public int Order { get; set; }

    public string Alias { get; set; }

    public bool ApplyNamingConventions { get; set; }

    public ScalarStyle ScalarStyle { get; set; }

    public YamlMemberAttribute()
    {
      this.ScalarStyle = ScalarStyle.Any;
      this.ApplyNamingConventions = true;
    }

    public YamlMemberAttribute(Type serializeAs)
      : this()
      => this.SerializeAs = serializeAs;
  }
}
