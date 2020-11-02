﻿// Decompiled with JetBrains decompiler
// Type: RocketThrustWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/RocketThrustWidget")]
public class RocketThrustWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  public Image graphBar;
  public Image graphDot;
  public LocText graphDotText;
  public Image hoverMarker;
  public ToolTip hoverTooltip;
  public RectTransform markersContainer;
  public Image markerTemplate;
  private RectTransform rectTransform;
  private float maxMass = 20000f;
  private float totalWidth = 5f;
  private bool mouseOver;
  public CommandModule commandModule;

  protected override void OnPrefabInit()
  {
  }

  public void Draw(CommandModule commandModule)
  {
    if ((Object) this.rectTransform == (Object) null)
      this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
    this.commandModule = commandModule;
    this.totalWidth = this.rectTransform.rect.width;
    this.UpdateGraphDotPos(commandModule);
  }

  private void UpdateGraphDotPos(CommandModule rocket)
  {
    this.totalWidth = this.rectTransform.rect.width;
    this.graphDot.rectTransform.SetLocalPosition(new Vector3(Mathf.Clamp(Mathf.Lerp(0.0f, this.totalWidth, rocket.rocketStats.GetTotalMass() / this.maxMass), 0.0f, this.totalWidth), 0.0f, 0.0f));
    this.graphDotText.text = "-" + Util.FormatWholeNumber(rocket.rocketStats.GetTotalThrust() - rocket.rocketStats.GetRocketMaxDistance()) + "km";
  }

  private void Update()
  {
    if (!this.mouseOver)
      return;
    if ((Object) this.rectTransform == (Object) null)
      this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
    Vector3 position = this.rectTransform.GetPosition();
    Vector2 size = this.rectTransform.rect.size;
    float x = Mathf.Clamp((float) ((double) KInputManager.GetMousePos().x - (double) position.x + (double) size.x / 2.0), 0.0f, this.totalWidth);
    this.hoverMarker.rectTransform.SetLocalPosition(new Vector3(x, 0.0f, 0.0f));
    float num = Mathf.Lerp(0.0f, this.maxMass, x / this.totalWidth);
    float totalThrust = this.commandModule.rocketStats.GetTotalThrust();
    float rocketMaxDistance = this.commandModule.rocketStats.GetRocketMaxDistance();
    this.hoverTooltip.SetSimpleTooltip((string) STRINGS.UI.STARMAP.ROCKETWEIGHT.MASS + GameUtil.GetFormattedMass(num, massFormat: GameUtil.MetricMassFormat.Kilogram) + "\n" + (string) STRINGS.UI.STARMAP.ROCKETWEIGHT.MASSPENALTY + Util.FormatWholeNumber(TUNING.ROCKETRY.CalculateMassWithPenalty(num)) + (string) STRINGS.UI.UNITSUFFIXES.DISTANCE.KILOMETER + "\n\n" + (string) STRINGS.UI.STARMAP.ROCKETWEIGHT.CURRENTMASS + GameUtil.GetFormattedMass(this.commandModule.rocketStats.GetTotalMass(), massFormat: GameUtil.MetricMassFormat.Kilogram) + "\n" + (string) STRINGS.UI.STARMAP.ROCKETWEIGHT.CURRENTMASSPENALTY + Util.FormatWholeNumber(totalThrust - rocketMaxDistance) + (string) STRINGS.UI.UNITSUFFIXES.DISTANCE.KILOMETER);
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.mouseOver = true;
    this.hoverMarker.SetAlpha(1f);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.mouseOver = false;
    this.hoverMarker.SetAlpha(0.0f);
  }
}
