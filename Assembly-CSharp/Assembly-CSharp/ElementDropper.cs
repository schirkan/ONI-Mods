// Decompiled with JetBrains decompiler
// Type: ElementDropper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ElementDropper")]
public class ElementDropper : KMonoBehaviour
{
  [SerializeField]
  public Tag emitTag;
  [SerializeField]
  public float emitMass;
  [SerializeField]
  public Vector3 emitOffset = Vector3.zero;
  [MyCmpGet]
  private Storage storage;
  private static readonly EventSystem.IntraObjectHandler<ElementDropper> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ElementDropper>((System.Action<ElementDropper, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<ElementDropper>(-1697596308, ElementDropper.OnStorageChangedDelegate);
  }

  private void OnStorageChanged(object data)
  {
    GameObject first = this.storage.FindFirst(this.emitTag);
    if ((UnityEngine.Object) first == (UnityEngine.Object) null || (double) first.GetComponent<PrimaryElement>().Mass < (double) this.emitMass)
      return;
    Pickupable component = first.GetComponent<Pickupable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component = component.Take(this.emitMass);
      component.transform.SetPosition(component.transform.GetPosition() + this.emitOffset);
      component.transform.parent = (Transform) null;
      this.Trigger(-1697596308, (object) component.gameObject);
      component.Trigger(856640610, (object) null);
    }
    else
    {
      this.storage.Drop(first);
      first.transform.SetPosition(first.transform.GetPosition() + this.emitOffset);
    }
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, component.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(component.TotalAmount), component.transform);
  }
}
