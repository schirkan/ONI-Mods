// Decompiled with JetBrains decompiler
// Type: Unsealable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Unsealable")]
public class Unsealable : Workable
{
  [Serialize]
  public bool facingRight;
  [Serialize]
  public bool unsealed;

  private Unsealable()
  {
  }

  public override CellOffset[] GetOffsets(int cell) => this.facingRight ? OffsetGroups.RightOnly : OffsetGroups.LeftOnly;

  protected override void OnPrefabInit()
  {
    this.faceTargetWhenWorking = true;
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_door_poi_kanim")
    };
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(3f);
  }

  protected override void OnStartWork(Worker worker) => base.OnStartWork(worker);

  protected override void OnCompleteWork(Worker worker)
  {
    this.unsealed = true;
    base.OnCompleteWork(worker);
    Deconstructable component = this.GetComponent<Deconstructable>();
    if (!((Object) component != (Object) null))
      return;
    component.allowDeconstruction = true;
    Game.Instance.Trigger(1980521255, (object) this.gameObject);
  }
}
