// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.YamlAliasNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
  [Serializable]
  internal class YamlAliasNode : YamlNode
  {
    internal YamlAliasNode(string anchor) => this.Anchor = anchor;

    internal override void ResolveAliases(DocumentLoadingState state) => throw new NotSupportedException("Resolving an alias on an alias node does not make sense");

    internal override void Emit(IEmitter emitter, EmitterState state) => throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be saved.");

    public override void Accept(IYamlVisitor visitor) => throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be visited.");

    public override bool Equals(object obj) => obj is YamlAliasNode yamlAliasNode && this.Equals((YamlNode) yamlAliasNode) && YamlNode.SafeEquals((object) this.Anchor, (object) yamlAliasNode.Anchor);

    public override int GetHashCode() => base.GetHashCode();

    internal override string ToString(RecursionLevel level) => "*" + this.Anchor;

    internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      YamlAliasNode yamlAliasNode = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = (YamlNode) yamlAliasNode;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }

    public override YamlNodeType NodeType => YamlNodeType.Alias;
  }
}
