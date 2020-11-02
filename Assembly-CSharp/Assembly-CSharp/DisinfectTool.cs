// Decompiled with JetBrains decompiler
// Type: DisinfectTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DisinfectTool : DragTool
{
  public static DisinfectTool Instance;

  public static void DestroyInstance() => DisinfectTool.Instance = (DisinfectTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DisinfectTool.Instance = this;
    this.interceptNumberKeysForPriority = true;
    this.viewMode = OverlayModes.Disease.ID;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    for (int layer = 0; layer < 40; ++layer)
    {
      GameObject gameObject = Grid.Objects[cell, layer];
      if ((Object) gameObject != (Object) null)
      {
        Disinfectable component = gameObject.GetComponent<Disinfectable>();
        if ((Object) component != (Object) null && component.GetComponent<PrimaryElement>().DiseaseCount > 0)
          component.MarkForDisinfect();
      }
    }
  }
}
