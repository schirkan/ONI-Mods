// Decompiled with JetBrains decompiler
// Type: DestinationAsteroid2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
  [SerializeField]
  private Image asteroidImage;
  [SerializeField]
  private KButton button;
  private ColonyDestinationAsteroidData asteroidData;

  public event System.Action<ColonyDestinationAsteroidData> OnClicked;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.button.onClick += new System.Action(this.OnClickInternal);
  }

  public void SetAsteroid(ColonyDestinationAsteroidData newAsteroidData)
  {
    if (newAsteroidData == this.asteroidData)
      return;
    this.asteroidData = newAsteroidData;
    this.asteroidImage.sprite = Assets.GetSprite((HashedString) this.asteroidData.sprite);
  }

  private void OnClickInternal()
  {
    DebugUtil.LogArgs((object) "Clicked asteroid", (object) this.asteroidData.worldPath);
    this.OnClicked(this.asteroidData);
  }
}
