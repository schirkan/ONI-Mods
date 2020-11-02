// Decompiled with JetBrains decompiler
// Type: SoundDescription
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public struct SoundDescription
{
  public string path;
  public float falloffDistanceSq;
  public SoundDescription.Parameter[] parameters;
  public OneShotSoundParameterUpdater[] oneShotParameterUpdaters;

  public int GetParameterIdx(HashedString name)
  {
    foreach (SoundDescription.Parameter parameter in this.parameters)
    {
      if (parameter.name == name)
        return parameter.idx;
    }
    return -1;
  }

  public struct Parameter
  {
    public HashedString name;
    public int idx;
    public const int INVALID_IDX = -1;
  }
}
