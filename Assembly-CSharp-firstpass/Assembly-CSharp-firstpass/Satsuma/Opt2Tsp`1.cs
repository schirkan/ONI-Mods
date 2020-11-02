// Decompiled with JetBrains decompiler
// Type: Satsuma.Opt2Tsp`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Opt2Tsp<TNode> : ITsp<TNode>
  {
    private List<TNode> tour;

    public Func<TNode, TNode, double> Cost { get; private set; }

    public IEnumerable<TNode> Tour => (IEnumerable<TNode>) this.tour;

    public double TourCost { get; private set; }

    public Opt2Tsp(Func<TNode, TNode, double> cost, IEnumerable<TNode> tour, double? tourCost)
    {
      this.Cost = cost;
      this.tour = tour.ToList<TNode>();
      this.TourCost = tourCost ?? TspUtils.GetTourCost<TNode>(tour, cost);
    }

    public bool Step()
    {
      bool flag = false;
      for (int index1 = 0; index1 < this.tour.Count - 3; ++index1)
      {
        int index2 = index1 + 2;
        for (int index3 = this.tour.Count - (index1 == 0 ? 2 : 1); index2 < index3; ++index2)
        {
          double num = this.Cost(this.tour[index1], this.tour[index2]) + this.Cost(this.tour[index1 + 1], this.tour[index2 + 1]) - (this.Cost(this.tour[index1], this.tour[index1 + 1]) + this.Cost(this.tour[index2], this.tour[index2 + 1]));
          if (num < 0.0)
          {
            this.TourCost += num;
            this.tour.Reverse(index1 + 1, index2 - index1);
            flag = true;
          }
        }
      }
      return flag;
    }

    public void Run()
    {
      do
        ;
      while (this.Step());
    }
  }
}
