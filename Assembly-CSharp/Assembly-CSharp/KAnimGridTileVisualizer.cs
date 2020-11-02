// Decompiled with JetBrains decompiler
// Type: KAnimGridTileVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Rendering;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGridTileVisualizer")]
public class KAnimGridTileVisualizer : KMonoBehaviour, IBlockTileInfo
{
  [SerializeField]
  public int blockTileConnectorID;
  private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnSelectionChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>((System.Action<KAnimGridTileVisualizer, object>) ((component, data) => component.OnSelectionChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnHighlightChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>((System.Action<KAnimGridTileVisualizer, object>) ((component, data) => component.OnHighlightChanged(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<KAnimGridTileVisualizer>(-1503271301, KAnimGridTileVisualizer.OnSelectionChangedDelegate);
    this.Subscribe<KAnimGridTileVisualizer>(-1201923725, KAnimGridTileVisualizer.OnHighlightChangedDelegate);
  }

  protected override void OnCleanUp()
  {
    Building component = this.GetComponent<Building>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      ObjectLayer tileLayer = component.Def.TileLayer;
      if ((UnityEngine.Object) Grid.Objects[cell, (int) tileLayer] == (UnityEngine.Object) this.gameObject)
        Grid.Objects[cell, (int) tileLayer] = (GameObject) null;
      TileVisualizer.RefreshCell(cell, tileLayer, component.Def.ReplacementLayer);
    }
    base.OnCleanUp();
  }

  private void OnSelectionChanged(object data)
  {
    bool enabled = (bool) data;
    World.Instance.blockTileRenderer.SelectCell(Grid.PosToCell(this.transform.GetPosition()), enabled);
  }

  private void OnHighlightChanged(object data)
  {
    bool enabled = (bool) data;
    World.Instance.blockTileRenderer.HighlightCell(Grid.PosToCell(this.transform.GetPosition()), enabled);
  }

  public int GetBlockTileConnectorID() => this.blockTileConnectorID;
}
