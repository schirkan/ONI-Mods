// Decompiled with JetBrains decompiler
// Type: MiningSounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
  private static HashedString HASH_PERCENTCOMPLETE = (HashedString) "percentComplete";
  [MyCmpGet]
  private LoopingSounds loopingSounds;
  private FMODAsset miningSound;
  [EventRef]
  private string miningSoundEvent;
  private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>((System.Action<MiningSounds, object>) ((component, data) => component.OnStartMiningSound(data)));
  private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>((System.Action<MiningSounds, object>) ((component, data) => component.OnStopMiningSound(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<MiningSounds>(-1762453998, MiningSounds.OnStartMiningSoundDelegate);
    this.Subscribe<MiningSounds>(939543986, MiningSounds.OnStopMiningSoundDelegate);
  }

  private void OnStartMiningSound(object data)
  {
    if (!((UnityEngine.Object) this.miningSound == (UnityEngine.Object) null) || !(data is Element element))
      return;
    string miningSound = element.substance.GetMiningSound();
    if (miningSound == null || miningSound == "")
      return;
    this.miningSoundEvent = GlobalAssets.GetSound("Mine_" + miningSound);
    if (this.miningSoundEvent == null)
      return;
    this.loopingSounds.StartSound(this.miningSoundEvent);
  }

  private void OnStopMiningSound(object data)
  {
    if (this.miningSoundEvent == null)
      return;
    this.loopingSounds.StopSound(this.miningSoundEvent);
    this.miningSound = (FMODAsset) null;
  }

  public void SetPercentComplete(float progress) => this.loopingSounds.SetParameter(this.miningSoundEvent, MiningSounds.HASH_PERCENTCOMPLETE, progress);
}
