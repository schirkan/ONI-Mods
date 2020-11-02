// Decompiled with JetBrains decompiler
// Type: ProcGenGame.Neighbors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

namespace ProcGenGame
{
  [SerializationConfig(MemberSerialization.OptOut)]
  public struct Neighbors
  {
    public TerrainCell n0;
    public TerrainCell n1;

    public Neighbors(TerrainCell a, TerrainCell b)
    {
      Debug.Assert(a != null && b != null, (object) "NULL Neighbor");
      this.n0 = a;
      this.n1 = b;
    }
  }
}
