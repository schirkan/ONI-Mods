// Decompiled with JetBrains decompiler
// Type: MIConvexHull.MathHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace MIConvexHull
{
  internal class MathHelper
  {
    private readonly int Dimension;
    private readonly int[] matrixPivots;
    private readonly double[] nDMatrix;
    private readonly double[] nDNormalHelperVector;
    private readonly double[] ntX;
    private readonly double[] ntY;
    private readonly double[] ntZ;
    private readonly double[] PositionData;

    internal MathHelper(int dimension, double[] positions)
    {
      this.PositionData = positions;
      this.Dimension = dimension;
      this.ntX = new double[this.Dimension];
      this.ntY = new double[this.Dimension];
      this.ntZ = new double[this.Dimension];
      this.nDNormalHelperVector = new double[this.Dimension];
      this.nDMatrix = new double[this.Dimension * this.Dimension];
      this.matrixPivots = new int[this.Dimension];
    }

    internal bool CalculateFacePlane(ConvexFaceInternal face, double[] center)
    {
      int[] vertices = face.Vertices;
      double[] normal = face.Normal;
      this.FindNormalVector(vertices, normal);
      if (double.IsNaN(normal[0]))
        return false;
      double num1 = 0.0;
      double num2 = 0.0;
      int num3 = vertices[0] * this.Dimension;
      for (int index = 0; index < this.Dimension; ++index)
      {
        double num4 = normal[index];
        num1 += num4 * this.PositionData[num3 + index];
        num2 += num4 * center[index];
      }
      face.Offset = -num1;
      if (num2 - num1 > 0.0)
      {
        for (int index = 0; index < this.Dimension; ++index)
          normal[index] = -normal[index];
        face.Offset = num1;
        face.IsNormalFlipped = true;
      }
      else
        face.IsNormalFlipped = false;
      return true;
    }

    internal double GetVertexDistance(int v, ConvexFaceInternal f)
    {
      double[] normal = f.Normal;
      int num = v * this.Dimension;
      double offset = f.Offset;
      for (int index = 0; index < normal.Length; ++index)
        offset += normal[index] * this.PositionData[num + index];
      return offset;
    }

    internal double[] VectorBetweenVertices(int toIndex, int fromIndex)
    {
      double[] target = new double[this.Dimension];
      this.VectorBetweenVertices(toIndex, fromIndex, target);
      return target;
    }

    private void VectorBetweenVertices(int toIndex, int fromIndex, double[] target)
    {
      int num1 = toIndex * this.Dimension;
      int num2 = fromIndex * this.Dimension;
      for (int index = 0; index < this.Dimension; ++index)
        target[index] = this.PositionData[num1 + index] - this.PositionData[num2 + index];
    }

    internal void RandomOffsetToLift(int index)
    {
      Random random = new Random();
      int index1 = index * this.Dimension + this.Dimension - 1;
      this.PositionData[index1] += this.PositionData[index1] * random.NextDouble();
    }

    private void FindNormalVector(int[] vertices, double[] normalData)
    {
      switch (this.Dimension)
      {
        case 2:
          this.FindNormalVector2D(vertices, normalData);
          break;
        case 3:
          this.FindNormalVector3D(vertices, normalData);
          break;
        case 4:
          this.FindNormalVector4D(vertices, normalData);
          break;
        default:
          this.FindNormalVectorND(vertices, normalData);
          break;
      }
    }

    private void FindNormalVector2D(int[] vertices, double[] normal)
    {
      this.VectorBetweenVertices(vertices[1], vertices[0], this.ntX);
      double num1 = -this.ntX[1];
      double num2 = this.ntX[0];
      double num3 = 1.0 / Math.Sqrt(num1 * num1 + num2 * num2);
      normal[0] = num3 * num1;
      normal[1] = num3 * num2;
    }

    private void FindNormalVector3D(int[] vertices, double[] normal)
    {
      this.VectorBetweenVertices(vertices[1], vertices[0], this.ntX);
      this.VectorBetweenVertices(vertices[2], vertices[1], this.ntY);
      double num1 = this.ntX[1] * this.ntY[2] - this.ntX[2] * this.ntY[1];
      double num2 = this.ntX[2] * this.ntY[0] - this.ntX[0] * this.ntY[2];
      double num3 = this.ntX[0] * this.ntY[1] - this.ntX[1] * this.ntY[0];
      double num4 = 1.0 / Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
      normal[0] = num4 * num1;
      normal[1] = num4 * num2;
      normal[2] = num4 * num3;
    }

    private void FindNormalVector4D(int[] vertices, double[] normal)
    {
      this.VectorBetweenVertices(vertices[1], vertices[0], this.ntX);
      this.VectorBetweenVertices(vertices[2], vertices[1], this.ntY);
      this.VectorBetweenVertices(vertices[3], vertices[2], this.ntZ);
      double[] ntX = this.ntX;
      double[] ntY = this.ntY;
      double[] ntZ = this.ntZ;
      double num1 = ntX[3] * (ntY[2] * ntZ[1] - ntY[1] * ntZ[2]) + ntX[2] * (ntY[1] * ntZ[3] - ntY[3] * ntZ[1]) + ntX[1] * (ntY[3] * ntZ[2] - ntY[2] * ntZ[3]);
      double num2 = ntX[3] * (ntY[0] * ntZ[2] - ntY[2] * ntZ[0]) + ntX[2] * (ntY[3] * ntZ[0] - ntY[0] * ntZ[3]) + ntX[0] * (ntY[2] * ntZ[3] - ntY[3] * ntZ[2]);
      double num3 = ntX[3] * (ntY[1] * ntZ[0] - ntY[0] * ntZ[1]) + ntX[1] * (ntY[0] * ntZ[3] - ntY[3] * ntZ[0]) + ntX[0] * (ntY[3] * ntZ[1] - ntY[1] * ntZ[3]);
      double num4 = ntX[2] * (ntY[0] * ntZ[1] - ntY[1] * ntZ[0]) + ntX[1] * (ntY[2] * ntZ[0] - ntY[0] * ntZ[2]) + ntX[0] * (ntY[1] * ntZ[2] - ntY[2] * ntZ[1]);
      double num5 = 1.0 / Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3 + num4 * num4);
      normal[0] = num5 * num1;
      normal[1] = num5 * num2;
      normal[2] = num5 * num3;
      normal[3] = num5 * num4;
    }

    private void FindNormalVectorND(int[] vertices, double[] normal)
    {
      int[] matrixPivots = this.matrixPivots;
      double[] nDmatrix = this.nDMatrix;
      double d = 0.0;
      for (int index1 = 0; index1 < this.Dimension; ++index1)
      {
        for (int index2 = 0; index2 < this.Dimension; ++index2)
        {
          int num = vertices[index2] * this.Dimension;
          for (int index3 = 0; index3 < this.Dimension; ++index3)
            nDmatrix[this.Dimension * index2 + index3] = index3 == index1 ? 1.0 : this.PositionData[num + index3];
        }
        MathHelper.LUFactor(nDmatrix, this.Dimension, matrixPivots, this.nDNormalHelperVector);
        double num1 = 1.0;
        for (int index2 = 0; index2 < this.Dimension; ++index2)
        {
          if (matrixPivots[index2] != index2)
            num1 *= -nDmatrix[this.Dimension * index2 + index2];
          else
            num1 *= nDmatrix[this.Dimension * index2 + index2];
        }
        normal[index1] = num1;
        d += num1 * num1;
      }
      double num2 = 1.0 / Math.Sqrt(d);
      for (int index = 0; index < normal.Length; ++index)
        normal[index] *= num2;
    }

    internal double GetSimplexVolume(double[][] edgeVectors, int lastIndex, double bigNumber)
    {
      double[] A = new double[this.Dimension * this.Dimension];
      int index1 = 0;
      for (int index2 = 0; index2 < this.Dimension; ++index2)
      {
        for (int index3 = 0; index3 < this.Dimension; ++index3)
        {
          if (index2 <= lastIndex)
            A[index1++] = edgeVectors[index2][index3];
          else
            A[index1] = Math.Pow(-1.0, (double) index1) * (double) index1++ / bigNumber;
        }
      }
      return Math.Abs(this.DeterminantDestructive(A));
    }

    private double DeterminantDestructive(double[] A)
    {
      switch (this.Dimension)
      {
        case 0:
          return 0.0;
        case 1:
          return A[0];
        case 2:
          return A[0] * A[3] - A[1] * A[2];
        case 3:
          return A[0] * A[4] * A[8] + A[1] * A[5] * A[6] + A[2] * A[3] * A[7] - A[0] * A[5] * A[7] - A[1] * A[3] * A[8] - A[2] * A[4] * A[6];
        default:
          int[] ipiv = new int[this.Dimension];
          double[] vecLUcolj = new double[this.Dimension];
          MathHelper.LUFactor(A, this.Dimension, ipiv, vecLUcolj);
          double num = 1.0;
          for (int index = 0; index < ipiv.Length; ++index)
          {
            num *= A[this.Dimension * index + index];
            if (ipiv[index] != index)
              num *= -1.0;
          }
          return num;
      }
    }

    private static void LUFactor(double[] data, int order, int[] ipiv, double[] vecLUcolj)
    {
      for (int index = 0; index < order; ++index)
        ipiv[index] = index;
      for (int val2 = 0; val2 < order; ++val2)
      {
        int num1 = val2 * order;
        int index1 = num1 + val2;
        for (int index2 = 0; index2 < order; ++index2)
          vecLUcolj[index2] = data[num1 + index2];
        for (int val1 = 0; val1 < order; ++val1)
        {
          int num2 = Math.Min(val1, val2);
          double num3 = 0.0;
          for (int index2 = 0; index2 < num2; ++index2)
            num3 += data[index2 * order + val1] * vecLUcolj[index2];
          data[num1 + val1] = (vecLUcolj[val1] -= num3);
        }
        int index3 = val2;
        for (int index2 = val2 + 1; index2 < order; ++index2)
        {
          if (Math.Abs(vecLUcolj[index2]) > Math.Abs(vecLUcolj[index3]))
            index3 = index2;
        }
        if (index3 != val2)
        {
          for (int index2 = 0; index2 < order; ++index2)
          {
            int num2 = index2 * order;
            int index4 = num2 + index3;
            int index5 = num2 + val2;
            double num3 = data[index4];
            data[index4] = data[index5];
            data[index5] = num3;
          }
          ipiv[val2] = index3;
        }
        if (val2 < order & data[index1] != 0.0)
        {
          for (int index2 = val2 + 1; index2 < order; ++index2)
            data[num1 + index2] /= data[index1];
        }
      }
    }
  }
}
