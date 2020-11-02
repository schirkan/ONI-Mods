// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.MergingParser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
  public sealed class MergingParser : IParser
  {
    private readonly List<ParsingEvent> _allEvents = new List<ParsingEvent>();
    private readonly IParser _innerParser;
    private int _currentIndex = -1;

    public MergingParser(IParser innerParser) => this._innerParser = innerParser;

    public ParsingEvent Current { get; private set; }

    public bool MoveNext()
    {
      if (this._currentIndex < 0)
      {
        while (this._innerParser.MoveNext())
          this._allEvents.Add(this._innerParser.Current);
        for (int index1 = this._allEvents.Count - 2; index1 >= 0; --index1)
        {
          if (this._allEvents[index1] is Scalar allEvent && allEvent.Value == "<<")
          {
            if (this._allEvents[index1 + 1] is AnchorAlias allEvent)
            {
              IEnumerable<ParsingEvent> mappingEvents = this.GetMappingEvents(allEvent.Value);
              this._allEvents.RemoveRange(index1, 2);
              this._allEvents.InsertRange(index1, mappingEvents);
            }
            else
            {
              if (this._allEvents[index1 + 1] is SequenceStart)
              {
                List<IEnumerable<ParsingEvent>> source = new List<IEnumerable<ParsingEvent>>();
                bool flag = false;
                for (int index2 = index1 + 2; index2 < this._allEvents.Count; ++index2)
                {
                  if (this._allEvents[index2] is AnchorAlias allEvent)
                    source.Add(this.GetMappingEvents(allEvent.Value));
                  else if (this._allEvents[index2] is SequenceEnd)
                  {
                    this._allEvents.RemoveRange(index1, index2 - index1 + 1);
                    this._allEvents.InsertRange(index1, source.SelectMany<IEnumerable<ParsingEvent>, ParsingEvent>((Func<IEnumerable<ParsingEvent>, IEnumerable<ParsingEvent>>) (e => e)));
                    flag = true;
                    break;
                  }
                }
                if (flag)
                  continue;
              }
              throw new SemanticErrorException(allEvent.Start, allEvent.End, "Unrecognized merge key pattern");
            }
          }
        }
      }
      int index = this._currentIndex + 1;
      if (index >= this._allEvents.Count)
        return false;
      this.Current = this._allEvents[index];
      this._currentIndex = index;
      return true;
    }

    private IEnumerable<ParsingEvent> GetMappingEvents(string mappingAlias)
    {
      MergingParser.ParsingEventCloner cloner = new MergingParser.ParsingEventCloner();
      int nesting = 0;
      return (IEnumerable<ParsingEvent>) this._allEvents.SkipWhile<ParsingEvent>((Func<ParsingEvent, bool>) (e => !(e is MappingStart mappingStart) || mappingStart.Anchor != mappingAlias)).Skip<ParsingEvent>(1).TakeWhile<ParsingEvent>((Func<ParsingEvent, bool>) (e => (nesting += e.NestingIncrease) >= 0)).Select<ParsingEvent, ParsingEvent>((Func<ParsingEvent, ParsingEvent>) (e => cloner.Clone(e))).ToList<ParsingEvent>();
    }

    private class ParsingEventCloner : IParsingEventVisitor
    {
      private ParsingEvent clonedEvent;

      public ParsingEvent Clone(ParsingEvent e)
      {
        e.Accept((IParsingEventVisitor) this);
        return this.clonedEvent;
      }

      void IParsingEventVisitor.Visit(AnchorAlias e) => this.clonedEvent = (ParsingEvent) new AnchorAlias(e.Value, e.Start, e.End);

      void IParsingEventVisitor.Visit(StreamStart e) => throw new NotSupportedException();

      void IParsingEventVisitor.Visit(StreamEnd e) => throw new NotSupportedException();

      void IParsingEventVisitor.Visit(DocumentStart e) => throw new NotSupportedException();

      void IParsingEventVisitor.Visit(DocumentEnd e) => throw new NotSupportedException();

      void IParsingEventVisitor.Visit(Scalar e) => this.clonedEvent = (ParsingEvent) new Scalar((string) null, e.Tag, e.Value, e.Style, e.IsPlainImplicit, e.IsQuotedImplicit, e.Start, e.End);

      void IParsingEventVisitor.Visit(SequenceStart e) => this.clonedEvent = (ParsingEvent) new SequenceStart((string) null, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);

      void IParsingEventVisitor.Visit(SequenceEnd e) => this.clonedEvent = (ParsingEvent) new SequenceEnd(e.Start, e.End);

      void IParsingEventVisitor.Visit(MappingStart e) => this.clonedEvent = (ParsingEvent) new MappingStart((string) null, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);

      void IParsingEventVisitor.Visit(MappingEnd e) => this.clonedEvent = (ParsingEvent) new MappingEnd(e.Start, e.End);

      void IParsingEventVisitor.Visit(Comment e) => throw new NotSupportedException();
    }
  }
}
