// Decompiled with JetBrains decompiler
// Type: OilFloaterMovementSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal class OilFloaterMovementSound : KMonoBehaviour
{
  public string sound;
  public bool isPlayingSound;
  public bool isMoving;
  private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>((System.Action<OilFloaterMovementSound, object>) ((component, data) => component.OnObjectMovementStateChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.sound = GlobalAssets.GetSound(this.sound);
    this.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged), nameof (OilFloaterMovementSound));
  }

  private void OnObjectMovementStateChanged(object data)
  {
    this.isMoving = (GameHashes) data == GameHashes.ObjectMovementWakeUp;
    this.UpdateSound();
  }

  private void OnCellChanged() => this.UpdateSound();

  private void UpdateSound()
  {
    bool flag = this.isMoving && this.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
    if (flag == this.isPlayingSound)
      return;
    LoopingSounds component = this.GetComponent<LoopingSounds>();
    if (flag)
      component.StartSound(this.sound);
    else
      component.StopSound(this.sound);
    this.isPlayingSound = flag;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged));
  }
}
