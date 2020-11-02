// Decompiled with JetBrains decompiler
// Type: MoveableLogicGateVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SkipSaveFileSerialization]
public class MoveableLogicGateVisualizer : LogicGateBase
{
  private int cell;
  protected List<GameObject> visChildren = new List<GameObject>();
  private static readonly EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer>((System.Action<MoveableLogicGateVisualizer, object>) ((component, data) => component.OnRotated(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.cell = -1;
    OverlayScreen.Instance.OnOverlayChanged += new System.Action<HashedString>(this.OnOverlayChanged);
    this.OnOverlayChanged(OverlayScreen.Instance.mode);
    this.Subscribe<MoveableLogicGateVisualizer>(-1643076535, MoveableLogicGateVisualizer.OnRotatedDelegate);
  }

  protected override void OnCleanUp()
  {
    OverlayScreen.Instance.OnOverlayChanged -= new System.Action<HashedString>(this.OnOverlayChanged);
    this.Unregister();
    base.OnCleanUp();
  }

  private void OnOverlayChanged(HashedString mode)
  {
    if (mode == OverlayModes.Logic.ID)
      this.Register();
    else
      this.Unregister();
  }

  private void OnRotated(object data)
  {
    this.Unregister();
    this.OnOverlayChanged(OverlayScreen.Instance.mode);
  }

  private void Update()
  {
    if (this.visChildren.Count <= 0)
      return;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (cell == this.cell)
      return;
    this.cell = cell;
    this.Unregister();
    this.Register();
  }

  private GameObject CreateUIElem(int cell, bool is_input)
  {
    GameObject gameObject = Util.KInstantiate(LogicGateBase.uiSrcData.prefab, Grid.CellToPosCCC(cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas);
    Image component = gameObject.GetComponent<Image>();
    component.sprite = is_input ? LogicGateBase.uiSrcData.inputSprite : LogicGateBase.uiSrcData.outputSprite;
    component.raycastTarget = false;
    return gameObject;
  }

  private void Register()
  {
    if (this.visChildren.Count > 0)
      return;
    this.enabled = true;
    this.visChildren.Add(this.CreateUIElem(this.OutputCellOne, false));
    if (this.RequiresFourOutputs)
    {
      this.visChildren.Add(this.CreateUIElem(this.OutputCellTwo, false));
      this.visChildren.Add(this.CreateUIElem(this.OutputCellThree, false));
      this.visChildren.Add(this.CreateUIElem(this.OutputCellFour, false));
    }
    this.visChildren.Add(this.CreateUIElem(this.InputCellOne, true));
    if (this.RequiresTwoInputs)
      this.visChildren.Add(this.CreateUIElem(this.InputCellTwo, true));
    else if (this.RequiresFourInputs)
    {
      this.visChildren.Add(this.CreateUIElem(this.InputCellTwo, true));
      this.visChildren.Add(this.CreateUIElem(this.InputCellThree, true));
      this.visChildren.Add(this.CreateUIElem(this.InputCellFour, true));
    }
    if (!this.RequiresControlInputs)
      return;
    this.visChildren.Add(this.CreateUIElem(this.ControlCellOne, true));
    this.visChildren.Add(this.CreateUIElem(this.ControlCellTwo, true));
  }

  private void Unregister()
  {
    if (this.visChildren.Count <= 0)
      return;
    this.enabled = false;
    this.cell = -1;
    foreach (GameObject visChild in this.visChildren)
      Util.KDestroyGameObject(visChild);
    this.visChildren.Clear();
  }
}
