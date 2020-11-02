// Decompiled with JetBrains decompiler
// Type: TileTemperature
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TileTemperature")]
public class TileTemperature : KMonoBehaviour
{
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private KSelectable selectable;

  protected override void OnPrefabInit()
  {
    this.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(TileTemperature.OnGetTemperature);
    this.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(TileTemperature.OnSetTemperature);
    base.OnPrefabInit();
  }

  protected override void OnSpawn() => base.OnSpawn();

  private static float OnGetTemperature(PrimaryElement primary_element)
  {
    SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
    if (!((Object) component != (Object) null) || !component.IsReady())
      return primary_element.InternalTemperature;
    int cell = Grid.PosToCell(primary_element.transform.GetPosition());
    return Grid.Temperature[cell];
  }

  private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
    if ((Object) component != (Object) null && component.IsReady())
      Debug.LogWarning((object) "Only set a tile's temperature during initialization. Otherwise you should be modifying the cell via the sim!");
    else
      primary_element.InternalTemperature = temperature;
  }
}
