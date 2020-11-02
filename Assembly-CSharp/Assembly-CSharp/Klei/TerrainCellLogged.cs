// Decompiled with JetBrains decompiler
// Type: Klei.TerrainCellLogged
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGenGame;
using VoronoiTree;

namespace Klei
{
  public class TerrainCellLogged : TerrainCell
  {
    public TerrainCellLogged()
    {
    }

    public TerrainCellLogged(ProcGen.Node node, Diagram.Site site)
      : base(node, site)
    {
    }

    public override void LogInfo(string evt, string param, float value)
    {
    }
  }
}
