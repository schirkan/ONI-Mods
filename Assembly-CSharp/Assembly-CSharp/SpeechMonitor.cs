// Decompiled with JetBrains decompiler
// Type: SpeechMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
  public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;
  public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State talking;
  public static string PREFIX_SAD = "sad";
  public static string PREFIX_HAPPY = "happy";
  public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;
  private static HashedString HASH_SNAPTO_MOUTH = (HashedString) "snapto_mouth";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
    this.satisfied.DoNothing();
    this.talking.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.BeginTalking)).Update(new System.Action<SpeechMonitor.Instance, float>(SpeechMonitor.UpdateTalking), UpdateRate.RENDER_EVERY_TICK).Target(this.mouth).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.EndTalking));
  }

  private static void CreateMouth(SpeechMonitor.Instance smi)
  {
    smi.mouth = Util.KInstantiate(Assets.GetPrefab((Tag) MouthAnimation.ID)).GetComponent<KBatchedAnimController>();
    smi.mouth.gameObject.SetActive(true);
    smi.sm.mouth.Set(smi.mouth.gameObject, smi);
  }

  private static void DestroyMouth(SpeechMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) smi.mouth != (UnityEngine.Object) null))
      return;
    Util.KDestroyGameObject((Component) smi.mouth);
    smi.mouth = (KBatchedAnimController) null;
  }

  private static string GetRandomSpeechAnim(string speech_prefix) => speech_prefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString();

  public static bool IsAllowedToPlaySpeech(GameObject go)
  {
    if (go.HasTag(GameTags.Dead))
      return false;
    if (go.GetComponent<Navigator>().IsMoving())
      return true;
    KAnim.Anim currentAnim = go.GetComponent<KBatchedAnimController>().GetCurrentAnim();
    return currentAnim == null || GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim);
  }

  public static void BeginTalking(SpeechMonitor.Instance smi)
  {
    smi.ev.clearHandle();
    if (smi.voiceEvent != null)
      smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.GetComponent<KBatchedAnimController>(), 0.0f, false);
    if (smi.ev.isValid())
    {
      smi.mouth.Play((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix));
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix));
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix));
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix));
    }
    else
    {
      smi.mouth.Play((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix));
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix));
    }
    SpeechMonitor.UpdateTalking(smi, 0.0f);
  }

  public static void EndTalking(SpeechMonitor.Instance smi) => smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);

  public static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
  {
    KAnim.Anim.FrameElement frameElement1 = new KAnim.Anim.FrameElement();
    frameElement1.symbol = (KAnimHashedString) HashedString.Invalid;
    int currentFrameIndex = controller.GetCurrentFrameIndex();
    KAnimBatch batch = controller.GetBatch();
    if (currentFrameIndex == -1 || batch == null)
      return frameElement1;
    KAnim.Anim.Frame frame = controller.GetBatch().group.data.GetFrame(currentFrameIndex);
    if (frame == KAnim.Anim.Frame.InvalidFrame)
      return frameElement1;
    for (int index1 = 0; index1 < frame.numElements; ++index1)
    {
      int index2 = frame.firstElementIdx + index1;
      if (index2 < batch.group.data.frameElements.Count)
      {
        KAnim.Anim.FrameElement frameElement2 = batch.group.data.frameElements[index2];
        if (!(frameElement2.symbol == HashedString.Invalid))
        {
          frameElement1 = frameElement2;
          break;
        }
      }
    }
    return frameElement1;
  }

  public static void UpdateTalking(SpeechMonitor.Instance smi, float dt)
  {
    if (smi.ev.isValid())
    {
      PLAYBACK_STATE state;
      int playbackState = (int) smi.ev.getPlaybackState(out state);
      if (state != PLAYBACK_STATE.PLAYING && state != PLAYBACK_STATE.STARTING)
      {
        smi.GoTo((StateMachine.BaseState) smi.sm.satisfied);
        smi.ev.clearHandle();
        return;
      }
    }
    KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(smi.mouth);
    if (firstFrameElement.symbol == HashedString.Invalid)
      return;
    smi.Get<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Tuning : TuningData<SpeechMonitor.Tuning>
  {
    public float randomSpeechIntervalMin;
    public float randomSpeechIntervalMax;
    public int speechCount;
  }

  public new class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
  {
    public KBatchedAnimController mouth;
    public string speechPrefix = "happy";
    public string voiceEvent;
    public EventInstance ev;

    public Instance(IStateMachineTarget master, SpeechMonitor.Def def)
      : base(master, def)
    {
    }

    public bool IsPlayingSpeech() => this.IsInsideState((StateMachine.BaseState) this.sm.talking);

    public void PlaySpeech(string speech_prefix, string voice_event)
    {
      this.speechPrefix = speech_prefix;
      this.voiceEvent = voice_event;
      this.GoTo((StateMachine.BaseState) this.sm.talking);
    }

    public void DrawMouth()
    {
      KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(this.smi.mouth);
      if (firstFrameElement.symbol == HashedString.Invalid)
        return;
      KAnim.Build.Symbol symbol1 = this.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol);
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      this.GetComponent<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, this.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
      KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID).GetSymbol((KAnimHashedString) SpeechMonitor.HASH_SNAPTO_MOUTH);
      KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(symbol1.build.batchTag).symbolFrameInstances[symbol1.firstFrameIdx + firstFrameElement.frame];
      symbolFrameInstance.buildImageIdx = this.GetComponent<SymbolOverrideController>().GetAtlasIdx(symbol1.build.GetTexture(0));
      component.SetSymbolOverride(symbol2.firstFrameIdx, symbolFrameInstance);
    }
  }
}
