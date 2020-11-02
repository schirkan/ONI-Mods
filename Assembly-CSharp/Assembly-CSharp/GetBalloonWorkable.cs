// Decompiled with JetBrains decompiler
// Type: GetBalloonWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
  private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString PST_ANIM = new HashedString("working_pst");
  private BalloonArtistChore.StatesInstance balloonArtist;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.faceTargetWhenWorking = true;
    this.workerStatusItem = (StatusItem) null;
    this.workingStatusItem = (StatusItem) null;
    this.workAnims = GetBalloonWorkable.GET_BALLOON_ANIMS;
    this.workingPstComplete = new HashedString[1]
    {
      GetBalloonWorkable.PST_ANIM
    };
    this.workingPstFailed = new HashedString[1]
    {
      GetBalloonWorkable.PST_ANIM
    };
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.balloonArtist.GiveBalloon();
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) "EquippableBalloon"), worker.transform.GetPosition());
    gameObject.GetComponent<Equippable>().Assign((IAssignableIdentity) worker.GetComponent<MinionIdentity>());
    gameObject.GetComponent<Equippable>().isEquipped = true;
    gameObject.SetActive(true);
    base.OnCompleteWork(worker);
  }

  public override Vector3 GetFacingTarget() => this.balloonArtist.master.transform.GetPosition();

  public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore) => this.balloonArtist = chore;
}
