// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.SequenceEnd
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Core.Events
{
  public class SequenceEnd : ParsingEvent
  {
    public override int NestingIncrease => -1;

    internal override EventType Type => EventType.SequenceEnd;

    public SequenceEnd(Mark start, Mark end)
      : base(start, end)
    {
    }

    public SequenceEnd()
      : this(Mark.Empty, Mark.Empty)
    {
    }

    public override string ToString() => "Sequence end";

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
