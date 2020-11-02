// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NamingConventions.HyphenatedNamingConvention
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
  public sealed class HyphenatedNamingConvention : INamingConvention
  {
    public string Apply(string value) => value.FromCamelCase("-");
  }
}
