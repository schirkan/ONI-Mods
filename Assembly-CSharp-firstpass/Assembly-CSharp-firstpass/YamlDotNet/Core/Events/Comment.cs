// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.Comment
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Core.Events
{
  public class Comment : ParsingEvent
  {
    public string Value { get; private set; }

    public bool IsInline { get; private set; }

    public Comment(string value, bool isInline)
      : this(value, isInline, Mark.Empty, Mark.Empty)
    {
    }

    public Comment(string value, bool isInline, Mark start, Mark end)
      : base(start, end)
    {
      this.Value = value;
      this.IsInline = isInline;
    }

    internal override EventType Type => EventType.Comment;

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
