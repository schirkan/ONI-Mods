// Decompiled with JetBrains decompiler
// Type: Satsuma.TspUtils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public static class TspUtils
  {
    public static double GetTourCost<TNode>(
      IEnumerable<TNode> tour,
      Func<TNode, TNode, double> cost)
    {
      double num = 0.0;
      if (tour.Any<TNode>())
      {
        TNode node1 = tour.First<TNode>();
        foreach (TNode node2 in tour.Skip<TNode>(1))
        {
          num += cost(node1, node2);
          node1 = node2;
        }
      }
      return num;
    }
  }
}
