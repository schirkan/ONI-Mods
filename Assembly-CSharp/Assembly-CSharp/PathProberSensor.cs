// Decompiled with JetBrains decompiler
// Type: PathProberSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class PathProberSensor : Sensor
{
  private Navigator navigator;

  public PathProberSensor(Sensors sensors)
    : base(sensors)
    => this.navigator = sensors.GetComponent<Navigator>();

  public override void Update() => this.navigator.UpdateProbe();
}
