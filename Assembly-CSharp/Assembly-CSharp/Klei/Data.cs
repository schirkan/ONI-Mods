// Decompiled with JetBrains decompiler
// Type: Klei.Data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using VoronoiTree;

namespace Klei
{
  public class Data
  {
    public int globalWorldSeed;
    public int globalWorldLayoutSeed;
    public int globalTerrainSeed;
    public int globalNoiseSeed;
    public int chunkEdgeSize = 32;
    public Vector2I subWorldSize = new Vector2I(512, 256);
    public WorldLayout worldLayout;
    public List<TerrainCell> terrainCells;
    public List<TerrainCell> overworldCells;
    public List<ProcGen.River> rivers;
    public GameSpawnData gameSpawnData;
    public Chunk world;
    public Tree voronoiTree;

    public Data()
    {
      this.worldLayout = new WorldLayout((WorldGen) null, 0);
      this.terrainCells = new List<TerrainCell>();
      this.overworldCells = new List<TerrainCell>();
      this.rivers = new List<ProcGen.River>();
      this.gameSpawnData = new GameSpawnData();
      this.world = new Chunk();
      this.voronoiTree = new Tree(0);
    }
  }
}
