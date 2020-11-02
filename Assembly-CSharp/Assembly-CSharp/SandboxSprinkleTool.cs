// Decompiled with JetBrains decompiler
// Type: SandboxSprinkleTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SandboxSprinkleTool : BrushTool
{
  public static SandboxSprinkleTool instance;
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();

  public static void DestroyInstance() => SandboxSprinkleTool.instance = (SandboxSprinkleTool) null;

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxSprinkleTool.instance = this;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.noiseScaleSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.noiseDensitySlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float) this.settings.GetIntSetting("SandboxTools.BrushSize"));
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
    {
      Color color = new Color(this.recentAffectedCellColor[recentlyAffectedCell].r, this.recentAffectedCellColor[recentlyAffectedCell].g, this.recentAffectedCellColor[recentlyAffectedCell].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 5f), -1f, 1f, 0.1f, 0.2f));
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, color));
    }
    foreach (int cellsInRadiu in this.cellsInRadius)
    {
      if (this.recentlyAffectedCells.Contains(cellsInRadiu))
      {
        Color radiusIndicatorColor = this.radiusIndicatorColor;
        Color color1 = this.recentAffectedCellColor[cellsInRadiu];
        color1.a = 0.2f;
        Color color2 = new Color((float) (((double) radiusIndicatorColor.r + (double) color1.r) / 2.0), (float) (((double) radiusIndicatorColor.g + (double) color1.g) / 2.0), (float) (((double) radiusIndicatorColor.b + (double) color1.b) / 2.0), radiusIndicatorColor.a + (1f - radiusIndicatorColor.a) * color1.a);
        colors.Add(new ToolMenu.CellColorData(cellsInRadiu, color2));
      }
      else
        colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
    }
  }

  public override void SetBrushSize(int radius)
  {
    this.brushRadius = radius;
    this.brushOffsets.Clear();
    for (int x = 0; x < this.brushRadius * 2; ++x)
    {
      for (int y = 0; y < this.brushRadius * 2; ++y)
      {
        if ((double) Vector2.Distance(new Vector2((float) x, (float) y), new Vector2((float) this.brushRadius, (float) this.brushRadius)) < (double) this.brushRadius - 0.800000011920929)
        {
          Vector2 xy = (Vector2) Grid.CellToXY(Grid.OffsetCell(this.currentCell, x, y));
          float num = PerlinSimplexNoise.noise(xy.x / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), xy.y / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), Time.realtimeSinceStartup);
          if ((double) this.settings.GetFloatSetting("SandboxTools.NoiseScale") <= (double) num)
            this.brushOffsets.Add(new Vector2((float) (x - this.brushRadius), (float) (y - this.brushRadius)));
        }
      }
    }
  }

  private void Update() => this.OnMouseMove(Grid.CellToPos(this.currentCell));

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    this.SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
  }

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    this.recentlyAffectedCells.Add(cell);
    Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
    if (!this.recentAffectedCellColor.ContainsKey(cell))
      this.recentAffectedCellColor.Add(cell, (Color) element.substance.uiColour);
    else
      this.recentAffectedCellColor[cell] = (Color) element.substance.uiColour;
    int index1 = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() =>
    {
      this.recentlyAffectedCells.Remove(cell);
      this.recentAffectedCellColor.Remove(cell);
    }))).index;
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
    this.SetBrushSize(this.brushRadius);
  }
}
