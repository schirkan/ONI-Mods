// Decompiled with JetBrains decompiler
// Type: PlantElementAbsorber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public struct PlantElementAbsorber
{
  public Storage storage;
  public PlantElementAbsorber.LocalInfo localInfo;
  public HandleVector<int>.Handle[] accumulators;
  public PlantElementAbsorber.ConsumeInfo[] consumedElements;

  public void Clear()
  {
    this.storage = (Storage) null;
    this.consumedElements = (PlantElementAbsorber.ConsumeInfo[]) null;
  }

  public struct ConsumeInfo
  {
    public Tag tag;
    public float massConsumptionRate;

    public ConsumeInfo(Tag tag, float mass_consumption_rate)
    {
      this.tag = tag;
      this.massConsumptionRate = mass_consumption_rate;
    }
  }

  public struct LocalInfo
  {
    public Tag tag;
    public float massConsumptionRate;
  }
}
