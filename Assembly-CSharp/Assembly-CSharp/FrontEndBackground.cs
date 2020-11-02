// Decompiled with JetBrains decompiler
// Type: FrontEndBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class FrontEndBackground : UIDupeRandomizer
{
  private KBatchedAnimController dreckoController;
  private float nextDreckoTime;
  private FrontEndBackground.Tuning tuning;
  [NonSerialized]
  public Camera baseCamera;

  protected override void Start()
  {
    this.tuning = TuningData<FrontEndBackground.Tuning>.Get();
    this.SetupCameras();
    base.Start();
    for (int minion_idx = 0; minion_idx < this.anims.Length; ++minion_idx)
    {
      int minionIndex = minion_idx;
      this.anims[minion_idx].minions[0].onAnimComplete += (KAnimControllerBase.KAnimEvent) (name => this.WaitForABit(minionIndex, name));
      this.WaitForABit(minion_idx, HashedString.Invalid);
    }
    this.dreckoController = this.transform.GetChild(0).Find("startmenu_drecko").GetComponent<KBatchedAnimController>();
    this.dreckoController.enabled = false;
    this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minFirstDreckoInterval, this.tuning.maxFirstDreckoInterval) + Time.unscaledTime;
  }

  protected override void Update()
  {
    base.Update();
    this.UpdateDrecko();
  }

  private void UpdateDrecko()
  {
    if ((double) Time.unscaledTime <= (double) this.nextDreckoTime)
      return;
    this.dreckoController.enabled = true;
    this.dreckoController.Play((HashedString) "idle");
    this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minDreckoInterval, this.tuning.maxDreckoInterval) + Time.unscaledTime;
  }

  private void WaitForABit(int minion_idx, HashedString name) => this.StartCoroutine(this.WaitForTime(minion_idx));

  private IEnumerator WaitForTime(int minion_idx)
  {
    FrontEndBackground frontEndBackground = this;
    frontEndBackground.anims[minion_idx].lastWaitTime = UnityEngine.Random.Range(frontEndBackground.anims[minion_idx].minSecondsBetweenAction, frontEndBackground.anims[minion_idx].maxSecondsBetweenAction);
    yield return (object) new WaitForSecondsRealtime(frontEndBackground.anims[minion_idx].lastWaitTime);
    frontEndBackground.GetNewBody(minion_idx);
    foreach (KBatchedAnimController minion in frontEndBackground.anims[minion_idx].minions)
    {
      minion.ClearQueue();
      minion.Play((HashedString) frontEndBackground.anims[minion_idx].anim_name);
    }
  }

  private void SetupCameras()
  {
    GameObject gameObject = new GameObject();
    gameObject.name = "Cameras";
    gameObject.transform.parent = this.transform.parent;
    Util.Reset(gameObject.transform);
    this.baseCamera = this.GetComponentInChildren<Camera>();
    this.baseCamera.name = "BaseCamera";
    this.baseCamera.transform.SetParent(gameObject.transform);
    this.baseCamera.transparencySortMode = TransparencySortMode.Orthographic;
    this.baseCamera.tag = "Untagged";
  }

  public class Tuning : TuningData<FrontEndBackground.Tuning>
  {
    public float minDreckoInterval;
    public float maxDreckoInterval;
    public float minFirstDreckoInterval;
    public float maxFirstDreckoInterval;
  }
}
