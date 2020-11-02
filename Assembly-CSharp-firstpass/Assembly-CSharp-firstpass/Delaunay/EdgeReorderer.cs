// Decompiled with JetBrains decompiler
// Type: Delaunay.EdgeReorderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.LR;
using Delaunay.Utils;
using System.Collections.Generic;

namespace Delaunay
{
  internal sealed class EdgeReorderer : IDisposable
  {
    private List<Edge> _edges;
    private List<Side> _edgeOrientations;

    public List<Edge> edges => this._edges;

    public List<Side> edgeOrientations => this._edgeOrientations;

    public EdgeReorderer(List<Edge> origEdges, VertexOrSite criterion)
    {
      this._edges = new List<Edge>();
      this._edgeOrientations = new List<Side>();
      if (origEdges.Count <= 0)
        return;
      this._edges = this.ReorderEdges(origEdges, criterion);
    }

    public void Dispose()
    {
      this._edges = (List<Edge>) null;
      this._edgeOrientations = (List<Side>) null;
    }

    private List<Edge> ReorderEdges(List<Edge> origEdges, VertexOrSite criterion)
    {
      int count = origEdges.Count;
      bool[] flagArray = new bool[count];
      int num1 = 0;
      for (int index = 0; index < count; ++index)
        flagArray[index] = false;
      List<Edge> edgeList = new List<Edge>();
      int index1 = 0;
      Edge origEdge1 = origEdges[index1];
      edgeList.Add(origEdge1);
      this._edgeOrientations.Add(Side.LEFT);
      ICoord coord1 = criterion == VertexOrSite.VERTEX ? (ICoord) origEdge1.leftVertex : (ICoord) origEdge1.leftSite;
      ICoord coord2 = criterion == VertexOrSite.VERTEX ? (ICoord) origEdge1.rightVertex : (ICoord) origEdge1.rightSite;
      if (coord1 == Vertex.VERTEX_AT_INFINITY || coord2 == Vertex.VERTEX_AT_INFINITY)
        return new List<Edge>();
      flagArray[index1] = true;
      int num2 = num1 + 1;
      while (num2 < count)
      {
        for (int index2 = 1; index2 < count; ++index2)
        {
          if (!flagArray[index2])
          {
            Edge origEdge2 = origEdges[index2];
            ICoord coord3 = criterion == VertexOrSite.VERTEX ? (ICoord) origEdge2.leftVertex : (ICoord) origEdge2.leftSite;
            ICoord coord4 = criterion == VertexOrSite.VERTEX ? (ICoord) origEdge2.rightVertex : (ICoord) origEdge2.rightSite;
            if (coord3 == Vertex.VERTEX_AT_INFINITY || coord4 == Vertex.VERTEX_AT_INFINITY)
              return new List<Edge>();
            if (coord3 == coord2)
            {
              coord2 = coord4;
              this._edgeOrientations.Add(Side.LEFT);
              edgeList.Add(origEdge2);
              flagArray[index2] = true;
            }
            else if (coord4 == coord1)
            {
              coord1 = coord3;
              this._edgeOrientations.Insert(0, Side.LEFT);
              edgeList.Insert(0, origEdge2);
              flagArray[index2] = true;
            }
            else if (coord3 == coord1)
            {
              coord1 = coord4;
              this._edgeOrientations.Insert(0, Side.RIGHT);
              edgeList.Insert(0, origEdge2);
              flagArray[index2] = true;
            }
            else if (coord4 == coord2)
            {
              coord2 = coord3;
              this._edgeOrientations.Add(Side.RIGHT);
              edgeList.Add(origEdge2);
              flagArray[index2] = true;
            }
            if (flagArray[index2])
              ++num2;
          }
        }
      }
      return edgeList;
    }
  }
}
