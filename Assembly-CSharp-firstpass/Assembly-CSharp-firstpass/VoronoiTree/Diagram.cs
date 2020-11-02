// Decompiled with JetBrains decompiler
// Type: VoronoiTree.Diagram
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay;
using Delaunay.Geo;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace VoronoiTree
{
  public class Diagram
  {
    public static int maxPowerIterations;
    public static float maxPowerError;
    private List<Vector2> points;
    private List<float> weights;
    private Rect bounds;
    private float weightSum;
    private List<uint> ids = new List<uint>();
    public int siteIndex;

    public Diagram() => this.diagram = (Voronoi) null;

    public Voronoi diagram { get; private set; }

    public Diagram(Rect bounds, IEnumerable<Diagram.Site> sites)
    {
      this.bounds = bounds;
      this.ids = new List<uint>();
      this.points = new List<Vector2>();
      this.weights = new List<float>();
      this.weightSum = 0.0f;
      IEnumerator<Diagram.Site> enumerator = sites.GetEnumerator();
      int num = 0;
      while (enumerator.MoveNext())
      {
        this.AddSite(enumerator.Current);
        ++num;
      }
      this.MakeVD();
    }

    private void AddSite(Diagram.Site site)
    {
      this.ids.Add(site.id);
      this.points.Add(site.position);
      this.weights.Add(site.weight);
      this.weightSum += site.weight;
      site.currentWeight = site.weight;
    }

    private void MakeVD() => this.diagram = new Voronoi(this.points, this.ids, this.weights, this.bounds);

    public void UpdateWeights(List<Diagram.Site> sites)
    {
      for (int index = 0; index < sites.Count; ++index)
      {
        Diagram.Site site = sites[index];
        site.position = site.poly.Centroid();
        site.currentWeight = Mathf.Max(site.currentWeight, 1f);
      }
      float num1 = 0.0f;
      for (int index = 0; index < sites.Count; ++index)
      {
        Diagram.Site site = sites[index];
        float num2 = site.poly.Area();
        float num3 = site.weight / this.weightSum * this.Area();
        float num4 = Mathf.Sqrt(num2 / 3.141593f) - Mathf.Sqrt(num3 / 3.141593f);
        float f = num3 / num2;
        if ((double) f > 1.1 && (double) site.previousWeightAdaption < 0.9 || (double) f < 0.9 && (double) site.previousWeightAdaption > 1.1)
          f = Mathf.Sqrt(f);
        if ((double) f < 1.1 && (double) f > 0.9 && (double) site.currentWeight != 1.0)
          f = Mathf.Sqrt(f);
        if ((double) site.currentWeight < 10.0)
          f *= f;
        if ((double) site.currentWeight > 10.0)
          f = Mathf.Sqrt(f);
        site.previousWeightAdaption = f;
        site.currentWeight *= f;
        if ((double) site.currentWeight < 1.0)
        {
          float num5 = Mathf.Sqrt(site.currentWeight) - num4;
          if ((double) num5 < 0.0)
          {
            site.currentWeight = (float) -((double) num5 * (double) num5);
            if ((double) site.currentWeight < (double) num1)
              num1 = site.currentWeight;
          }
        }
      }
      if ((double) num1 < 0.0)
      {
        float num2 = -num1;
        for (int index = 0; index < sites.Count; ++index)
          sites[index].currentWeight += num2 + 1f;
      }
      float num6 = 1f;
      for (int index = 0; index < sites.Count; ++index)
      {
        Diagram.Site site1 = sites[index];
        List<uint> neighbours = this.diagram.ListNeighborSitesIDsForSite(this.points[index]);
        for (int nIndex = 0; nIndex < neighbours.Count; nIndex++)
        {
          Diagram.Site site2 = sites.Find((Predicate<Diagram.Site>) (s => (int) s.id == (int) neighbours[nIndex]));
          float num2 = (site1.position - site2.position).sqrMagnitude / (Mathf.Abs(site1.currentWeight - site2.currentWeight) + 1f);
          if ((double) num2 < (double) num6)
            num6 = num2;
        }
      }
      for (int index = 0; index < sites.Count; ++index)
      {
        sites[index].currentWeight *= num6;
        this.weights[index] = sites[index].currentWeight;
        this.points[index] = sites[index].position;
      }
    }

    private float Area() => this.bounds.width * this.bounds.height;

    public int completeIterations { get; set; }

    public List<Diagram.Site> ComputePowerDiagram(
      List<Diagram.Site> sites,
      int maxIterations)
    {
      this.completeIterations = 0;
      for (int index = 1; index <= maxIterations; ++index)
      {
        this.UpdateWeights(sites);
        this.MakeVD();
        float b = 0.0f;
        foreach (Diagram.Site site in sites)
        {
          float num1 = site.poly.Area();
          float num2 = site.weight / this.weightSum * this.Area();
          b = Mathf.Max(Mathf.Abs(num1 - num2) / num2, b);
        }
        if ((double) b < 1.0 / 1000.0)
        {
          this.completeIterations = index;
          break;
        }
      }
      return sites;
    }

    public int GetIdxForNode(uint nodeID)
    {
      for (int index = 0; index < this.points.Count; ++index)
      {
        if ((int) this.ids[index] == (int) nodeID)
          return index;
      }
      return -1;
    }

    public List<uint> GetNodeIdsForTopEdgeCells()
    {
      List<uint> uintList = new List<uint>();
      for (int index = 0; index < this.points.Count; ++index)
      {
        if (this.IsTopEdgeCell(index))
          uintList.Add(this.ids[index]);
      }
      return uintList;
    }

    public bool IsTopEdgeCell(int cell)
    {
      if (cell < 0 || cell >= this.points.Count)
        return false;
      List<Vector2> vector2List = this.diagram.Region(this.points[cell]);
      if (vector2List.Count == 0)
        return false;
      Vector2 vector2_1 = vector2List[0];
      for (int index = 1; index < vector2List.Count; ++index)
      {
        Vector2 vector2_2 = vector2List[index];
        if ((double) vector2_1.y == (double) vector2_2.y && (double) vector2_2.y == (double) this.bounds.height)
          return true;
        vector2_1 = vector2_2;
      }
      return (double) vector2_1.y == (double) vector2List[0].y && (double) vector2List[0].y == (double) this.bounds.height;
    }

    [SerializationConfig(MemberSerialization.OptIn)]
    public class Site
    {
      [Serialize]
      public uint id;
      [Serialize]
      public float weight;
      public float currentWeight;
      public float previousWeightAdaption;
      [Serialize]
      public Vector2 position;
      [Serialize]
      public Polygon poly;
      [Serialize]
      public HashSet<KeyValuePair<uint, int>> neighbours;

      public Site() => this.neighbours = new HashSet<KeyValuePair<uint, int>>();

      public Site(uint id, Vector2 pos, float weight = 1f)
      {
        this.id = id;
        this.position = pos;
        this.weight = weight;
        this.currentWeight = weight;
        this.neighbours = new HashSet<KeyValuePair<uint, int>>();
      }

      [OnDeserializing]
      internal void OnDeserializingMethod() => this.neighbours = new HashSet<KeyValuePair<uint, int>>();
    }
  }
}
