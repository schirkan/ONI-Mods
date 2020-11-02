﻿// Decompiled with JetBrains decompiler
// Type: LoopingSoundParameterUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public abstract class LoopingSoundParameterUpdater
{
  public HashedString parameter { get; private set; }

  public LoopingSoundParameterUpdater(HashedString parameter) => this.parameter = parameter;

  public abstract void Add(LoopingSoundParameterUpdater.Sound sound);

  public abstract void Update(float dt);

  public abstract void Remove(LoopingSoundParameterUpdater.Sound sound);

  public struct Sound
  {
    public EventInstance ev;
    public HashedString path;
    public Transform transform;
    public SoundDescription description;
    public bool objectIsSelectedAndVisible;
  }
}
