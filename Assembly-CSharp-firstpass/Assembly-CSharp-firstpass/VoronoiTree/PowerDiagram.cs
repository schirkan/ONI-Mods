// Decompiled with JetBrains decompiler
// Type: VoronoiTree.PowerDiagram
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using MIConvexHull;
using ProcGen.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VoronoiTree
{
  public class PowerDiagram
  {
    public const Winding ForcedWinding = Winding.COUNTERCLOCKWISE;
    public MapGraph mg = new MapGraph(0);
    private Polygon bounds;
    private List<Site> externalEdgePoints = new List<Site>();
    private float weightSum;
    private List<Site> sites = new List<Site>();
    private List<PowerDiagram.DualSite2d> dualSites = new List<PowerDiagram.DualSite2d>();
    private ConvexHull<PowerDiagram.DualSite3d, PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> debug_LastHull;

    public VoronoiMesh<PowerDiagram.DualSite2d, Site, VoronoiEdge<PowerDiagram.DualSite2d, Site>> voronoiMesh { get; private set; }

    public List<Site> GetSites() => this.sites;

    public int completedIterations { get; set; }

    public PowerDiagram(Polygon polyBounds, IEnumerable<Site> newSites)
    {
      this.bounds = polyBounds;
      this.bounds.ForceWinding(Winding.COUNTERCLOCKWISE);
      this.weightSum = 0.0f;
      this.sites.Clear();
      IEnumerator<Site> enumerator = newSites.GetEnumerator();
      int num = 0;
      while (enumerator.MoveNext())
      {
        if (!this.bounds.Contains(enumerator.Current.position))
          Debug.LogErrorFormat("Cant feed points [{0}] to powerdiagram that are outside its area [{1}] ", (object) enumerator.Current.id, (object) enumerator.Current.position);
        if (this.bounds.Contains(enumerator.Current.position))
          this.AddSite(enumerator.Current);
        ++num;
      }
      Vector2 vector2_1 = this.bounds.Centroid();
      for (int index = 0; index < this.bounds.Vertices.Count; ++index)
      {
        Vector2 vertex1 = this.bounds.Vertices[index];
        Vector2 vertex2 = this.bounds.Vertices[index < this.bounds.Vertices.Count - 1 ? index + 1 : 0];
        Vector2 vector2_2 = vertex1 - vector2_1;
        Vector2 vector2_3 = vector2_2.normalized * 1000f;
        Site site1 = new Site(vertex1 + vector2_3);
        site1.dummy = true;
        this.externalEdgePoints.Add(site1);
        site1.weight = Mathf.Epsilon;
        site1.currentWeight = Mathf.Epsilon;
        this.dualSites.Add(new PowerDiagram.DualSite2d(site1));
        vector2_2 = (vertex2 - vertex1) * 0.5f + vertex2 - vector2_1;
        Vector2 vector2_4 = vector2_2.normalized * 1000f;
        Site site2 = new Site(vertex2 + vector2_4);
        site2.dummy = true;
        site2.weight = Mathf.Epsilon;
        site2.currentWeight = Mathf.Epsilon;
        this.externalEdgePoints.Add(site2);
        this.dualSites.Add(new PowerDiagram.DualSite2d(site2));
      }
    }

    public void ComputePowerDiagram(int maxIterations, float threashold = 1f)
    {
      this.completedIterations = 0;
      float b = 0.0f;
      foreach (Site site in this.sites)
        site.position = site.poly != null ? site.poly.Centroid() : throw new Exception("site poly is null for [" + (object) site.id + "]" + (object) site.position);
      for (int index = 0; index <= maxIterations; ++index)
      {
        try
        {
          this.UpdateWeights(this.sites);
          this.ComputePD();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Error [" + (object) b + "] iters " + (object) this.completedIterations + "/" + (object) maxIterations + " Exception:" + ex.Message + "\n" + ex.StackTrace));
          break;
        }
        b = 0.0f;
        foreach (Site site in this.sites)
        {
          double num1 = site.poly == null ? 0.100000001490116 : (double) site.poly.Area();
          float num2 = site.weight / this.weightSum * this.bounds.Area();
          double num3 = (double) num2;
          b = Mathf.Max(Mathf.Abs((float) (num1 - num3)) / num2, b);
        }
        if ((double) b < (double) threashold)
        {
          this.completedIterations = index;
          break;
        }
        ++this.completedIterations;
      }
    }

    public void ComputeVD()
    {
      this.voronoiMesh = VoronoiMesh.Create<PowerDiagram.DualSite2d, Site>((IList<PowerDiagram.DualSite2d>) this.dualSites);
      foreach (Site vertex1 in this.voronoiMesh.Vertices)
      {
        Vector2 circumcenter1 = vertex1.Circumcenter;
        Cell cell1 = this.mg.GetCell(circumcenter1);
        foreach (PowerDiagram.DualSite2d vertex2 in vertex1.Vertices)
        {
          if (!vertex2.visited)
          {
            vertex2.visited = true;
            if (!vertex2.site.dummy)
            {
              List<Vector2> verts = new List<Vector2>();
              vertex2.site.neighbours = this.TouchingFaces(vertex2, vertex1);
              foreach (Site neighbour in vertex2.site.neighbours)
              {
                Vector2 circumcenter2 = neighbour.Circumcenter;
                Color.red.a = 0.3f;
                verts.Add(circumcenter2);
                Vector2 position = circumcenter2;
                Cell cell2 = this.mg.GetCell(position);
                if (cell1 != null && cell2 != null)
                {
                  cell1.Add(cell2);
                  cell2.Add(cell1);
                  Corner corner1 = this.mg.GetCorner(circumcenter1);
                  Corner corner2 = this.mg.GetCorner(position);
                  cell1.Add(corner1);
                  cell1.Add(corner2);
                  cell2.Add(corner1);
                  cell2.Add(corner2);
                }
              }
              if (verts.Count > 0)
              {
                Polygon polygon = PowerDiagram.PolyForRandomPoints(verts);
                vertex2.site.poly = polygon.Clip(this.bounds);
              }
            }
          }
        }
      }
      this.ClipNeighbors();
    }

    public void ComputeVD3d()
    {
      List<PowerDiagram.DualSite3d> dualSite3dList = new List<PowerDiagram.DualSite3d>();
      foreach (Site site in this.sites)
        dualSite3dList.Add(site.ToDualSite());
      for (int index = 0; index < this.externalEdgePoints.Count; ++index)
        dualSite3dList.Add(this.externalEdgePoints[index].ToDualSite());
      foreach (PowerDiagram.TriangulationCellExt<PowerDiagram.DualSite3d> vertex1 in VoronoiMesh.Create<PowerDiagram.DualSite3d, PowerDiagram.TriangulationCellExt<PowerDiagram.DualSite3d>>((IList<PowerDiagram.DualSite3d>) dualSite3dList).Vertices)
      {
        Vector3 zero = Vector3.zero;
        foreach (PowerDiagram.DualSite3d vertex2 in vertex1.Vertices)
          zero += vertex2.coord;
        DebugExtension.DebugPoint(zero * 0.3333333f, Color.red);
      }
    }

    private bool ContainsVert(Site face, PowerDiagram.DualSite2d target)
    {
      if (face == null || face.Vertices == null)
        return false;
      for (int index = 0; index < face.Vertices.Length; ++index)
      {
        if (face.Vertices[index] == target)
          return true;
      }
      return false;
    }

    private void AddSite(Site site)
    {
      this.weightSum += site.weight;
      site.currentWeight = site.weight;
      this.sites.Add(site);
      this.dualSites.Add(new PowerDiagram.DualSite2d(site));
    }

    private List<Site> TouchingFaces(PowerDiagram.DualSite2d site, Site startingFace)
    {
      List<Site> siteList = new List<Site>();
      Stack<Site> siteStack = new Stack<Site>();
      siteStack.Push(startingFace);
      while (siteStack.Count > 0)
      {
        Site face = siteStack.Pop();
        if (this.ContainsVert(face, site) && !siteList.Contains(face))
        {
          siteList.Add(face);
          for (int index = 0; index < face.Adjacency.Length; ++index)
          {
            if (this.ContainsVert(face.Adjacency[index], site))
              siteStack.Push(face.Adjacency[index]);
          }
        }
      }
      return siteList;
    }

    private void ClipNeighbors()
    {
      foreach (Site site in this.sites)
      {
        Cell cell1 = this.mg.GetCell(site.position);
        if (cell1 != null && cell1.corners != null && cell1.corners.Count > 2)
        {
          Cell cell2 = this.mg.GetCell(site.position);
          if (site.poly != null)
          {
            foreach (ProcGen.Map.Edge edge in cell2.edges)
            {
              LineSegment segment = new LineSegment(new Vector2?(edge.corner0.position), new Vector2?(edge.corner1.position));
              LineSegment intersectingSegment = new LineSegment(new Vector2?(), new Vector2?());
              Vector2 zero1 = Vector2.zero;
              Vector2 zero2 = Vector2.zero;
              bool flag = site.poly.ClipSegment(segment, ref intersectingSegment, ref zero1, ref zero2);
              if (intersectingSegment.p0.HasValue && intersectingSegment.p1.HasValue && flag)
              {
                edge.corner0.SetPosition(intersectingSegment.p0.Value);
                edge.corner1.SetPosition(intersectingSegment.p1.Value);
              }
            }
          }
        }
      }
    }

    private PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> GetNeigborFaceForEdge(
      PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> currentFace,
      PowerDiagram.DualSite3d sharedVert0,
      PowerDiagram.DualSite3d sharedVert1)
    {
      for (int index1 = 0; index1 < currentFace.Adjacency.Length; ++index1)
      {
        PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> convexFaceExt = currentFace.Adjacency[index1];
        if (convexFaceExt != null)
        {
          int num = 0;
          for (int index2 = 0; index2 < convexFaceExt.Vertices.Length; ++index2)
          {
            if (sharedVert0 == convexFaceExt.Vertices[index2])
              ++num;
            else if (sharedVert1 == convexFaceExt.Vertices[index2])
              ++num;
            if (num == 2)
              return convexFaceExt;
          }
        }
      }
      return (PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>) null;
    }

    private PowerDiagram.Edge GetEdge(
      PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> face0,
      PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> face1)
    {
      PowerDiagram.Edge edge = (PowerDiagram.Edge) null;
      for (int index1 = 0; index1 < face0.Vertices.Length; ++index1)
      {
        for (int index2 = 0; index2 < face1.Vertices.Length; ++index2)
        {
          if (face0.Vertices[index1] == face1.Vertices[index2])
          {
            if (edge == null)
              edge = new PowerDiagram.Edge(face0.Vertices[index1], (PowerDiagram.DualSite3d) null);
            else
              edge.Second = face0.Vertices[index1];
          }
        }
      }
      return edge;
    }

    private bool ContainsVert(
      PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> face,
      PowerDiagram.DualSite3d target)
    {
      for (int index = 0; index < face.Vertices.Length; ++index)
      {
        if (face.Vertices[index] == target)
          return true;
      }
      return false;
    }

    private List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> TouchingFaces(
      PowerDiagram.DualSite3d site,
      PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> startingFace)
    {
      List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> convexFaceExtList = new List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>>();
      Stack<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> convexFaceExtStack = new Stack<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>>();
      convexFaceExtStack.Push(startingFace);
      while (convexFaceExtStack.Count > 0)
      {
        PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> face = convexFaceExtStack.Pop();
        if (this.ContainsVert(face, site) && !convexFaceExtList.Contains(face))
        {
          convexFaceExtList.Add(face);
          for (int index = 0; index < face.Adjacency.Length; ++index)
          {
            if (this.ContainsVert(face.Adjacency[index], site) && !convexFaceExtList.Contains(face.Adjacency[index]))
              convexFaceExtStack.Push(face.Adjacency[index]);
          }
        }
      }
      return convexFaceExtList;
    }

    private List<Site> GenerateNeighbors(
      PowerDiagram.DualSite3d dualSite,
      PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> startingFace)
    {
      List<Site> siteList = new List<Site>();
      List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> convexFaceExtList = new List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>>();
      Stack<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> convexFaceExtStack = new Stack<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>>();
      convexFaceExtStack.Push(startingFace);
      while (convexFaceExtStack.Count > 0)
      {
        PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> face0 = convexFaceExtStack.Pop();
        convexFaceExtList.Add(face0);
        for (int index = 0; index < face0.Adjacency.Length; ++index)
        {
          if (this.ContainsVert(face0.Adjacency[index], dualSite) && !convexFaceExtList.Contains(face0.Adjacency[index]))
          {
            PowerDiagram.Edge edge = this.GetEdge(face0, face0.Adjacency[index]);
            PowerDiagram.DualSite3d dualSite3d = edge.First == dualSite ? edge.Second : edge.First;
            Debug.Assert(dualSite3d != dualSite, (object) "We're our own neighbour??");
            Debug.Assert(dualSite3d.site.id == -1 || !siteList.Contains(dualSite3d.site), (object) "Tried adding a site twice!");
            siteList.Add(dualSite3d.site);
            convexFaceExtStack.Push(face0.Adjacency[index]);
          }
        }
      }
      return siteList;
    }

    private void ComputePD()
    {
      List<PowerDiagram.DualSite3d> dual3dSites = new List<PowerDiagram.DualSite3d>();
      foreach (Site site in this.sites)
        dual3dSites.Add(site.ToDualSite());
      for (int index = 0; index < this.externalEdgePoints.Count; ++index)
        dual3dSites.Add(this.externalEdgePoints[index].ToDualSite());
      this.CheckPositions(dual3dSites);
      ConvexHull<PowerDiagram.DualSite3d, PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> hull = PowerDiagram.CreateHull((IList<PowerDiagram.DualSite3d>) dual3dSites);
      foreach (PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> face in hull.Faces)
      {
        if (face.Normal[2] < -(double) Mathf.Epsilon)
        {
          foreach (PowerDiagram.DualSite3d vertex in face.Vertices)
          {
            if (!vertex.site.dummy && !vertex.visited)
            {
              vertex.visited = true;
              List<Vector2> verts = new List<Vector2>();
              List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> convexFaceExtList = this.TouchingFaces(vertex, face);
              vertex.site.neighbours = this.GenerateNeighbors(vertex, face);
              foreach (PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> convexFaceExt in convexFaceExtList)
              {
                Vector2 dualPoint = convexFaceExt.GetDualPoint();
                verts.Add(dualPoint);
              }
              Polygon polygon1 = PowerDiagram.PolyForRandomPoints(verts);
              Polygon polygon2 = polygon1.Clip(this.bounds);
              if (polygon2 == null)
              {
                polygon1.DebugDraw(Color.yellow);
                vertex.site.poly.DebugDraw(Color.black, inset: 2f);
                DebugExtension.DebugCircle2d(vertex.site.position, Color.magenta, 5f, jumpPerSegment: 20f);
              }
              else
                vertex.site.poly = polygon2;
            }
          }
        }
      }
      this.debug_LastHull = hull;
    }

    private void UpdateWeights(List<Site> sites)
    {
      foreach (Site site in sites)
      {
        site.position = site.poly != null ? site.poly.Centroid() : throw new Exception("site poly is null for [" + (object) site.id + "]" + (object) site.position);
        site.currentWeight = Mathf.Max(site.currentWeight, 1f);
      }
      float num1 = 0.0f;
      foreach (Site site in sites)
      {
        float num2 = site.poly == null ? 0.1f : site.poly.Area();
        float num3 = site.weight / this.weightSum * this.bounds.Area();
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
        foreach (Site site in sites)
          site.currentWeight += num2 + 1f;
      }
      float num6 = 1f;
      foreach (Site site in sites)
      {
        foreach (Site neighbour in site.neighbours)
        {
          float num2 = (site.position - neighbour.position).sqrMagnitude / (Mathf.Abs(site.currentWeight - neighbour.currentWeight) + 1f);
          if ((double) num2 < (double) num6)
            num6 = num2;
        }
      }
      foreach (Site site in sites)
        site.currentWeight *= num6;
    }

    private List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> GetNeigborFaces(
      PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> currentFace)
    {
      List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> convexFaceExtList = new List<PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>>();
      for (int index = 0; index < currentFace.Adjacency.Length; ++index)
      {
        PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d> convexFaceExt = currentFace.Adjacency[index];
        if (convexFaceExt != null)
          convexFaceExtList.Add(convexFaceExt);
      }
      return convexFaceExtList;
    }

    private void CheckPositions(List<PowerDiagram.DualSite3d> dual3dSites)
    {
      for (int index1 = 0; index1 < dual3dSites.Count; ++index1)
      {
        if (!dual3dSites[index1].site.dummy)
        {
          Debug.Assert((double) dual3dSites[index1].site.currentWeight != 0.0);
          for (int index2 = index1 + 1; index2 < dual3dSites.Count; ++index2)
          {
            if (!dual3dSites[index2].site.dummy && dual3dSites[index1].coord == dual3dSites[index2].coord)
              dual3dSites[index2].coord += new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, 0.0f);
          }
        }
      }
    }

    public static Polygon PolyForRandomPoints(List<Vector2> verts)
    {
      double[][] numArray = new double[verts.Count][];
      for (int index = 0; index < verts.Count; ++index)
        numArray[index] = new double[2]
        {
          (double) verts[index].x,
          (double) verts[index].y
        };
      double[][] array = ConvexHull.Create((IList<double[]>) numArray).Points.Select<DefaultVertex, double[]>((Func<DefaultVertex, double[]>) (p => p.Position)).ToArray<double[]>();
      Polygon polygon = new Polygon();
      for (int index = 0; index < array.Length; ++index)
        polygon.Add(new Vector2((float) array[index][0], (float) array[index][1]));
      polygon.Initialize();
      polygon.ForceWinding(Winding.COUNTERCLOCKWISE);
      return polygon;
    }

    public static ConvexHull<PowerDiagram.DualSite3d, PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>> CreateHull(
      IList<PowerDiagram.DualSite3d> data,
      double PlaneDistanceTolerance = 1E-10)
    {
      return ConvexHull<PowerDiagram.DualSite3d, PowerDiagram.ConvexFaceExt<PowerDiagram.DualSite3d>>.Create(data, PlaneDistanceTolerance);
    }

    private class Edge : MathUtil.Pair<PowerDiagram.DualSite3d, PowerDiagram.DualSite3d>
    {
      public Edge(PowerDiagram.DualSite3d first, PowerDiagram.DualSite3d second)
      {
        this.First = first;
        this.Second = second;
      }
    }

    public class ConvexFaceExt<TVertex> : ConvexFace<TVertex, PowerDiagram.ConvexFaceExt<TVertex>>
      where TVertex : IVertex
    {
      private Site site;
      private Vector2 dualPoint;
      private Vector2? circumCenter;

      public TVertex vertex0 => this.Vertices[0];

      public TVertex vertex1 => this.Vertices[1];

      public TVertex vertex2 => this.Vertices[2];

      public PowerDiagram.ConvexFaceExt<TVertex> edge0 => this.Adjacency[0];

      public PowerDiagram.ConvexFaceExt<TVertex> edge1 => this.Adjacency[1];

      public PowerDiagram.ConvexFaceExt<TVertex> edge2 => this.Adjacency[2];

      public Vector2 GetDualPoint()
      {
        if ((double) this.dualPoint.x == 0.0 && (double) this.dualPoint.y == 0.0)
        {
          Vector3 vector3_1;
          ref Vector3 local1 = ref vector3_1;
          TVertex vertex = this.vertex0;
          double num1 = vertex.Position[0];
          vertex = this.vertex0;
          double num2 = vertex.Position[1];
          vertex = this.vertex0;
          double num3 = vertex.Position[2];
          local1 = new Vector3((float) num1, (float) num2, (float) num3);
          Vector3 vector3_2;
          ref Vector3 local2 = ref vector3_2;
          vertex = this.vertex1;
          double num4 = vertex.Position[0];
          vertex = this.vertex1;
          double num5 = vertex.Position[1];
          vertex = this.vertex1;
          double num6 = vertex.Position[2];
          local2 = new Vector3((float) num4, (float) num5, (float) num6);
          Vector3 vector3_3;
          ref Vector3 local3 = ref vector3_3;
          vertex = this.vertex2;
          double num7 = vertex.Position[0];
          vertex = this.vertex2;
          double num8 = vertex.Position[1];
          vertex = this.vertex2;
          double num9 = vertex.Position[2];
          local3 = new Vector3((float) num7, (float) num8, (float) num9);
          double num10 = (double) vector3_1.y * ((double) vector3_2.z - (double) vector3_3.z) + (double) vector3_2.y * ((double) vector3_3.z - (double) vector3_1.z) + (double) vector3_3.y * ((double) vector3_1.z - (double) vector3_2.z);
          double num11 = (double) vector3_1.z * ((double) vector3_2.x - (double) vector3_3.x) + (double) vector3_2.z * ((double) vector3_3.x - (double) vector3_1.x) + (double) vector3_3.z * ((double) vector3_1.x - (double) vector3_2.x);
          double num12 = -0.5 / ((double) vector3_1.x * ((double) vector3_2.y - (double) vector3_3.y) + (double) vector3_2.x * ((double) vector3_3.y - (double) vector3_1.y) + (double) vector3_3.x * ((double) vector3_1.y - (double) vector3_2.y));
          this.dualPoint = new Vector2((float) (num10 * num12), (float) (num11 * num12));
        }
        return this.dualPoint;
      }

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
        TVertex[] vertices = this.Vertices;
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

      public Vector2 Circumcenter
      {
        get
        {
          this.circumCenter = new Vector2?(this.circumCenter ?? this.GetCircumcenter());
          return this.circumCenter.Value;
        }
      }
    }

    public class TriangulationCellExt<TVertex> : TriangulationCell<TVertex, PowerDiagram.TriangulationCellExt<TVertex>>
      where TVertex : IVertex
    {
      public TVertex Vertex0 => this.Vertices[0];

      public TVertex Vertex1 => this.Vertices[1];

      public TVertex Vertex2 => this.Vertices[2];

      public PowerDiagram.TriangulationCellExt<TVertex> Edge0 => this.Adjacency[0];

      public PowerDiagram.TriangulationCellExt<TVertex> Edge1 => this.Adjacency[1];

      public PowerDiagram.TriangulationCellExt<TVertex> Edge2 => this.Adjacency[2];
    }

    public class DualSite2d : IVertex
    {
      public double[] Position => new double[2]
      {
        (double) this.site.position[0],
        (double) this.site.position[1]
      };

      public Site site { get; set; }

      public bool visited { get; set; }

      public DualSite2d(Site site)
      {
        this.site = site;
        this.visited = false;
      }
    }

    public class DualSite3d : IVertex
    {
      public double[] Position => new double[3]
      {
        (double) this.coord[0],
        (double) this.coord[1],
        (double) this.coord[2]
      };

      public Vector3 coord { get; set; }

      public Site site { get; set; }

      public bool visited { get; set; }

      public DualSite3d()
        : this(0.0, 0.0, 0.0)
      {
      }

      public DualSite3d(double _x, double _y, double _z)
      {
        this.coord = new Vector3((float) _x, (float) _y, (float) _z);
        this.visited = false;
      }

      public DualSite3d(Vector3 pos)
      {
        this.coord = pos;
        this.visited = false;
      }

      public DualSite3d(double _x, double _y, double _z, Site _originalSite)
        : this(_x, _y, _z)
      {
        this.site = _originalSite;
        this.visited = false;
      }
    }
  }
}
