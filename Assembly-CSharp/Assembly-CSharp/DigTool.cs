// Decompiled with JetBrains decompiler
// Type: DigTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DigTool : DragTool
{
  public static DigTool Instance;

  public static void DestroyInstance() => DigTool.Instance = (DigTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DigTool.Instance = this;
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (!Grid.Solid[cell])
    {
      foreach (Uprootable uprootable in Components.Uprootables.Items)
      {
        if (Grid.PosToCell(uprootable.gameObject) == cell)
        {
          uprootable.MarkForUproot();
          break;
        }
        OccupyArea area = uprootable.area;
        if ((Object) area != (Object) null && area.CheckIsOccupying(cell))
          uprootable.MarkForUproot();
      }
    }
    if (DebugHandler.InstantBuildMode)
    {
      if (!Grid.IsValidCell(cell) || !Grid.Solid[cell] || Grid.Foundation[cell])
        return;
      WorldDamage.Instance.DestroyCell(cell);
    }
    else
    {
      GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
      if (!((Object) gameObject != (Object) null))
        return;
      Prioritizable component = gameObject.GetComponent<Prioritizable>();
      if (!((Object) component != (Object) null))
        return;
      component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
    }
  }

  public static GameObject PlaceDig(int cell, int animationDelay = 0)
  {
    if (Grid.Solid[cell] && !Grid.Foundation[cell] && (Object) Grid.Objects[cell, 7] == (Object) null)
    {
      for (int layer = 0; layer < 40; ++layer)
      {
        if ((Object) Grid.Objects[cell, layer] != (Object) null && (Object) Grid.Objects[cell, layer].GetComponent<Constructable>() != (Object) null)
          return (GameObject) null;
      }
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")));
      gameObject.SetActive(true);
      Grid.Objects[cell, 7] = gameObject;
      Vector3 posCbc = Grid.CellToPosCBC(cell, DigTool.Instance.visualizerLayer);
      float num = -0.15f;
      posCbc.z += num;
      gameObject.transform.SetPosition(posCbc);
      gameObject.GetComponentInChildren<EasingAnimations>().PlayAnimation("ScaleUp", Mathf.Max(0.0f, (float) animationDelay * 0.02f));
      return gameObject;
    }
    return (Object) Grid.Objects[cell, 7] != (Object) null ? Grid.Objects[cell, 7] : (GameObject) null;
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    ToolMenu.Instance.PriorityScreen.Show();
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    ToolMenu.Instance.PriorityScreen.Show(false);
  }
}
