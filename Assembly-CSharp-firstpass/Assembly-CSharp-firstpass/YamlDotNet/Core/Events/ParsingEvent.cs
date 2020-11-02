// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.ParsingEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Core.Events
{
  public abstract class ParsingEvent
  {
    private readonly Mark start;
    private readonly Mark end;

    public virtual int NestingIncrease => 0;

    internal abstract EventType Type { get; }

    public Mark Start => this.start;

    public Mark End => this.end;

    public abstract void Accept(IParsingEventVisitor visitor);

    internal ParsingEvent(Mark start, Mark end)
    {
      this.start = start;
      this.end = end;
    }
  }
}
