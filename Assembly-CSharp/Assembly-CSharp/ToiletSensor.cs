﻿// Decompiled with JetBrains decompiler
// Type: ToiletSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class ToiletSensor : Sensor
{
  private Navigator navigator;
  private IUsable toilet;
  private bool areThereAnyToilets;
  private bool areThereAnyUsableToilets;

  public ToiletSensor(Sensors sensors)
    : base(sensors)
    => this.navigator = this.GetComponent<Navigator>();

  public override void Update()
  {
    IUsable usable1 = (IUsable) null;
    int num = int.MaxValue;
    bool flag1 = false;
    foreach (IUsable usable2 in Components.Toilets.Items)
    {
      if (usable2.IsUsable())
      {
        flag1 = true;
        int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(usable2.transform.GetPosition()));
        if (navigationCost != -1 && navigationCost < num)
        {
          usable1 = usable2;
          num = navigationCost;
        }
      }
    }
    bool flag2 = Components.Toilets.Count > 0;
    if (usable1 == this.toilet && flag2 == this.areThereAnyToilets && this.areThereAnyUsableToilets == flag1)
      return;
    this.toilet = usable1;
    this.areThereAnyToilets = flag2;
    this.areThereAnyUsableToilets = flag1;
    this.Trigger(-752545459);
  }

  public bool AreThereAnyToilets() => this.areThereAnyToilets;

  public bool AreThereAnyUsableToilets() => this.areThereAnyUsableToilets;

  public IUsable GetNearestUsableToilet() => this.toilet;
}