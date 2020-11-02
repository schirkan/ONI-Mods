// Decompiled with JetBrains decompiler
// Type: Database.RoomTypeCategories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Database
{
  public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
  {
    public RoomTypeCategory None;
    public RoomTypeCategory Food;
    public RoomTypeCategory Sleep;
    public RoomTypeCategory Recreation;
    public RoomTypeCategory Bathroom;
    public RoomTypeCategory Hospital;
    public RoomTypeCategory Industrial;
    public RoomTypeCategory Agricultural;
    public RoomTypeCategory Park;

    private RoomTypeCategory Add(string id, string name, string colorName)
    {
      RoomTypeCategory resource = new RoomTypeCategory(id, name, colorName);
      this.Add(resource);
      return resource;
    }

    public RoomTypeCategories(ResourceSet parent)
      : base(nameof (RoomTypeCategories), parent)
    {
      this.Initialize();
      this.None = this.Add(nameof (None), "", "roomNone");
      this.Food = this.Add(nameof (Food), "", "roomFood");
      this.Sleep = this.Add(nameof (Sleep), "", "roomSleep");
      this.Recreation = this.Add(nameof (Recreation), "", "roomRecreation");
      this.Bathroom = this.Add(nameof (Bathroom), "", "roomBathroom");
      this.Hospital = this.Add(nameof (Hospital), "", "roomHospital");
      this.Industrial = this.Add(nameof (Industrial), "", "roomIndustrial");
      this.Agricultural = this.Add(nameof (Agricultural), "", "roomAgricultural");
      this.Park = this.Add(nameof (Park), "", "roomPark");
    }
  }
}
