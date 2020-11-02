// Decompiled with JetBrains decompiler
// Type: AmbientSoundManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AmbientSoundManager")]
public class AmbientSoundManager : KMonoBehaviour
{
  [MyCmpAdd]
  private LoopingSounds loopingSounds;

  public static AmbientSoundManager Instance { get; private set; }

  public static void Destroy() => AmbientSoundManager.Instance = (AmbientSoundManager) null;

  protected override void OnPrefabInit() => AmbientSoundManager.Instance = this;

  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    AmbientSoundManager.Instance = (AmbientSoundManager) null;
  }
}
