﻿// Decompiled with JetBrains decompiler
// Type: NoisePolluter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter
{
  public const string ID = "NoisePolluter";
  public int radius;
  public int noise;
  public AttributeInstance dB;
  public AttributeInstance dBRadius;
  private NoiseSplat splat;
  public System.Action refreshCallback;
  public System.Action<object> refreshPartionerCallback;
  public System.Action<object> onCollectNoisePollutersCallback;
  public bool isMovable;
  [MyCmpReq]
  public OccupyArea occupyArea;
  private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<NoisePolluter>((System.Action<NoisePolluter, object>) ((component, data) => component.OnActiveChanged(data)));

  public static bool IsNoiseableCell(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    return Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell);
  }

  public void ResetCells()
  {
    if (this.radius != 0)
      return;
    Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", (object) this.GetName());
  }

  public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
  {
    this.sourceName = name;
    this.noise = dB;
  }

  public int GetRadius() => this.radius;

  public int GetNoise() => this.noise;

  public GameObject GetGameObject() => this.gameObject;

  public void SetSplat(NoiseSplat new_splat) => this.splat = new_splat;

  public void Clear()
  {
    if (this.splat == null)
      return;
    this.splat.Clear();
    this.splat = (NoiseSplat) null;
  }

  public Vector2 GetPosition() => (Vector2) this.transform.GetPosition();

  public string sourceName { get; private set; }

  public bool active { get; private set; }

  public void SetActive(bool active = true)
  {
    if (!active && this.splat != null)
    {
      AudioEventManager.Get().ClearNoiseSplat(this.splat);
      this.splat.Clear();
    }
    this.active = active;
  }

  public void Refresh()
  {
    if (!this.active)
      return;
    if (this.splat != null)
    {
      AudioEventManager.Get().ClearNoiseSplat(this.splat);
      this.splat.Clear();
    }
    KSelectable component = this.GetComponent<KSelectable>();
    string name = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetName() : this.name;
    GameObject gameObject = this.GetComponent<KMonoBehaviour>().gameObject;
    this.splat = AudioEventManager.Get().CreateNoiseSplat(this.GetPosition(), this.noise, this.radius, name, gameObject);
  }

  private void OnActiveChanged(object data)
  {
    this.SetActive(((Operational) data).IsActive);
    this.Refresh();
  }

  public void SetValues(EffectorValues values)
  {
    this.noise = values.amount;
    this.radius = values.radius;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.radius == 0 || this.noise == 0)
    {
      Debug.LogWarning((object) ("Noisepollutor::OnSpawn [" + this.GetName() + "] noise: [" + (object) this.noise + "] radius: [" + (object) this.radius + "]"));
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }
    else
    {
      this.ResetCells();
      Operational component1 = this.GetComponent<Operational>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        this.Subscribe<NoisePolluter>(824508782, NoisePolluter.OnActiveChangedDelegate);
      this.refreshCallback = new System.Action(this.Refresh);
      this.refreshPartionerCallback = (System.Action<object>) (data => this.Refresh());
      this.onCollectNoisePollutersCallback = new System.Action<object>(this.OnCollectNoisePolluters);
      Attributes attributes = this.GetAttributes();
      Db db = Db.Get();
      this.dB = attributes.Add(db.BuildingAttributes.NoisePollution);
      this.dBRadius = attributes.Add(db.BuildingAttributes.NoisePollutionRadius);
      if (this.noise != 0 && this.radius != 0)
      {
        AttributeModifier modifier1 = new AttributeModifier(db.BuildingAttributes.NoisePollution.Id, (float) this.noise, (string) UI.TOOLTIPS.BASE_VALUE);
        AttributeModifier modifier2 = new AttributeModifier(db.BuildingAttributes.NoisePollutionRadius.Id, (float) this.radius, (string) UI.TOOLTIPS.BASE_VALUE);
        attributes.Add(modifier1);
        attributes.Add(modifier2);
      }
      else
        Debug.LogWarning((object) ("Noisepollutor::OnSpawn [" + this.GetName() + "] radius: [" + (object) this.radius + "] noise: [" + (object) this.noise + "]"));
      KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
      this.isMovable = (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.isMovable;
      Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "NoisePolluter.OnSpawn");
      this.dB.OnDirty += this.refreshCallback;
      this.dBRadius.OnDirty += this.refreshCallback;
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
        return;
      this.OnActiveChanged((object) component1.IsActive);
    }
  }

  private void OnCellChange() => this.Refresh();

  private void OnCollectNoisePolluters(object data) => ((List<NoisePolluter>) data).Add(this);

  public string GetName()
  {
    if (string.IsNullOrEmpty(this.sourceName))
      this.sourceName = this.GetComponent<KSelectable>().GetName();
    return this.sourceName;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.isSpawned)
    {
      if (this.dB != null)
      {
        this.dB.OnDirty -= this.refreshCallback;
        this.dBRadius.OnDirty -= this.refreshCallback;
      }
      if (this.isMovable)
        Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    }
    if (this.splat == null)
      return;
    AudioEventManager.Get().ClearNoiseSplat(this.splat);
    this.splat.Clear();
  }

  public float GetNoiseForCell(int cell) => this.splat.GetDBForCell(cell);

  public List<Descriptor> GetEffectDescriptions()
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.dB != null && this.dBRadius != null)
    {
      float totalValue1 = this.dB.GetTotalValue();
      float totalValue2 = this.dBRadius.GetTotalValue();
      string format = (string) (this.noise > 0 ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE) + "\n\n" + this.dB.GetAttributeValueTooltip();
      string str = GameUtil.AddPositiveSign(totalValue1.ToString(), (double) totalValue1 > 0.0);
      Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.NOISE_CREATED, (object) str, (object) totalValue2), string.Format(format, (object) str, (object) totalValue2));
      descriptorList.Add(descriptor);
    }
    else if (this.noise != 0)
    {
      string format = (string) (this.noise >= 0 ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE);
      string str = GameUtil.AddPositiveSign(this.noise.ToString(), this.noise > 0);
      Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.NOISE_CREATED, (object) str, (object) this.radius), string.Format(format, (object) str, (object) this.radius));
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => this.GetEffectDescriptions();
}
