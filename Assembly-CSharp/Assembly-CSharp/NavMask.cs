// Decompiled with JetBrains decompiler
// Type: NavMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class NavMask
{
  public virtual bool IsTraversable(
    PathFinder.PotentialPath path,
    int from_cell,
    int cost,
    int transition_id,
    PathFinderAbilities abilities)
  {
    return true;
  }

  public virtual void ApplyTraversalToPath(ref PathFinder.PotentialPath path, int from_cell)
  {
  }
}
