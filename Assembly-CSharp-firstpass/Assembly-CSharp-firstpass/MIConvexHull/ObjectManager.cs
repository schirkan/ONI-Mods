// Decompiled with JetBrains decompiler
// Type: MIConvexHull.ObjectManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace MIConvexHull
{
  internal class ObjectManager
  {
    private readonly int Dimension;
    private FaceConnector ConnectorStack;
    private readonly SimpleList<DeferredFace> DeferredFaceStack;
    private readonly SimpleList<IndexBuffer> EmptyBufferStack;
    private ConvexFaceInternal[] FacePool;
    private int FacePoolSize;
    private int FacePoolCapacity;
    private readonly IndexBuffer FreeFaceIndices;
    private readonly ConvexHullAlgorithm Hull;

    public ObjectManager(ConvexHullAlgorithm hull)
    {
      this.Dimension = hull.NumOfDimensions;
      this.Hull = hull;
      this.FacePool = hull.FacePool;
      this.FacePoolSize = 0;
      this.FacePoolCapacity = hull.FacePool.Length;
      this.FreeFaceIndices = new IndexBuffer();
      this.EmptyBufferStack = new SimpleList<IndexBuffer>();
      this.DeferredFaceStack = new SimpleList<DeferredFace>();
    }

    public void DepositFace(int faceIndex)
    {
      foreach (int num in this.FacePool[faceIndex].AdjacentFaces)
        num = -1;
      this.FreeFaceIndices.Push(faceIndex);
    }

    private void ReallocateFacePool()
    {
      ConvexFaceInternal[] convexFaceInternalArray = new ConvexFaceInternal[2 * this.FacePoolCapacity];
      bool[] flagArray = new bool[2 * this.FacePoolCapacity];
      Array.Copy((Array) this.FacePool, (Array) convexFaceInternalArray, this.FacePoolCapacity);
      Buffer.BlockCopy((Array) this.Hull.AffectedFaceFlags, 0, (Array) flagArray, 0, this.FacePoolCapacity);
      this.FacePoolCapacity = 2 * this.FacePoolCapacity;
      this.Hull.FacePool = convexFaceInternalArray;
      this.FacePool = convexFaceInternalArray;
      this.Hull.AffectedFaceFlags = flagArray;
    }

    private int CreateFace()
    {
      int facePoolSize = this.FacePoolSize;
      ConvexFaceInternal convexFaceInternal = new ConvexFaceInternal(this.Dimension, facePoolSize, this.GetVertexBuffer());
      ++this.FacePoolSize;
      if (this.FacePoolSize > this.FacePoolCapacity)
        this.ReallocateFacePool();
      this.FacePool[facePoolSize] = convexFaceInternal;
      return facePoolSize;
    }

    public int GetFace() => this.FreeFaceIndices.Count > 0 ? this.FreeFaceIndices.Pop() : this.CreateFace();

    public void DepositConnector(FaceConnector connector)
    {
      if (this.ConnectorStack == null)
      {
        connector.Next = (FaceConnector) null;
        this.ConnectorStack = connector;
      }
      else
      {
        connector.Next = this.ConnectorStack;
        this.ConnectorStack = connector;
      }
    }

    public FaceConnector GetConnector()
    {
      if (this.ConnectorStack == null)
        return new FaceConnector(this.Dimension);
      FaceConnector connectorStack = this.ConnectorStack;
      this.ConnectorStack = this.ConnectorStack.Next;
      connectorStack.Next = (FaceConnector) null;
      return connectorStack;
    }

    public void DepositVertexBuffer(IndexBuffer buffer)
    {
      buffer.Clear();
      this.EmptyBufferStack.Push(buffer);
    }

    public IndexBuffer GetVertexBuffer() => this.EmptyBufferStack.Count == 0 ? new IndexBuffer() : this.EmptyBufferStack.Pop();

    public void DepositDeferredFace(DeferredFace face) => this.DeferredFaceStack.Push(face);

    public DeferredFace GetDeferredFace() => this.DeferredFaceStack.Count == 0 ? new DeferredFace() : this.DeferredFaceStack.Pop();
  }
}
