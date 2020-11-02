// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.YamlSequenceNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace YamlDotNet.RepresentationModel
{
  [DebuggerDisplay("Count = {children.Count}")]
  [Serializable]
  public sealed class YamlSequenceNode : YamlNode, IEnumerable<YamlNode>, IEnumerable, IYamlConvertible
  {
    private readonly IList<YamlNode> children = (IList<YamlNode>) new List<YamlNode>();

    public IList<YamlNode> Children => this.children;

    public SequenceStyle Style { get; set; }

    internal YamlSequenceNode(IParser parser, DocumentLoadingState state) => this.Load(parser, state);

    private void Load(IParser parser, DocumentLoadingState state)
    {
      SequenceStart sequenceStart = parser.Expect<SequenceStart>();
      this.Load((NodeEvent) sequenceStart, state);
      this.Style = sequenceStart.Style;
      bool flag = false;
      while (!parser.Accept<SequenceEnd>())
      {
        YamlNode node = YamlNode.ParseNode(parser, state);
        this.children.Add(node);
        flag |= node is YamlAliasNode;
      }
      if (flag)
        state.AddNodeWithUnresolvedAliases((YamlNode) this);
      parser.Expect<SequenceEnd>();
    }

    public YamlSequenceNode()
    {
    }

    public YamlSequenceNode(params YamlNode[] children)
      : this((IEnumerable<YamlNode>) children)
    {
    }

    public YamlSequenceNode(IEnumerable<YamlNode> children)
    {
      foreach (YamlNode child in children)
        this.children.Add(child);
    }

    public void Add(YamlNode child) => this.children.Add(child);

    public void Add(string child) => this.children.Add((YamlNode) new YamlScalarNode(child));

    internal override void ResolveAliases(DocumentLoadingState state)
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index] is YamlAliasNode)
          this.children[index] = state.GetNode(this.children[index].Anchor, true, this.children[index].Start, this.children[index].End);
      }
    }

    internal override void Emit(IEmitter emitter, EmitterState state)
    {
      emitter.Emit((ParsingEvent) new SequenceStart(this.Anchor, this.Tag, true, this.Style));
      foreach (YamlNode child in (IEnumerable<YamlNode>) this.children)
        child.Save(emitter, state);
      emitter.Emit((ParsingEvent) new SequenceEnd());
    }

    public override void Accept(IYamlVisitor visitor) => visitor.Visit(this);

    public override bool Equals(object obj)
    {
      if (!(obj is YamlSequenceNode yamlSequenceNode) || !this.Equals((YamlNode) yamlSequenceNode) || this.children.Count != yamlSequenceNode.children.Count)
        return false;
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (!YamlNode.SafeEquals((object) this.children[index], (object) yamlSequenceNode.children[index]))
          return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int h1 = base.GetHashCode();
      foreach (YamlNode child in (IEnumerable<YamlNode>) this.children)
        h1 = YamlNode.CombineHashCodes(h1, YamlNode.GetHashCode((object) child));
      return h1;
    }

    internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
    {
      YamlSequenceNode yamlSequenceNode = this;
      level.Increment();
      yield return (YamlNode) yamlSequenceNode;
      foreach (YamlNode child in (IEnumerable<YamlNode>) yamlSequenceNode.children)
      {
        foreach (YamlNode safeAllNode in child.SafeAllNodes(level))
          yield return safeAllNode;
      }
      level.Decrement();
    }

    public override YamlNodeType NodeType => YamlNodeType.Sequence;

    internal override string ToString(RecursionLevel level)
    {
      if (!level.TryIncrement())
        return "WARNING! INFINITE RECURSION!";
      StringBuilder stringBuilder = new StringBuilder("[ ");
      foreach (YamlNode child in (IEnumerable<YamlNode>) this.children)
      {
        if (stringBuilder.Length > 2)
          stringBuilder.Append(", ");
        stringBuilder.Append(child.ToString(level));
      }
      stringBuilder.Append(" ]");
      level.Decrement();
      return stringBuilder.ToString();
    }

    public IEnumerator<YamlNode> GetEnumerator() => this.Children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    void IYamlConvertible.Read(
      IParser parser,
      Type expectedType,
      ObjectDeserializer nestedObjectDeserializer)
    {
      this.Load(parser, new DocumentLoadingState());
    }

    void IYamlConvertible.Write(
      IEmitter emitter,
      ObjectSerializer nestedObjectSerializer)
    {
      this.Emit(emitter, new EmitterState());
    }
  }
}
