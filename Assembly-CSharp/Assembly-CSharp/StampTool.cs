// Decompiled with JetBrains decompiler
// Type: StampTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class StampTool : InterfaceTool
{
  public static StampTool Instance;
  public TemplateContainer stampTemplate;
  public GameObject PlacerPrefab;
  private bool ready = true;
  private int placementCell = Grid.InvalidCell;
  private bool selectAffected;
  private bool deactivateOnStamp;

  public static void DestroyInstance() => StampTool.Instance = (StampTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    StampTool.Instance = this;
  }

  public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
  {
    this.stampTemplate = template;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    this.selectAffected = SelectAffected;
    this.deactivateOnStamp = DeactivateOnStamp;
  }

  private void Update() => this.RefreshPreview(Grid.PosToCell(this.GetCursorPos()));

  private Vector3 GetCursorPos() => PlayerController.GetCursorPos(KInputManager.GetMousePos());

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    this.Stamp((Vector2) cursor_pos);
  }

  private void Stamp(Vector2 pos)
  {
    if (!this.ready)
      return;
    int cell1 = Grid.PosToCell(pos);
    Vector2f size = this.stampTemplate.info.size;
    int x1 = Mathf.FloorToInt((float) (-(double) size.X / 2.0));
    int cell2 = Grid.OffsetCell(cell1, x1, 0);
    int cell3 = Grid.PosToCell(pos);
    size = this.stampTemplate.info.size;
    int x2 = Mathf.FloorToInt(size.X / 2f);
    int cell4 = Grid.OffsetCell(cell3, x2, 0);
    int cell5 = Grid.PosToCell(pos);
    size = this.stampTemplate.info.size;
    int y1 = 1 + Mathf.FloorToInt((float) (-(double) size.Y / 2.0));
    int cell6 = Grid.OffsetCell(cell5, 0, y1);
    int cell7 = Grid.PosToCell(pos);
    size = this.stampTemplate.info.size;
    int y2 = 1 + Mathf.FloorToInt(size.Y / 2f);
    int cell8 = Grid.OffsetCell(cell7, 0, y2);
    if (!Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || (!Grid.IsValidBuildingCell(cell8) || !Grid.IsValidBuildingCell(cell6)))
      return;
    this.ready = false;
    bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause();
    List<GameObject> objects_to_destroy = new List<GameObject>();
    for (int index = 0; index < this.stampTemplate.cells.Count; ++index)
    {
      for (int layer = 0; layer < 34; ++layer)
      {
        GameObject gameObject = Grid.Objects[Grid.XYToCell((int) ((double) pos.x + (double) this.stampTemplate.cells[index].location_x), (int) ((double) pos.y + (double) this.stampTemplate.cells[index].location_y)), layer];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && !objects_to_destroy.Contains(gameObject))
          objects_to_destroy.Add(gameObject);
      }
    }
    TemplateLoader.Stamp(this.stampTemplate, pos, (System.Action) (() => this.CompleteStamp(pauseOnComplete, objects_to_destroy)));
    if (this.selectAffected)
    {
      DebugBaseTemplateButton.Instance.ClearSelection();
      for (int index = 0; index < this.stampTemplate.cells.Count; ++index)
        DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int) ((double) pos.x + (double) this.stampTemplate.cells[index].location_x), (int) ((double) pos.y + (double) this.stampTemplate.cells[index].location_y)));
    }
    if (!this.deactivateOnStamp)
      return;
    this.DeactivateTool();
  }

  private void CompleteStamp(bool pause, List<GameObject> objects_to_destroy = null)
  {
    if (objects_to_destroy != null)
    {
      foreach (GameObject original in objects_to_destroy)
      {
        if ((UnityEngine.Object) original != (UnityEngine.Object) null)
          Util.KDestroyGameObject(original);
      }
    }
    if (pause)
      SpeedControlScreen.Instance.Pause();
    this.ready = true;
    this.OnDeactivateTool((InterfaceTool) null);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.RefreshPreview(Grid.InvalidCell);
  }

  public void RefreshPreview(int new_placement_cell)
  {
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    foreach (TemplateClasses.Cell cell1 in this.stampTemplate.cells)
    {
      if (this.placementCell != Grid.InvalidCell)
      {
        int cell2 = Grid.OffsetCell(this.placementCell, new CellOffset(cell1.location_x, cell1.location_y));
        if (Grid.IsValidCell(cell2))
          intList1.Add(cell2);
      }
      if (new_placement_cell != Grid.InvalidCell)
      {
        int cell2 = Grid.OffsetCell(new_placement_cell, new CellOffset(cell1.location_x, cell1.location_y));
        if (Grid.IsValidCell(cell2))
          intList2.Add(cell2);
      }
    }
    this.placementCell = new_placement_cell;
    foreach (int cell in intList1)
    {
      if (!intList2.Contains(cell))
      {
        GameObject go = Grid.Objects[cell, 6];
        if ((UnityEngine.Object) go != (UnityEngine.Object) null)
          go.DeleteObject();
      }
    }
    foreach (int cell in intList2)
    {
      if (!intList1.Contains(cell) && (UnityEngine.Object) Grid.Objects[cell, 6] == (UnityEngine.Object) null)
      {
        GameObject gameObject = Util.KInstantiate(this.PlacerPrefab);
        Grid.Objects[cell, 6] = gameObject;
        Vector3 posCbc = Grid.CellToPosCBC(cell, this.visualizerLayer);
        float num = -0.15f;
        posCbc.z += num;
        gameObject.transform.SetPosition(posCbc);
      }
    }
  }
}
