// Decompiled with JetBrains decompiler
// Type: MainCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MainCamera : MonoBehaviour
{
  private void Awake()
  {
    if ((Object) Camera.main != (Object) null)
      Object.Destroy((Object) Camera.main.gameObject);
    this.gameObject.tag = nameof (MainCamera);
  }
}
