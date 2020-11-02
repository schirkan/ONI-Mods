// Decompiled with JetBrains decompiler
// Type: SandboxHeatTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SandboxHeatTool : BrushTool
{
  public static SandboxHeatTool instance;
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

  public static void DestroyInstance() => SandboxHeatTool.instance = (SandboxHeatTool) null;

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxHeatTool.instance = this;
    this.viewMode = OverlayModes.Temperature.ID;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.temperatureAdditiveSlider.row.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int recentlyAffectedCell in this.recentlyAffectedCells)
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, this.recentlyAffectedCellColor));
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos) => base.OnMouseMove(cursorPos);

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    if (this.recentlyAffectedCells.Contains(cell))
      return;
    this.recentlyAffectedCells.Add(cell);
    int index = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() => this.recentlyAffectedCells.Remove(cell)))).index;
    float num1 = Grid.Temperature[cell] + SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.TemperatureAdditive");
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        num1 -= 273.15f;
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        num1 -= 255.372f;
        break;
    }
    float num2 = Mathf.Clamp(num1, 1f, 9999f);
    int gameCell = cell;
    int id = (int) Grid.Element[cell].id;
    CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
    double num3 = (double) Grid.Mass[cell];
    double num4 = (double) num2;
    int num5 = index;
    int num6 = (int) Grid.DiseaseIdx[cell];
    int diseaseCount = Grid.DiseaseCount[cell];
    int callbackIdx = num5;
    SimMessages.ReplaceElement(gameCell, (SimHashes) id, sandBoxTool, (float) num3, (float) num4, (byte) num6, diseaseCount, callbackIdx);
  }
}
