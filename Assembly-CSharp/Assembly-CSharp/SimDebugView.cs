// Decompiled with JetBrains decompiler
// Type: SimDebugView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[AddComponentMenu("KMonoBehaviour/scripts/SimDebugView")]
public class SimDebugView : KMonoBehaviour
{
  [SerializeField]
  public Material material;
  public Material diseaseMaterial;
  public bool hideFOW;
  public const int colourSize = 4;
  private byte[] texBytes;
  private int currentFrame;
  [SerializeField]
  private Texture2D tex;
  [SerializeField]
  private GameObject plane;
  private HashedString mode = global::OverlayModes.Power.ID;
  private SimDebugView.GameGridMode gameGridMode = SimDebugView.GameGridMode.DigAmount;
  private PathProber selectedPathProber;
  public float minTempExpected = 173.15f;
  public float maxTempExpected = 423.15f;
  public float minMassExpected = 1.0001f;
  public float maxMassExpected = 10000f;
  public float minPressureExpected = 1.300003f;
  public float maxPressureExpected = 201.3f;
  public float minThermalConductivity;
  public float maxThermalConductivity = 30f;
  public float thresholdRange = 1f / 1000f;
  public float thresholdOpacity = 0.8f;
  public static float minimumBreathable = 0.05f;
  public static float optimallyBreathable = 1f;
  public SimDebugView.ColorThreshold[] temperatureThresholds;
  public SimDebugView.ColorThreshold[] heatFlowThresholds;
  public Color32[] networkColours;
  public Gradient breathableGradient = new Gradient();
  public Color32 unbreathableColour = (Color32) new Color(0.5f, 0.0f, 0.0f);
  public Color32[] toxicColour = new Color32[2]
  {
    (Color32) new Color(0.5f, 0.0f, 0.5f),
    (Color32) new Color(1f, 0.0f, 1f)
  };
  public static SimDebugView Instance;
  private WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData> updateSimViewWorkItems = new WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData>();
  private int selectedCell;
  private Dictionary<HashedString, System.Action<SimDebugView, Texture>> dataUpdateFuncs = new Dictionary<HashedString, System.Action<SimDebugView, Texture>>()
  {
    {
      global::OverlayModes.Temperature.ID,
      new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
    },
    {
      global::OverlayModes.Oxygen.ID,
      new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
    },
    {
      global::OverlayModes.Decor.ID,
      new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
    },
    {
      global::OverlayModes.TileMode.ID,
      new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint)
    },
    {
      global::OverlayModes.Disease.ID,
      new System.Action<SimDebugView, Texture>(SimDebugView.SetDisease)
    }
  };
  private Dictionary<HashedString, Func<SimDebugView, int, Color>> getColourFuncs = new Dictionary<HashedString, Func<SimDebugView, int, Color>>()
  {
    {
      global::OverlayModes.ThermalConductivity.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetThermalConductivityColour)
    },
    {
      global::OverlayModes.Temperature.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetNormalizedTemperatureColourMode)
    },
    {
      global::OverlayModes.Disease.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetDiseaseColour)
    },
    {
      global::OverlayModes.Decor.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetDecorColour)
    },
    {
      global::OverlayModes.Oxygen.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetOxygenMapColour)
    },
    {
      global::OverlayModes.Light.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetLightColour)
    },
    {
      global::OverlayModes.Radiation.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetRadiationColour)
    },
    {
      global::OverlayModes.Rooms.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetRoomsColour)
    },
    {
      global::OverlayModes.TileMode.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetTileColour)
    },
    {
      global::OverlayModes.Suit.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
    },
    {
      global::OverlayModes.Priorities.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
    },
    {
      global::OverlayModes.Crop.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
    },
    {
      global::OverlayModes.Harvest.ID,
      new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
    },
    {
      SimDebugView.OverlayModes.GameGrid,
      new Func<SimDebugView, int, Color>(SimDebugView.GetGameGridColour)
    },
    {
      SimDebugView.OverlayModes.StateChange,
      new Func<SimDebugView, int, Color>(SimDebugView.GetStateChangeColour)
    },
    {
      SimDebugView.OverlayModes.SimCheckErrorMap,
      new Func<SimDebugView, int, Color>(SimDebugView.GetSimCheckErrorMapColour)
    },
    {
      SimDebugView.OverlayModes.Foundation,
      new Func<SimDebugView, int, Color>(SimDebugView.GetFoundationColour)
    },
    {
      SimDebugView.OverlayModes.FakeFloor,
      new Func<SimDebugView, int, Color>(SimDebugView.GetFakeFloorColour)
    },
    {
      SimDebugView.OverlayModes.DupePassable,
      new Func<SimDebugView, int, Color>(SimDebugView.GetDupePassableColour)
    },
    {
      SimDebugView.OverlayModes.DupeImpassable,
      new Func<SimDebugView, int, Color>(SimDebugView.GetDupeImpassableColour)
    },
    {
      SimDebugView.OverlayModes.CritterImpassable,
      new Func<SimDebugView, int, Color>(SimDebugView.GetCritterImpassableColour)
    },
    {
      SimDebugView.OverlayModes.MinionGroupProber,
      new Func<SimDebugView, int, Color>(SimDebugView.GetMinionGroupProberColour)
    },
    {
      SimDebugView.OverlayModes.PathProber,
      new Func<SimDebugView, int, Color>(SimDebugView.GetPathProberColour)
    },
    {
      SimDebugView.OverlayModes.Reserved,
      new Func<SimDebugView, int, Color>(SimDebugView.GetReservedColour)
    },
    {
      SimDebugView.OverlayModes.AllowPathFinding,
      new Func<SimDebugView, int, Color>(SimDebugView.GetAllowPathFindingColour)
    },
    {
      SimDebugView.OverlayModes.Danger,
      new Func<SimDebugView, int, Color>(SimDebugView.GetDangerColour)
    },
    {
      SimDebugView.OverlayModes.MinionOccupied,
      new Func<SimDebugView, int, Color>(SimDebugView.GetMinionOccupiedColour)
    },
    {
      SimDebugView.OverlayModes.Pressure,
      new Func<SimDebugView, int, Color>(SimDebugView.GetPressureMapColour)
    },
    {
      SimDebugView.OverlayModes.TileType,
      new Func<SimDebugView, int, Color>(SimDebugView.GetTileTypeColour)
    },
    {
      SimDebugView.OverlayModes.State,
      new Func<SimDebugView, int, Color>(SimDebugView.GetStateMapColour)
    },
    {
      SimDebugView.OverlayModes.SolidLiquid,
      new Func<SimDebugView, int, Color>(SimDebugView.GetSolidLiquidMapColour)
    },
    {
      SimDebugView.OverlayModes.Mass,
      new Func<SimDebugView, int, Color>(SimDebugView.GetMassColour)
    },
    {
      SimDebugView.OverlayModes.Joules,
      new Func<SimDebugView, int, Color>(SimDebugView.GetJoulesColour)
    }
  };
  public static readonly Color[] dbColours = new Color[13]
  {
    new Color(0.0f, 0.0f, 0.0f, 0.0f),
    new Color(1f, 1f, 1f, 0.3f),
    new Color(0.7058824f, 0.8235294f, 1f, 0.2f),
    new Color(0.0f, 0.3137255f, 1f, 0.3f),
    new Color(0.7058824f, 1f, 0.7058824f, 0.5f),
    new Color(0.07843138f, 1f, 0.0f, 0.7f),
    new Color(1f, 0.9019608f, 0.7058824f, 0.9f),
    new Color(1f, 0.8235294f, 0.0f, 0.9f),
    new Color(1f, 0.7176471f, 0.3019608f, 0.9f),
    new Color(1f, 0.4156863f, 0.0f, 0.9f),
    new Color(1f, 0.7058824f, 0.7058824f, 1f),
    new Color(1f, 0.0f, 0.0f, 1f),
    new Color(1f, 0.0f, 0.0f, 1f)
  };
  private static float minMinionTemperature = 260f;
  private static float maxMinionTemperature = 310f;
  private static float minMinionPressure = 80f;

  public static void DestroyInstance() => SimDebugView.Instance = (SimDebugView) null;

  protected override void OnPrefabInit()
  {
    SimDebugView.Instance = this;
    this.material = UnityEngine.Object.Instantiate<Material>(this.material);
    this.diseaseMaterial = UnityEngine.Object.Instantiate<Material>(this.diseaseMaterial);
  }

  protected override void OnSpawn()
  {
    SimDebugViewCompositor.Instance.material.SetColor("_Color0", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color1", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[1].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color2", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[2].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color3", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[3].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color4", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[4].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color5", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[5].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color6", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[6].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color7", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[7].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color0", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[0].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color1", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[1].colorName));
    SimDebugViewCompositor.Instance.material.SetColor("_Color2", (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[2].colorName));
    this.SetMode(global::OverlayModes.None.ID);
  }

  public void OnReset()
  {
    this.plane = SimDebugView.CreatePlane(nameof (SimDebugView), this.transform);
    this.tex = SimDebugView.CreateTexture(out this.texBytes, Grid.WidthInCells, Grid.HeightInCells);
    this.plane.GetComponent<Renderer>().sharedMaterial = this.material;
    this.plane.GetComponent<Renderer>().sharedMaterial.mainTexture = (Texture) this.tex;
    this.plane.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, -6f));
    this.SetMode(global::OverlayModes.None.ID);
  }

  public static Texture2D CreateTexture(out byte[] textureBytes, int width, int height)
  {
    textureBytes = new byte[width * height * 4];
    Texture2D texture2D = new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None);
    texture2D.name = nameof (SimDebugView);
    texture2D.wrapMode = TextureWrapMode.Clamp;
    texture2D.filterMode = FilterMode.Point;
    return texture2D;
  }

  public static GameObject CreatePlane(string layer, Transform parent)
  {
    GameObject go = new GameObject();
    go.name = "overlayViewDisplayPlane";
    go.SetLayerRecursively(LayerMask.NameToLayer(layer));
    go.transform.SetParent(parent);
    go.transform.SetPosition(Vector3.zero);
    go.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
    MeshFilter meshFilter = go.AddComponent<MeshFilter>();
    Mesh mesh1 = new Mesh();
    Mesh mesh2 = mesh1;
    meshFilter.mesh = mesh2;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    float y = 2f * (float) Grid.HeightInCells;
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3((float) Grid.WidthInCells, 0.0f, 0.0f),
      new Vector3(0.0f, y, 0.0f),
      new Vector3(Grid.WidthInMeters, y, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 2f),
      new Vector2(1f, 2f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    mesh1.vertices = vector3Array2;
    mesh1.uv = vector2Array2;
    mesh1.triangles = numArray2;
    Vector2 vector2 = new Vector2((float) Grid.WidthInCells, y);
    mesh1.bounds = new Bounds(new Vector3(0.5f * vector2.x, 0.5f * vector2.y, 0.0f), new Vector3(vector2.x, vector2.y, 0.0f));
    return go;
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.plane == (UnityEngine.Object) null)
      return;
    bool flag = this.mode != global::OverlayModes.None.ID;
    this.plane.SetActive(flag);
    SimDebugViewCompositor.Instance.Toggle(this.mode != global::OverlayModes.None.ID && !GameUtil.IsCapturingTimeLapse());
    SimDebugViewCompositor.Instance.material.SetVector("_Thresholds0", new Vector4(0.1f, 0.2f, 0.3f, 0.4f));
    SimDebugViewCompositor.Instance.material.SetVector("_Thresholds1", new Vector4(0.5f, 0.6f, 0.7f, 0.8f));
    float x = 0.0f;
    if (this.mode == global::OverlayModes.ThermalConductivity.ID || this.mode == global::OverlayModes.Temperature.ID)
      x = 1f;
    SimDebugViewCompositor.Instance.material.SetVector("_ThresholdParameters", new Vector4(x, this.thresholdRange, this.thresholdOpacity, 0.0f));
    if (!flag)
      return;
    this.UpdateData(this.tex, this.texBytes, this.mode, (byte) 192);
  }

  private static void SetDefaultBilinear(SimDebugView instance, Texture texture)
  {
    Renderer component = instance.plane.GetComponent<Renderer>();
    component.sharedMaterial = instance.material;
    component.sharedMaterial.mainTexture = (Texture) instance.tex;
    texture.filterMode = FilterMode.Bilinear;
  }

  private static void SetDefaultPoint(SimDebugView instance, Texture texture)
  {
    Renderer component = instance.plane.GetComponent<Renderer>();
    component.sharedMaterial = instance.material;
    component.sharedMaterial.mainTexture = (Texture) instance.tex;
    texture.filterMode = FilterMode.Point;
  }

  private static void SetDisease(SimDebugView instance, Texture texture)
  {
    Renderer component = instance.plane.GetComponent<Renderer>();
    component.sharedMaterial = instance.diseaseMaterial;
    component.sharedMaterial.mainTexture = (Texture) instance.tex;
    texture.filterMode = FilterMode.Bilinear;
  }

  public void UpdateData(
    Texture2D texture,
    byte[] textureBytes,
    HashedString viewMode,
    byte alpha)
  {
    System.Action<SimDebugView, Texture> action;
    if (!this.dataUpdateFuncs.TryGetValue(viewMode, out action))
      action = new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
    action(this, (Texture) texture);
    int min_x;
    int min_y;
    int max_x;
    int max_y;
    Grid.GetVisibleExtents(out min_x, out min_y, out max_x, out max_y);
    this.selectedPathProber = (PathProber) null;
    KSelectable selected = SelectTool.Instance.selected;
    if ((UnityEngine.Object) selected != (UnityEngine.Object) null)
      this.selectedPathProber = selected.GetComponent<PathProber>();
    this.updateSimViewWorkItems.Reset(new SimDebugView.UpdateSimViewSharedData(this, this.texBytes, viewMode, this));
    int num = 16;
    for (int y0 = min_y; y0 <= max_y; y0 += num)
    {
      int y1 = Math.Min(y0 + num - 1, max_y);
      this.updateSimViewWorkItems.Add(new SimDebugView.UpdateSimViewWorkItem(min_x, y0, max_x, y1));
    }
    this.currentFrame = Time.frameCount;
    this.selectedCell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    GlobalJobManager.Run((IWorkItemCollection) this.updateSimViewWorkItems);
    texture.LoadRawTextureData(textureBytes);
    texture.Apply();
  }

  public void SetGameGridMode(SimDebugView.GameGridMode mode) => this.gameGridMode = mode;

  public SimDebugView.GameGridMode GetGameGridMode() => this.gameGridMode;

  public void SetMode(HashedString mode)
  {
    this.mode = mode;
    Game.Instance.gameObject.Trigger(1798162660, (object) mode);
  }

  public HashedString GetMode() => this.mode;

  public static Color TemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp((float) (((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected)), 0.0f, 1f)) * 171.0) / 360.0), 1f, 1f);
  }

  public static Color LiquidTemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    double num = ((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected);
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp((float) num, 0.5f, 1f)) * 171.0) / 360.0), Mathf.Clamp((float) num, 0.0f, 1f), 1f);
  }

  public static Color SolidTemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp((float) (((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected)), 0.5f, 1f)) * 171.0) / 360.0), 1f, 1f);
  }

  public static Color GasTemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp((float) (((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected)), 0.0f, 0.5f)) * 171.0) / 360.0), 1f, 1f);
  }

  public Color NormalizedTemperature(float temperature)
  {
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < this.temperatureThresholds.Length; ++index3)
    {
      if ((double) temperature <= (double) this.temperatureThresholds[index3].value)
      {
        index2 = index3;
        break;
      }
      index1 = index3;
      index2 = index3;
    }
    float a = 0.0f;
    if (index1 != index2)
      a = (float) (((double) temperature - (double) this.temperatureThresholds[index1].value) / ((double) this.temperatureThresholds[index2].value - (double) this.temperatureThresholds[index1].value));
    float t = Mathf.Min(Mathf.Max(a, 0.0f), 1f);
    return Color.Lerp((Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[index1].colorName), (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[index2].colorName), t);
  }

  public Color NormalizedHeatFlow(int cell)
  {
    int index1 = 0;
    int index2 = 0;
    float thermalComfort = GameUtil.GetThermalComfort(cell);
    for (int index3 = 0; index3 < this.heatFlowThresholds.Length; ++index3)
    {
      if ((double) thermalComfort <= (double) this.heatFlowThresholds[index3].value)
      {
        index2 = index3;
        break;
      }
      index1 = index3;
      index2 = index3;
    }
    float a = 0.0f;
    if (index1 != index2)
      a = (float) (((double) thermalComfort - (double) this.heatFlowThresholds[index1].value) / ((double) this.heatFlowThresholds[index2].value - (double) this.heatFlowThresholds[index1].value));
    float t = Mathf.Min(Mathf.Max(a, 0.0f), 1f);
    Color color = Color.Lerp((Color) GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[index1].colorName), (Color) GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[index2].colorName), t);
    if (Grid.Solid[cell])
      color = Color.black;
    return color;
  }

  private static bool IsInsulated(int cell) => (uint) (Grid.Element[cell].state & Element.State.TemperatureInsulated) > 0U;

  private static Color GetDiseaseColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (Grid.DiseaseIdx[cell] != byte.MaxValue)
    {
      color = (Color) GlobalAssets.Instance.colorSet.GetColorByName(Db.Get().Diseases[(int) Grid.DiseaseIdx[cell]].overlayColourName);
      color.a = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
    }
    else
      color.a = 0.0f;
    return color;
  }

  private static Color GetHeatFlowColour(SimDebugView instance, int cell) => instance.NormalizedHeatFlow(cell);

  private static Color GetBlack(SimDebugView instance, int cell) => Color.black;

  public static Color GetLightColour(SimDebugView instance, int cell)
  {
    Color lightOverlay = (Color) GlobalAssets.Instance.colorSet.lightOverlay;
    lightOverlay.a = Mathf.Clamp(Mathf.Sqrt((float) (Grid.LightIntensity[cell] + LightGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0.0f, 1f);
    if (Grid.LightIntensity[cell] > 72000)
    {
      float num = (float) (((double) Grid.LightIntensity[cell] + (double) LightGridManager.previewLux[cell] - 72000.0) / 8000.0) / 10f;
      lightOverlay.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, (float) ((double) Grid.CellToPos2D(cell).y / 8.0 + (double) instance.currentFrame / 32.0)) * num);
    }
    return lightOverlay;
  }

  public static Color GetRadiationColour(SimDebugView instance, int cell)
  {
    Color color = new Color(0.2f, 0.9f, 0.3f, Mathf.Clamp(Mathf.Sqrt((float) (Grid.RadiationCount[cell] + RadiationGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0.0f, 1f));
    if (Grid.RadiationCount[cell] > 72000)
    {
      float num = (float) (((double) Grid.RadiationCount[cell] + (double) LightGridManager.previewLux[cell] - 72000.0) / 8000.0) / 10f;
      color.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, (float) ((double) Grid.CellToPos2D(cell).y / 8.0 + (double) instance.currentFrame / 32.0)) * num);
    }
    return color;
  }

  public static Color GetRoomsColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (Grid.IsValidCell(instance.selectedCell))
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
      if (cavityForCell != null && cavityForCell.room != null)
      {
        color = (Color) GlobalAssets.Instance.colorSet.GetColorByName(cavityForCell.room.roomType.category.colorName);
        color.a = 0.45f;
        if (Game.Instance.roomProber.GetCavityForCell(instance.selectedCell) == cavityForCell)
          color.a += 0.3f;
      }
    }
    return color;
  }

  public static Color GetJoulesColour(SimDebugView instance, int cell) => Color.Lerp(Color.black, Color.red, (float) (0.5 * ((double) Grid.Element[cell].specificHeatCapacity * (double) Grid.Temperature[cell] * ((double) Grid.Mass[cell] * 1000.0)) / ((double) ElementLoader.FindElementByHash(SimHashes.SandStone).specificHeatCapacity * 294.0 * 1000000.0)));

  public static Color GetNormalizedTemperatureColourMode(SimDebugView instance, int cell)
  {
    switch (Game.Instance.temperatureOverlayMode)
    {
      case Game.TemperatureOverlayModes.AbsoluteTemperature:
        return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
      case Game.TemperatureOverlayModes.AdaptiveTemperature:
        return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
      case Game.TemperatureOverlayModes.HeatFlow:
        return SimDebugView.GetHeatFlowColour(instance, cell);
      case Game.TemperatureOverlayModes.StateChange:
        return SimDebugView.GetStateChangeProximityColour(instance, cell);
      default:
        return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
    }
  }

  public static Color GetStateChangeProximityColour(SimDebugView instance, int cell)
  {
    float temperature = Grid.Temperature[cell];
    Element element = Grid.Element[cell];
    float lowTemp = element.lowTemp;
    float highTemp = element.highTemp;
    if (element.IsGas)
    {
      float maxTempExpected = Mathf.Min(lowTemp + 150f, highTemp);
      return SimDebugView.GasTemperatureToColor(temperature, lowTemp, maxTempExpected);
    }
    if (!element.IsSolid)
      return SimDebugView.TemperatureToColor(temperature, lowTemp, highTemp);
    float minTempExpected = Mathf.Max(highTemp - 150f, lowTemp);
    return SimDebugView.SolidTemperatureToColor(temperature, minTempExpected, highTemp);
  }

  public static Color GetNormalizedTemperatureColour(SimDebugView instance, int cell)
  {
    float temperature = Grid.Temperature[cell];
    return instance.NormalizedTemperature(temperature);
  }

  private static Color GetGameGridColour(SimDebugView instance, int cell)
  {
    Color color = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
    switch (instance.gameGridMode)
    {
      case SimDebugView.GameGridMode.GameSolidMap:
        color = Grid.Solid[cell] ? Color.white : Color.black;
        break;
      case SimDebugView.GameGridMode.Lighting:
        color = Grid.LightCount[cell] > 0 || LightGridManager.previewLux[cell] > 0 ? Color.white : Color.black;
        break;
      case SimDebugView.GameGridMode.DigAmount:
        if (Grid.Element[cell].IsSolid)
        {
          color = Color.HSVToRGB(1f - Grid.Damage[cell] / (float) byte.MaxValue, 1f, 1f);
          break;
        }
        break;
      case SimDebugView.GameGridMode.DupePassable:
        color = Grid.DupePassable[cell] ? Color.white : Color.black;
        break;
    }
    return color;
  }

  public Color32 GetColourForID(int id) => this.networkColours[id % this.networkColours.Length];

  private static Color GetThermalConductivityColour(SimDebugView instance, int cell)
  {
    int num1 = SimDebugView.IsInsulated(cell) ? 1 : 0;
    Color color = Color.black;
    float num2 = instance.maxThermalConductivity - instance.minThermalConductivity;
    if (num1 == 0 && (double) num2 != 0.0)
    {
      float num3 = Mathf.Min(Mathf.Max((Grid.Element[cell].thermalConductivity - instance.minThermalConductivity) / num2, 0.0f), 1f);
      color = new Color(num3, num3, num3);
    }
    return color;
  }

  private static Color GetPressureMapColour(SimDebugView instance, int cell)
  {
    Color32 color32 = (Color32) Color.black;
    if ((double) Grid.Pressure[cell] > 0.0)
    {
      double num = (double) Mathf.Clamp((float) (((double) Grid.Pressure[cell] - (double) instance.minPressureExpected) / ((double) instance.maxPressureExpected - (double) instance.minPressureExpected)), 0.0f, 1f) * 0.899999976158142;
      color32 = (Color32) new Color((float) num, (float) num, (float) num, 1f);
    }
    return (Color) color32;
  }

  private static Color GetOxygenMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (!Grid.IsLiquid(cell) && !Grid.Solid[cell])
    {
      if ((double) Grid.Mass[cell] > (double) SimDebugView.minimumBreathable && (Grid.Element[cell].id == SimHashes.Oxygen || Grid.Element[cell].id == SimHashes.ContaminatedOxygen))
      {
        float time = Mathf.Clamp((Grid.Mass[cell] - SimDebugView.minimumBreathable) / SimDebugView.optimallyBreathable, 0.0f, 1f);
        color = instance.breathableGradient.Evaluate(time);
      }
      else
        color = (Color) instance.unbreathableColour;
    }
    return color;
  }

  private static Color GetTileColour(SimDebugView instance, int cell)
  {
    float num = 0.33f;
    Color color = new Color(num, num, num);
    Element element = Grid.Element[cell];
    bool flag = false;
    foreach (Tag tileOverlayFilter in Game.Instance.tileOverlayFilters)
    {
      if (element.HasTag(tileOverlayFilter))
        flag = true;
    }
    if (flag)
      color = (Color) element.substance.uiColour;
    return color;
  }

  private static Color GetTileTypeColour(SimDebugView instance, int cell) => (Color) Grid.Element[cell].substance.uiColour;

  private static Color GetStateMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    switch (Grid.Element[cell].state & Element.State.Solid)
    {
      case Element.State.Gas:
        color = Color.yellow;
        break;
      case Element.State.Liquid:
        color = Color.green;
        break;
      case Element.State.Solid:
        color = Color.blue;
        break;
    }
    return color;
  }

  private static Color GetSolidLiquidMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    switch (Grid.Element[cell].state & Element.State.Solid)
    {
      case Element.State.Liquid:
        color = Color.green;
        break;
      case Element.State.Solid:
        color = Color.blue;
        break;
    }
    return color;
  }

  private static Color GetStateChangeColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    Element element = Grid.Element[cell];
    if (!element.IsVacuum)
    {
      double num1 = (double) Grid.Temperature[cell];
      float num2 = element.lowTemp * 0.05f;
      float a = Mathf.Abs((float) num1 - element.lowTemp) / num2;
      float num3 = element.highTemp * 0.05f;
      float b = Mathf.Abs((float) num1 - element.highTemp) / num3;
      color = Color.Lerp(Color.black, Color.red, Mathf.Max(0.0f, 1f - Mathf.Min(a, b)));
    }
    return color;
  }

  private static Color GetDecorColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (!Grid.Solid[cell])
    {
      float f = GameUtil.GetDecorAtCell(cell) / 100f;
      color = (double) f <= 0.0 ? Color.Lerp((Color) GlobalAssets.Instance.colorSet.decorBaseline, (Color) GlobalAssets.Instance.colorSet.decorNegative, Mathf.Abs(f)) : Color.Lerp((Color) GlobalAssets.Instance.colorSet.decorBaseline, (Color) GlobalAssets.Instance.colorSet.decorPositive, Mathf.Abs(f));
    }
    return color;
  }

  private static Color GetDangerColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    SimDebugView.DangerAmount dangerAmount = SimDebugView.DangerAmount.None;
    if (!Grid.Element[cell].IsSolid)
    {
      float num = 0.0f;
      if ((double) Grid.Temperature[cell] < (double) SimDebugView.minMinionTemperature)
        num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.minMinionTemperature);
      if ((double) Grid.Temperature[cell] > (double) SimDebugView.maxMinionTemperature)
        num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.maxMinionTemperature);
      if ((double) num > 0.0)
      {
        if ((double) num < 10.0)
          dangerAmount = SimDebugView.DangerAmount.VeryLow;
        else if ((double) num < 30.0)
          dangerAmount = SimDebugView.DangerAmount.Low;
        else if ((double) num < 100.0)
          dangerAmount = SimDebugView.DangerAmount.Moderate;
        else if ((double) num < 200.0)
          dangerAmount = SimDebugView.DangerAmount.High;
        else if ((double) num < 400.0)
          dangerAmount = SimDebugView.DangerAmount.VeryHigh;
        else if ((double) num > 800.0)
          dangerAmount = SimDebugView.DangerAmount.Extreme;
      }
    }
    if (dangerAmount < SimDebugView.DangerAmount.VeryHigh && (Grid.Element[cell].IsVacuum || Grid.Element[cell].IsGas && (Grid.Element[cell].id != SimHashes.Oxygen || (double) Grid.Pressure[cell] < (double) SimDebugView.minMinionPressure)))
      ++dangerAmount;
    if (dangerAmount != SimDebugView.DangerAmount.None)
      color = Color.HSVToRGB((float) ((80.0 - (double) ((float) dangerAmount / 6f) * 80.0) / 360.0), 1f, 1f);
    return color;
  }

  private static Color GetSimCheckErrorMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    Element element = Grid.Element[cell];
    float f1 = Grid.Mass[cell];
    float f2 = Grid.Temperature[cell];
    if (float.IsNaN(f1) || float.IsNaN(f2) || ((double) f1 > 10000.0 || (double) f2 > 10000.0))
      return Color.red;
    if (element.IsVacuum)
      color = (double) f2 == 0.0 ? ((double) f1 == 0.0 ? Color.gray : Color.blue) : Color.yellow;
    else if ((double) f2 < 10.0)
      color = Color.red;
    else if ((double) Grid.Mass[cell] < 1.0 && (double) Grid.Pressure[cell] < 1.0)
      color = Color.green;
    else if ((double) f2 > (double) element.highTemp + 3.0 && element.highTempTransition != null)
      color = Color.magenta;
    else if ((double) f2 < (double) element.lowTemp + 3.0 && element.lowTempTransition != null)
      color = Color.cyan;
    return color;
  }

  private static Color GetFakeFloorColour(SimDebugView instance, int cell) => !Grid.FakeFloor[cell] ? Color.black : Color.cyan;

  private static Color GetFoundationColour(SimDebugView instance, int cell) => !Grid.Foundation[cell] ? Color.black : Color.white;

  private static Color GetDupePassableColour(SimDebugView instance, int cell) => !Grid.DupePassable[cell] ? Color.black : Color.green;

  private static Color GetCritterImpassableColour(SimDebugView instance, int cell) => !Grid.CritterImpassable[cell] ? Color.black : Color.yellow;

  private static Color GetDupeImpassableColour(SimDebugView instance, int cell) => !Grid.DupeImpassable[cell] ? Color.black : Color.red;

  private static Color GetMinionOccupiedColour(SimDebugView instance, int cell) => !((UnityEngine.Object) Grid.Objects[cell, 0] != (UnityEngine.Object) null) ? Color.black : Color.white;

  private static Color GetMinionGroupProberColour(SimDebugView instance, int cell) => !MinionGroupProber.Get().IsReachable(cell) ? Color.black : Color.white;

  private static Color GetPathProberColour(SimDebugView instance, int cell) => !((UnityEngine.Object) instance.selectedPathProber != (UnityEngine.Object) null) || instance.selectedPathProber.GetCost(cell) == -1 ? Color.black : Color.white;

  private static Color GetReservedColour(SimDebugView instance, int cell) => !Grid.Reserved[cell] ? Color.black : Color.white;

  private static Color GetAllowPathFindingColour(SimDebugView instance, int cell) => !Grid.AllowPathfinding[cell] ? Color.black : Color.white;

  private static Color GetMassColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (!SimDebugView.IsInsulated(cell))
    {
      float num = Grid.Mass[cell];
      if ((double) num > 0.0)
        color = Color.HSVToRGB(1f - (float) (((double) num - (double) SimDebugView.Instance.minMassExpected) / ((double) SimDebugView.Instance.maxMassExpected - (double) SimDebugView.Instance.minMassExpected)), 1f, 1f);
    }
    return color;
  }

  public static class OverlayModes
  {
    public static readonly HashedString Mass = (HashedString) nameof (Mass);
    public static readonly HashedString Pressure = (HashedString) nameof (Pressure);
    public static readonly HashedString GameGrid = (HashedString) nameof (GameGrid);
    public static readonly HashedString ScenePartitioner = (HashedString) nameof (ScenePartitioner);
    public static readonly HashedString ConduitUpdates = (HashedString) nameof (ConduitUpdates);
    public static readonly HashedString Flow = (HashedString) nameof (Flow);
    public static readonly HashedString StateChange = (HashedString) nameof (StateChange);
    public static readonly HashedString SimCheckErrorMap = (HashedString) nameof (SimCheckErrorMap);
    public static readonly HashedString DupePassable = (HashedString) nameof (DupePassable);
    public static readonly HashedString Foundation = (HashedString) nameof (Foundation);
    public static readonly HashedString FakeFloor = (HashedString) nameof (FakeFloor);
    public static readonly HashedString CritterImpassable = (HashedString) nameof (CritterImpassable);
    public static readonly HashedString DupeImpassable = (HashedString) nameof (DupeImpassable);
    public static readonly HashedString MinionGroupProber = (HashedString) nameof (MinionGroupProber);
    public static readonly HashedString PathProber = (HashedString) nameof (PathProber);
    public static readonly HashedString Reserved = (HashedString) nameof (Reserved);
    public static readonly HashedString AllowPathFinding = (HashedString) nameof (AllowPathFinding);
    public static readonly HashedString Danger = (HashedString) nameof (Danger);
    public static readonly HashedString MinionOccupied = (HashedString) nameof (MinionOccupied);
    public static readonly HashedString TileType = (HashedString) nameof (TileType);
    public static readonly HashedString State = (HashedString) nameof (State);
    public static readonly HashedString SolidLiquid = (HashedString) nameof (SolidLiquid);
    public static readonly HashedString Joules = (HashedString) nameof (Joules);
  }

  public enum GameGridMode
  {
    GameSolidMap,
    Lighting,
    RoomMap,
    Style,
    PlantDensity,
    DigAmount,
    DupePassable,
  }

  [Serializable]
  public struct ColorThreshold
  {
    public string colorName;
    public float value;
  }

  private struct UpdateSimViewSharedData
  {
    public SimDebugView instance;
    public HashedString simViewMode;
    public SimDebugView simDebugView;
    public byte[] textureBytes;

    public UpdateSimViewSharedData(
      SimDebugView instance,
      byte[] texture_bytes,
      HashedString sim_view_mode,
      SimDebugView sim_debug_view)
    {
      this.instance = instance;
      this.textureBytes = texture_bytes;
      this.simViewMode = sim_view_mode;
      this.simDebugView = sim_debug_view;
    }
  }

  private struct UpdateSimViewWorkItem : IWorkItem<SimDebugView.UpdateSimViewSharedData>
  {
    private int x0;
    private int y0;
    private int x1;
    private int y1;

    public UpdateSimViewWorkItem(int x0, int y0, int x1, int y1)
    {
      this.x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
      this.x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
      this.y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
      this.y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);
    }

    public void Run(SimDebugView.UpdateSimViewSharedData shared_data)
    {
      Func<SimDebugView, int, Color> func;
      if (!shared_data.instance.getColourFuncs.TryGetValue(shared_data.simViewMode, out func))
        func = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
      for (int y0 = this.y0; y0 <= this.y1; ++y0)
      {
        int cell1 = Grid.XYToCell(this.x0, y0);
        int cell2 = Grid.XYToCell(this.x1, y0);
        for (int index1 = cell1; index1 <= cell2; ++index1)
        {
          Color color = func(shared_data.instance, index1);
          int index2 = index1 * 4;
          shared_data.textureBytes[index2] = (byte) ((double) Mathf.Min(color.r, 1f) * (double) byte.MaxValue);
          shared_data.textureBytes[index2 + 1] = (byte) ((double) Mathf.Min(color.g, 1f) * (double) byte.MaxValue);
          shared_data.textureBytes[index2 + 2] = (byte) ((double) Mathf.Min(color.b, 1f) * (double) byte.MaxValue);
          shared_data.textureBytes[index2 + 3] = (byte) ((double) Mathf.Min(color.a, 1f) * (double) byte.MaxValue);
        }
      }
    }
  }

  public enum DangerAmount
  {
    None = 0,
    VeryLow = 1,
    Low = 2,
    Moderate = 3,
    High = 4,
    VeryHigh = 5,
    Extreme = 6,
    MAX_DANGERAMOUNT = 6,
  }
}
