// Decompiled with JetBrains decompiler
// Type: DestroyAfter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/DestroyAfter")]
public class DestroyAfter : KMonoBehaviour
{
  private ParticleSystem[] particleSystems;

  protected override void OnSpawn() => this.particleSystems = this.gameObject.GetComponentsInChildren<ParticleSystem>(true);

  private bool IsAlive()
  {
    for (int index = 0; index < this.particleSystems.Length; ++index)
    {
      if (this.particleSystems[index].IsAlive(false))
        return true;
    }
    return false;
  }

  private void Update()
  {
    if (this.particleSystems == null || this.IsAlive())
      return;
    this.DeleteObject();
  }
}
