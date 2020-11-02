// Decompiled with JetBrains decompiler
// Type: FactionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/FactionManager")]
public class FactionManager : KMonoBehaviour
{
  public static FactionManager Instance;
  public Faction Duplicant = new Faction(FactionManager.FactionID.Duplicant);
  public Faction Friendly = new Faction(FactionManager.FactionID.Friendly);
  public Faction Hostile = new Faction(FactionManager.FactionID.Hostile);
  public Faction Predator = new Faction(FactionManager.FactionID.Predator);
  public Faction Prey = new Faction(FactionManager.FactionID.Prey);
  public Faction Pest = new Faction(FactionManager.FactionID.Pest);

  public static void DestroyInstance() => FactionManager.Instance = (FactionManager) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    FactionManager.Instance = this;
  }

  protected override void OnSpawn() => base.OnSpawn();

  public Faction GetFaction(FactionManager.FactionID faction)
  {
    switch (faction)
    {
      case FactionManager.FactionID.Duplicant:
        return this.Duplicant;
      case FactionManager.FactionID.Friendly:
        return this.Friendly;
      case FactionManager.FactionID.Hostile:
        return this.Hostile;
      case FactionManager.FactionID.Prey:
        return this.Prey;
      case FactionManager.FactionID.Predator:
        return this.Predator;
      case FactionManager.FactionID.Pest:
        return this.Pest;
      default:
        return (Faction) null;
    }
  }

  public FactionManager.Disposition GetDisposition(
    FactionManager.FactionID of_faction,
    FactionManager.FactionID to_faction)
  {
    return FactionManager.Instance.GetFaction(of_faction).Dispositions.ContainsKey(to_faction) ? FactionManager.Instance.GetFaction(of_faction).Dispositions[to_faction] : FactionManager.Disposition.Neutral;
  }

  public enum FactionID
  {
    Duplicant,
    Friendly,
    Hostile,
    Prey,
    Predator,
    Pest,
    NumberOfFactions,
  }

  public enum Disposition
  {
    Assist,
    Neutral,
    Attack,
  }
}
