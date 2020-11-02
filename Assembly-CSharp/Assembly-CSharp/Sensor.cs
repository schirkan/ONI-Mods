// Decompiled with JetBrains decompiler
// Type: Sensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sensor
{
  protected Sensors sensors;

  public string Name { get; private set; }

  public Sensor(Sensors sensors)
  {
    this.sensors = sensors;
    this.Name = this.GetType().Name;
  }

  public ComponentType GetComponent<ComponentType>() => this.sensors.GetComponent<ComponentType>();

  public GameObject gameObject => this.sensors.gameObject;

  public Transform transform => this.gameObject.transform;

  public void Trigger(int hash, object data = null) => this.sensors.Trigger(hash, data);

  public virtual void Update()
  {
  }

  public virtual void ShowEditor()
  {
  }
}
