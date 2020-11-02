// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.MappingStart
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;

namespace YamlDotNet.Core.Events
{
  public class MappingStart : NodeEvent
  {
    private readonly bool isImplicit;
    private readonly MappingStyle style;

    public override int NestingIncrease => 1;

    internal override EventType Type => EventType.MappingStart;

    public bool IsImplicit => this.isImplicit;

    public override bool IsCanonical => !this.isImplicit;

    public MappingStyle Style => this.style;

    public MappingStart(
      string anchor,
      string tag,
      bool isImplicit,
      MappingStyle style,
      Mark start,
      Mark end)
      : base(anchor, tag, start, end)
    {
      this.isImplicit = isImplicit;
      this.style = style;
    }

    public MappingStart(string anchor, string tag, bool isImplicit, MappingStyle style)
      : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
    {
    }

    public MappingStart()
      : this((string) null, (string) null, true, MappingStyle.Any, Mark.Empty, Mark.Empty)
    {
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Mapping start [anchor = {0}, tag = {1}, isImplicit = {2}, style = {3}]", (object) this.Anchor, (object) this.Tag, (object) this.isImplicit, (object) this.style);

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
