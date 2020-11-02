// Decompiled with JetBrains decompiler
// Type: WorldDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using STRINGS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/WorldDamage")]
public class WorldDamage : KMonoBehaviour
{
  public KBatchedAnimController leakEffect;
  [SerializeField]
  private FMODAsset leakSound;
  [SerializeField]
  [EventRef]
  private string leakSoundMigrated;
  private float damageAmount = 0.0008333334f;
  private const float SPAWN_DELAY = 1f;
  private Dictionary<int, float> spawnTimes = new Dictionary<int, float>();
  private List<int> expiredCells = new List<int>();

  public static WorldDamage Instance { get; private set; }

  public static void DestroyInstance() => WorldDamage.Instance = (WorldDamage) null;

  protected override void OnPrefabInit() => WorldDamage.Instance = this;

  public void RestoreDamageToValue(int cell, float amount)
  {
    if ((double) Grid.Damage[cell] <= (double) amount)
      return;
    Grid.Damage[cell] = amount;
  }

  public float ApplyDamage(Sim.WorldDamageInfo damage_info) => this.ApplyDamage(damage_info.gameCell, this.damageAmount, damage_info.damageSourceOffset, (string) BUILDINGS.DAMAGESOURCES.LIQUID_PRESSURE, (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LIQUID_PRESSURE);

  public float ApplyDamage(
    int cell,
    float amount,
    int src_cell,
    string source_name = null,
    string pop_text = null)
  {
    float num1 = 0.0f;
    if (Grid.Solid[cell])
    {
      float num2 = Grid.Damage[cell];
      num1 = Mathf.Min(amount, 1f - num2);
      float b = num2 + amount;
      bool flag = (double) b > 0.150000005960464;
      if (flag)
      {
        GameObject go = Grid.Objects[cell, 9];
        if ((UnityEngine.Object) go != (UnityEngine.Object) null)
        {
          BuildingHP component = go.GetComponent<BuildingHP>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            int num3 = Mathf.RoundToInt(Mathf.Max((float) component.HitPoints - (1f - b) * (float) component.MaxHitPoints, 0.0f));
            go.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
            {
              damage = num3,
              source = source_name,
              popString = pop_text
            });
          }
        }
      }
      Grid.Damage[cell] = Mathf.Min(1f, b);
      if ((double) Grid.Damage[cell] >= 1.0)
        this.DestroyCell(cell);
      else if (Grid.IsValidCell(src_cell) & flag)
      {
        Element elem = Grid.Element[src_cell];
        if (elem.IsLiquid && (double) Grid.Mass[src_cell] > 1.0)
        {
          int offset = cell - src_cell;
          switch (offset)
          {
            case -1:
            case 1:
              int index = cell + offset;
              if (Grid.IsValidCell(index))
              {
                Element element = Grid.Element[index];
                if (!element.IsSolid && (!element.IsLiquid || element.id == elem.id && (double) Grid.Mass[index] <= 100.0) && (((int) Grid.Properties[index] & 2) == 0 && !this.spawnTimes.ContainsKey(index)))
                {
                  this.spawnTimes[index] = Time.realtimeSinceStartup;
                  int idx = (int) elem.idx;
                  float temperature = Grid.Temperature[src_cell];
                  this.StartCoroutine(this.DelayedSpawnFX(src_cell, index, offset, elem, idx, temperature));
                  break;
                }
                break;
              }
              break;
            default:
              if (offset == Grid.WidthInCells || offset == -Grid.WidthInCells)
                goto case -1;
              else
                break;
          }
        }
      }
    }
    return num1;
  }

  private void ReleaseGO(GameObject go) => go.DeleteObject();

  private IEnumerator DelayedSpawnFX(
    int src_cell,
    int dest_cell,
    int offset,
    Element elem,
    int idx,
    float temperature)
  {
    WorldDamage worldDamage = this;
    yield return (object) new WaitForSeconds(UnityEngine.Random.value * 0.25f);
    Vector3 posCcc = Grid.CellToPosCCC(dest_cell, Grid.SceneLayer.Front);
    GameObject gameObject = GameUtil.KInstantiate(worldDamage.leakEffect.gameObject, posCcc, Grid.SceneLayer.Front);
    KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
    component.TintColour = elem.substance.colour;
    component.onDestroySelf = new System.Action<GameObject>(worldDamage.ReleaseGO);
    SimMessages.AddRemoveSubstance(src_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, -1f, temperature, byte.MaxValue, 0);
    if (offset == -1)
    {
      component.Play((HashedString) "side");
      component.FlipX = true;
      component.enabled = false;
      component.enabled = true;
      gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.right * 0.5f);
      FallingWater.instance.AddParticle(dest_cell, (byte) idx, 1f, temperature, byte.MaxValue, 0, true);
    }
    else if (offset == Grid.WidthInCells)
    {
      gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.up * 0.5f);
      component.Play((HashedString) "floor");
      component.enabled = false;
      component.enabled = true;
      SimMessages.AddRemoveSubstance(dest_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, 1f, temperature, byte.MaxValue, 0);
    }
    else if (offset == -Grid.WidthInCells)
    {
      component.Play((HashedString) "ceiling");
      component.enabled = false;
      component.enabled = true;
      gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.up * 0.5f);
      FallingWater.instance.AddParticle(dest_cell, (byte) idx, 1f, temperature, byte.MaxValue, 0, true);
    }
    else
    {
      component.Play((HashedString) "side");
      component.enabled = false;
      component.enabled = true;
      gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.right * 0.5f);
      FallingWater.instance.AddParticle(dest_cell, (byte) idx, 1f, temperature, byte.MaxValue, 0, true);
    }
    if (CameraController.Instance.IsAudibleSound(gameObject.transform.GetPosition(), (HashedString) worldDamage.leakSoundMigrated))
      SoundEvent.PlayOneShot(worldDamage.leakSoundMigrated, gameObject.transform.GetPosition());
    yield return (object) null;
  }

  private void Update()
  {
    this.expiredCells.Clear();
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    foreach (KeyValuePair<int, float> spawnTime in this.spawnTimes)
    {
      if ((double) realtimeSinceStartup - (double) spawnTime.Value > 1.0)
        this.expiredCells.Add(spawnTime.Key);
    }
    foreach (int expiredCell in this.expiredCells)
      this.spawnTimes.Remove(expiredCell);
    this.expiredCells.Clear();
  }

  public void DestroyCell(int cell)
  {
    if (!Grid.Solid[cell])
      return;
    SimMessages.Dig(cell);
  }

  public void OnSolidStateChanged(int cell) => Grid.Damage[cell] = 0.0f;

  public void OnDigComplete(
    int cell,
    float mass,
    float temperature,
    byte element_idx,
    byte disease_idx,
    int disease_count)
  {
    Vector3 pos = Grid.CellToPos(cell, CellAlignment.RandomInternal, Grid.SceneLayer.Ore);
    Element element = ElementLoader.elements[(int) element_idx];
    Grid.Damage[cell] = 0.0f;
    WorldDamage.Instance.PlaySoundForSubstance(element, pos);
    float num = mass * 0.5f;
    if ((double) num <= 0.0)
      return;
    GameObject gameObject = element.substance.SpawnResource(pos, num, temperature, disease_idx, disease_count);
    if (!((UnityEngine.Object) gameObject.GetComponent<Pickupable>() != (UnityEngine.Object) null) || !WorldInventory.Instance.IsReachable(gameObject.GetComponent<Pickupable>()))
      return;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, Mathf.RoundToInt(num).ToString() + " " + element.name, gameObject.transform);
  }

  private void PlaySoundForSubstance(Element element, Vector3 pos)
  {
    string sound = GlobalAssets.GetSound("Break_" + (element.substance.GetMiningBreakSound() ?? (!element.HasTag(GameTags.RefinedMetal) ? (!element.HasTag(GameTags.Metal) ? "Rock" : "RawMetal") : "RefinedMetal")));
    if (!(bool) (UnityEngine.Object) CameraController.Instance || !CameraController.Instance.IsAudibleSound(pos, (HashedString) sound))
      return;
    KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos));
  }
}
