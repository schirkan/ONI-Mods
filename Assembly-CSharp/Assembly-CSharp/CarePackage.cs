// Decompiled with JetBrains decompiler
// Type: CarePackage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

public class CarePackage : StateMachineComponent<CarePackage.SMInstance>
{
  [Serialize]
  public CarePackageInfo info;
  private Reactable reactable;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    if (this.info != null)
      this.SetAnimToInfo();
    this.reactable = this.CreateReactable();
  }

  public Reactable CreateReactable() => (Reactable) new EmoteReactable(this.gameObject, (HashedString) "UpgradeFX", Db.Get().ChoreTypes.Emote, (HashedString) "anim_cheer_kanim").AddStep(new EmoteReactable.EmoteStep()
  {
    anim = (HashedString) "cheer_pre"
  }).AddStep(new EmoteReactable.EmoteStep()
  {
    anim = (HashedString) "cheer_loop"
  }).AddStep(new EmoteReactable.EmoteStep()
  {
    anim = (HashedString) "cheer_pst"
  });

  protected override void OnCleanUp()
  {
    this.reactable.Cleanup();
    base.OnCleanUp();
  }

  public void SetInfo(CarePackageInfo info)
  {
    this.info = info;
    this.SetAnimToInfo();
  }

  private void SetAnimToInfo()
  {
    GameObject prefab1 = Util.KInstantiate(Assets.GetPrefab("Meter".ToTag()), this.gameObject);
    GameObject prefab2 = Assets.GetPrefab((Tag) this.info.id);
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    KBatchedAnimController component2 = prefab2.GetComponent<KBatchedAnimController>();
    SymbolOverrideController component3 = prefab2.GetComponent<SymbolOverrideController>();
    KBatchedAnimController component4 = prefab1.GetComponent<KBatchedAnimController>();
    component4.transform.SetLocalPosition(Vector3.forward);
    component4.AnimFiles = component2.AnimFiles;
    component4.isMovable = true;
    component4.animWidth = component2.animWidth;
    component4.animHeight = component2.animHeight;
    if ((Object) component3 != (Object) null)
    {
      SymbolOverrideController prefab3 = SymbolOverrideControllerUtil.AddToPrefab(prefab1);
      foreach (SymbolOverrideController.SymbolEntry getSymbolOverride in component3.GetSymbolOverrides)
        prefab3.AddSymbolOverride(getSymbolOverride.targetSymbol, getSymbolOverride.sourceSymbol);
    }
    component4.initialAnim = component2.initialAnim;
    component4.initialMode = KAnim.PlayMode.Loop;
    KBatchedAnimTracker component5 = prefab1.GetComponent<KBatchedAnimTracker>();
    component5.controller = component1;
    component5.symbol = new HashedString("snapTO_object");
    component5.offset = new Vector3(0.0f, 0.5f, 0.0f);
    prefab1.SetActive(true);
    component1.SetSymbolVisiblity((KAnimHashedString) "snapTO_object", false);
    KAnimLink kanimLink = new KAnimLink((KAnimControllerBase) component1, (KAnimControllerBase) component4);
  }

  private void SpawnContents()
  {
    if (this.info == null)
    {
      Debug.LogWarning((object) "CarePackage has no data to spawn from. Probably a save from before the CarePackage info data was serialized.");
    }
    else
    {
      GameObject gameObject = (GameObject) null;
      GameObject prefab = Assets.GetPrefab((Tag) this.info.id);
      Element element = ElementLoader.GetElement(this.info.id.ToTag());
      Vector3 position = this.transform.position + Vector3.up / 2f;
      if (element == null && (Object) prefab != (Object) null)
      {
        for (int index = 0; (double) index < (double) this.info.quantity; ++index)
        {
          gameObject = Util.KInstantiate(prefab, position);
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(true);
        }
      }
      else if (element != null)
      {
        float quantity = this.info.quantity;
        gameObject = element.substance.SpawnResource(position, quantity, element.defaultValues.temperature, byte.MaxValue, 0, forceTemperature: true);
      }
      else
        Debug.LogWarning((object) ("Can't find spawnable thing from tag " + this.info.id));
      if (!((Object) gameObject != (Object) null))
        return;
      gameObject.SetActive(true);
    }
  }

  public class SMInstance : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.GameInstance
  {
    public List<Chore> activeUseChores;

    public SMInstance(CarePackage master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage>
  {
    public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State spawn;
    public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State open;
    public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State pst;
    public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State destroy;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.spawn;
      this.spawn.PlayAnim("portalbirth").OnAnimQueueComplete(this.open);
      this.open.PlayAnim("portalbirth_pst").QueueAnim("object_idle_loop").Exit((StateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State.Callback) (smi => smi.master.SpawnContents())).ScheduleGoTo(1f, (StateMachine.BaseState) this.pst);
      this.pst.PlayAnim("object_idle_pst").ScheduleGoTo(5f, (StateMachine.BaseState) this.destroy);
      this.destroy.Enter((StateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State.Callback) (smi => Util.KDestroyGameObject(smi.master.gameObject)));
    }
  }
}
