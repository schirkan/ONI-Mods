// Decompiled with JetBrains decompiler
// Type: Satsuma.IGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public interface IGraph : IArcLookup
  {
    IEnumerable<Node> Nodes();

    IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All);

    IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All);

    IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All);

    int NodeCount();

    int ArcCount(ArcFilter filter = ArcFilter.All);

    int ArcCount(Node u, ArcFilter filter = ArcFilter.All);

    int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All);

    bool HasNode(Node node);

    bool HasArc(Arc arc);
  }
}
