// Decompiled with JetBrains decompiler
// Type: Baggable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Baggable")]
public class Baggable : KMonoBehaviour
{
  [SerializeField]
  private KAnimFile minionAnimOverride;
  public bool mustStandOntopOfTrapForPickup;
  [Serialize]
  public bool wrangled;
  public bool useGunForPickup;
  private static readonly EventSystem.IntraObjectHandler<Baggable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Baggable>((System.Action<Baggable, object>) ((component, data) => component.OnStore(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.minionAnimOverride = Assets.GetAnim((HashedString) "anim_restrain_creature_kanim");
    Pickupable pickupable1 = this.gameObject.AddOrGet<Pickupable>();
    pickupable1.workAnims = new HashedString[2]
    {
      new HashedString("capture"),
      new HashedString("pickup")
    };
    pickupable1.workAnimPlayMode = KAnim.PlayMode.Once;
    pickupable1.workingPstComplete = (HashedString[]) null;
    pickupable1.workingPstFailed = (HashedString[]) null;
    pickupable1.overrideAnims = new KAnimFile[1]
    {
      this.minionAnimOverride
    };
    pickupable1.trackOnPickup = false;
    pickupable1.useGunforPickup = this.useGunForPickup;
    pickupable1.synchronizeAnims = false;
    pickupable1.SetWorkTime(3f);
    if (this.mustStandOntopOfTrapForPickup)
    {
      Pickupable pickupable2 = pickupable1;
      CellOffset[] offsets = new CellOffset[2];
      offsets[1] = new CellOffset(0, -1);
      pickupable2.SetOffsets(offsets);
    }
    this.Subscribe<Baggable>(856640610, Baggable.OnStoreDelegate);
    if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.transform.parent.GetComponent<Trap>() != (UnityEngine.Object) null)
        this.GetComponent<KBatchedAnimController>().enabled = true;
      if ((UnityEngine.Object) this.transform.parent.GetComponent<EggIncubator>() != (UnityEngine.Object) null)
        this.wrangled = true;
    }
    if (!this.wrangled)
      return;
    this.SetWrangled();
  }

  private void OnStore(object data)
  {
    Storage cmp = data as Storage;
    if (((UnityEngine.Object) cmp != (UnityEngine.Object) null ? 1 : (data != null ? ((bool) data ? 1 : 0) : 0)) != 0)
    {
      this.gameObject.AddTag(GameTags.Creatures.Bagged);
      if (!(bool) (UnityEngine.Object) cmp || !cmp.HasTag(GameTags.Minion))
        return;
      this.SetVisible(false);
    }
    else
      this.Free();
  }

  private void SetVisible(bool visible)
  {
    KAnimControllerBase component1 = this.gameObject.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.enabled != visible)
      component1.enabled = visible;
    KSelectable component2 = this.gameObject.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || component2.enabled == visible)
      return;
    component2.enabled = visible;
  }

  public void SetWrangled()
  {
    this.wrangled = true;
    Navigator component = this.GetComponent<Navigator>();
    if ((bool) (UnityEngine.Object) component && component.IsValidNavType(NavType.Floor))
      component.SetCurrentNavType(NavType.Floor);
    this.gameObject.AddTag(GameTags.Creatures.Bagged);
  }

  public void Free()
  {
    this.gameObject.RemoveTag(GameTags.Creatures.Bagged);
    this.wrangled = false;
    this.SetVisible(true);
  }
}
