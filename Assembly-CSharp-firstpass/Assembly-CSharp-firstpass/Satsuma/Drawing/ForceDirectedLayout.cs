// Decompiled with JetBrains decompiler
// Type: Satsuma.Drawing.ForceDirectedLayout
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma.Drawing
{
  public sealed class ForceDirectedLayout
  {
    public const double DefaultStartingTemperature = 0.2;
    public const double DefaultMinimumTemperature = 0.01;
    public const double DefaultTemperatureAttenuation = 0.95;

    public IGraph Graph { get; private set; }

    public Dictionary<Satsuma.Node, PointD> NodePositions { get; private set; }

    public Func<double, double> SpringForce { get; set; }

    public Func<double, double> ElectricForce { get; set; }

    public Func<PointD, PointD> ExternalForce { get; set; }

    public double Temperature { get; set; }

    public double TemperatureAttenuation { get; set; }

    public ForceDirectedLayout(IGraph graph, Func<Satsuma.Node, PointD> initialPositions = null, int seed = -1)
    {
      this.Graph = graph;
      this.NodePositions = new Dictionary<Satsuma.Node, PointD>();
      this.SpringForce = (Func<double, double>) (d => 2.0 * Math.Log(d));
      this.ElectricForce = (Func<double, double>) (d => 1.0 / (d * d));
      this.ExternalForce = (Func<PointD, PointD>) null;
      this.TemperatureAttenuation = 0.95;
      this.Initialize(initialPositions, seed);
    }

    public void Initialize(Func<Satsuma.Node, PointD> initialPositions = null, int seed = -1)
    {
      if (initialPositions == null)
      {
        Random r = seed != -1 ? new Random(seed) : new Random();
        initialPositions = (Func<Satsuma.Node, PointD>) (node => new PointD(r.NextDouble(), r.NextDouble()));
      }
      foreach (Satsuma.Node node in this.Graph.Nodes())
        this.NodePositions[node] = initialPositions(node);
      this.Temperature = 0.2;
    }

    public void Step()
    {
      Dictionary<Satsuma.Node, PointD> dictionary = new Dictionary<Satsuma.Node, PointD>();
      foreach (Satsuma.Node node1 in this.Graph.Nodes())
      {
        PointD nodePosition1 = this.NodePositions[node1];
        double x = 0.0;
        double y = 0.0;
        foreach (Arc arc in this.Graph.Arcs(node1))
        {
          PointD nodePosition2 = this.NodePositions[this.Graph.Other(arc, node1)];
          double num1 = nodePosition1.Distance(nodePosition2);
          double num2 = this.Temperature * this.SpringForce(num1);
          x += (nodePosition2.X - nodePosition1.X) / num1 * num2;
          y += (nodePosition2.Y - nodePosition1.Y) / num1 * num2;
        }
        foreach (Satsuma.Node node2 in this.Graph.Nodes())
        {
          if (!(node2 == node1))
          {
            PointD nodePosition2 = this.NodePositions[node2];
            double num1 = nodePosition1.Distance(nodePosition2);
            double num2 = this.Temperature * this.ElectricForce(num1);
            x += (nodePosition1.X - nodePosition2.X) / num1 * num2;
            y += (nodePosition1.Y - nodePosition2.Y) / num1 * num2;
          }
        }
        if (this.ExternalForce != null)
        {
          PointD pointD = this.ExternalForce(nodePosition1);
          x += this.Temperature * pointD.X;
          y += this.Temperature * pointD.Y;
        }
        dictionary[node1] = new PointD(x, y);
      }
      foreach (Satsuma.Node node in this.Graph.Nodes())
        this.NodePositions[node] += dictionary[node];
      this.Temperature *= this.TemperatureAttenuation;
    }

    public void Run(double minimumTemperature = 0.01)
    {
      while (this.Temperature > minimumTemperature)
        this.Step();
    }
  }
}
