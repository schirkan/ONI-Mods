// Decompiled with JetBrains decompiler
// Type: KAnimSequencer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimSequencer")]
public class KAnimSequencer : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  public bool autoRun;
  [Serialize]
  public KAnimSequencer.KAnimSequence[] sequence = new KAnimSequencer.KAnimSequence[0];
  private int currentIndex;
  private KBatchedAnimController kbac;
  private MinionBrain mb;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.kbac = this.GetComponent<KBatchedAnimController>();
    this.mb = this.GetComponent<MinionBrain>();
    if (!this.autoRun)
      return;
    this.PlaySequence();
  }

  public void Reset() => this.currentIndex = 0;

  public void PlaySequence()
  {
    if (this.sequence == null || this.sequence.Length == 0)
      return;
    if ((UnityEngine.Object) this.mb != (UnityEngine.Object) null)
      this.mb.Suspend("AnimSequencer");
    this.kbac.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.PlayNext);
    this.PlayNext((HashedString) (string) null);
  }

  private void PlayNext(HashedString name)
  {
    if (this.sequence.Length > this.currentIndex)
    {
      this.kbac.Play(new HashedString(this.sequence[this.currentIndex].anim), this.sequence[this.currentIndex].mode, this.sequence[this.currentIndex].speed);
      ++this.currentIndex;
    }
    else
    {
      this.kbac.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.PlayNext);
      if (!((UnityEngine.Object) this.mb != (UnityEngine.Object) null))
        return;
      this.mb.Resume("AnimSequencer");
    }
  }

  [SerializationConfig(MemberSerialization.OptOut)]
  [Serializable]
  public class KAnimSequence
  {
    public string anim;
    public float speed = 1f;
    public KAnim.PlayMode mode = KAnim.PlayMode.Once;
  }
}
