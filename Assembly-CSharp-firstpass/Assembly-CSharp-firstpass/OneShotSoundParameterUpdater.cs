// Decompiled with JetBrains decompiler
// Type: OneShotSoundParameterUpdater
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD.Studio;

public abstract class OneShotSoundParameterUpdater
{
  public HashedString parameter { get; private set; }

  public OneShotSoundParameterUpdater(HashedString parameter) => this.parameter = parameter;

  public virtual void Update(float dt)
  {
  }

  public abstract void Play(OneShotSoundParameterUpdater.Sound sound);

  public struct Sound
  {
    public EventInstance ev;
    public SoundDescription description;
    public HashedString path;
  }
}
