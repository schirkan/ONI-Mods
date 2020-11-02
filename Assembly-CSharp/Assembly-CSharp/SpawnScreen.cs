// Decompiled with JetBrains decompiler
// Type: SpawnScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SpawnScreen")]
public class SpawnScreen : KMonoBehaviour
{
  public GameObject Screen;

  protected override void OnPrefabInit() => Util.KInstantiateUI(this.Screen, this.gameObject);
}
