// Decompiled with JetBrains decompiler
// Type: RoleStationSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RoleStationSideScreen : SideScreenContent
{
  public GameObject content;
  private GameObject target;
  public LocText DescriptionText;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public override bool IsValidForTarget(GameObject target) => false;
}
