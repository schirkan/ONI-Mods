// Decompiled with JetBrains decompiler
// Type: VoronoiTree.Site
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using KSerialization;
using MIConvexHull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace VoronoiTree
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Site : TriangulationCell<PowerDiagram.DualSite2d, Site>
  {
    public float weight;
    public float currentWeight;
    public float previousWeightAdaption;
    [Serialize]
    public int id = -1;
    [Serialize]
    public Vector2 position;
    [Serialize]
    public Polygon poly;
    [Serialize]
    public List<Site> neighbours;
    private Vector2? circumCenter;
    private Vector2? centroid;

    public bool dummy { get; set; }

    public Site() => this.dummy = true;

    public Site(Vector2 pos)
    {
      this.position = pos;
      this.weight = Mathf.Epsilon;
      this.dummy = false;
      this.previousWeightAdaption = 1f;
    }

    public Site(float x, float y)
      : this(new Vector2(x, y))
    {
    }

    public Site(uint siteId, Vector2 pos, float siteWeight = 1f)
    {
      this.dummy = false;
      this.neighbours = new List<Site>();
      this.id = (int) siteId;
      this.position = pos;
      this.weight = siteWeight;
      this.currentWeight = this.weight;
    }

    [OnDeserializing]
    internal void OnDeserializingMethod() => this.neighbours = new List<Site>();

    public PowerDiagram.DualSite3d ToDualSite() => new PowerDiagram.DualSite3d((double) this.position.x, (double) this.position.y, (double) this.position.x * (double) this.position.x + (double) this.position.y * (double) this.position.y - (double) this.currentWeight, this);

    private double Det(double[,] m) => m[0, 0] * (m[1, 1] * m[2, 2] - m[2, 1] * m[1, 2]) - m[0, 1] * (m[1, 0] * m[2, 2] - m[2, 0] * m[1, 2]) + m[0, 2] * (m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1]);

    private double LengthSquared(double[] v)
    {
      double num1 = 0.0;
      for (int index = 0; index < v.Length; ++index)
      {
        double num2 = v[index];
        num1 += num2 * num2;
      }
      return num1;
    }

    private Vector2 GetCircumcenter()
    {
      PowerDiagram.DualSite2d[] vertices = this.Vertices;
      double[,] m = new double[3, 3];
      for (int index = 0; index < 3; ++index)
      {
        m[index, 0] = vertices[index].Position[0];
        m[index, 1] = vertices[index].Position[1];
        m[index, 2] = 1.0;
      }
      double num1 = -1.0 / (2.0 * this.Det(m));
      for (int index = 0; index < 3; ++index)
        m[index, 0] = this.LengthSquared(vertices[index].Position);
      double num2 = -this.Det(m);
      for (int index = 0; index < 3; ++index)
        m[index, 1] = vertices[index].Position[0];
      double num3 = this.Det(m);
      return new Vector2((float) (num1 * num2), (float) (num1 * num3));
    }

    private Vector2 GetCentroid() => new Vector2((float) ((IEnumerable<PowerDiagram.DualSite2d>) this.Vertices).Select<PowerDiagram.DualSite2d, double>((Func<PowerDiagram.DualSite2d, double>) (v => v.Position[0])).Average(), (float) ((IEnumerable<PowerDiagram.DualSite2d>) this.Vertices).Select<PowerDiagram.DualSite2d, double>((Func<PowerDiagram.DualSite2d, double>) (v => v.Position[1])).Average());

    public Vector2 Circumcenter
    {
      get
      {
        this.circumCenter = new Vector2?(this.circumCenter ?? this.GetCircumcenter());
        return this.circumCenter.Value;
      }
    }

    public Vector2 Centroid
    {
      get
      {
        if (this.poly != null)
          return this.poly.Centroid();
        this.centroid = new Vector2?(this.centroid ?? this.GetCentroid());
        return this.centroid.Value;
      }
    }
  }
}
