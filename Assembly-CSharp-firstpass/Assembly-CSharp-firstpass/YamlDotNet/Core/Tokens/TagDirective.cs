// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.TagDirective
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public class TagDirective : Token
  {
    private readonly string handle;
    private readonly string prefix;
    private static readonly Regex tagHandleValidator = new Regex("^!([0-9A-Za-z_\\-]*!)?$", RegexOptions.None);

    public string Handle => this.handle;

    public string Prefix => this.prefix;

    public TagDirective(string handle, string prefix)
      : this(handle, prefix, Mark.Empty, Mark.Empty)
    {
    }

    public TagDirective(string handle, string prefix, Mark start, Mark end)
      : base(start, end)
    {
      if (string.IsNullOrEmpty(handle))
        throw new ArgumentNullException(nameof (handle), "Tag handle must not be empty.");
      this.handle = TagDirective.tagHandleValidator.IsMatch(handle) ? handle : throw new ArgumentException("Tag handle must start and end with '!' and contain alphanumerical characters only.", nameof (handle));
      this.prefix = !string.IsNullOrEmpty(prefix) ? prefix : throw new ArgumentNullException(nameof (prefix), "Tag prefix must not be empty.");
    }

    public override bool Equals(object obj) => obj is TagDirective tagDirective && this.handle.Equals(tagDirective.handle) && this.prefix.Equals(tagDirective.prefix);

    public override int GetHashCode() => this.handle.GetHashCode() ^ this.prefix.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} => {1}", (object) this.handle, (object) this.prefix);
  }
}
