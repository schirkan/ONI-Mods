// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.StreamStart
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Core.Events
{
  public class StreamStart : ParsingEvent
  {
    public override int NestingIncrease => 1;

    internal override EventType Type => EventType.StreamStart;

    public StreamStart()
      : this(Mark.Empty, Mark.Empty)
    {
    }

    public StreamStart(Mark start, Mark end)
      : base(start, end)
    {
    }

    public override string ToString() => "Stream start";

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
