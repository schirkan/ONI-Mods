// Decompiled with JetBrains decompiler
// Type: GlassForge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class GlassForge : ComplexFabricator
{
  private Guid statusHandle;
  private static readonly EventSystem.IntraObjectHandler<GlassForge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<GlassForge>((System.Action<GlassForge, object>) ((component, data) => component.CheckPipes(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<GlassForge>(-2094018600, GlassForge.CheckPipesDelegate);
  }

  private void CheckPipes(object data)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    int cell = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), GlassForgeConfig.outPipeOffset);
    GameObject gameObject = Grid.Objects[cell, 16];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      if ((double) gameObject.GetComponent<PrimaryElement>().Element.highTemp > (double) ElementLoader.FindElementByHash(SimHashes.MoltenGlass).lowTemp)
        component.RemoveStatusItem(this.statusHandle);
      else
        this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt);
    }
    else
      component.RemoveStatusItem(this.statusHandle);
  }
}
