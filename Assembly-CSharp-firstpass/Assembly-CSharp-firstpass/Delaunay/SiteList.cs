// Decompiled with JetBrains decompiler
// Type: Delaunay.SiteList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using Delaunay.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
  public sealed class SiteList : IDisposable
  {
    private List<Site> _sites;
    private int _currentIndex;
    private bool _sorted;

    public SiteList()
    {
      this._sites = new List<Site>();
      this._sorted = false;
    }

    public void Dispose()
    {
      if (this._sites == null)
        return;
      for (int index = 0; index < this._sites.Count; ++index)
        this._sites[index].Dispose();
      this._sites.Clear();
      this._sites = (List<Site>) null;
    }

    public int Add(Site site)
    {
      this._sorted = false;
      this._sites.Add(site);
      return this._sites.Count;
    }

    public int Count => this._sites.Count;

    public Site Next()
    {
      if (!this._sorted)
        Debug.LogError((object) "SiteList::next():  sites have not been sorted");
      return this._currentIndex < this._sites.Count ? this._sites[this._currentIndex++] : (Site) null;
    }

    internal Rect GetSitesBounds()
    {
      if (!this._sorted)
      {
        Site.SortSites(this._sites);
        this._currentIndex = 0;
        this._sorted = true;
      }
      if (this._sites.Count == 0)
        return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      float x = float.MaxValue;
      float num = float.MinValue;
      for (int index = 0; index < this._sites.Count; ++index)
      {
        Site site = this._sites[index];
        if ((double) site.x < (double) x)
          x = site.x;
        if ((double) site.x > (double) num)
          num = site.x;
      }
      float y1 = this._sites[0].y;
      float y2 = this._sites[this._sites.Count - 1].y;
      return new Rect(x, y1, num - x, y2 - y1);
    }

    public List<uint> SiteColors()
    {
      List<uint> uintList = new List<uint>();
      for (int index = 0; index < this._sites.Count; ++index)
        uintList.Add(this._sites[index].color);
      return uintList;
    }

    public List<Vector2> SiteCoords()
    {
      List<Vector2> vector2List = new List<Vector2>();
      for (int index = 0; index < this._sites.Count; ++index)
        vector2List.Add(this._sites[index].Coord);
      return vector2List;
    }

    public void ScaleWeight(float scale)
    {
      for (int index = 0; index < this._sites.Count; ++index)
        this._sites[index].scaled_weight = this._sites[index].weight * scale;
    }

    public List<Circle> Circles()
    {
      List<Circle> circleList = new List<Circle>();
      for (int index = 0; index < this._sites.Count; ++index)
      {
        float radius = 0.0f;
        Edge edge = this._sites[index].NearestEdge();
        if (!edge.IsPartOfConvexHull())
          radius = edge.SitesDistance() * 0.5f;
        circleList.Add(new Circle(this._sites[index].x, this._sites[index].y, radius));
      }
      return circleList;
    }

    public List<List<Vector2>> Regions(Rect plotBounds)
    {
      List<List<Vector2>> vector2ListList = new List<List<Vector2>>();
      for (int index = 0; index < this._sites.Count; ++index)
      {
        Site site = this._sites[index];
        vector2ListList.Add(site.Region(plotBounds));
      }
      return vector2ListList;
    }

    public List<List<Vector2>> Regions(Polygon plotBounds)
    {
      List<List<Vector2>> vector2ListList = new List<List<Vector2>>();
      for (int index = 0; index < this._sites.Count; ++index)
      {
        Site site = this._sites[index];
        vector2ListList.Add(site.Region(plotBounds));
      }
      return vector2ListList;
    }
  }
}
