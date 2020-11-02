// Decompiled with JetBrains decompiler
// Type: Klei.SimSaveFileStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei
{
  public class SimSaveFileStructure
  {
    public int WidthInCells;
    public int HeightInCells;
    public byte[] Sim;
    public WorldDetailSave worldDetail;

    public SimSaveFileStructure() => this.worldDetail = new WorldDetailSave();
  }
}
