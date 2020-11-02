// Decompiled with JetBrains decompiler
// Type: CPUBudget
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class CPUBudget
{
  public static Stopwatch stopwatch = Stopwatch.StartNew();
  private static Dictionary<ICPULoad, CPUBudget.Node> nodes = new Dictionary<ICPULoad, CPUBudget.Node>();

  public static int coreCount
  {
    get
    {
      int overrideCoreCount = TuningData<CPUBudget.Tuning>.Get().overrideCoreCount;
      return 0 >= overrideCoreCount || overrideCoreCount >= SystemInfo.processorCount ? SystemInfo.processorCount : overrideCoreCount;
    }
  }

  public static float ComputeDuration(long start) => (float) ((CPUBudget.stopwatch.ElapsedTicks - start) * 1000000L / Stopwatch.Frequency) / 1000f;

  public static void AddRoot(ICPULoad root) => CPUBudget.nodes.Add(root, new CPUBudget.Node()
  {
    load = root,
    children = new List<CPUBudget.Node>(),
    frameTime = root.GetEstimatedFrameTime(),
    loadBalanceThreshold = TuningData<CPUBudget.Tuning>.Get().defaultLoadBalanceThreshold
  });

  public static void AddChild(ICPULoad parent, ICPULoad child, float loadBalanceThreshold)
  {
    CPUBudget.Node node = new CPUBudget.Node()
    {
      load = child,
      children = new List<CPUBudget.Node>(),
      frameTime = child.GetEstimatedFrameTime(),
      loadBalanceThreshold = loadBalanceThreshold
    };
    CPUBudget.nodes.Add(child, node);
    CPUBudget.nodes[parent].children.Add(node);
  }

  public static void AddChild(ICPULoad parent, ICPULoad child) => CPUBudget.AddChild(parent, child, TuningData<CPUBudget.Tuning>.Get().defaultLoadBalanceThreshold);

  public static void FinalizeChildren(ICPULoad parent)
  {
    CPUBudget.Node node1 = CPUBudget.nodes[parent];
    List<CPUBudget.Node> children = CPUBudget.nodes[parent].children;
    float num = 0.0f;
    foreach (CPUBudget.Node node2 in children)
    {
      CPUBudget.FinalizeChildren(node2.load);
      num += node2.frameTime;
    }
    for (int index = 0; index != children.Count; ++index)
    {
      CPUBudget.Node node2 = children[index];
      node2.frameTime = node1.frameTime * (node2.frameTime / num);
      children[index] = node2;
    }
  }

  public static void Start(ICPULoad cpuLoad)
  {
    CPUBudget.Node node = CPUBudget.nodes[cpuLoad];
    node.start = CPUBudget.stopwatch.ElapsedTicks;
    CPUBudget.nodes[cpuLoad] = node;
  }

  public static void End(ICPULoad cpuLoad)
  {
    CPUBudget.Node node = CPUBudget.nodes[cpuLoad];
    float frameTimeDelta = node.frameTime - CPUBudget.ComputeDuration(node.start);
    if ((double) node.loadBalanceThreshold >= (double) Math.Abs(frameTimeDelta))
      return;
    CPUBudget.Balance(cpuLoad, frameTimeDelta);
  }

  public static void Balance(ICPULoad cpuLoad, float frameTimeDelta)
  {
    CPUBudget.Node node1 = CPUBudget.nodes[cpuLoad];
    List<CPUBudget.Node> children = node1.children;
    if (children.Count == 0)
    {
      if (!node1.load.AdjustLoad(node1.frameTime, frameTimeDelta))
        return;
      node1.frameTime += frameTimeDelta;
    }
    else
    {
      for (int index = 0; index != children.Count; ++index)
      {
        CPUBudget.Node node2 = children[index];
        float num = node2.frameTime / node1.frameTime;
        float frameTimeDelta1 = frameTimeDelta * num;
        CPUBudget.Balance(node2.load, frameTimeDelta1);
        children[index] = node2;
      }
    }
  }

  private class Tuning : TuningData<CPUBudget.Tuning>
  {
    public int overrideCoreCount = -1;
    public float defaultLoadBalanceThreshold = 0.1f;
  }

  private struct Node
  {
    public ICPULoad load;
    public List<CPUBudget.Node> children;
    public long start;
    public float frameTime;
    public float loadBalanceThreshold;
  }
}
