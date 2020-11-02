// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.DocumentLoadingState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
  internal class DocumentLoadingState
  {
    private readonly IDictionary<string, YamlNode> anchors = (IDictionary<string, YamlNode>) new Dictionary<string, YamlNode>();
    private readonly IList<YamlNode> nodesWithUnresolvedAliases = (IList<YamlNode>) new List<YamlNode>();

    public void AddAnchor(YamlNode node)
    {
      if (node.Anchor == null)
        throw new ArgumentException("The specified node does not have an anchor");
      if (this.anchors.ContainsKey(node.Anchor))
        this.anchors[node.Anchor] = node;
      else
        this.anchors.Add(node.Anchor, node);
    }

    public YamlNode GetNode(string anchor, bool throwException, Mark start, Mark end)
    {
      YamlNode yamlNode;
      if (this.anchors.TryGetValue(anchor, out yamlNode))
        return yamlNode;
      if (throwException)
        throw new AnchorNotFoundException(start, end, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The anchor '{0}' does not exists", (object) anchor));
      return (YamlNode) null;
    }

    public void AddNodeWithUnresolvedAliases(YamlNode node) => this.nodesWithUnresolvedAliases.Add(node);

    public void ResolveAliases()
    {
      foreach (YamlNode withUnresolvedAlias in (IEnumerable<YamlNode>) this.nodesWithUnresolvedAliases)
        withUnresolvedAlias.ResolveAliases(this);
    }
  }
}
