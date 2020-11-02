// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.StreamFragment
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
  public sealed class StreamFragment : IYamlConvertible
  {
    private readonly List<ParsingEvent> events = new List<ParsingEvent>();

    public IList<ParsingEvent> Events => (IList<ParsingEvent>) this.events;

    void IYamlConvertible.Read(
      IParser parser,
      Type expectedType,
      ObjectDeserializer nestedObjectDeserializer)
    {
      this.events.Clear();
      int num = 0;
      while (parser.MoveNext())
      {
        this.events.Add(parser.Current);
        num += parser.Current.NestingIncrease;
        if (num <= 0)
        {
          Debug.Assert(num == 0);
          return;
        }
      }
      throw new InvalidOperationException("The parser has reached the end before deserialization completed.");
    }

    void IYamlConvertible.Write(
      IEmitter emitter,
      ObjectSerializer nestedObjectSerializer)
    {
      foreach (ParsingEvent @event in this.events)
        emitter.Emit(@event);
    }
  }
}
