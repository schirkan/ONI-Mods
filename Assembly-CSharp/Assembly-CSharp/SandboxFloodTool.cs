// Decompiled with JetBrains decompiler
// Type: SandboxFloodTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SandboxFloodTool : FloodTool
{
  public static SandboxFloodTool instance;
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected HashSet<int> cellsToAffect = new HashSet<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

  public static void DestroyInstance() => SandboxFloodTool.instance = (SandboxFloodTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxFloodTool.instance = this;
    this.floodCriteria = (Func<int, bool>) (cell => Grid.IsValidCell(cell) && Grid.Element[cell] == Grid.Element[this.mouseCell]);
    this.paintArea = (System.Action<HashSet<int>>) (cells =>
    {
      foreach (int cell in cells)
        this.PaintCell(cell);
    });
  }

  private void PaintCell(int cell)
  {
    this.recentlyAffectedCells.Add(cell);
    Game.CallbackInfo callbackInfo = new Game.CallbackInfo((System.Action) (() => this.recentlyAffectedCells.Remove(cell)));
    Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
    int index1 = Game.Instance.callbackManager.Add(callbackInfo).index;
    int gameCell = cell;
    int id = (int) element.id;
    CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
    double floatSetting1 = (double) this.settings.GetFloatSetting("SandboxTools.Mass");
    double floatSetting2 = (double) this.settings.GetFloatSetting("SandbosTools.Temperature");
    int num = index1;
    int index2 = (int) Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get(this.settings.GetStringSetting("SandboxTools.SelectedDisease")).id);
    int intSetting = this.settings.GetIntSetting("SandboxTools.DiseaseCount");
    int callbackIdx = num;
    SimMessages.ReplaceElement(gameCell, (SimHashes) id, sandBoxTool, (float) floatSetting1, (float) floatSetting2, (byte) index2, intSetting, callbackIdx);
  }

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
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
    foreach (int cell in this.cellsToAffect)
      colors.Add(new ToolMenu.CellColorData(cell, (Color) this.areaColour));
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    this.cellsToAffect = this.Flood(Grid.PosToCell(cursorPos));
  }
}
