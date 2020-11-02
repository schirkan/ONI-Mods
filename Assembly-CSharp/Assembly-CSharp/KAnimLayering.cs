// Decompiled with JetBrains decompiler
// Type: KAnimLayering
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class KAnimLayering
{
  private bool isForeground;
  private KAnimControllerBase controller;
  private KAnimControllerBase foregroundController;
  private KAnimLink link;
  private Grid.SceneLayer layer = Grid.SceneLayer.BuildingFront;
  public static readonly KAnimHashedString UI = new KAnimHashedString("ui");

  public KAnimLayering(KAnimControllerBase controller, Grid.SceneLayer layer)
  {
    this.controller = controller;
    this.layer = layer;
  }

  public void SetLayer(Grid.SceneLayer layer)
  {
    this.layer = layer;
    if (!((Object) this.foregroundController != (Object) null))
      return;
    this.foregroundController.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, (float) ((double) Grid.GetLayerZ(layer) - (double) this.controller.gameObject.transform.GetPosition().z - 0.100000001490116)));
  }

  public void SetIsForeground(bool is_foreground) => this.isForeground = is_foreground;

  public bool GetIsForeground() => this.isForeground;

  private static bool IsAnimLayered(KAnimFile[] anims)
  {
    for (int index = 0; index < anims.Length; ++index)
    {
      KAnimFile anim = anims[index];
      if (!((Object) anim == (Object) null))
      {
        KAnimFileData data = anim.GetData();
        if (data.build != null)
        {
          foreach (KAnim.Build.Symbol symbol in data.build.symbols)
          {
            if ((symbol.flags & 8) != 0)
              return true;
          }
        }
      }
    }
    return false;
  }

  private void HideSymbolsInternal()
  {
    foreach (KAnimFile animFile in this.controller.AnimFiles)
    {
      if (!((Object) animFile == (Object) null))
      {
        KAnimFileData data = animFile.GetData();
        if (data.build != null)
        {
          KAnim.Build.Symbol[] symbols = data.build.symbols;
          for (int index = 0; index < symbols.Length; ++index)
          {
            if ((uint) (symbols[index].flags & 8) > 0U != this.isForeground && !(symbols[index].hash == KAnimLayering.UI))
              this.controller.SetSymbolVisiblity(symbols[index].hash, false);
          }
        }
      }
    }
  }

  public void HideSymbols()
  {
    if ((Object) EntityPrefabs.Instance == (Object) null || this.isForeground)
      return;
    KAnimFile[] animFiles = this.controller.AnimFiles;
    bool flag = KAnimLayering.IsAnimLayered(animFiles);
    if (flag && (Object) this.foregroundController == (Object) null && this.layer != Grid.SceneLayer.NoLayer)
    {
      GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, this.controller.gameObject);
      gameObject.name = this.controller.name + "_fg";
      this.foregroundController = gameObject.GetComponent<KAnimControllerBase>();
      this.foregroundController.AnimFiles = animFiles;
      this.foregroundController.GetLayering().SetIsForeground(true);
      this.foregroundController.initialAnim = this.controller.initialAnim;
      this.link = new KAnimLink(this.controller, this.foregroundController);
      this.Dirty();
      KAnimSynchronizer synchronizer = this.controller.GetSynchronizer();
      synchronizer.Add(this.foregroundController);
      synchronizer.Sync(this.foregroundController);
      Vector3 position = new Vector3(0.0f, 0.0f, (float) ((double) Grid.GetLayerZ(this.layer) - (double) this.controller.gameObject.transform.GetPosition().z - 0.100000001490116));
      gameObject.transform.SetLocalPosition(position);
      gameObject.SetActive(true);
    }
    else if (!flag && (Object) this.foregroundController != (Object) null)
    {
      this.controller.GetSynchronizer().Remove(this.foregroundController);
      this.foregroundController.gameObject.DeleteObject();
      this.link = (KAnimLink) null;
    }
    if (!((Object) this.foregroundController != (Object) null))
      return;
    this.HideSymbolsInternal();
    this.foregroundController.GetLayering()?.HideSymbolsInternal();
  }

  public void Dirty()
  {
    if ((Object) this.foregroundController == (Object) null)
      return;
    this.foregroundController.Offset = this.controller.Offset;
    this.foregroundController.Pivot = this.controller.Pivot;
    this.foregroundController.Rotation = this.controller.Rotation;
    this.foregroundController.FlipX = this.controller.FlipX;
    this.foregroundController.FlipY = this.controller.FlipY;
  }
}
