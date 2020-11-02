﻿// Decompiled with JetBrains decompiler
// Type: Comet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class Comet : KMonoBehaviour, ISim33ms
{
  private const SimHashes EXHAUST_ELEMENT = SimHashes.CarbonDioxide;
  private const float EXHAUST_RATE = 50f;
  public Vector2 spawnVelocity = new Vector2(12f, 15f);
  public Vector2 spawnAngle = new Vector2(-100f, -80f);
  public Vector2 massRange;
  public Vector2 temperatureRange;
  public SpawnFXHashes explosionEffectHash;
  public int splashRadius = 1;
  public int addTiles;
  public int addTilesMinHeight;
  public int addTilesMaxHeight;
  public int entityDamage = 1;
  public float totalTileDamage = 0.2f;
  private float addTileMass;
  public Vector2 elementReplaceTileTemperatureRange = new Vector2(800f, 1000f);
  public Vector2I explosionOreCount = new Vector2I(0, 0);
  private float explosionMass;
  public Vector2 explosionTemperatureRange = new Vector2(500f, 700f);
  public Vector2 explosionSpeedRange = new Vector2(8f, 14f);
  public float windowDamageMultiplier = 5f;
  public float bunkerDamageMultiplier;
  public string impactSound;
  public string flyingSound;
  public int flyingSoundID;
  private HashedString FLYING_SOUND_ID_PARAMETER = (HashedString) "meteorType";
  [Serialize]
  private Vector2 velocity;
  [Serialize]
  private float remainingTileDamage;
  private Vector3 previousPosition;
  private bool hasExploded;
  private LoopingSounds loopingSounds;
  private List<GameObject> damagedEntities = new List<GameObject>();
  private List<int> destroyedCells = new List<int>();
  private const float MAX_DISTANCE_TEST = 6f;

  private float GetVolume(GameObject gameObject)
  {
    float num = 1f;
    GameObject gameObject1 = gameObject;
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1.GetComponent<KSelectable>() != (UnityEngine.Object) null && gameObject1.GetComponent<KSelectable>().IsSelected)
      num = 1f;
    return num;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.remainingTileDamage = this.totalTileDamage;
    this.loopingSounds = this.gameObject.GetComponent<LoopingSounds>();
    this.flyingSound = GlobalAssets.GetSound("Meteor_LP");
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RandomizeValues();
    this.StartLoopingSound();
  }

  public void RandomizeValues()
  {
    float num1 = UnityEngine.Random.Range(this.massRange.x, this.massRange.y);
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    component.Mass = num1;
    component.Temperature = UnityEngine.Random.Range(this.temperatureRange.x, this.temperatureRange.y);
    float num2 = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
    float f = (float) ((double) num2 * 3.14159274101257 / 180.0);
    float num3 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
    this.velocity = new Vector2(-Mathf.Cos(f) * num3, Mathf.Sin(f) * num3);
    this.GetComponent<KBatchedAnimController>().Rotation = (float) (-(double) num2 - 90.0);
    if (this.addTiles > 0)
    {
      float num4 = UnityEngine.Random.Range(0.95f, 0.98f);
      this.explosionMass = num1 * (1f - num4);
      this.addTileMass = num1 * num4;
    }
    else
    {
      this.explosionMass = num1;
      this.addTileMass = 0.0f;
    }
  }

  [ContextMenu("Explode")]
  private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
  {
    this.PlayImpactSound(pos);
    Vector3 pos1 = pos;
    pos1.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
    Game.Instance.SpawnFX(this.explosionEffectHash, pos1, 0.0f);
    Substance substance = element.substance;
    int num1 = UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
    Vector2 vector2_1 = -this.velocity.normalized;
    Vector2 vector2_2 = new Vector2(vector2_1.y, -vector2_1.x);
    ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) pos.x - 3, (int) pos.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) pooledList);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) pooledList)
    {
      GameObject gameObject = (partitionerEntry.obj as Pickupable).gameObject;
      if (!((UnityEngine.Object) gameObject.GetComponent<MinionIdentity>() != (UnityEngine.Object) null) && gameObject.GetDef<CreatureFallMonitor.Def>() == null)
      {
        Vector2 initial_velocity = (((Vector2) (gameObject.transform.GetPosition() - pos)).normalized + new Vector2(0.0f, 0.55f)) * (0.5f * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y));
        if (GameComps.Fallers.Has((object) gameObject))
          GameComps.Fallers.Remove(gameObject);
        if (GameComps.Gravities.Has((object) gameObject))
          GameComps.Gravities.Remove(gameObject);
        GameComps.Fallers.Add(gameObject, initial_velocity);
      }
    }
    pooledList.Recycle();
    int num2 = this.splashRadius + 1;
    for (int y = -num2; y <= num2; ++y)
    {
      for (int x = -num2; x <= num2; ++x)
      {
        int cell1 = Grid.OffsetCell(cell, x, y);
        if (Grid.IsValidCell(cell1) && !this.destroyedCells.Contains(cell1))
        {
          float num3 = (float) ((1.0 - (double) Mathf.Abs(x) / (double) num2) * (1.0 - (double) Mathf.Abs(y) / (double) num2));
          if ((double) num3 > 0.0)
          {
            double num4 = (double) this.DamageTiles(cell1, prev_cell, (float) ((double) num3 * (double) this.totalTileDamage * 0.5));
          }
        }
      }
    }
    float mass1 = num1 > 0 ? this.explosionMass / (float) num1 : 1f;
    float temperature = UnityEngine.Random.Range(this.explosionTemperatureRange.x, this.explosionTemperatureRange.y);
    for (int index = 0; index < num1; ++index)
    {
      Vector2 normalized = (vector2_1 + vector2_2 * UnityEngine.Random.Range(-1f, 1f)).normalized;
      Vector3 vector3 = (Vector3) (normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y));
      Vector3 position = (Vector3) (normalized.normalized * 0.75f) + new Vector3(0.0f, 0.55f, 0.0f) + pos;
      GameObject go = substance.SpawnResource(position, mass1, temperature, byte.MaxValue, 0);
      if (GameComps.Fallers.Has((object) go))
        GameComps.Fallers.Remove(go);
      GameComps.Fallers.Add(go, (Vector2) vector3);
    }
    if (this.addTiles <= 0)
      return;
    int num5 = Mathf.Min(this.addTiles, Mathf.Clamp(Mathf.RoundToInt((float) this.addTiles * (float) (1.0 - (double) (this.GetDepthOfElement(cell, element) - this.addTilesMinHeight) / (double) (this.addTilesMaxHeight - this.addTilesMinHeight))), 1, this.addTiles));
    HashSetPool<int, Comet>.PooledHashSet pooledHashSet1 = HashSetPool<int, Comet>.Allocate();
    HashSetPool<int, Comet>.PooledHashSet pooledHashSet2 = HashSetPool<int, Comet>.Allocate();
    QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue1 = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
    int x1 = -1;
    int x2 = 1;
    if ((double) this.velocity.x < 0.0)
    {
      x1 *= -1;
      x2 *= -1;
    }
    pooledQueue1.Enqueue(new GameUtil.FloodFillInfo()
    {
      cell = prev_cell,
      depth = 0
    });
    QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue2 = pooledQueue1;
    GameUtil.FloodFillInfo floodFillInfo1 = new GameUtil.FloodFillInfo();
    floodFillInfo1.cell = Grid.OffsetCell(prev_cell, new CellOffset(x1, 0));
    floodFillInfo1.depth = 0;
    GameUtil.FloodFillInfo floodFillInfo2 = floodFillInfo1;
    pooledQueue2.Enqueue(floodFillInfo2);
    QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue3 = pooledQueue1;
    floodFillInfo1 = new GameUtil.FloodFillInfo();
    floodFillInfo1.cell = Grid.OffsetCell(prev_cell, new CellOffset(x2, 0));
    floodFillInfo1.depth = 0;
    GameUtil.FloodFillInfo floodFillInfo3 = floodFillInfo1;
    pooledQueue3.Enqueue(floodFillInfo3);
    GameUtil.FloodFillConditional((Queue<GameUtil.FloodFillInfo>) pooledQueue1, new Func<int, bool>(this.SpawnTilesCellTest), (ICollection<int>) pooledHashSet2, (ICollection<int>) pooledHashSet1, 10);
    float mass2 = num5 > 0 ? this.addTileMass / (float) this.addTiles : 1f;
    UnstableGroundManager component = World.Instance.GetComponent<UnstableGroundManager>();
    foreach (int cell1 in (HashSet<int>) pooledHashSet1)
    {
      if (num5 > 0)
      {
        component.Spawn(cell1, element, mass2, temperature, byte.MaxValue, 0);
        --num5;
      }
      else
        break;
    }
    pooledHashSet1.Recycle();
    pooledHashSet2.Recycle();
    pooledQueue1.Recycle();
  }

  private int GetDepthOfElement(int cell, Element element)
  {
    int num = 0;
    for (int cell1 = Grid.CellBelow(cell); Grid.IsValidCell(cell1) && Grid.Element[cell1] == element; cell1 = Grid.CellBelow(cell1))
      ++num;
    return num;
  }

  private bool SpawnTilesCellTest(int cell) => Grid.IsValidCell(cell) && !Grid.Solid[cell];

  [ContextMenu("DamageTiles")]
  private float DamageTiles(int cell, int prev_cell, float input_damage)
  {
    GameObject tile_go = Grid.Objects[cell, 9];
    float num1 = 1f;
    bool flag = false;
    if ((UnityEngine.Object) tile_go != (UnityEngine.Object) null)
    {
      if (tile_go.GetComponent<KPrefabID>().HasTag(GameTags.Window))
        num1 = this.windowDamageMultiplier;
      else if (tile_go.GetComponent<KPrefabID>().HasTag(GameTags.Bunker))
      {
        num1 = this.bunkerDamageMultiplier;
        if ((UnityEngine.Object) tile_go.GetComponent<Door>() != (UnityEngine.Object) null)
          Game.Instance.savedInfo.blockedCometWithBunkerDoor = true;
      }
      SimCellOccupier component = tile_go.GetComponent<SimCellOccupier>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.doReplaceElement)
        flag = true;
    }
    Element element = !flag ? Grid.Element[cell] : tile_go.GetComponent<PrimaryElement>().Element;
    if ((double) element.strength == 0.0)
      return 0.0f;
    float amount = input_damage * num1 / element.strength;
    this.PlayTileDamageSound(element, Grid.CellToPos(cell), tile_go);
    if ((double) amount == 0.0)
      return 0.0f;
    float num2;
    if (flag)
    {
      BuildingHP component = tile_go.GetComponent<BuildingHP>();
      double num3 = (double) component.HitPoints / (double) component.MaxHitPoints;
      float f = amount * (float) component.MaxHitPoints;
      component.gameObject.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
      {
        damage = Mathf.RoundToInt(f),
        source = (string) BUILDINGS.DAMAGESOURCES.COMET,
        popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
      });
      double num4 = (double) amount;
      num2 = Mathf.Min((float) num3, (float) num4);
    }
    else
      num2 = WorldDamage.Instance.ApplyDamage(cell, amount, prev_cell, (string) BUILDINGS.DAMAGESOURCES.COMET, (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET);
    this.destroyedCells.Add(cell);
    float num5 = num2 / amount;
    return input_damage * (1f - num5);
  }

  private void DamageThings(Vector3 pos, int cell, int damage)
  {
    if (!Grid.IsValidCell(cell))
      return;
    GameObject building_go = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) building_go != (UnityEngine.Object) null)
    {
      BuildingHP component1 = building_go.GetComponent<BuildingHP>();
      Building component2 = building_go.GetComponent<Building>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && !this.damagedEntities.Contains(building_go))
      {
        float f = building_go.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? (float) damage * this.bunkerDamageMultiplier : (float) damage;
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2.Def != (UnityEngine.Object) null)
          this.PlayBuildingDamageSound(component2.Def, Grid.CellToPos(cell), building_go);
        component1.gameObject.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
        {
          damage = Mathf.RoundToInt(f),
          source = (string) BUILDINGS.DAMAGESOURCES.COMET,
          popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
        });
        this.damagedEntities.Add(building_go);
      }
    }
    ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) pos.x, (int) pos.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) pooledList);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) pooledList)
    {
      Pickupable pickupable = partitionerEntry.obj as Pickupable;
      Health component = pickupable.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !this.damagedEntities.Contains(pickupable.gameObject))
      {
        float amount = pickupable.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? (float) damage * this.bunkerDamageMultiplier : (float) damage;
        component.Damage(amount);
        this.damagedEntities.Add(pickupable.gameObject);
      }
    }
    pooledList.Recycle();
  }

  private float GetDistanceFromImpact()
  {
    float num1 = this.velocity.x / this.velocity.y;
    Vector3 position = this.transform.GetPosition();
    float y = 0.0f;
    while ((double) y > -6.0)
    {
      float num2 = y - 1f;
      y = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
      Vector3 vector3 = new Vector3(y * num1, y, 0.0f);
      int cell = Grid.PosToCell(position + vector3);
      if (Grid.IsValidCell(cell) && Grid.Solid[cell])
        return vector3.magnitude;
    }
    return 6f;
  }

  public float GetSoundDistance() => this.GetDistanceFromImpact();

  private void PlayTileDamageSound(Element element, Vector3 pos, GameObject tile_go)
  {
    string sound = GlobalAssets.GetSound("MeteorDamage_" + (element.substance.GetMiningBreakSound() ?? (!element.HasTag(GameTags.RefinedMetal) ? (!element.HasTag(GameTags.Metal) ? "Rock" : "RawMetal") : "RefinedMetal")));
    if (!(bool) (UnityEngine.Object) CameraController.Instance || !CameraController.Instance.IsAudibleSound(pos, (HashedString) sound))
      return;
    float volume = this.GetVolume(tile_go);
    KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos), volume);
  }

  private void PlayBuildingDamageSound(BuildingDef def, Vector3 pos, GameObject building_go)
  {
    if (!((UnityEngine.Object) def != (UnityEngine.Object) null))
      return;
    string sound = GlobalAssets.GetSound(StringFormatter.Combine("MeteorDamage_Building_", def.AudioCategory)) ?? GlobalAssets.GetSound("MeteorDamage_Building_Metal");
    if (sound == null || !(bool) (UnityEngine.Object) CameraController.Instance || !CameraController.Instance.IsAudibleSound(pos, (HashedString) sound))
      return;
    float volume = this.GetVolume(building_go);
    KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos), volume);
  }

  public void Sim33ms(float dt)
  {
    if (this.hasExploded)
      return;
    Vector2 vector2_1 = new Vector2((float) Grid.WidthInCells, (float) Grid.HeightInCells) * -0.1f;
    Vector2 vector2_2 = new Vector2((float) Grid.WidthInCells, (float) Grid.HeightInCells) * 1.1f;
    Vector3 position = this.transform.GetPosition();
    Vector3 vector3 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0.0f);
    int cell1 = Grid.PosToCell(vector3);
    this.loopingSounds.UpdateVelocity(this.flyingSound, (Vector2) (vector3 - position));
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.CarbonDioxide);
    if (Grid.IsValidCell(cell1) && !Grid.Solid[cell1])
      SimMessages.EmitMass(cell1, elementByHash.idx, dt * 50f, elementByHash.defaultValues.temperature, (byte) 0, 0);
    if ((double) vector3.x < (double) vector2_1.x || (double) vector2_2.x < (double) vector3.x || (double) vector3.y < (double) vector2_1.y)
      Util.KDestroyGameObject(this.gameObject);
    int cell2 = Grid.PosToCell((KMonoBehaviour) this);
    int cell3 = Grid.PosToCell(this.previousPosition);
    if (cell2 != cell3)
    {
      if (Grid.IsValidCell(cell2) && Grid.Solid[cell2])
      {
        PrimaryElement component = this.GetComponent<PrimaryElement>();
        this.remainingTileDamage = this.DamageTiles(cell2, cell3, this.remainingTileDamage);
        if ((double) this.remainingTileDamage <= 0.0)
        {
          this.Explode(position, cell2, cell3, component.Element);
          this.hasExploded = true;
          Util.KDestroyGameObject(this.gameObject);
          return;
        }
      }
      else
        this.DamageThings(position, cell2, this.entityDamage);
    }
    this.previousPosition = position;
    this.transform.SetPosition(vector3);
  }

  private void PlayImpactSound(Vector3 pos)
  {
    if (this.impactSound == null)
      this.impactSound = "Meteor_Large_Impact";
    this.loopingSounds.StopSound(this.flyingSound);
    string sound = GlobalAssets.GetSound(this.impactSound);
    if (!CameraController.Instance.IsAudibleSound(pos, (HashedString) sound))
      return;
    float volume = this.GetVolume(this.gameObject);
    pos.z = 0.0f;
    EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
    int num = (int) instance.setParameterValue("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
    KFMOD.EndOneShot(instance);
  }

  private void StartLoopingSound()
  {
    this.loopingSounds.StartSound(this.flyingSound);
    this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float) this.flyingSoundID);
  }
}
