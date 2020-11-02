﻿// Decompiled with JetBrains decompiler
// Type: Rendering.World.Tile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Rendering.World
{
  public struct Tile
  {
    public int Idx;
    public TileCells TileCells;
    public int MaskCount;

    public Tile(int idx, int tile_x, int tile_y, int mask_count)
    {
      this.Idx = idx;
      this.TileCells = new TileCells(tile_x, tile_y);
      this.MaskCount = mask_count;
    }
  }
}
