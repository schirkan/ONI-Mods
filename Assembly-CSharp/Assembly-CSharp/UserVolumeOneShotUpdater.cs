// Decompiled with JetBrains decompiler
// Type: UserVolumeOneShotUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal abstract class UserVolumeOneShotUpdater : OneShotSoundParameterUpdater
{
  private string playerPref;

  public UserVolumeOneShotUpdater(string parameter, string player_pref)
    : base((HashedString) parameter)
    => this.playerPref = player_pref;

  public override void Play(OneShotSoundParameterUpdater.Sound sound)
  {
    if (string.IsNullOrEmpty(this.playerPref))
      return;
    float num1 = KPlayerPrefs.GetFloat(this.playerPref);
    int num2 = (int) sound.ev.setParameterValueByIndex(sound.description.GetParameterIdx(this.parameter), num1);
  }
}
