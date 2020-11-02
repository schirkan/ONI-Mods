﻿// Decompiled with JetBrains decompiler
// Type: PrebuildTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PrebuildTool : InterfaceTool
{
  public static PrebuildTool Instance;
  private BuildingDef def;

  public static void DestroyInstance() => PrebuildTool.Instance = (PrebuildTool) null;

  protected override void OnPrefabInit() => PrebuildTool.Instance = this;

  protected override void OnActivateTool()
  {
    this.viewMode = this.def.ViewMode;
    base.OnActivateTool();
  }

  public void Activate(BuildingDef def, PlanScreen.RequirementsState reqState)
  {
    this.def = def;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    PrebuildToolHoverTextCard component = this.GetComponent<PrebuildToolHoverTextCard>();
    component.currentReqState = reqState;
    component.currentDef = def;
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    UISounds.PlaySound(UISounds.Sound.Negative);
    base.OnLeftClickDown(cursor_pos);
  }
}
