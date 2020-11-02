// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.YamlVisitorBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace YamlDotNet.RepresentationModel
{
  public abstract class YamlVisitorBase : IYamlVisitor
  {
    public virtual void Visit(YamlStream stream) => this.VisitChildren(stream);

    public virtual void Visit(YamlDocument document) => this.VisitChildren(document);

    public virtual void Visit(YamlScalarNode scalar)
    {
    }

    public virtual void Visit(YamlSequenceNode sequence) => this.VisitChildren(sequence);

    public virtual void Visit(YamlMappingNode mapping) => this.VisitChildren(mapping);

    protected virtual void VisitPair(YamlNode key, YamlNode value)
    {
      key.Accept((IYamlVisitor) this);
      value.Accept((IYamlVisitor) this);
    }

    protected virtual void VisitChildren(YamlStream stream)
    {
      foreach (YamlDocument document in (IEnumerable<YamlDocument>) stream.Documents)
        document.Accept((IYamlVisitor) this);
    }

    protected virtual void VisitChildren(YamlDocument document)
    {
      if (document.RootNode == null)
        return;
      document.RootNode.Accept((IYamlVisitor) this);
    }

    protected virtual void VisitChildren(YamlSequenceNode sequence)
    {
      foreach (YamlNode child in (IEnumerable<YamlNode>) sequence.Children)
        child.Accept((IYamlVisitor) this);
    }

    protected virtual void VisitChildren(YamlMappingNode mapping)
    {
      foreach (KeyValuePair<YamlNode, YamlNode> child in (IEnumerable<KeyValuePair<YamlNode, YamlNode>>) mapping.Children)
        this.VisitPair(child.Key, child.Value);
    }
  }
}
