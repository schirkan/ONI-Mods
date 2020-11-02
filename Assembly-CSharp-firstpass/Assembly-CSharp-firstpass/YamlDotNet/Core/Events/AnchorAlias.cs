// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.AnchorAlias
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;

namespace YamlDotNet.Core.Events
{
  public class AnchorAlias : ParsingEvent
  {
    private readonly string value;

    internal override EventType Type => EventType.Alias;

    public string Value => this.value;

    public AnchorAlias(string value, Mark start, Mark end)
      : base(start, end)
    {
      if (string.IsNullOrEmpty(value))
        throw new YamlException(start, end, "Anchor value must not be empty.");
      this.value = NodeEvent.anchorValidator.IsMatch(value) ? value : throw new YamlException(start, end, "Anchor value must contain alphanumerical characters only.");
    }

    public AnchorAlias(string value)
      : this(value, Mark.Empty, Mark.Empty)
    {
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Alias [value = {0}]", (object) this.value);

    public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
  }
}
