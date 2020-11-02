// Decompiled with JetBrains decompiler
// Type: GeneratedEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class GeneratedEquipment
{
  public static void LoadGeneratedEquipment()
  {
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new EquippableBalloonConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new AtmoSuitConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new JetSuitConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new WarmVestConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new CoolVestConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new FunkyVestConfig());
  }
}
