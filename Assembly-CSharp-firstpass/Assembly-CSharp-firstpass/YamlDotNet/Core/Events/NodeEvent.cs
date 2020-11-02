// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.NodeEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core.Events
{
  public abstract class NodeEvent : ParsingEvent
  {
    internal static readonly Regex anchorValidator = new Regex("^[0-9a-zA-Z_\\-]+$", RegexOptions.None);
    private readonly string anchor;
    private readonly string tag;

    public string Anchor => this.anchor;

    public string Tag => this.tag;

    public abstract bool IsCanonical { get; }

    protected NodeEvent(string anchor, string tag, Mark start, Mark end)
      : base(start, end)
    {
      switch (anchor)
      {
        case "":
          throw new ArgumentException("Anchor value must not be empty.", nameof (anchor));
        case null:
          switch (tag)
          {
            case "":
              throw new ArgumentException("Tag value must not be empty.", nameof (tag));
            default:
              this.anchor = anchor;
              this.tag = tag;
              return;
          }
        default:
          if (!NodeEvent.anchorValidator.IsMatch(anchor))
            throw new ArgumentException("Anchor value must contain alphanumerical characters only.", nameof (anchor));
          goto case null;
      }
    }

    protected NodeEvent(string anchor, string tag)
      : this(anchor, tag, Mark.Empty, Mark.Empty)
    {
    }
  }
}
