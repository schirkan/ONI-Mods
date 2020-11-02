// Decompiled with JetBrains decompiler
// Type: WorldGenSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGen;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/WorldGenSpawner")]
public class WorldGenSpawner : KMonoBehaviour
{
  [Serialize]
  private Prefab[] spawnInfos;
  [Serialize]
  private bool hasPlacedTemplates;
  private List<WorldGenSpawner.Spawnable> spawnables = new List<WorldGenSpawner.Spawnable>();

  public bool SpawnsRemain() => this.spawnables.Count > 0;

  public void SpawnEverything()
  {
    for (int index = 0; index < this.spawnables.Count; ++index)
      this.spawnables[index].TrySpawn();
  }

  public void ClearSpawnersInArea(Vector2 root_position, CellOffset[] area)
  {
    for (int index = 0; index < this.spawnables.Count; ++index)
    {
      if (Grid.IsCellOffsetOf(Grid.PosToCell(root_position), this.spawnables[index].cell, area))
        this.spawnables[index].FreeResources();
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.hasPlacedTemplates)
      return;
    this.DoReveal();
  }

  protected override void OnSpawn()
  {
    if (!this.hasPlacedTemplates)
    {
      this.PlaceTemplates();
      this.hasPlacedTemplates = true;
    }
    if (this.spawnInfos == null)
      return;
    for (int index = 0; index < this.spawnInfos.Length; ++index)
      this.AddSpawnable(this.spawnInfos[index]);
  }

  [System.Runtime.Serialization.OnSerializing]
  private void OnSerializing()
  {
    List<Prefab> prefabList = new List<Prefab>();
    for (int index = 0; index < this.spawnables.Count; ++index)
    {
      WorldGenSpawner.Spawnable spawnable = this.spawnables[index];
      if (!spawnable.isSpawned)
        prefabList.Add(spawnable.spawnInfo);
    }
    this.spawnInfos = prefabList.ToArray();
  }

  private void AddSpawnable(Prefab prefab) => this.spawnables.Add(new WorldGenSpawner.Spawnable(prefab));

  public void AddLegacySpawner(Tag tag, int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    this.AddSpawnable(new Prefab(tag.Name, Prefab.Type.Other, xy.x, xy.y, SimHashes.Carbon));
  }

  private void PlaceTemplates()
  {
    this.spawnables = new List<WorldGenSpawner.Spawnable>();
    foreach (Prefab building in SaveGame.Instance.worldGen.SpawnData.buildings)
    {
      building.type = Prefab.Type.Building;
      this.AddSpawnable(building);
    }
    foreach (Prefab elementalOre in SaveGame.Instance.worldGen.SpawnData.elementalOres)
    {
      elementalOre.type = Prefab.Type.Ore;
      this.AddSpawnable(elementalOre);
    }
    foreach (Prefab otherEntity in SaveGame.Instance.worldGen.SpawnData.otherEntities)
    {
      otherEntity.type = Prefab.Type.Other;
      this.AddSpawnable(otherEntity);
    }
    foreach (Prefab pickupable in SaveGame.Instance.worldGen.SpawnData.pickupables)
    {
      pickupable.type = Prefab.Type.Pickupable;
      this.AddSpawnable(pickupable);
    }
    SaveGame.Instance.worldGen.SpawnData.buildings.Clear();
    SaveGame.Instance.worldGen.SpawnData.elementalOres.Clear();
    SaveGame.Instance.worldGen.SpawnData.otherEntities.Clear();
    SaveGame.Instance.worldGen.SpawnData.pickupables.Clear();
  }

  private void DoReveal()
  {
    Game.Instance.Reset(SaveGame.Instance.worldGen.SpawnData);
    for (int i = 0; i < Grid.CellCount; ++i)
    {
      Grid.Revealed[i] = false;
      Grid.Spawnable[i] = (byte) 0;
    }
    float innerRadius = 16.5f;
    float radius = 18f;
    Vector2I baseStartPos = SaveGame.Instance.worldGen.SpawnData.baseStartPos;
    GridVisibility.Reveal(baseStartPos.x, baseStartPos.y, radius, innerRadius);
  }

  private class Spawnable
  {
    private HandleVector<int>.Handle fogOfWarPartitionerEntry;
    private HandleVector<int>.Handle solidChangedPartitionerEntry;

    public Prefab spawnInfo { get; private set; }

    public bool isSpawned { get; private set; }

    public int cell { get; private set; }

    public Spawnable(Prefab spawn_info)
    {
      this.spawnInfo = spawn_info;
      int num = Grid.XYToCell(this.spawnInfo.location_x, this.spawnInfo.location_y);
      GameObject prefab = Assets.GetPrefab((Tag) spawn_info.id);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        WorldSpawnableMonitor.Def def = prefab.GetDef<WorldSpawnableMonitor.Def>();
        if (def != null && def.adjustSpawnLocationCb != null)
          num = def.adjustSpawnLocationCb(num);
      }
      this.cell = num;
      Debug.Assert(Grid.IsValidCell(this.cell));
      if (Grid.Spawnable[this.cell] > (byte) 0)
        this.TrySpawn();
      else
        this.fogOfWarPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnReveal", (object) this, this.cell, GameScenePartitioner.Instance.fogOfWarChangedLayer, new System.Action<object>(this.OnReveal));
    }

    private void OnReveal(object data)
    {
      if (Grid.Spawnable[this.cell] <= (byte) 0)
        return;
      this.TrySpawn();
    }

    private void OnSolidChanged(object data)
    {
      if (Grid.Solid[this.cell])
        return;
      GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
      Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
      this.Spawn();
    }

    public void FreeResources()
    {
      if (this.solidChangedPartitionerEntry.IsValid())
      {
        GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
        if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
          Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
      }
      GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
      this.isSpawned = true;
    }

    public void TrySpawn()
    {
      if (this.isSpawned || this.solidChangedPartitionerEntry.IsValid())
        return;
      GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
      GameObject prefab = Assets.GetPrefab(this.GetPrefabTag());
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        bool flag = false;
        if ((UnityEngine.Object) prefab.GetComponent<Pickupable>() != (UnityEngine.Object) null && !prefab.HasTag(GameTags.Creatures.Digger))
          flag = true;
        else if (prefab.GetDef<BurrowMonitor.Def>() != null)
          flag = true;
        if (flag && Grid.Solid[this.cell])
        {
          this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnSolidChanged", (object) this, this.cell, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
          Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(this.cell);
        }
        else
          this.Spawn();
      }
      else
        this.Spawn();
    }

    private Tag GetPrefabTag()
    {
      Mob mob = SettingsCache.mobs.GetMob(this.spawnInfo.id);
      return mob != null && mob.prefabName != null ? new Tag(mob.prefabName) : new Tag(this.spawnInfo.id);
    }

    private void Spawn()
    {
      this.isSpawned = true;
      GameObject go = WorldGenSpawner.Spawnable.GetSpawnableCallback(this.spawnInfo.type)(this.spawnInfo, 0);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && (bool) (UnityEngine.Object) go)
      {
        go.SetActive(true);
        go.Trigger(1119167081);
      }
      this.FreeResources();
    }

    public static WorldGenSpawner.Spawnable.PlaceEntityFn GetSpawnableCallback(
      Prefab.Type type)
    {
      switch (type)
      {
        case Prefab.Type.Building:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceBuilding);
        case Prefab.Type.Ore:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceElementalOres);
        case Prefab.Type.Pickupable:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlacePickupables);
        case Prefab.Type.Other:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
        default:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
      }
    }

    public delegate GameObject PlaceEntityFn(Prefab prefab, int root_cell);
  }
}
