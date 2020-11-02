// Decompiled with JetBrains decompiler
// Type: Delaunay.EdgeList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Utils;
using UnityEngine;

namespace Delaunay
{
  internal sealed class EdgeList : IDisposable
  {
    private float _deltax;
    private float _xmin;
    private int _hashsize;
    private Halfedge[] _hash;
    private Halfedge _leftEnd;
    private Halfedge _rightEnd;

    public Halfedge leftEnd => this._leftEnd;

    public Halfedge rightEnd => this._rightEnd;

    public void Dispose()
    {
      Halfedge halfedge1 = this._leftEnd;
      while (halfedge1 != this._rightEnd)
      {
        Halfedge halfedge2 = halfedge1;
        halfedge1 = halfedge1.edgeListRightNeighbor;
        halfedge2.Dispose();
      }
      this._leftEnd = (Halfedge) null;
      this._rightEnd.Dispose();
      this._rightEnd = (Halfedge) null;
      for (int index = 0; index < this._hashsize; ++index)
        this._hash[index] = (Halfedge) null;
      this._hash = (Halfedge[]) null;
    }

    public EdgeList(float xmin, float deltax, int sqrt_nsites)
    {
      this._xmin = xmin;
      this._deltax = deltax;
      this._hashsize = 2 * sqrt_nsites;
      this._hash = new Halfedge[this._hashsize];
      this._leftEnd = Halfedge.CreateDummy();
      this._rightEnd = Halfedge.CreateDummy();
      this._leftEnd.edgeListLeftNeighbor = (Halfedge) null;
      this._leftEnd.edgeListRightNeighbor = this._rightEnd;
      this._rightEnd.edgeListLeftNeighbor = this._leftEnd;
      this._rightEnd.edgeListRightNeighbor = (Halfedge) null;
      this._hash[0] = this._leftEnd;
      this._hash[this._hashsize - 1] = this._rightEnd;
    }

    public void Insert(Halfedge lb, Halfedge newHalfedge)
    {
      newHalfedge.edgeListLeftNeighbor = lb;
      newHalfedge.edgeListRightNeighbor = lb.edgeListRightNeighbor;
      lb.edgeListRightNeighbor.edgeListLeftNeighbor = newHalfedge;
      lb.edgeListRightNeighbor = newHalfedge;
    }

    public void Remove(Halfedge halfEdge)
    {
      halfEdge.edgeListLeftNeighbor.edgeListRightNeighbor = halfEdge.edgeListRightNeighbor;
      halfEdge.edgeListRightNeighbor.edgeListLeftNeighbor = halfEdge.edgeListLeftNeighbor;
      halfEdge.edge = Edge.DELETED;
      halfEdge.edgeListLeftNeighbor = halfEdge.edgeListRightNeighbor = (Halfedge) null;
    }

    public Halfedge EdgeListLeftNeighbor(Vector2 p)
    {
      int b = (int) (((double) p.x - (double) this._xmin) / (double) this._deltax * (double) this._hashsize);
      if (b < 0)
        b = 0;
      if (b >= this._hashsize)
        b = this._hashsize - 1;
      Halfedge halfedge = this.GetHash(b);
      if (halfedge == null)
      {
        int num = 1;
        while ((halfedge = this.GetHash(b - num)) == null && (halfedge = this.GetHash(b + num)) == null)
          ++num;
      }
      if (halfedge == this.leftEnd || halfedge != this.rightEnd && halfedge.IsLeftOf(p))
      {
        do
        {
          halfedge = halfedge.edgeListRightNeighbor;
        }
        while (halfedge != this.rightEnd && halfedge.IsLeftOf(p));
        halfedge = halfedge.edgeListLeftNeighbor;
      }
      else
      {
        do
        {
          halfedge = halfedge.edgeListLeftNeighbor;
        }
        while (halfedge != this.leftEnd && !halfedge.IsLeftOf(p));
      }
      if (b > 0 && b < this._hashsize - 1)
        this._hash[b] = halfedge;
      return halfedge;
    }

    private Halfedge GetHash(int b)
    {
      if (b < 0 || b >= this._hashsize)
        return (Halfedge) null;
      Halfedge halfedge = this._hash[b];
      if (halfedge == null || halfedge.edge != Edge.DELETED)
        return halfedge;
      this._hash[b] = (Halfedge) null;
      return (Halfedge) null;
    }
  }
}
