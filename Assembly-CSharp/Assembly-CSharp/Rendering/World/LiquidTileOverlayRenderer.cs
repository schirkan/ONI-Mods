﻿// Decompiled with JetBrains decompiler
// Type: Rendering.World.LiquidTileOverlayRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Rendering.World
{
  public class LiquidTileOverlayRenderer : TileRenderer
  {
    public static LiquidTileOverlayRenderer Instance;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
      LiquidTileOverlayRenderer.Instance = this;
    }

    protected override Mask[] GetMasks() => new Mask[3]
    {
      new Mask(this.Atlas, 0, false, false, false, false),
      new Mask(this.Atlas, 0, false, true, false, false),
      new Mask(this.Atlas, 1, false, false, false, false)
    };

    public void OnShadersReloaded()
    {
      foreach (Element element in ElementLoader.elements)
      {
        if (element.IsLiquid && element.substance != null && (UnityEngine.Object) element.substance.material != (UnityEngine.Object) null)
        {
          Material material = new Material(element.substance.material);
          this.InitAlphaMaterial(material, element);
          int idx = element.substance.idx;
          for (int index1 = 0; index1 < this.Masks.Length; ++index1)
          {
            int index2 = idx * this.Masks.Length + index1;
            element.substance.RefreshPropertyBlock();
            this.Brushes[index2].SetMaterial(material, element.substance.propertyBlock);
          }
        }
      }
    }

    public override void LoadBrushes()
    {
      this.Brushes = new Brush[ElementLoader.elements.Count * this.Masks.Length];
      foreach (Element element in ElementLoader.elements)
      {
        if (element.IsLiquid && element.substance != null && (UnityEngine.Object) element.substance.material != (UnityEngine.Object) null)
        {
          Material material = new Material(element.substance.material);
          this.InitAlphaMaterial(material, element);
          int idx = element.substance.idx;
          for (int index = 0; index < this.Masks.Length; ++index)
          {
            int id = idx * this.Masks.Length + index;
            element.substance.RefreshPropertyBlock();
            this.Brushes[id] = new Brush(id, element.id.ToString(), material, this.Masks[index], this.ActiveBrushes, this.DirtyBrushes, this.TileGridWidth, element.substance.propertyBlock);
          }
        }
      }
    }

    private void InitAlphaMaterial(Material alpha_material, Element element)
    {
      alpha_material.name = element.name;
      alpha_material.renderQueue = RenderQueues.BlockTiles + element.substance.idx;
      alpha_material.EnableKeyword("ALPHA");
      alpha_material.DisableKeyword("OPAQUE");
      alpha_material.SetTexture("_AlphaTestMap", (Texture) this.Atlas.texture);
      alpha_material.SetInt("_SrcAlpha", 5);
      alpha_material.SetInt("_DstAlpha", 10);
      alpha_material.SetInt("_ZWrite", 0);
      alpha_material.SetColor("_Colour", (Color) element.substance.colour);
    }

    private bool RenderLiquid(int cell, int cell_above)
    {
      bool flag = false;
      if (Grid.Element[cell].IsSolid)
      {
        Element element = Grid.Element[cell_above];
        if (element.IsLiquid && (UnityEngine.Object) element.substance.material != (UnityEngine.Object) null)
          flag = true;
      }
      return flag;
    }

    private void SetBrushIdx(
      int i,
      ref Tile tile,
      int substance_idx,
      LiquidTileOverlayRenderer.LiquidConnections connections,
      Brush[] brush_array,
      int[] brush_grid)
    {
      if (connections == LiquidTileOverlayRenderer.LiquidConnections.Empty)
      {
        brush_grid[tile.Idx * 4 + i] = -1;
      }
      else
      {
        int num = (int) connections;
        Brush brush = brush_array[substance_idx * tile.MaskCount + num - 1];
        brush.Add(tile.Idx);
        brush_grid[tile.Idx * 4 + i] = brush.Id;
      }
    }

    public override void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid)
    {
      if (this.RenderLiquid(tile.TileCells.Cell0, tile.TileCells.Cell2))
      {
        if (this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
          this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Both, brush_array, brush_grid);
        else
          this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Left, brush_array, brush_grid);
      }
      else
      {
        if (!this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
          return;
        this.SetBrushIdx(1, ref tile, Grid.Element[tile.TileCells.Cell3].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Right, brush_array, brush_grid);
      }
    }

    private enum LiquidConnections
    {
      Left = 1,
      Right = 2,
      Both = 3,
      Empty = 128, // 0x00000080
    }
  }
}
