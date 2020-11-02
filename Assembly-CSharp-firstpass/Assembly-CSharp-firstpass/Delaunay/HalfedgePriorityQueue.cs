// Decompiled with JetBrains decompiler
// Type: Delaunay.HalfedgePriorityQueue
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Utils;
using UnityEngine;

namespace Delaunay
{
  internal sealed class HalfedgePriorityQueue : IDisposable
  {
    private Halfedge[] _hash;
    private int _count;
    private int _minBucket;
    private int _hashsize;
    private float _ymin;
    private float _deltay;

    public HalfedgePriorityQueue(float ymin, float deltay, int sqrt_nsites)
    {
      this._ymin = ymin;
      this._deltay = deltay;
      this._hashsize = 4 * sqrt_nsites;
      this.Initialize();
    }

    public void Dispose()
    {
      for (int index = 0; index < this._hashsize; ++index)
      {
        this._hash[index].Dispose();
        this._hash[index] = (Halfedge) null;
      }
      this._hash = (Halfedge[]) null;
    }

    private void Initialize()
    {
      this._count = 0;
      this._minBucket = 0;
      this._hash = new Halfedge[this._hashsize];
      for (int index = 0; index < this._hashsize; ++index)
      {
        this._hash[index] = Halfedge.CreateDummy();
        this._hash[index].nextInPriorityQueue = (Halfedge) null;
      }
    }

    public void Insert(Halfedge halfEdge)
    {
      int index = this.Bucket(halfEdge);
      if (index < this._minBucket)
        this._minBucket = index;
      Halfedge halfedge = this._hash[index];
      Halfedge nextInPriorityQueue;
      while ((nextInPriorityQueue = halfedge.nextInPriorityQueue) != null && ((double) halfEdge.ystar > (double) nextInPriorityQueue.ystar || (double) halfEdge.ystar == (double) nextInPriorityQueue.ystar && (double) halfEdge.vertex.x > (double) nextInPriorityQueue.vertex.x))
        halfedge = nextInPriorityQueue;
      halfEdge.nextInPriorityQueue = halfedge.nextInPriorityQueue;
      halfedge.nextInPriorityQueue = halfEdge;
      ++this._count;
    }

    public void Remove(Halfedge halfEdge)
    {
      int index = this.Bucket(halfEdge);
      if (halfEdge.vertex == null)
        return;
      Halfedge nextInPriorityQueue = this._hash[index];
      while (nextInPriorityQueue.nextInPriorityQueue != halfEdge)
        nextInPriorityQueue = nextInPriorityQueue.nextInPriorityQueue;
      nextInPriorityQueue.nextInPriorityQueue = halfEdge.nextInPriorityQueue;
      --this._count;
      halfEdge.vertex = (Vertex) null;
      halfEdge.nextInPriorityQueue = (Halfedge) null;
      halfEdge.Dispose();
    }

    private int Bucket(Halfedge halfEdge)
    {
      int num = (int) (((double) halfEdge.ystar - (double) this._ymin) / (double) this._deltay * (double) this._hashsize);
      if (num < 0)
        num = 0;
      if (num >= this._hashsize)
        num = this._hashsize - 1;
      return num;
    }

    private bool IsEmpty(int bucket) => this._hash[bucket].nextInPriorityQueue == null;

    private void AdjustMinBucket()
    {
      while (this._minBucket < this._hashsize - 1 && this.IsEmpty(this._minBucket))
        ++this._minBucket;
    }

    public bool Empty() => this._count == 0;

    public Vector2 Min()
    {
      this.AdjustMinBucket();
      Halfedge nextInPriorityQueue = this._hash[this._minBucket].nextInPriorityQueue;
      return new Vector2(nextInPriorityQueue.vertex.x, nextInPriorityQueue.ystar);
    }

    public Halfedge ExtractMin()
    {
      Halfedge nextInPriorityQueue = this._hash[this._minBucket].nextInPriorityQueue;
      this._hash[this._minBucket].nextInPriorityQueue = nextInPriorityQueue.nextInPriorityQueue;
      --this._count;
      nextInPriorityQueue.nextInPriorityQueue = (Halfedge) null;
      return nextInPriorityQueue;
    }
  }
}
