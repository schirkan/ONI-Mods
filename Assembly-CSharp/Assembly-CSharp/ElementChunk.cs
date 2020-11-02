// Decompiled with JetBrains decompiler
// Type: ElementChunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ElementChunk")]
public class ElementChunk : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<ElementChunk> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<ElementChunk>((System.Action<ElementChunk, object>) ((component, data) => component.OnAbsorb(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GameComps.OreSizeVisualizers.Add(this.gameObject);
    GameComps.ElementSplitters.Add(this.gameObject);
    this.Subscribe<ElementChunk>(-2064133523, ElementChunk.OnAbsorbDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Vector3 position = this.transform.GetPosition();
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
    this.transform.SetPosition(position);
    Element element = this.GetComponent<PrimaryElement>().Element;
    KSelectable component = this.GetComponent<KSelectable>();
    Func<Element> func = (Func<Element>) (() => element);
    component.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, (object) func);
    component.AddStatusItem(Db.Get().MiscStatusItems.OreMass, (object) this.gameObject);
    component.AddStatusItem(Db.Get().MiscStatusItems.OreTemp, (object) this.gameObject);
  }

  protected override void OnCleanUp()
  {
    GameComps.ElementSplitters.Remove(this.gameObject);
    GameComps.OreSizeVisualizers.Remove(this.gameObject);
    base.OnCleanUp();
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
      return;
    PrimaryElement primaryElement = pickupable.PrimaryElement;
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    float mass1 = primaryElement.Mass;
    if ((double) mass1 > 0.0)
    {
      PrimaryElement component = this.GetComponent<PrimaryElement>();
      float mass2 = component.Mass;
      float temperature = (double) mass2 > 0.0 ? SimUtil.CalculateFinalTemperature(mass2, component.Temperature, mass1, primaryElement.Temperature) : primaryElement.Temperature;
      component.SetMassTemperature(mass2 + mass1, temperature);
    }
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null))
      return;
    string sound = GlobalAssets.GetSound("Ore_absorb");
    Vector3 position = pickupable.transform.GetPosition();
    position.z = 0.0f;
    if (sound == null || !CameraController.Instance.IsAudibleSound(position, (HashedString) sound))
      return;
    KFMOD.PlayOneShot(sound, position);
  }
}
