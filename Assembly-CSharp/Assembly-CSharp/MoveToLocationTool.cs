﻿// Decompiled with JetBrains decompiler
// Type: MoveToLocationTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MoveToLocationTool : InterfaceTool
{
  public static MoveToLocationTool Instance;
  private Navigator targetNavigator;

  public static void DestroyInstance() => MoveToLocationTool.Instance = (MoveToLocationTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    MoveToLocationTool.Instance = this;
    this.visualizer = Util.KInstantiate(this.visualizer);
  }

  public void Activate(Navigator navigator)
  {
    this.targetNavigator = navigator;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  public bool CanMoveTo(int target_cell) => this.targetNavigator.CanReach(target_cell);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.visualizer.gameObject.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.visualizer.gameObject.SetActive(false);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    if (!((Object) this.targetNavigator != (Object) null))
      return;
    int mouseCell = DebugHandler.GetMouseCell();
    MoveToLocationMonitor.Instance smi = this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>();
    if (this.CanMoveTo(mouseCell) && smi != null)
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
      smi.MoveToLocation(mouseCell);
      SelectTool.Instance.Activate();
    }
    else
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
  }

  private void RefreshColor()
  {
    Color c = new Color(0.91f, 0.21f, 0.2f);
    if (this.CanMoveTo(DebugHandler.GetMouseCell()))
      c = Color.white;
    this.SetColor(this.visualizer, c);
  }

  public override void OnMouseMove(Vector3 cursor_pos)
  {
    base.OnMouseMove(cursor_pos);
    this.RefreshColor();
  }

  private void SetColor(GameObject root, Color c) => root.GetComponentInChildren<MeshRenderer>().material.color = c;
}
