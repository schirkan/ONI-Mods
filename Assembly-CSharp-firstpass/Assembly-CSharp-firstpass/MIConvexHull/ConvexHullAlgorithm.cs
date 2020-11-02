// Decompiled with JetBrains decompiler
// Type: MIConvexHull.ConvexHullAlgorithm
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace MIConvexHull
{
  internal class ConvexHullAlgorithm
  {
    internal readonly int NumOfDimensions;
    private readonly bool IsLifted;
    private readonly double PlaneDistanceTolerance;
    private readonly IVertex[] Vertices;
    private double[] Positions;
    private readonly bool[] VertexVisited;
    private readonly int NumberOfVertices;
    internal ConvexFaceInternal[] FacePool;
    internal bool[] AffectedFaceFlags;
    private int ConvexHullSize;
    private readonly FaceList UnprocessedFaces;
    private readonly IndexBuffer ConvexFaces;
    private int CurrentVertex;
    private double MaxDistance;
    private int FurthestVertex;
    private readonly double[] Center;
    private readonly int[] UpdateBuffer;
    private readonly int[] UpdateIndices;
    private readonly IndexBuffer TraverseStack;
    private readonly IndexBuffer EmptyBuffer;
    private IndexBuffer BeyondBuffer;
    private readonly IndexBuffer AffectedFaceBuffer;
    private readonly SimpleList<DeferredFace> ConeFaceBuffer;
    private readonly HashSet<int> SingularVertices;
    private readonly ConnectorList[] ConnectorTable;
    private readonly ObjectManager ObjectManager;
    private readonly MathHelper mathHelper;
    private readonly List<int>[] boundingBoxPoints;
    private int indexOfDimensionWithLeastExtremes;
    private readonly double[] minima;
    private readonly double[] maxima;

    internal static ConvexHull<TVertex, TFace> GetConvexHull<TVertex, TFace>(
      IList<TVertex> data,
      double PlaneDistanceTolerance)
      where TVertex : IVertex
      where TFace : ConvexFace<TVertex, TFace>, new()
    {
      ConvexHullAlgorithm convexHullAlgorithm = new ConvexHullAlgorithm(data.Cast<IVertex>().ToArray<IVertex>(), false, PlaneDistanceTolerance);
      convexHullAlgorithm.GetConvexHull();
      if (convexHullAlgorithm.NumOfDimensions == 2)
        return convexHullAlgorithm.Return2DResultInOrder<TVertex, TFace>(data);
      return new ConvexHull<TVertex, TFace>()
      {
        Points = (IEnumerable<TVertex>) convexHullAlgorithm.GetHullVertices<TVertex>(data),
        Faces = (IEnumerable<TFace>) convexHullAlgorithm.GetConvexFaces<TVertex, TFace>()
      };
    }

    private ConvexHullAlgorithm(IVertex[] vertices, bool lift, double PlaneDistanceTolerance)
    {
      this.IsLifted = lift;
      this.Vertices = vertices;
      this.NumberOfVertices = vertices.Length;
      this.NumOfDimensions = this.DetermineDimension();
      if (this.IsLifted)
        ++this.NumOfDimensions;
      if (this.NumOfDimensions < 2)
        throw new InvalidOperationException("Dimension of the input must be 2 or greater.");
      if (this.NumberOfVertices <= this.NumOfDimensions)
        throw new ArgumentException("There are too few vertices (m) for the n-dimensional space. (m must be greater than the n, but m is " + (object) this.NumberOfVertices + " and n is " + (object) this.NumOfDimensions);
      this.PlaneDistanceTolerance = PlaneDistanceTolerance;
      this.UnprocessedFaces = new FaceList();
      this.ConvexFaces = new IndexBuffer();
      this.FacePool = new ConvexFaceInternal[(this.NumOfDimensions + 1) * 10];
      this.AffectedFaceFlags = new bool[(this.NumOfDimensions + 1) * 10];
      this.ObjectManager = new ObjectManager(this);
      this.Center = new double[this.NumOfDimensions];
      this.TraverseStack = new IndexBuffer();
      this.UpdateBuffer = new int[this.NumOfDimensions];
      this.UpdateIndices = new int[this.NumOfDimensions];
      this.EmptyBuffer = new IndexBuffer();
      this.AffectedFaceBuffer = new IndexBuffer();
      this.ConeFaceBuffer = new SimpleList<DeferredFace>();
      this.SingularVertices = new HashSet<int>();
      this.BeyondBuffer = new IndexBuffer();
      this.ConnectorTable = new ConnectorList[2017];
      for (int index = 0; index < 2017; ++index)
        this.ConnectorTable[index] = new ConnectorList();
      this.VertexVisited = new bool[this.NumberOfVertices];
      this.Positions = new double[this.NumberOfVertices * this.NumOfDimensions];
      this.boundingBoxPoints = new List<int>[this.NumOfDimensions];
      this.minima = new double[this.NumOfDimensions];
      this.maxima = new double[this.NumOfDimensions];
      this.mathHelper = new MathHelper(this.NumOfDimensions, this.Positions);
    }

    private int DetermineDimension()
    {
      Random random = new Random();
      List<int> source = new List<int>();
      for (int index = 0; index < 10; ++index)
        source.Add(this.Vertices[random.Next(this.NumberOfVertices)].Position.Length);
      int num = source.Min();
      return num == source.Max() ? num : throw new ArgumentException("Invalid input data (non-uniform dimension).");
    }

    private void GetConvexHull()
    {
      this.SerializeVerticesToPositions();
      this.FindBoundingBoxPoints();
      this.ShiftAndScalePositions();
      this.CreateInitialSimplex();
      while (this.UnprocessedFaces.First != null)
      {
        ConvexFaceInternal first = this.UnprocessedFaces.First;
        this.CurrentVertex = first.FurthestVertex;
        this.UpdateCenter();
        this.TagAffectedFaces(first);
        if (!this.SingularVertices.Contains(this.CurrentVertex) && this.CreateCone())
          this.CommitCone();
        else
          this.HandleSingular();
        int count = this.AffectedFaceBuffer.Count;
        for (int i = 0; i < count; ++i)
          this.AffectedFaceFlags[this.AffectedFaceBuffer[i]] = false;
      }
    }

    private void SerializeVerticesToPositions()
    {
      int num1 = 0;
      if (this.IsLifted)
      {
        foreach (IVertex vertex in this.Vertices)
        {
          double num2 = 0.0;
          int num3 = this.NumOfDimensions - 1;
          for (int index = 0; index < num3; ++index)
          {
            double num4 = vertex.Position[index];
            this.Positions[num1++] = num4;
            num2 += num4 * num4;
          }
          this.Positions[num1++] = num2;
        }
      }
      else
      {
        foreach (IVertex vertex in this.Vertices)
        {
          for (int index = 0; index < this.NumOfDimensions; ++index)
            this.Positions[num1++] = vertex.Position[index];
        }
      }
    }

    private void FindBoundingBoxPoints()
    {
      this.indexOfDimensionWithLeastExtremes = -1;
      int num1 = int.MaxValue;
      for (int i = 0; i < this.NumOfDimensions; i++)
      {
        List<int> intList1 = new List<int>();
        List<int> intList2 = new List<int>();
        double min = double.PositiveInfinity;
        double num2 = double.NegativeInfinity;
        for (int vIndex = 0; vIndex < this.NumberOfVertices; ++vIndex)
        {
          double coordinate = this.GetCoordinate(vIndex, i);
          double num3 = min - coordinate;
          if (num3 >= this.PlaneDistanceTolerance)
          {
            min = coordinate;
            intList1.Clear();
            intList1.Add(vIndex);
          }
          else if (num3 > 0.0)
          {
            min = coordinate;
            intList1.RemoveAll((Predicate<int>) (index => min - this.GetCoordinate(index, i) > this.PlaneDistanceTolerance));
            intList1.Add(vIndex);
          }
          else if (num3 > -this.PlaneDistanceTolerance)
            intList1.Add(vIndex);
          double num4 = coordinate - num2;
          if (num4 >= this.PlaneDistanceTolerance)
          {
            num2 = coordinate;
            intList2.Clear();
            intList2.Add(vIndex);
          }
          else if (num4 > 0.0)
          {
            num2 = coordinate;
            intList2.RemoveAll((Predicate<int>) (index => min - this.GetCoordinate(index, i) > this.PlaneDistanceTolerance));
            intList2.Add(vIndex);
          }
          else if (num4 > -this.PlaneDistanceTolerance)
            intList2.Add(vIndex);
        }
        this.minima[i] = min;
        this.maxima[i] = num2;
        intList1.AddRange((IEnumerable<int>) intList2);
        if (intList1.Count < num1)
        {
          num1 = intList1.Count;
          this.indexOfDimensionWithLeastExtremes = i;
        }
        this.boundingBoxPoints[i] = intList1;
      }
    }

    private void ShiftAndScalePositions()
    {
      int length = this.Positions.Length;
      if (this.IsLifted)
      {
        int index1 = this.NumOfDimensions - 1;
        double num = 2.0 / (((IEnumerable<double>) this.minima).Sum<double>((Func<double, double>) (x => Math.Abs(x))) + ((IEnumerable<double>) this.maxima).Sum<double>((Func<double, double>) (x => Math.Abs(x))) - Math.Abs(this.maxima[index1]) - Math.Abs(this.minima[index1]));
        this.minima[index1] *= num;
        this.maxima[index1] *= num;
        for (int index2 = index1; index2 < length; index2 += this.NumOfDimensions)
          this.Positions[index2] *= num;
      }
      double[] numArray = new double[this.NumOfDimensions];
      for (int index = 0; index < this.NumOfDimensions; ++index)
        numArray[index] = this.maxima[index] != this.minima[index] ? this.maxima[index] - this.minima[index] - this.minima[index] : 0.0;
      for (int index = 0; index < length; ++index)
        this.Positions[index] += numArray[index % this.NumOfDimensions];
    }

    private void CreateInitialSimplex()
    {
      List<int> initialPoints = this.FindInitialPoints();
      int[] numArray = new int[this.NumOfDimensions + 1];
      for (int index1 = 0; index1 < this.NumOfDimensions + 1; ++index1)
      {
        int[] array = new int[this.NumOfDimensions];
        int index2 = 0;
        int num1 = 0;
        for (; index2 <= this.NumOfDimensions; ++index2)
        {
          if (index1 != index2)
          {
            if (index2 == initialPoints.Count)
              ;
            int num2 = initialPoints[index2];
            array[num1++] = num2;
          }
        }
        ConvexFaceInternal face = this.FacePool[this.ObjectManager.GetFace()];
        face.Vertices = array;
        Array.Sort<int>(array);
        this.mathHelper.CalculateFacePlane(face, this.Center);
        numArray[index1] = face.Index;
      }
      for (int index1 = 0; index1 < this.NumOfDimensions; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.NumOfDimensions + 1; ++index2)
          this.UpdateAdjacency(this.FacePool[numArray[index1]], this.FacePool[numArray[index2]]);
      }
      foreach (int index in numArray)
      {
        ConvexFaceInternal face = this.FacePool[index];
        this.FindBeyondVertices(face);
        if (face.VerticesBeyond.Count == 0)
          this.ConvexFaces.Add(face.Index);
        else
          this.UnprocessedFaces.Add(face);
      }
      foreach (int index in initialPoints)
        this.VertexVisited[index] = false;
    }

    private List<int> FindInitialPoints()
    {
      double bigNumber = ((IEnumerable<double>) this.maxima).Sum() * (double) this.NumOfDimensions * (double) this.NumberOfVertices;
      int fromIndex = this.boundingBoxPoints[this.indexOfDimensionWithLeastExtremes].First<int>();
      int toIndex1 = this.boundingBoxPoints[this.indexOfDimensionWithLeastExtremes].Last<int>();
      this.boundingBoxPoints[this.indexOfDimensionWithLeastExtremes].RemoveAt(0);
      this.boundingBoxPoints[this.indexOfDimensionWithLeastExtremes].RemoveAt(this.boundingBoxPoints[this.indexOfDimensionWithLeastExtremes].Count - 1);
      List<int> intList = new List<int>()
      {
        fromIndex,
        toIndex1
      };
      this.VertexVisited[fromIndex] = this.VertexVisited[toIndex1] = true;
      this.CurrentVertex = fromIndex;
      this.UpdateCenter();
      this.CurrentVertex = toIndex1;
      this.UpdateCenter();
      double[][] edgeVectors = new double[this.NumOfDimensions][];
      edgeVectors[0] = this.mathHelper.VectorBetweenVertices(toIndex1, fromIndex);
      List<int> list1 = ((IEnumerable<List<int>>) this.boundingBoxPoints).SelectMany<List<int>, int>((Func<List<int>, IEnumerable<int>>) (x => (IEnumerable<int>) x)).ToList<int>();
      int lastIndex = 1;
      while (lastIndex < this.NumOfDimensions && list1.Any<int>())
      {
        int num1 = -1;
        double[] numArray = new double[0];
        double num2 = 0.0;
        for (int index = list1.Count - 1; index >= 0; --index)
        {
          int toIndex2 = list1[index];
          if (intList.Contains(toIndex2))
          {
            list1.RemoveAt(index);
          }
          else
          {
            edgeVectors[lastIndex] = this.mathHelper.VectorBetweenVertices(toIndex2, fromIndex);
            double simplexVolume = this.mathHelper.GetSimplexVolume(edgeVectors, lastIndex, bigNumber);
            if (num2 < simplexVolume)
            {
              num2 = simplexVolume;
              num1 = toIndex2;
              numArray = edgeVectors[lastIndex];
            }
          }
        }
        list1.Remove(num1);
        if (num1 != -1)
        {
          intList.Add(num1);
          edgeVectors[lastIndex++] = numArray;
          this.CurrentVertex = num1;
          this.UpdateCenter();
        }
        else
          break;
      }
      if (intList.Count <= this.NumOfDimensions)
      {
        List<int> list2 = Enumerable.Range(0, this.NumberOfVertices).ToList<int>();
        while (lastIndex < this.NumOfDimensions && list2.Any<int>())
        {
          int num1 = -1;
          double[] numArray = new double[0];
          double num2 = 0.0;
          for (int index = list2.Count - 1; index >= 0; --index)
          {
            int toIndex2 = list2[index];
            if (intList.Contains(toIndex2))
            {
              list2.RemoveAt(index);
            }
            else
            {
              edgeVectors[lastIndex] = this.mathHelper.VectorBetweenVertices(toIndex2, fromIndex);
              double simplexVolume = this.mathHelper.GetSimplexVolume(edgeVectors, lastIndex, bigNumber);
              if (num2 < simplexVolume)
              {
                num2 = simplexVolume;
                num1 = toIndex2;
                numArray = edgeVectors[lastIndex];
              }
            }
          }
          list2.Remove(num1);
          if (num1 != -1)
          {
            intList.Add(num1);
            edgeVectors[lastIndex++] = numArray;
            this.CurrentVertex = num1;
            this.UpdateCenter();
          }
          else
            break;
        }
      }
      if (intList.Count <= this.NumOfDimensions && this.IsLifted)
      {
        List<int> list2 = Enumerable.Range(0, this.NumberOfVertices).ToList<int>();
        while (lastIndex < this.NumOfDimensions && list2.Any<int>())
        {
          int num1 = -1;
          double[] numArray = new double[0];
          double num2 = 0.0;
          for (int index = list2.Count - 1; index >= 0; --index)
          {
            int num3 = list2[index];
            if (intList.Contains(num3))
            {
              list2.RemoveAt(index);
            }
            else
            {
              this.mathHelper.RandomOffsetToLift(num3);
              edgeVectors[lastIndex] = this.mathHelper.VectorBetweenVertices(num3, fromIndex);
              double simplexVolume = this.mathHelper.GetSimplexVolume(edgeVectors, lastIndex, bigNumber);
              if (num2 < simplexVolume)
              {
                num2 = simplexVolume;
                num1 = num3;
                numArray = edgeVectors[lastIndex];
              }
            }
          }
          list2.Remove(num1);
          if (num1 != -1)
          {
            intList.Add(num1);
            edgeVectors[lastIndex++] = numArray;
            this.CurrentVertex = num1;
            this.UpdateCenter();
          }
          else
            break;
        }
      }
      if (intList.Count <= this.NumOfDimensions && this.IsLifted)
        throw new ArgumentException("The input data is degenerate. It appears to exist in " + (object) this.NumOfDimensions + " dimensions, but it is a " + (object) (this.NumOfDimensions - 1) + " dimensional set (i.e. the point of collinear, coplanar, or co-hyperplanar.)");
      return intList;
    }

    private void UpdateAdjacency(ConvexFaceInternal l, ConvexFaceInternal r)
    {
      int[] vertices1 = l.Vertices;
      int[] vertices2 = r.Vertices;
      for (int index = 0; index < vertices1.Length; ++index)
        this.VertexVisited[vertices1[index]] = false;
      for (int index = 0; index < vertices2.Length; ++index)
        this.VertexVisited[vertices2[index]] = true;
      int index1 = 0;
      while (index1 < vertices1.Length && this.VertexVisited[vertices1[index1]])
        ++index1;
      if (index1 == this.NumOfDimensions)
        return;
      for (int index2 = index1 + 1; index2 < vertices1.Length; ++index2)
      {
        if (!this.VertexVisited[vertices1[index2]])
          return;
      }
      l.AdjacentFaces[index1] = r.Index;
      for (int index2 = 0; index2 < vertices1.Length; ++index2)
        this.VertexVisited[vertices1[index2]] = false;
      int index3 = 0;
      while (index3 < vertices2.Length && !this.VertexVisited[vertices2[index3]])
        ++index3;
      r.AdjacentFaces[index3] = l.Index;
    }

    private void FindBeyondVertices(ConvexFaceInternal face)
    {
      IndexBuffer verticesBeyond = face.VerticesBeyond;
      this.MaxDistance = double.NegativeInfinity;
      this.FurthestVertex = 0;
      for (int v = 0; v < this.NumberOfVertices; ++v)
      {
        if (!this.VertexVisited[v])
          this.IsBeyond(face, verticesBeyond, v);
      }
      face.FurthestVertex = this.FurthestVertex;
    }

    private void TagAffectedFaces(ConvexFaceInternal currentFace)
    {
      this.AffectedFaceBuffer.Clear();
      this.AffectedFaceBuffer.Add(currentFace.Index);
      this.TraverseAffectedFaces(currentFace.Index);
    }

    private void TraverseAffectedFaces(int currentFace)
    {
      this.TraverseStack.Clear();
      this.TraverseStack.Push(currentFace);
      this.AffectedFaceFlags[currentFace] = true;
      while (this.TraverseStack.Count > 0)
      {
        ConvexFaceInternal convexFaceInternal = this.FacePool[this.TraverseStack.Pop()];
        for (int index = 0; index < this.NumOfDimensions; ++index)
        {
          int adjacentFace = convexFaceInternal.AdjacentFaces[index];
          if (!this.AffectedFaceFlags[adjacentFace] && this.mathHelper.GetVertexDistance(this.CurrentVertex, this.FacePool[adjacentFace]) >= this.PlaneDistanceTolerance)
          {
            this.AffectedFaceBuffer.Add(adjacentFace);
            this.AffectedFaceFlags[adjacentFace] = true;
            this.TraverseStack.Push(adjacentFace);
          }
        }
      }
    }

    private DeferredFace MakeDeferredFace(
      ConvexFaceInternal face,
      int faceIndex,
      ConvexFaceInternal pivot,
      int pivotIndex,
      ConvexFaceInternal oldFace)
    {
      DeferredFace deferredFace = this.ObjectManager.GetDeferredFace();
      deferredFace.Face = face;
      deferredFace.FaceIndex = faceIndex;
      deferredFace.Pivot = pivot;
      deferredFace.PivotIndex = pivotIndex;
      deferredFace.OldFace = oldFace;
      return deferredFace;
    }

    private void ConnectFace(FaceConnector connector)
    {
      ConnectorList connectorList = this.ConnectorTable[(int) (connector.HashCode % 2017U)];
      for (FaceConnector faceConnector = connectorList.First; faceConnector != null; faceConnector = faceConnector.Next)
      {
        if (FaceConnector.AreConnectable(connector, faceConnector, this.NumOfDimensions))
        {
          connectorList.Remove(faceConnector);
          FaceConnector.Connect(faceConnector, connector);
          faceConnector.Face = (ConvexFaceInternal) null;
          connector.Face = (ConvexFaceInternal) null;
          this.ObjectManager.DepositConnector(faceConnector);
          this.ObjectManager.DepositConnector(connector);
          return;
        }
      }
      connectorList.Add(connector);
    }

    private bool CreateCone()
    {
      int currentVertex = this.CurrentVertex;
      this.ConeFaceBuffer.Clear();
      for (int i = 0; i < this.AffectedFaceBuffer.Count; ++i)
      {
        int index1 = this.AffectedFaceBuffer[i];
        ConvexFaceInternal oldFace = this.FacePool[index1];
        int index2 = 0;
        for (int index3 = 0; index3 < this.NumOfDimensions; ++index3)
        {
          int adjacentFace = oldFace.AdjacentFaces[index3];
          if (!this.AffectedFaceFlags[adjacentFace])
          {
            this.UpdateBuffer[index2] = adjacentFace;
            this.UpdateIndices[index2] = index3;
            ++index2;
          }
        }
        for (int index3 = 0; index3 < index2; ++index3)
        {
          ConvexFaceInternal pivot = this.FacePool[this.UpdateBuffer[index3]];
          int pivotIndex = 0;
          int[] adjacentFaces = pivot.AdjacentFaces;
          for (int index4 = 0; index4 < adjacentFaces.Length; ++index4)
          {
            if (index1 == adjacentFaces[index4])
            {
              pivotIndex = index4;
              break;
            }
          }
          int updateIndex = this.UpdateIndices[index3];
          ConvexFaceInternal face = this.FacePool[this.ObjectManager.GetFace()];
          int[] vertices = face.Vertices;
          for (int index4 = 0; index4 < this.NumOfDimensions; ++index4)
            vertices[index4] = oldFace.Vertices[index4];
          int num = vertices[updateIndex];
          int faceIndex;
          if (currentVertex < num)
          {
            faceIndex = 0;
            for (int index4 = updateIndex - 1; index4 >= 0; --index4)
            {
              if (vertices[index4] > currentVertex)
              {
                vertices[index4 + 1] = vertices[index4];
              }
              else
              {
                faceIndex = index4 + 1;
                break;
              }
            }
          }
          else
          {
            faceIndex = this.NumOfDimensions - 1;
            for (int index4 = updateIndex + 1; index4 < this.NumOfDimensions; ++index4)
            {
              if (vertices[index4] < currentVertex)
              {
                vertices[index4 - 1] = vertices[index4];
              }
              else
              {
                faceIndex = index4 - 1;
                break;
              }
            }
          }
          vertices[faceIndex] = this.CurrentVertex;
          if (!this.mathHelper.CalculateFacePlane(face, this.Center))
            return false;
          this.ConeFaceBuffer.Add(this.MakeDeferredFace(face, faceIndex, pivot, pivotIndex, oldFace));
        }
      }
      return true;
    }

    private void CommitCone()
    {
      for (int i = 0; i < this.ConeFaceBuffer.Count; ++i)
      {
        DeferredFace face1 = this.ConeFaceBuffer[i];
        ConvexFaceInternal face2 = face1.Face;
        ConvexFaceInternal pivot = face1.Pivot;
        ConvexFaceInternal oldFace = face1.OldFace;
        int faceIndex = face1.FaceIndex;
        face2.AdjacentFaces[faceIndex] = pivot.Index;
        pivot.AdjacentFaces[face1.PivotIndex] = face2.Index;
        for (int edgeIndex = 0; edgeIndex < this.NumOfDimensions; ++edgeIndex)
        {
          if (edgeIndex != faceIndex)
          {
            FaceConnector connector = this.ObjectManager.GetConnector();
            connector.Update(face2, edgeIndex, this.NumOfDimensions);
            this.ConnectFace(connector);
          }
        }
        if (pivot.VerticesBeyond.Count == 0)
          this.FindBeyondVertices(face2, oldFace.VerticesBeyond);
        else if (pivot.VerticesBeyond.Count < oldFace.VerticesBeyond.Count)
          this.FindBeyondVertices(face2, pivot.VerticesBeyond, oldFace.VerticesBeyond);
        else
          this.FindBeyondVertices(face2, oldFace.VerticesBeyond, pivot.VerticesBeyond);
        if (face2.VerticesBeyond.Count == 0)
        {
          this.ConvexFaces.Add(face2.Index);
          this.UnprocessedFaces.Remove(face2);
          this.ObjectManager.DepositVertexBuffer(face2.VerticesBeyond);
          face2.VerticesBeyond = this.EmptyBuffer;
        }
        else
          this.UnprocessedFaces.Add(face2);
        this.ObjectManager.DepositDeferredFace(face1);
      }
      for (int i = 0; i < this.AffectedFaceBuffer.Count; ++i)
      {
        int faceIndex = this.AffectedFaceBuffer[i];
        this.UnprocessedFaces.Remove(this.FacePool[faceIndex]);
        this.ObjectManager.DepositFace(faceIndex);
      }
    }

    private void IsBeyond(ConvexFaceInternal face, IndexBuffer beyondVertices, int v)
    {
      double vertexDistance = this.mathHelper.GetVertexDistance(v, face);
      if (vertexDistance < this.PlaneDistanceTolerance)
        return;
      if (vertexDistance > this.MaxDistance)
      {
        if (vertexDistance - this.MaxDistance < this.PlaneDistanceTolerance)
        {
          if (this.LexCompare(v, this.FurthestVertex) > 0)
          {
            this.MaxDistance = vertexDistance;
            this.FurthestVertex = v;
          }
        }
        else
        {
          this.MaxDistance = vertexDistance;
          this.FurthestVertex = v;
        }
      }
      beyondVertices.Add(v);
    }

    private int LexCompare(int u, int v)
    {
      int num1 = u * this.NumOfDimensions;
      int num2 = v * this.NumOfDimensions;
      for (int index = 0; index < this.NumOfDimensions; ++index)
      {
        int num3 = this.Positions[num1 + index].CompareTo(this.Positions[num2 + index]);
        if (num3 != 0)
          return num3;
      }
      return 0;
    }

    private void FindBeyondVertices(
      ConvexFaceInternal face,
      IndexBuffer beyond,
      IndexBuffer beyond1)
    {
      IndexBuffer beyondBuffer = this.BeyondBuffer;
      this.MaxDistance = double.NegativeInfinity;
      this.FurthestVertex = 0;
      for (int i = 0; i < beyond1.Count; ++i)
        this.VertexVisited[beyond1[i]] = true;
      this.VertexVisited[this.CurrentVertex] = false;
      for (int i = 0; i < beyond.Count; ++i)
      {
        int v = beyond[i];
        if (v != this.CurrentVertex)
        {
          this.VertexVisited[v] = false;
          this.IsBeyond(face, beyondBuffer, v);
        }
      }
      for (int i = 0; i < beyond1.Count; ++i)
      {
        int v = beyond1[i];
        if (this.VertexVisited[v])
          this.IsBeyond(face, beyondBuffer, v);
      }
      face.FurthestVertex = this.FurthestVertex;
      IndexBuffer verticesBeyond = face.VerticesBeyond;
      face.VerticesBeyond = beyondBuffer;
      if (verticesBeyond.Count > 0)
        verticesBeyond.Clear();
      this.BeyondBuffer = verticesBeyond;
    }

    private void FindBeyondVertices(ConvexFaceInternal face, IndexBuffer beyond)
    {
      IndexBuffer beyondBuffer = this.BeyondBuffer;
      this.MaxDistance = double.NegativeInfinity;
      this.FurthestVertex = 0;
      for (int i = 0; i < beyond.Count; ++i)
      {
        int v = beyond[i];
        if (v != this.CurrentVertex)
          this.IsBeyond(face, beyondBuffer, v);
      }
      face.FurthestVertex = this.FurthestVertex;
      IndexBuffer verticesBeyond = face.VerticesBeyond;
      face.VerticesBeyond = beyondBuffer;
      if (verticesBeyond.Count > 0)
        verticesBeyond.Clear();
      this.BeyondBuffer = verticesBeyond;
    }

    private void UpdateCenter()
    {
      for (int index = 0; index < this.NumOfDimensions; ++index)
        this.Center[index] *= (double) this.ConvexHullSize;
      ++this.ConvexHullSize;
      double num1 = 1.0 / (double) this.ConvexHullSize;
      int num2 = this.CurrentVertex * this.NumOfDimensions;
      for (int index = 0; index < this.NumOfDimensions; ++index)
        this.Center[index] = num1 * (this.Center[index] + this.Positions[num2 + index]);
    }

    private void RollbackCenter()
    {
      for (int index = 0; index < this.NumOfDimensions; ++index)
        this.Center[index] *= (double) this.ConvexHullSize;
      --this.ConvexHullSize;
      double num1 = this.ConvexHullSize > 0 ? 1.0 / (double) this.ConvexHullSize : 0.0;
      int num2 = this.CurrentVertex * this.NumOfDimensions;
      for (int index = 0; index < this.NumOfDimensions; ++index)
        this.Center[index] = num1 * (this.Center[index] - this.Positions[num2 + index]);
    }

    private void HandleSingular()
    {
      this.RollbackCenter();
      this.SingularVertices.Add(this.CurrentVertex);
      for (int i1 = 0; i1 < this.AffectedFaceBuffer.Count; ++i1)
      {
        ConvexFaceInternal face = this.FacePool[this.AffectedFaceBuffer[i1]];
        IndexBuffer verticesBeyond = face.VerticesBeyond;
        for (int i2 = 0; i2 < verticesBeyond.Count; ++i2)
          this.SingularVertices.Add(verticesBeyond[i2]);
        this.ConvexFaces.Add(face.Index);
        this.UnprocessedFaces.Remove(face);
        this.ObjectManager.DepositVertexBuffer(face.VerticesBeyond);
        face.VerticesBeyond = this.EmptyBuffer;
      }
    }

    private double GetCoordinate(int vIndex, int dimension) => this.Positions[vIndex * this.NumOfDimensions + dimension];

    private TVertex[] GetHullVertices<TVertex>(IList<TVertex> data)
    {
      int count = this.ConvexFaces.Count;
      int length = 0;
      for (int index = 0; index < this.NumberOfVertices; ++index)
        this.VertexVisited[index] = false;
      for (int i = 0; i < count; ++i)
      {
        foreach (int vertex in this.FacePool[this.ConvexFaces[i]].Vertices)
        {
          if (!this.VertexVisited[vertex])
          {
            this.VertexVisited[vertex] = true;
            ++length;
          }
        }
      }
      TVertex[] vertexArray = new TVertex[length];
      for (int index = 0; index < this.NumberOfVertices; ++index)
      {
        if (this.VertexVisited[index])
          vertexArray[--length] = data[index];
      }
      return vertexArray;
    }

    private TFace[] GetConvexFaces<TVertex, TFace>()
      where TVertex : IVertex
      where TFace : ConvexFace<TVertex, TFace>, new()
    {
      IndexBuffer convexFaces = this.ConvexFaces;
      int count = convexFaces.Count;
      TFace[] faceArray1 = new TFace[count];
      for (int i = 0; i < count; ++i)
      {
        ConvexFaceInternal convexFaceInternal = this.FacePool[convexFaces[i]];
        TVertex[] vertexArray = new TVertex[this.NumOfDimensions];
        for (int index = 0; index < this.NumOfDimensions; ++index)
          vertexArray[index] = (TVertex) this.Vertices[convexFaceInternal.Vertices[index]];
        TFace[] faceArray2 = faceArray1;
        int index1 = i;
        TFace face = new TFace();
        face.Vertices = vertexArray;
        face.Adjacency = new TFace[this.NumOfDimensions];
        face.Normal = this.IsLifted ? (double[]) null : convexFaceInternal.Normal;
        faceArray2[index1] = face;
        convexFaceInternal.Tag = i;
      }
      for (int i = 0; i < count; ++i)
      {
        ConvexFaceInternal convexFaceInternal = this.FacePool[convexFaces[i]];
        TFace face1 = faceArray1[i];
        for (int index = 0; index < this.NumOfDimensions; ++index)
        {
          if (convexFaceInternal.AdjacentFaces[index] >= 0)
            face1.Adjacency[index] = faceArray1[this.FacePool[convexFaceInternal.AdjacentFaces[index]].Tag];
        }
        if (convexFaceInternal.IsNormalFlipped)
        {
          TVertex vertex = face1.Vertices[0];
          face1.Vertices[0] = face1.Vertices[this.NumOfDimensions - 1];
          face1.Vertices[this.NumOfDimensions - 1] = vertex;
          TFace face2 = face1.Adjacency[0];
          face1.Adjacency[0] = face1.Adjacency[this.NumOfDimensions - 1];
          face1.Adjacency[this.NumOfDimensions - 1] = face2;
        }
      }
      return faceArray1;
    }

    private ConvexHull<TVertex, TFace> Return2DResultInOrder<TVertex, TFace>(
      IList<TVertex> data)
      where TVertex : IVertex
      where TFace : ConvexFace<TVertex, TFace>, new()
    {
      TFace[] convexFaces = this.GetConvexFaces<TVertex, TFace>();
      int length = convexFaces.Length;
      Dictionary<TVertex, TFace> dictionary = new Dictionary<TVertex, TFace>();
      foreach (TFace face in convexFaces)
        dictionary.Add(face.Vertices[1], face);
      TVertex vertex1 = convexFaces[0].Vertices[1];
      TVertex vertex2 = convexFaces[0].Vertices[0];
      List<TVertex> vertexList = new List<TVertex>();
      vertexList.Add(vertex1);
      List<TFace> faceList = new List<TFace>();
      faceList.Add(convexFaces[1]);
      int index1 = 0;
      int num1 = 0;
      TFace face1;
      for (; !vertex2.Equals((object) vertex1); vertex2 = face1.Vertices[0])
      {
        vertexList.Add(vertex2);
        face1 = dictionary[vertex2];
        faceList.Add(face1);
        double num2 = vertex2.Position[0];
        TVertex vertex3 = vertexList[index1];
        double num3 = vertex3.Position[0];
        if (num2 >= num3)
        {
          double num4 = vertex2.Position[0];
          vertex3 = vertexList[index1];
          double num5 = vertex3.Position[0];
          if (num4 == num5)
          {
            double num6 = vertex2.Position[1];
            vertex3 = vertexList[index1];
            double num7 = vertex3.Position[1];
            if (num6 > num7)
              goto label_8;
          }
          else
            goto label_8;
        }
        index1 = num1;
label_8:
        ++num1;
      }
      TVertex[] vertexArray = new TVertex[length];
      for (int index2 = 0; index2 < length; ++index2)
      {
        int index3 = (index2 + index1) % length;
        vertexArray[index2] = vertexList[index3];
        convexFaces[index2] = faceList[index3];
      }
      return new ConvexHull<TVertex, TFace>()
      {
        Points = (IEnumerable<TVertex>) vertexArray,
        Faces = (IEnumerable<TFace>) convexFaces
      };
    }

    internal static TCell[] GetDelaunayTriangulation<TVertex, TCell>(IList<TVertex> data)
      where TVertex : IVertex
      where TCell : TriangulationCell<TVertex, TCell>, new()
    {
      ConvexHullAlgorithm convexHullAlgorithm = new ConvexHullAlgorithm(data.Cast<IVertex>().ToArray<IVertex>(), true, 1E-10);
      convexHullAlgorithm.GetConvexHull();
      convexHullAlgorithm.RemoveUpperFaces();
      return convexHullAlgorithm.GetConvexFaces<TVertex, TCell>();
    }

    private void RemoveUpperFaces()
    {
      IndexBuffer convexFaces = this.ConvexFaces;
      int index1 = this.NumOfDimensions - 1;
      for (int i = convexFaces.Count - 1; i >= 0; --i)
      {
        int index2 = convexFaces[i];
        ConvexFaceInternal convexFaceInternal1 = this.FacePool[index2];
        if (convexFaceInternal1.Normal[index1] >= 0.0)
        {
          for (int index3 = 0; index3 < convexFaceInternal1.AdjacentFaces.Length; ++index3)
          {
            int adjacentFace = convexFaceInternal1.AdjacentFaces[index3];
            if (adjacentFace >= 0)
            {
              ConvexFaceInternal convexFaceInternal2 = this.FacePool[adjacentFace];
              for (int index4 = 0; index4 < convexFaceInternal2.AdjacentFaces.Length; ++index4)
              {
                if (convexFaceInternal2.AdjacentFaces[index4] == index2)
                  convexFaceInternal2.AdjacentFaces[index4] = -1;
              }
            }
          }
          convexFaces[i] = convexFaces[convexFaces.Count - 1];
          convexFaces.Pop();
        }
      }
    }
  }
}
