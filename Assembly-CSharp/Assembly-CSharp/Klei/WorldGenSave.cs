// Decompiled with JetBrains decompiler
// Type: Klei.WorldGenSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei
{
  public class WorldGenSave
  {
    public Vector2I version;
    public Dictionary<string, object> stats;
    public Data data;
    public string worldID;
    public List<string> traitIDs;

    public WorldGenSave()
    {
      this.data = new Data();
      this.stats = new Dictionary<string, object>();
    }
  }
}
