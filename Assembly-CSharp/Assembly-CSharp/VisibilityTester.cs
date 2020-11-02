// Decompiled with JetBrains decompiler
// Type: VisibilityTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/VisibilityTester")]
public class VisibilityTester : KMonoBehaviour
{
  public static VisibilityTester Instance;
  public bool enableTesting;

  public static void DestroyInstance() => VisibilityTester.Instance = (VisibilityTester) null;

  protected override void OnPrefabInit() => VisibilityTester.Instance = this;

  private void Update()
  {
    if ((Object) SelectTool.Instance == (Object) null || (Object) SelectTool.Instance.selected == (Object) null || !this.enableTesting)
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) SelectTool.Instance.selected);
    int mouseCell = DebugHandler.GetMouseCell();
    string text = "" + "Source Cell: " + (object) cell + "\n" + "Target Cell: " + (object) mouseCell + "\n" + "Visible: " + Grid.VisibilityTest(cell, mouseCell).ToString();
    for (int index = 0; index < 10000; ++index)
      Grid.VisibilityTest(cell, mouseCell);
    DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
  }
}
