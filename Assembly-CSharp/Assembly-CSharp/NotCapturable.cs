// Decompiled with JetBrains decompiler
// Type: NotCapturable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/NotCapturable")]
public class NotCapturable : KMonoBehaviour
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if ((Object) this.GetComponent<Capturable>() != (Object) null)
      DebugUtil.LogErrorArgs((Object) this, (object) "Entity has both Capturable and NotCapturable!");
    Components.NotCapturables.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.NotCapturables.Remove(this);
    base.OnCleanUp();
  }
}
