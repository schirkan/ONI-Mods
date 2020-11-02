// Decompiled with JetBrains decompiler
// Type: SpeedOneShotUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
  public SpeedOneShotUpdater()
    : base((HashedString) "Speed")
  {
  }

  public override void Play(OneShotSoundParameterUpdater.Sound sound)
  {
    int num = (int) sound.ev.setParameterValueByIndex(sound.description.GetParameterIdx(this.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue());
  }
}
