// Decompiled with JetBrains decompiler
// Type: Database.TechTreeTitles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Database
{
  public class TechTreeTitles : ResourceSet<TechTreeTitle>
  {
    public TechTreeTitles(ResourceSet parent)
      : base("TreeTitles", parent)
    {
    }

    public void Load(TextAsset tree_file)
    {
      foreach (ResourceTreeNode node in (ResourceLoader<ResourceTreeNode>) new ResourceTreeLoader<ResourceTreeNode>(tree_file))
      {
        if (string.Equals(node.Id.Substring(0, 1), "_"))
        {
          TechTreeTitle techTreeTitle = new TechTreeTitle(node.Id, (ResourceSet) this, (string) Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + node.Id.ToUpper()), node);
        }
      }
    }
  }
}
