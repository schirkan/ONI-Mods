// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Constants
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
  internal static class Constants
  {
    public static readonly TagDirective[] DefaultTagDirectives = new TagDirective[2]
    {
      new TagDirective("!", "!"),
      new TagDirective("!!", "tag:yaml.org,2002:")
    };
    public const int MajorVersion = 1;
    public const int MinorVersion = 1;
    public const char HandleCharacter = '!';
    public const string DefaultHandle = "!";
  }
}
