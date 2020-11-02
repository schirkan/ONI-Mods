// Decompiled with JetBrains decompiler
// Type: DestinationSelectPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/DestinationSelectPanel")]
public class DestinationSelectPanel : KMonoBehaviour
{
  [SerializeField]
  private GameObject asteroidPrefab;
  [SerializeField]
  private KButtonDrag dragTarget;
  [SerializeField]
  private MultiToggle leftArrowButton;
  [SerializeField]
  private MultiToggle rightArrowButton;
  [SerializeField]
  private RectTransform asteroidContainer;
  [SerializeField]
  private float asteroidFocusScale = 2f;
  [SerializeField]
  private float asteroidXSeparation = 240f;
  [SerializeField]
  private float focusScaleSpeed = 0.5f;
  [SerializeField]
  private float centeringSpeed = 0.5f;
  private float offset;
  private int selectedIndex = -1;
  private List<DestinationAsteroid2> asteroids = new List<DestinationAsteroid2>();
  private int numAsteroids;
  private List<string> worldNames;
  private Dictionary<string, ColonyDestinationAsteroidData> asteroidData = new Dictionary<string, ColonyDestinationAsteroidData>();
  private Vector2 dragStartPos;
  private Vector2 dragLastPos;
  private bool isDragging;
  private const string debugFmt = "{world}: {seed} [{traits}] {{settings}}";

  public event System.Action<ColonyDestinationAsteroidData> OnAsteroidClicked;

  private float min => this.asteroidContainer.rect.x + this.offset;

