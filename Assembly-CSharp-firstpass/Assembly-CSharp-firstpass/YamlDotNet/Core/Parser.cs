﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Parser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
  public class Parser : IParser
  {
    private readonly Stack<ParserState> states = new Stack<ParserState>();
    private readonly TagDirectiveCollection tagDirectives = new TagDirectiveCollection();
    private ParserState state;
    private readonly IScanner scanner;
    private ParsingEvent currentEvent;
    private Token currentToken;
    private readonly Parser.EventQueue pendingEvents = new Parser.EventQueue();

    private Token GetCurrentToken()
    {
      if (this.currentToken == null)
      {
        while (this.scanner.MoveNextWithoutConsuming())
        {
          this.currentToken = this.scanner.Current;
          if (this.currentToken is YamlDotNet.Core.Tokens.Comment currentToken)
            this.pendingEvents.Enqueue((ParsingEvent) new YamlDotNet.Core.Events.Comment(currentToken.Value, currentToken.IsInline, currentToken.Start, currentToken.End));
          else
            break;
        }
      }
      return this.currentToken;
    }

    public Parser(TextReader input)
      : this((IScanner) new Scanner(input))
    {
    }

    public Parser(IScanner scanner) => this.scanner = scanner;

    public ParsingEvent Current => this.currentEvent;

    public bool MoveNext()
    {
      if (this.state == ParserState.StreamEnd)
      {
        this.currentEvent = (ParsingEvent) null;
        return false;
      }
      if (this.pendingEvents.Count == 0)
        this.pendingEvents.Enqueue(this.StateMachine());
      this.currentEvent = this.pendingEvents.Dequeue();
      return true;
    }

    private ParsingEvent StateMachine()
    {
      switch (this.state)
      {
        case ParserState.StreamStart:
          return this.ParseStreamStart();
        case ParserState.ImplicitDocumentStart:
          return this.ParseDocumentStart(true);
        case ParserState.DocumentStart:
          return this.ParseDocumentStart(false);
        case ParserState.DocumentContent:
          return this.ParseDocumentContent();
        case ParserState.DocumentEnd:
          return this.ParseDocumentEnd();
        case ParserState.BlockNode:
          return this.ParseNode(true, false);
        case ParserState.BlockNodeOrIndentlessSequence:
          return this.ParseNode(true, true);
        case ParserState.FlowNode:
          return this.ParseNode(false, false);
        case ParserState.BlockSequenceFirstEntry:
          return this.ParseBlockSequenceEntry(true);
        case ParserState.BlockSequenceEntry:
          return this.ParseBlockSequenceEntry(false);
        case ParserState.IndentlessSequenceEntry:
          return this.ParseIndentlessSequenceEntry();
        case ParserState.BlockMappingFirstKey:
          return this.ParseBlockMappingKey(true);
        case ParserState.BlockMappingKey:
          return this.ParseBlockMappingKey(false);
        case ParserState.BlockMappingValue:
          return this.ParseBlockMappingValue();
        case ParserState.FlowSequenceFirstEntry:
          return this.ParseFlowSequenceEntry(true);
        case ParserState.FlowSequenceEntry:
          return this.ParseFlowSequenceEntry(false);
        case ParserState.FlowSequenceEntryMappingKey:
          return this.ParseFlowSequenceEntryMappingKey();
        case ParserState.FlowSequenceEntryMappingValue:
          return this.ParseFlowSequenceEntryMappingValue();
        case ParserState.FlowSequenceEntryMappingEnd:
          return this.ParseFlowSequenceEntryMappingEnd();
        case ParserState.FlowMappingFirstKey:
          return this.ParseFlowMappingKey(true);
        case ParserState.FlowMappingKey:
          return this.ParseFlowMappingKey(false);
        case ParserState.FlowMappingValue:
          return this.ParseFlowMappingValue(false);
        case ParserState.FlowMappingEmptyValue:
          return this.ParseFlowMappingValue(true);
        default:
          Debug.Assert(false, (object) "Invalid state");
          throw new InvalidOperationException();
      }
    }

    private void Skip()
    {
      if (this.currentToken == null)
        return;
      this.currentToken = (Token) null;
      this.scanner.ConsumeCurrent();
    }

    private ParsingEvent ParseStreamStart()
    {
      if (!(this.GetCurrentToken() is YamlDotNet.Core.Tokens.StreamStart currentToken))
      {
        Token currentToken = this.GetCurrentToken();
        throw new SemanticErrorException(currentToken.Start, currentToken.End, "Did not find expected <stream-start>.");
      }
      this.Skip();
      this.state = ParserState.ImplicitDocumentStart;
      return (ParsingEvent) new YamlDotNet.Core.Events.StreamStart(currentToken.Start, currentToken.End);
    }

    private ParsingEvent ParseDocumentStart(bool isImplicit)
    {
      if (!isImplicit)
      {
        while (this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentEnd)
          this.Skip();
      }
      if (isImplicit && !(this.GetCurrentToken() is VersionDirective) && (!(this.GetCurrentToken() is TagDirective) && !(this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentStart)) && !(this.GetCurrentToken() is YamlDotNet.Core.Tokens.StreamEnd))
      {
        TagDirectiveCollection tags = new TagDirectiveCollection();
        this.ProcessDirectives(tags);
        this.states.Push(ParserState.DocumentEnd);
        this.state = ParserState.BlockNode;
        return (ParsingEvent) new YamlDotNet.Core.Events.DocumentStart((VersionDirective) null, tags, true, this.GetCurrentToken().Start, this.GetCurrentToken().End);
      }
      if (!(this.GetCurrentToken() is YamlDotNet.Core.Tokens.StreamEnd))
      {
        Mark start1 = this.GetCurrentToken().Start;
        TagDirectiveCollection tags1 = new TagDirectiveCollection();
        VersionDirective version = this.ProcessDirectives(tags1);
        Token currentToken = this.GetCurrentToken();
        if (!(currentToken is YamlDotNet.Core.Tokens.DocumentStart))
          throw new SemanticErrorException(currentToken.Start, currentToken.End, "Did not find expected <document start>.");
        this.states.Push(ParserState.DocumentEnd);
        this.state = ParserState.DocumentContent;
        TagDirectiveCollection tags2 = tags1;
        Mark start2 = start1;
        Mark end = currentToken.End;
        YamlDotNet.Core.Events.DocumentStart documentStart = new YamlDotNet.Core.Events.DocumentStart(version, tags2, false, start2, end);
        this.Skip();
        return (ParsingEvent) documentStart;
      }
      this.state = ParserState.StreamEnd;
      YamlDotNet.Core.Events.StreamEnd streamEnd = new YamlDotNet.Core.Events.StreamEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
      if (!this.scanner.MoveNextWithoutConsuming())
        return (ParsingEvent) streamEnd;
      throw new InvalidOperationException("The scanner should contain no more tokens.");
    }

    private VersionDirective ProcessDirectives(TagDirectiveCollection tags)
    {
      VersionDirective versionDirective = (VersionDirective) null;
      bool flag = false;
      while (true)
      {
        if (this.GetCurrentToken() is VersionDirective currentToken)
        {
          if (versionDirective == null)
          {
            if (currentToken.Version.Major == 1 && currentToken.Version.Minor == 1)
            {
              versionDirective = currentToken;
              flag = true;
            }
            else
              goto label_5;
          }
          else
            break;
        }
        else if (this.GetCurrentToken() is TagDirective currentToken)
        {
          if (!tags.Contains(currentToken.Handle))
          {
            tags.Add(currentToken);
            flag = true;
          }
          else
            goto label_9;
        }
        else
          goto label_12;
        this.Skip();
      }
      throw new SemanticErrorException(currentToken.Start, currentToken.End, "Found duplicate %YAML directive.");
label_5:
      throw new SemanticErrorException(currentToken.Start, currentToken.End, "Found incompatible YAML document.");
label_9:
      throw new SemanticErrorException(currentToken.Start, currentToken.End, "Found duplicate %TAG directive.");
label_12:
      Parser.AddTagDirectives(tags, (IEnumerable<TagDirective>) Constants.DefaultTagDirectives);
      if (flag)
        this.tagDirectives.Clear();
      Parser.AddTagDirectives(this.tagDirectives, (IEnumerable<TagDirective>) tags);
      return versionDirective;
    }

    private static void AddTagDirectives(
      TagDirectiveCollection directives,
      IEnumerable<TagDirective> source)
    {
      foreach (TagDirective directive in source)
      {
        if (!directives.Contains(directive))
          directives.Add(directive);
      }
    }

    private ParsingEvent ParseDocumentContent()
    {
      if (!(this.GetCurrentToken() is VersionDirective) && !(this.GetCurrentToken() is TagDirective) && (!(this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentStart) && !(this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentEnd)) && !(this.GetCurrentToken() is YamlDotNet.Core.Tokens.StreamEnd))
        return this.ParseNode(true, false);
      this.state = this.states.Pop();
      return Parser.ProcessEmptyScalar(this.scanner.CurrentPosition);
    }

    private static ParsingEvent ProcessEmptyScalar(Mark position) => (ParsingEvent) new YamlDotNet.Core.Events.Scalar((string) null, (string) null, string.Empty, ScalarStyle.Plain, true, false, position, position);

    private ParsingEvent ParseNode(bool isBlock, bool isIndentlessSequence)
    {
      if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.AnchorAlias currentToken)
      {
        this.state = this.states.Pop();
        YamlDotNet.Core.Events.AnchorAlias anchorAlias = new YamlDotNet.Core.Events.AnchorAlias(currentToken.Value, currentToken.Start, currentToken.End);
        this.Skip();
        return (ParsingEvent) anchorAlias;
      }
      Mark start = this.GetCurrentToken().Start;
      anchor = (Anchor) null;
      tag = (YamlDotNet.Core.Tokens.Tag) null;
      while (true)
      {
        while (anchor != null || !(this.GetCurrentToken() is Anchor anchor))
        {
          if (tag == null && this.GetCurrentToken() is YamlDotNet.Core.Tokens.Tag tag)
          {
            this.Skip();
          }
          else
          {
            string tag = (string) null;
            if (tag != null)
            {
              if (string.IsNullOrEmpty(tag.Handle))
              {
                tag = tag.Suffix;
              }
              else
              {
                if (!this.tagDirectives.Contains(tag.Handle))
                  throw new SemanticErrorException(tag.Start, tag.End, "While parsing a node, find undefined tag handle.");
                tag = this.tagDirectives[tag.Handle].Prefix + tag.Suffix;
              }
            }
            if (string.IsNullOrEmpty(tag))
              tag = (string) null;
            string anchor = anchor != null ? (string.IsNullOrEmpty(anchor.Value) ? (string) null : anchor.Value) : (string) null;
            bool flag = string.IsNullOrEmpty(tag);
            if (isIndentlessSequence && this.GetCurrentToken() is BlockEntry)
            {
              this.state = ParserState.IndentlessSequenceEntry;
              return (ParsingEvent) new SequenceStart(anchor, tag, flag, SequenceStyle.Block, start, this.GetCurrentToken().End);
            }
            if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.Scalar currentToken)
            {
              bool isPlainImplicit = false;
              bool isQuotedImplicit = false;
              if (currentToken.Style == ScalarStyle.Plain && tag == null || tag == "!")
                isPlainImplicit = true;
              else if (tag == null)
                isQuotedImplicit = true;
              this.state = this.states.Pop();
              YamlDotNet.Core.Events.Scalar scalar = new YamlDotNet.Core.Events.Scalar(anchor, tag, currentToken.Value, currentToken.Style, isPlainImplicit, isQuotedImplicit, start, currentToken.End);
              this.Skip();
              return (ParsingEvent) scalar;
            }
            if (this.GetCurrentToken() is FlowSequenceStart currentToken)
            {
              this.state = ParserState.FlowSequenceFirstEntry;
              return (ParsingEvent) new SequenceStart(anchor, tag, flag, SequenceStyle.Flow, start, currentToken.End);
            }
            if (this.GetCurrentToken() is FlowMappingStart currentToken)
            {
              this.state = ParserState.FlowMappingFirstKey;
              return (ParsingEvent) new MappingStart(anchor, tag, flag, MappingStyle.Flow, start, currentToken.End);
            }
            if (isBlock)
            {
              if (this.GetCurrentToken() is BlockSequenceStart currentToken)
              {
                this.state = ParserState.BlockSequenceFirstEntry;
                return (ParsingEvent) new SequenceStart(anchor, tag, flag, SequenceStyle.Block, start, currentToken.End);
              }
              if (this.GetCurrentToken() is BlockMappingStart)
              {
                this.state = ParserState.BlockMappingFirstKey;
                return (ParsingEvent) new MappingStart(anchor, tag, flag, MappingStyle.Block, start, this.GetCurrentToken().End);
              }
            }
            if (anchor != null || tag != null)
            {
              this.state = this.states.Pop();
              return (ParsingEvent) new YamlDotNet.Core.Events.Scalar(anchor, tag, string.Empty, ScalarStyle.Plain, flag, false, start, this.GetCurrentToken().End);
            }
            Token currentToken1 = this.GetCurrentToken();
            throw new SemanticErrorException(currentToken1.Start, currentToken1.End, "While parsing a node, did not find expected node content.");
          }
        }
        this.Skip();
      }
    }

    private ParsingEvent ParseDocumentEnd()
    {
      bool isImplicit = true;
      Mark start = this.GetCurrentToken().Start;
      Mark end = start;
      if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.DocumentEnd)
      {
        end = this.GetCurrentToken().End;
        this.Skip();
        isImplicit = false;
      }
      this.state = ParserState.DocumentStart;
      return (ParsingEvent) new YamlDotNet.Core.Events.DocumentEnd(isImplicit, start, end);
    }

    private ParsingEvent ParseBlockSequenceEntry(bool isFirst)
    {
      if (isFirst)
      {
        this.GetCurrentToken();
        this.Skip();
      }
      if (this.GetCurrentToken() is BlockEntry)
      {
        Mark end = this.GetCurrentToken().End;
        this.Skip();
        if (!(this.GetCurrentToken() is BlockEntry) && !(this.GetCurrentToken() is BlockEnd))
        {
          this.states.Push(ParserState.BlockSequenceEntry);
          return this.ParseNode(true, false);
        }
        this.state = ParserState.BlockSequenceEntry;
        return Parser.ProcessEmptyScalar(end);
      }
      if (this.GetCurrentToken() is BlockEnd)
      {
        this.state = this.states.Pop();
        SequenceEnd sequenceEnd = new SequenceEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
        this.Skip();
        return (ParsingEvent) sequenceEnd;
      }
      Token currentToken = this.GetCurrentToken();
      throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a block collection, did not find expected '-' indicator.");
    }

    private ParsingEvent ParseIndentlessSequenceEntry()
    {
      if (this.GetCurrentToken() is BlockEntry)
      {
        Mark end = this.GetCurrentToken().End;
        this.Skip();
        if (!(this.GetCurrentToken() is BlockEntry) && !(this.GetCurrentToken() is Key) && (!(this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value) && !(this.GetCurrentToken() is BlockEnd)))
        {
          this.states.Push(ParserState.IndentlessSequenceEntry);
          return this.ParseNode(true, false);
        }
        this.state = ParserState.IndentlessSequenceEntry;
        return Parser.ProcessEmptyScalar(end);
      }
      this.state = this.states.Pop();
      return (ParsingEvent) new SequenceEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
    }

    private ParsingEvent ParseBlockMappingKey(bool isFirst)
    {
      if (isFirst)
      {
        this.GetCurrentToken();
        this.Skip();
      }
      if (this.GetCurrentToken() is Key)
      {
        Mark end = this.GetCurrentToken().End;
        this.Skip();
        if (!(this.GetCurrentToken() is Key) && !(this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value) && !(this.GetCurrentToken() is BlockEnd))
        {
          this.states.Push(ParserState.BlockMappingValue);
          return this.ParseNode(true, true);
        }
        this.state = ParserState.BlockMappingValue;
        return Parser.ProcessEmptyScalar(end);
      }
      if (this.GetCurrentToken() is BlockEnd)
      {
        this.state = this.states.Pop();
        MappingEnd mappingEnd = new MappingEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
        this.Skip();
        return (ParsingEvent) mappingEnd;
      }
      Token currentToken = this.GetCurrentToken();
      throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a block mapping, did not find expected key.");
    }

    private ParsingEvent ParseBlockMappingValue()
    {
      if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value)
      {
        Mark end = this.GetCurrentToken().End;
        this.Skip();
        if (!(this.GetCurrentToken() is Key) && !(this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value) && !(this.GetCurrentToken() is BlockEnd))
        {
          this.states.Push(ParserState.BlockMappingKey);
          return this.ParseNode(true, true);
        }
        this.state = ParserState.BlockMappingKey;
        return Parser.ProcessEmptyScalar(end);
      }
      this.state = ParserState.BlockMappingKey;
      return Parser.ProcessEmptyScalar(this.GetCurrentToken().Start);
    }

    private ParsingEvent ParseFlowSequenceEntry(bool isFirst)
    {
      if (isFirst)
      {
        this.GetCurrentToken();
        this.Skip();
      }
      if (!(this.GetCurrentToken() is FlowSequenceEnd))
      {
        if (!isFirst)
        {
          if (this.GetCurrentToken() is FlowEntry)
          {
            this.Skip();
          }
          else
          {
            Token currentToken = this.GetCurrentToken();
            throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a flow sequence, did not find expected ',' or ']'.");
          }
        }
        if (this.GetCurrentToken() is Key)
        {
          this.state = ParserState.FlowSequenceEntryMappingKey;
          MappingStart mappingStart = new MappingStart((string) null, (string) null, true, MappingStyle.Flow);
          this.Skip();
          return (ParsingEvent) mappingStart;
        }
        if (!(this.GetCurrentToken() is FlowSequenceEnd))
        {
          this.states.Push(ParserState.FlowSequenceEntry);
          return this.ParseNode(false, false);
        }
      }
      this.state = this.states.Pop();
      SequenceEnd sequenceEnd = new SequenceEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
      this.Skip();
      return (ParsingEvent) sequenceEnd;
    }

    private ParsingEvent ParseFlowSequenceEntryMappingKey()
    {
      if (!(this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value) && !(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowSequenceEnd))
      {
        this.states.Push(ParserState.FlowSequenceEntryMappingValue);
        return this.ParseNode(false, false);
      }
      Mark end = this.GetCurrentToken().End;
      this.Skip();
      this.state = ParserState.FlowSequenceEntryMappingValue;
      return Parser.ProcessEmptyScalar(end);
    }

    private ParsingEvent ParseFlowSequenceEntryMappingValue()
    {
      if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value)
      {
        this.Skip();
        if (!(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowSequenceEnd))
        {
          this.states.Push(ParserState.FlowSequenceEntryMappingEnd);
          return this.ParseNode(false, false);
        }
      }
      this.state = ParserState.FlowSequenceEntryMappingEnd;
      return Parser.ProcessEmptyScalar(this.GetCurrentToken().Start);
    }

    private ParsingEvent ParseFlowSequenceEntryMappingEnd()
    {
      this.state = ParserState.FlowSequenceEntry;
      return (ParsingEvent) new MappingEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
    }

    private ParsingEvent ParseFlowMappingKey(bool isFirst)
    {
      if (isFirst)
      {
        this.GetCurrentToken();
        this.Skip();
      }
      if (!(this.GetCurrentToken() is FlowMappingEnd))
      {
        if (!isFirst)
        {
          if (this.GetCurrentToken() is FlowEntry)
          {
            this.Skip();
          }
          else
          {
            Token currentToken = this.GetCurrentToken();
            throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a flow mapping,  did not find expected ',' or '}'.");
          }
        }
        if (this.GetCurrentToken() is Key)
        {
          this.Skip();
          if (!(this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value) && !(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowMappingEnd))
          {
            this.states.Push(ParserState.FlowMappingValue);
            return this.ParseNode(false, false);
          }
          this.state = ParserState.FlowMappingValue;
          return Parser.ProcessEmptyScalar(this.GetCurrentToken().Start);
        }
        if (!(this.GetCurrentToken() is FlowMappingEnd))
        {
          this.states.Push(ParserState.FlowMappingEmptyValue);
          return this.ParseNode(false, false);
        }
      }
      this.state = this.states.Pop();
      MappingEnd mappingEnd = new MappingEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
      this.Skip();
      return (ParsingEvent) mappingEnd;
    }

    private ParsingEvent ParseFlowMappingValue(bool isEmpty)
    {
      if (isEmpty)
      {
        this.state = ParserState.FlowMappingKey;
        return Parser.ProcessEmptyScalar(this.GetCurrentToken().Start);
      }
      if (this.GetCurrentToken() is YamlDotNet.Core.Tokens.Value)
      {
        this.Skip();
        if (!(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowMappingEnd))
        {
          this.states.Push(ParserState.FlowMappingKey);
          return this.ParseNode(false, false);
        }
      }
      this.state = ParserState.FlowMappingKey;
      return Parser.ProcessEmptyScalar(this.GetCurrentToken().Start);
    }

    private class EventQueue
    {
      private readonly Queue<ParsingEvent> highPriorityEvents = new Queue<ParsingEvent>();
      private readonly Queue<ParsingEvent> normalPriorityEvents = new Queue<ParsingEvent>();

      public void Enqueue(ParsingEvent @event)
      {
        switch (@event.Type)
        {
          case EventType.StreamStart:
          case EventType.DocumentStart:
            this.highPriorityEvents.Enqueue(@event);
            break;
          default:
            this.normalPriorityEvents.Enqueue(@event);
            break;
        }
      }

      public ParsingEvent Dequeue() => this.highPriorityEvents.Count <= 0 ? this.normalPriorityEvents.Dequeue() : this.highPriorityEvents.Dequeue();

      public int Count => this.highPriorityEvents.Count + this.normalPriorityEvents.Count;
    }
  }
}
