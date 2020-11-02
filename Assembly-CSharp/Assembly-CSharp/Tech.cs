// Decompiled with JetBrains decompiler
// Type: Tech
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Tech : Resource
{
  public List<Tech> requiredTech = new List<Tech>();
  public List<Tech> unlockedTech = new List<Tech>();
  public List<TechItem> unlockedItems = new List<TechItem>();
  public int tier;
  public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();
  public string desc;
  private ResourceTreeNode node;

  public Vector2 center => this.node.center;

  public float width => this.node.width;

  public float height => this.node.height;

  public List<ResourceTreeNode.Edge> edges => this.node.edges;

  public Tech(string id, ResourceSet parent, string name, string desc, ResourceTreeNode node)
    : base(id, parent, name)
  {
    this.desc = desc;
    this.node = node;
  }

  public bool CanAfford(ResearchPointInventory pointInventory)
  {
    foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
    {
      if ((double) pointInventory.PointsByTypeID[keyValuePair.Key] < (double) keyValuePair.Value)
        return false;
    }
    return true;
  }

  public string CostString(ResearchTypes types)
  {
    string str = "";
    foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
    {
      str += string.Format("{0}:{1}", (object) types.GetResearchType(keyValuePair.Key).name.ToString(), (object) keyValuePair.Value.ToString());
      str += "\n";
    }
    return str;
  }

  public bool IsComplete()
  {
    if (!((Object) Research.Instance != (Object) null))
      return false;
    TechInstance techInstance = Research.Instance.Get(this);
    return techInstance != null && techInstance.IsComplete();
  }

  public bool ArePrerequisitesComplete()
  {
    foreach (Tech tech in this.requiredTech)
    {
      if (!tech.IsComplete())
        return false;
    }
    return true;
  }
}