  private float max => this.min + this.asteroidContainer.rect.width;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.dragTarget.onBeginDrag += new System.Action(this.BeginDrag);
    this.dragTarget.onDrag += new System.Action(this.Drag);
    this.dragTarget.onEndDrag += new System.Action(this.EndDrag);
    this.leftArrowButton.onClick += new System.Action(this.ClickLeft);
    this.rightArrowButton.onClick += new System.Action(this.ClickRight);
  }

  private void BeginDrag()
  {
    this.dragStartPos = (Vector2) Input.mousePosition;
    this.dragLastPos = this.dragStartPos;
    this.isDragging = true;
    KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Start"));
  }

  private void Drag()
  {
    Vector2 mousePosition = (Vector2) Input.mousePosition;
    float num = mousePosition.x - this.dragLastPos.x;
    this.dragLastPos = mousePosition;
    this.offset += num;
    int selectedIndex1 = this.selectedIndex;
    this.selectedIndex = Mathf.RoundToInt(-this.offset / this.asteroidXSeparation);
    this.selectedIndex = Mathf.Clamp(this.selectedIndex, 0, this.worldNames.Count - 1);
    int selectedIndex2 = this.selectedIndex;
    if (selectedIndex1 == selectedIndex2)
      return;
    this.OnAsteroidClicked(this.asteroidData[this.worldNames[this.selectedIndex]]);
    KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll"));
  }

  private void EndDrag()
  {
    this.Drag();
    this.isDragging = false;
    KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Stop"));
  }

  private void ClickLeft()
  {
    this.selectedIndex = Mathf.Clamp(this.selectedIndex - 1, 0, this.worldNames.Count - 1);
    this.OnAsteroidClicked(this.asteroidData[this.worldNames[this.selectedIndex]]);
  }

  private void ClickRight()
  {
    this.selectedIndex = Mathf.Clamp(this.selectedIndex + 1, 0, this.worldNames.Count - 1);
    this.OnAsteroidClicked(this.asteroidData[this.worldNames[this.selectedIndex]]);
  }

  protected override void OnSpawn()
  {
    WorldGen.LoadSettings();
    this.worldNames = SettingsCache.worlds.GetNames();
    foreach (string worldName in this.worldNames)
    {
      ColonyDestinationAsteroidData destinationAsteroidData = new ColonyDestinationAsteroidData(worldName, 0);
      this.asteroidData[worldName] = destinationAsteroidData;
    }
    this.worldNames.Sort((Comparison<string>) ((a, b) => this.asteroidData[a].difficulty.CompareTo(this.asteroidData[b].difficulty)));
  }

  private void Update()
  {
    if (!this.isDragging)
    {
      float num1 = this.offset + (float) this.selectedIndex * this.asteroidXSeparation;
      float num2 = 0.0f;
      if ((double) num1 != 0.0)
        num2 = -num1;
      float b = Mathf.Clamp(num2, (float) (-(double) this.asteroidXSeparation * 2.0), this.asteroidXSeparation * 2f);
      if ((double) b != 0.0)
      {
        float a = this.centeringSpeed * Time.unscaledDeltaTime;
        float num3 = b * this.centeringSpeed * Time.unscaledDeltaTime;
        if ((double) num3 > 0.0 && (double) num3 < (double) a)
          num3 = Mathf.Min(a, b);
        else if ((double) num3 < 0.0 && (double) num3 > -(double) a)
          num3 = Mathf.Max(-a, b);
        this.offset += num3;
      }
    }
    Rect rect = this.asteroidContainer.rect;
    float x1 = rect.min.x;
    rect = this.asteroidContainer.rect;
    float x2 = rect.max.x;
    this.offset = Mathf.Clamp(this.offset, (float) -(this.worldNames.Count - 1) * this.asteroidXSeparation + x1, x2);
    this.RePlaceAsteroids();
  }

  [ContextMenu("RePlaceAsteroids")]
  public void RePlaceAsteroids()
  {
    this.BeginAsteroidDrawing();
    for (int index = 0; index < this.worldNames.Count; ++index)
    {
      if (index != this.selectedIndex)
      {
        float x = this.offset + (float) index * this.asteroidXSeparation;
        if ((double) x + (double) this.offset + (double) this.asteroidXSeparation >= (double) this.min && (double) x + (double) this.offset - (double) this.asteroidXSeparation <= (double) this.max)
        {
          this.GetAsteroid(this.worldNames[index], 1f).transform.SetLocalPosition(new Vector3(x, 0.0f, 0.0f));
          if (this.numAsteroids > 100)
            break;
        }
      }
    }
    float x1 = this.offset + (float) this.selectedIndex * this.asteroidXSeparation;
    this.GetAsteroid(this.worldNames[this.selectedIndex], this.asteroidFocusScale).transform.SetLocalPosition(new Vector3(x1, 0.0f, 0.0f));
    this.EndAsteroidDrawing();
  }

  private void BeginAsteroidDrawing() => this.numAsteroids = 0;

  private DestinationAsteroid2 GetAsteroid(string name, float scale)
  {
    DestinationAsteroid2 destinationAsteroid2;
    if (this.numAsteroids < this.asteroids.Count)
    {
      destinationAsteroid2 = this.asteroids[this.numAsteroids];
    }
    else
    {
      destinationAsteroid2 = Util.KInstantiateUI<DestinationAsteroid2>(this.asteroidPrefab, this.asteroidContainer.gameObject);
      destinationAsteroid2.OnClicked += this.OnAsteroidClicked;
      this.asteroids.Add(destinationAsteroid2);
    }
    this.asteroidData[name].TargetScale = scale;
    this.asteroidData[name].Scale += (this.asteroidData[name].TargetScale - this.asteroidData[name].Scale) * this.focusScaleSpeed * Time.unscaledDeltaTime;
    destinationAsteroid2.transform.localScale = Vector3.one * this.asteroidData[name].Scale;
    destinationAsteroid2.SetAsteroid(this.asteroidData[name]);
    ++this.numAsteroids;
    return destinationAsteroid2;
  }

  private void EndAsteroidDrawing()
  {
    for (int index = 0; index < this.asteroids.Count; ++index)
      this.asteroids[index].gameObject.SetActive(index < this.numAsteroids);
  }

  public ColonyDestinationAsteroidData SelectAsteroid(
    string name,
    int seed)
  {
    this.selectedIndex = this.worldNames.IndexOf(name);
    this.asteroidData[name].ReInitialize(seed);
    return this.asteroidData[name];
  }

  public void ScrollLeft() => this.OnAsteroidClicked(this.asteroidData[this.worldNames[Mathf.Max(this.selectedIndex - 1, 0)]]);

  public void ScrollRight() => this.OnAsteroidClicked(this.asteroidData[this.worldNames[Mathf.Min(this.selectedIndex + 1, this.worldNames.Count - 1)]]);

  private void DebugCurrentSetting()
  {
    ColonyDestinationAsteroidData destinationAsteroidData = this.asteroidData[this.worldNames[this.selectedIndex]];
    string str1 = "{world}: {seed} [{traits}] {{settings}}";
    string properName = destinationAsteroidData.properName;
    string newValue1 = destinationAsteroidData.seed.ToString();
    string str2 = str1.Replace("{world}", properName).Replace("{seed}", newValue1);
    List<AsteroidDescriptor> traitDescriptors = destinationAsteroidData.GetTraitDescriptors();
    string[] strArray = new string[traitDescriptors.Count];
    for (int index = 0; index < traitDescriptors.Count; ++index)
      strArray[index] = traitDescriptors[index].text;
    string newValue2 = string.Join(", ", strArray);
    string str3 = str2.Replace("{traits}", newValue2);
    switch (CustomGameSettings.Instance.customGameMode)
    {
      case CustomGameSettings.CustomGameMode.Survival:
        str3 = str3.Replace("{settings}", "Survival");
        break;
      case CustomGameSettings.CustomGameMode.Nosweat:
        str3 = str3.Replace("{settings}", "Nosweat");
        break;
      case CustomGameSettings.CustomGameMode.Custom:
        List<string> stringList = new List<string>();
        foreach (KeyValuePair<string, SettingConfig> qualitySetting in CustomGameSettings.Instance.QualitySettings)
        {
          if (qualitySetting.Value.coordinate_dimension >= 0 && qualitySetting.Value.coordinate_dimension_width >= 0)
          {
            SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(qualitySetting.Key);
            if (currentQualitySetting.id != qualitySetting.Value.default_level_id)
              stringList.Add(string.Format("{0}={1}", (object) qualitySetting.Value.label, (object) currentQualitySetting.label));
          }
        }
        str3 = str3.Replace("{settings}", string.Join(", ", stringList.ToArray()));
        break;
    }
    Debug.Log((object) str3);
  }
}
