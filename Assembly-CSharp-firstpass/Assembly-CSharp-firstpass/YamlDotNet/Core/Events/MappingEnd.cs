// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.MappingEnd
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Core.Events
{
  public class MappingEnd : ParsingEvent
  {
    public override int NestingIncrease => -1;

    internal override EventType Type => EventType.MappingEnd;

    public MappingEnd(Mark start, Mark end)
      : base(start, end)
    {
    }

    public MappingEnd()
      : this(Mark.Empty, Mark.Empty)
    {
    }

    public override string ToString() => "Mapping end";

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
