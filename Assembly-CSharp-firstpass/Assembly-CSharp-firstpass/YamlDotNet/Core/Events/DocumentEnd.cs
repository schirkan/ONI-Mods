// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.DocumentEnd
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;

namespace YamlDotNet.Core.Events
{
  public class DocumentEnd : ParsingEvent
  {
    private readonly bool isImplicit;

    public override int NestingIncrease => -1;

    internal override EventType Type => EventType.DocumentEnd;

    public bool IsImplicit => this.isImplicit;

    public DocumentEnd(bool isImplicit, Mark start, Mark end)
      : base(start, end)
      => this.isImplicit = isImplicit;

    public DocumentEnd(bool isImplicit)
      : this(isImplicit, Mark.Empty, Mark.Empty)
    {
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Document end [isImplicit = {0}]", (object) this.isImplicit);

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
