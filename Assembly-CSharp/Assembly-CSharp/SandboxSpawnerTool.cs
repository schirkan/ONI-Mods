﻿// Decompiled with JetBrains decompiler
// Type: SandboxSpawnerTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SandboxSpawnerTool : InterfaceTool
{
  protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
  private int currentCell;

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    this.currentCell = Grid.PosToCell(cursorPos);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos) => this.Place(Grid.PosToCell(cursor_pos));

  private void Place(int cell)
  {
    if (!Grid.IsValidBuildingCell(cell))
      return;
    string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
    GameObject prefab = Assets.GetPrefab((Tag) stringSetting);
    if (stringSetting == MinionConfig.ID)
      this.SpawnMinion();
    else if ((Object) prefab.GetComponent<Building>() != (Object) null)
    {
      BuildingDef def = prefab.GetComponent<Building>().Def;
      def.Build(cell, Orientation.Neutral, (Storage) null, (IList<Tag>) def.DefaultElements(), 298.15f);
    }
    else
      GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(this.currentCell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures).SetActive(true);
    UISounds.PlaySound(UISounds.Sound.ClickObject);
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.entitySelector.row.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
  }

  private void SpawnMinion()
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) MinionConfig.ID));
    gameObject.name = Assets.GetPrefab((Tag) MinionConfig.ID).name;
    Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
    Vector3 posCbc = Grid.CellToPosCBC(this.currentCell, Grid.SceneLayer.Move);
    gameObject.transform.SetLocalPosition(posCbc);
    gameObject.SetActive(true);
    new MinionStartingStats(false).Apply(gameObject);
  }
}
