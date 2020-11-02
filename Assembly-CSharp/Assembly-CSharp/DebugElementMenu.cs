// Decompiled with JetBrains decompiler
// Type: DebugElementMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugElementMenu : KButtonMenu
{
  public static DebugElementMenu Instance;
  public GameObject root;

  protected override void OnPrefabInit()
  {
    DebugElementMenu.Instance = this;
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
  }

  public void Turnoff() => this.root.gameObject.SetActive(false);
}
