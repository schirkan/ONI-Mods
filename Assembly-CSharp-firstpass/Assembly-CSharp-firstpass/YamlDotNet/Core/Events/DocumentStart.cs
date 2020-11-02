// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.DocumentStart
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core.Events
{
  public class DocumentStart : ParsingEvent
  {
    private readonly TagDirectiveCollection tags;
    private readonly VersionDirective version;
    private readonly bool isImplicit;

    public override int NestingIncrease => 1;

    internal override EventType Type => EventType.DocumentStart;

    public TagDirectiveCollection Tags => this.tags;

    public VersionDirective Version => this.version;

    public bool IsImplicit => this.isImplicit;

    public DocumentStart(
      VersionDirective version,
      TagDirectiveCollection tags,
      bool isImplicit,
      Mark start,
      Mark end)
      : base(start, end)
    {
      this.version = version;
      this.tags = tags;
      this.isImplicit = isImplicit;
    }

    public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit)
      : this(version, tags, isImplicit, Mark.Empty, Mark.Empty)
    {
    }

    public DocumentStart(Mark start, Mark end)
      : this((VersionDirective) null, (TagDirectiveCollection) null, true, start, end)
    {
    }

    public DocumentStart()
      : this((VersionDirective) null, (TagDirectiveCollection) null, true, Mark.Empty, Mark.Empty)
    {
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Document start [isImplicit = {0}]", (object) this.isImplicit);

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
