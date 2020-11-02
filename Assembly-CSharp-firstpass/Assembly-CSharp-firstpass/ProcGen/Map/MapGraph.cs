// Decompiled with JetBrains decompiler
// Type: ProcGen.Map.MapGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using Satsuma;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace ProcGen.Map
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class MapGraph : Graph
  {
    [Serialize]
    public List<Cell> cellList;
    [Serialize]
    public List<Corner> cornerList;
    [Serialize]
    public List<Edge> edgeList;

    public List<Cell> cells => this.cellList;

    public List<Corner> corners => this.cornerList;

    public List<Edge> edges => this.edgeList;

    public MapGraph(int seed)
      : base(seed)
    {
      this.cellList = new List<Cell>();
      this.cornerList = new List<Corner>();
      this.edgeList = new List<Edge>();
    }

    public Edge GetEdge(Corner corner0, Corner corner1, bool createOK = true) => this.GetEdge(corner0, corner1, createOK, out bool _);

    public Edge GetEdge(Corner corner0, Corner corner1, bool createOK, out bool didCreate)
    {
      didCreate = false;
      Edge edge1 = this.edgeList.Find((Predicate<Edge>) (e =>
      {
        if (e.corner0 == corner0 && e.corner1 == corner1)
          return true;
        return e.corner1 == corner0 && e.corner0 == corner1;
      }));
      if (edge1 != null)
        return edge1;
      if (!createOK)
      {
        Debug.LogWarning((object) "Cant create Edge but no edge found");
        return (Edge) null;
      }
      Edge edge2 = new Edge(this.baseGraph.AddArc(corner0.node, corner1.node, Directedness.Undirected), corner0, corner1);
      this.arcList.Add((ProcGen.Arc) edge2);
      this.edgeList.Add(edge2);
      didCreate = true;
      return edge2;
    }

    public Edge GetEdge(
      Corner corner0,
      Corner corner1,
      Cell site0,
      Cell site1,
      bool createOK = true)
    {
      return this.GetEdge(corner0, corner1, site0, site1, createOK, out bool _);
    }

    public Edge GetEdge(
      Corner corner0,
      Corner corner1,
      Cell site0,
      Cell site1,
      bool createOK,
      out bool didCreate)
    {
      didCreate = false;
      Edge edge1 = this.edgeList.Find((Predicate<Edge>) (e =>
      {
        if (e.corner0 == corner0 && e.corner1 == corner1)
          return true;
        return e.corner1 == corner0 && e.corner0 == corner1;
      }));
      if (edge1 != null)
        return edge1;
      if (!createOK)
      {
        Debug.LogWarning((object) "Cant create Edge but no edge found");
        return (Edge) null;
      }
      Edge edge2 = new Edge(this.baseGraph.AddArc(corner0.node, corner1.node, Directedness.Undirected), corner0, corner1, site0, site1);
      this.arcList.Add((ProcGen.Arc) edge2);
      this.edgeList.Add(edge2);
      didCreate = true;
      return edge2;
    }

    public Corner GetCorner(Vector2 position, bool createOK = true)
    {
      Corner corner = this.cornerList.Find((Predicate<Corner>) (c =>
      {
        Vector2 vector2 = c.position - position;
        return (double) vector2.x < 1.0 && (double) vector2.x > -1.0 && (double) vector2.y < 1.0 && (double) vector2.y > -1.0;
      }));
      if (corner == null)
      {
        if (!createOK)
        {
          Debug.LogWarning((object) "Cant create Corner but no corner found");
          return (Corner) null;
        }
        corner = new Corner(this.baseGraph.AddNode());
        this.nodeList.Add((ProcGen.Node) corner);
        corner.SetPosition(position);
        this.cornerList.Add(corner);
      }
      return corner;
    }

    public Cell GetCell(Satsuma.Node node) => this.cellList.Find((Predicate<Cell>) (c => c.node == node));

    public Cell GetCell(Vector2 position) => this.cellList.Find((Predicate<Cell>) (c =>
    {
      Vector2 vector2 = c.position - position;
      return (double) vector2.x < 1.0 && (double) vector2.x > -1.0 && (double) vector2.y < 1.0 && (double) vector2.y > -1.0;
    }));

    public Cell GetCell(Vector2 position, Satsuma.Node node, bool createOK = true) => this.GetCell(position, node, createOK, out bool _);

    public Cell GetCell(Vector2 position, Satsuma.Node node, bool createOK, out bool didCreate)
    {
      Cell cell = this.cellList.Find((Predicate<Cell>) (c =>
      {
        Vector2 vector2 = c.position - position;
        return (double) vector2.x < 1.0 && (double) vector2.x > -1.0 && (double) vector2.y < 1.0 && (double) vector2.y > -1.0;
      }));
      didCreate = false;
      if (cell == null)
      {
        if (!createOK)
        {
          Debug.LogWarning((object) "Cant create Cell but no cell found");
          return (Cell) null;
        }
        cell = this.cellList.Find((Predicate<Cell>) (c => c.node == node));
        if (cell == null)
        {
          cell = new Cell(node);
          didCreate = true;
          cell.SetPosition(position);
          this.cellList.Add(cell);
        }
        else
          Debug.LogWarning((object) ("GetCell Same node [" + (object) node.Id + "] differnt position!"));
      }
      return cell;
    }

    public List<Edge> GetEdgesWithTag(Tag tag)
    {
      List<Edge> edgeList = new List<Edge>();
      for (int index = 0; index < this.edgeList.Count; ++index)
      {
        if (this.edgeList[index].tags.Contains(tag))
          edgeList.Add(this.edgeList[index]);
      }
      return edgeList;
    }

    public void Remove(Edge n)
    {
      n.site0.Remove(n);
      n.site1.Remove(n);
      this.edges.Remove(n);
    }

    public void Validate()
    {
      for (int index1 = 0; index1 < this.cellList.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.cellList.Count; ++index2)
        {
          if (index2 != index1)
          {
            if (this.cellList[index1] == this.cellList[index2])
            {
              Debug.LogError((object) "Duplicate cell (class)");
              return;
            }
            if (this.cellList[index1].position == this.cellList[index2].position)
            {
              Debug.LogError((object) "Duplicate cell (position)");
              return;
            }
            if (this.cellList[index1].node == this.cellList[index2].node)
            {
              Debug.LogError((object) "Duplicate cell (node)");
              return;
            }
          }
        }
      }
      for (int index1 = 0; index1 < this.cornerList.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.cornerList.Count; ++index2)
        {
          if (index2 != index1)
          {
            if (this.cornerList[index1] == this.cornerList[index2])
            {
              Debug.LogError((object) "Duplicate corner (class)");
              return;
            }
            if (this.cornerList[index1].position == this.cornerList[index2].position)
            {
              Debug.LogError((object) "Duplicate corner (position)");
              return;
            }
            if (this.cornerList[index1].node == this.cornerList[index2].node)
            {
              Debug.LogError((object) "Duplicate corner (node)");
              return;
            }
          }
        }
      }
      for (int index1 = 0; index1 < this.edgeList.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.edgeList.Count; ++index2)
        {
          if (index2 != index1)
          {
            Edge edge1 = this.edgeList[index1];
            Edge edge2 = this.edgeList[index2];
            if (edge1 == edge2)
            {
              Debug.LogError((object) "Duplicate edge (class)");
              return;
            }
            if (edge1.arc == edge2.arc)
            {
              object[] objArray = new object[9]
              {
                (object) "Duplicate EDGE [",
                (object) edge1.arc,
                (object) "] & [",
                (object) edge2.arc,
                (object) "] - (ARC) [",
                null,
                null,
                null,
                null
              };
              Satsuma.Node node = edge1.site0.node;
              objArray[5] = (object) node.Id;
              objArray[6] = (object) "] &  [";
              node = edge1.site1.node;
              objArray[7] = (object) node.Id;
              objArray[8] = (object) "]";
              Debug.LogError((object) string.Concat(objArray));
              return;
            }
            if (edge1.corner0 == edge2.corner0 && edge1.corner1 == edge2.corner1)
            {
              Debug.LogError((object) "Duplicate edge (corner same order)");
              return;
            }
            if (edge1.corner0 == edge2.corner1 && edge1.corner1 == edge2.corner0)
            {
              Debug.LogError((object) "Duplicate edge (corner different order)");
              return;
            }
            if (edge1.site0 != edge1.site1 && edge2.site0 != edge2.site1)
            {
              if (edge1.site0 == edge2.site0 && edge1.site1 == edge2.site1)
              {
                Debug.LogError((object) "Duplicate edge (site same order)");
                return;
              }
              if (edge1.site0 == edge2.site1 && edge1.site1 == edge2.site0)
              {
                Debug.LogError((object) ("Duplicate Edge [" + (object) edge1.arc.Id + "] -> [" + (object) edge1.corner0.node.Id + "<-->" + (object) edge1.corner1.node.Id + "] sites: [" + (object) edge1.site0.node.Id + " -- " + (object) edge1.site1.node.Id + "] and [" + (object) edge2.arc.Id + "] -> [" + (object) edge2.corner0.node.Id + "<-->" + (object) edge2.corner1.node.Id + "] sites: [" + (object) edge2.site0.node.Id + " -- " + (object) edge2.site1.node.Id + "] - (site differnt order)"));
                Debug.Log((object) ("CE 0: " + (object) edge1.corner0.position + " 1: " + (object) edge1.corner1.position));
                Debug.Log((object) ("OE 0: " + (object) edge2.corner0.position + " 1: " + (object) edge2.corner1.position));
                Debug.Log((object) ("Sites C 0: " + (object) edge1.site0.position + " 1: " + (object) edge1.site1.position));
                DebugExtension.DebugCircle2d(edge1.site0.position, UnityEngine.Color.red, duration: 15f);
                DebugExtension.DebugCircle2d(edge1.site1.position, UnityEngine.Color.magenta, 2f, 15f);
                Debug.Log((object) ("Sites O 0: " + (object) edge2.site0.position + " 1: " + (object) edge2.site1.position));
                DebugExtension.DebugCircle2d(edge2.site0.position, UnityEngine.Color.green, 3f, 15f);
                DebugExtension.DebugCircle2d(edge2.site1.position, UnityEngine.Color.cyan, 4f, 15f);
              }
              else
              {
                if (edge1.site0.node == edge2.site0.node && edge1.site1.node == edge2.site1.node)
                {
                  Debug.LogError((object) "Duplicate edge (site node same order)");
                  return;
                }
                if (edge1.site1.node == edge2.site0.node && edge1.site0.node == edge2.site1.node)
                {
                  Debug.LogError((object) "Duplicate edge (site node differnt order)");
                  return;
                }
              }
            }
          }
        }
      }
    }

    [OnDeserialized]
    internal new void OnDeserializedMethod()
    {
      try
      {
        base.OnDeserializedMethod();
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Error deserialising " + ex.Message + "\n" + ex.StackTrace));
      }
    }
  }
}
