// Decompiled with JetBrains decompiler
// Type: Klei.SaveFileRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei
{
  internal class SaveFileRoot
  {
    public int WidthInCells;
    public int HeightInCells;
    public Dictionary<string, byte[]> streamed;
    public string worldID;
    public List<ModInfo> requiredMods;
    public List<KMod.Label> active_mods;

    public SaveFileRoot() => this.streamed = new Dictionary<string, byte[]>();
  }
}
