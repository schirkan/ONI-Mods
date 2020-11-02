// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.VersionDirective
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public class VersionDirective : Token
  {
    private readonly YamlDotNet.Core.Version version;

    public YamlDotNet.Core.Version Version => this.version;

    public VersionDirective(YamlDotNet.Core.Version version)
      : this(version, Mark.Empty, Mark.Empty)
    {
    }

    public VersionDirective(YamlDotNet.Core.Version version, Mark start, Mark end)
      : base(start, end)
      => this.version = version;

    public override bool Equals(object obj) => obj is VersionDirective versionDirective && this.version.Equals((object) versionDirective.version);

    public override int GetHashCode() => this.version.GetHashCode();
  }
}
