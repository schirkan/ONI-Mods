// Decompiled with JetBrains decompiler
// Type: Satsuma.ArcLookupExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Satsuma
{
  public static class ArcLookupExtensions
  {
    public static string ArcToString(this IArcLookup graph, Arc arc) => arc == Arc.Invalid ? "Arc.Invalid" : graph.U(arc).ToString() + (graph.IsEdge(arc) ? (object) "<-->" : (object) "--->") + (object) graph.V(arc);

    public static Node Other(this IArcLookup graph, Arc arc, Node node)
    {
      Node node1 = graph.U(arc);
      return node1 != node ? node1 : graph.V(arc);
    }

    public static Node[] Nodes(this IArcLookup graph, Arc arc, bool allowDuplicates = true)
    {
      Node node1 = graph.U(arc);
      Node node2 = graph.V(arc);
      return !allowDuplicates && node1 == node2 ? new Node[1]
      {
        node1
      } : new Node[2]{ node1, node2 };
    }
  }
}
