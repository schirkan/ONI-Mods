// Decompiled with JetBrains decompiler
// Type: PropertyTextures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

[AddComponentMenu("KMonoBehaviour/scripts/PropertyTextures")]
public class PropertyTextures : KMonoBehaviour, ISim200ms
{
  [NonSerialized]
  public bool ForceLightEverywhere;
  [SerializeField]
  private Vector2 PressureRange = new Vector2(15f, 200f);
  [SerializeField]
  private float MinPressureVisibility = 0.1f;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float TemperatureStateChangeRange = 0.05f;
  public static PropertyTextures instance;
  public static IntPtr externalFlowTex;
  public static IntPtr externalLiquidTex;
  public static IntPtr externalExposedToSunlight;
  public static IntPtr externalSolidDigAmountTex;
  [SerializeField]
  private Vector2 coldRange;
  [SerializeField]
  private Vector2 hotRange;
  public static float FogOfWarScale;
  private int WorldSizeID;
  private int FogOfWarScaleID;
  private int PropTexWsToCsID;
  private int PropTexCsToWsID;
  private int TopBorderHeightID;
  private int NextPropertyIdx;
  public TextureBuffer[] textureBuffers;
  public TextureLerper[] lerpers;
  private TexturePagePool texturePagePool;
  [SerializeField]
  private Texture2D[] externallyUpdatedTextures;
  private PropertyTextures.TextureProperties[] textureProperties = new PropertyTextures.TextureProperties[12]
  {
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.Flow,
      textureFormat = TextureFormat.RGFloat,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = true,
      updatedExternally = true,
      blend = true,
      blendSpeed = 0.25f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.Liquid,
      textureFormat = TextureFormat.RGBA32,
      filterMode = FilterMode.Point,
      updateEveryFrame = true,
      updatedExternally = true,
      blend = true,
      blendSpeed = 1f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.ExposedToSunlight,
      textureFormat = TextureFormat.Alpha8,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = true,
      updatedExternally = true,
      blend = false,
      blendSpeed = 0.0f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.SolidDigAmount,
      textureFormat = TextureFormat.RGB24,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = true,
      updatedExternally = false,
      blend = false,
      blendSpeed = 0.0f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.GasColour,
      textureFormat = TextureFormat.RGBA32,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = false,
      updatedExternally = false,
      blend = true,
      blendSpeed = 0.25f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.GasDanger,
      textureFormat = TextureFormat.Alpha8,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = false,
      updatedExternally = false,
      blend = true,
      blendSpeed = 0.25f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.GasPressure,
      textureFormat = TextureFormat.Alpha8,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = false,
      updatedExternally = false,
      blend = true,
      blendSpeed = 0.25f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.FogOfWar,
      textureFormat = TextureFormat.Alpha8,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = true,
      updatedExternally = false,
      blend = false,
      blendSpeed = 0.0f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.WorldLight,
      textureFormat = TextureFormat.RGBA32,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = false,
      updatedExternally = false,
      blend = false,
      blendSpeed = 0.0f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.StateChange,
      textureFormat = TextureFormat.Alpha8,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = false,
      updatedExternally = false,
      blend = false,
      blendSpeed = 0.0f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.SolidLiquidGasMass,
      textureFormat = TextureFormat.RGBA32,
      filterMode = FilterMode.Point,
      updateEveryFrame = true,
      updatedExternally = false,
      blend = false,
      blendSpeed = 0.0f
    },
    new PropertyTextures.TextureProperties()
    {
      simProperty = PropertyTextures.Property.Temperature,
      textureFormat = TextureFormat.RGB24,
      filterMode = FilterMode.Bilinear,
      updateEveryFrame = false,
      updatedExternally = false,
      blend = false,
      blendSpeed = 0.0f
    }
  };
  private List<PropertyTextures.TextureProperties> allTextureProperties = new List<PropertyTextures.TextureProperties>();
  private WorkItemCollection<PropertyTextures.WorkItem, object> workItems = new WorkItemCollection<PropertyTextures.WorkItem, object>();

  public static void DestroyInstance()
  {
    ShaderReloader.Unregister(new System.Action(PropertyTextures.instance.OnShadersReloaded));
    PropertyTextures.externalFlowTex = IntPtr.Zero;
    PropertyTextures.externalLiquidTex = IntPtr.Zero;
    PropertyTextures.externalExposedToSunlight = IntPtr.Zero;
    PropertyTextures.externalSolidDigAmountTex = IntPtr.Zero;
    PropertyTextures.instance = (PropertyTextures) null;
  }

  protected override void OnPrefabInit()
  {
    PropertyTextures.instance = this;
    base.OnPrefabInit();
    ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
  }

  public static bool IsFogOfWarEnabled => (double) PropertyTextures.FogOfWarScale < 1.0;

  public void SetFilterMode(PropertyTextures.Property property, FilterMode mode) => this.textureProperties[(int) property].filterMode = mode;

  public Texture GetTexture(PropertyTextures.Property property) => (Texture) this.textureBuffers[(int) property].texture;

  private string GetShaderPropertyName(PropertyTextures.Property property) => "_" + property.ToString() + "Tex";

  protected override void OnSpawn()
  {
    if (GenericGameSettings.instance.disableFogOfWar)
      PropertyTextures.FogOfWarScale = 1f;
    this.WorldSizeID = Shader.PropertyToID("_WorldSizeInfo");
    this.FogOfWarScaleID = Shader.PropertyToID("_FogOfWarScale");
    this.PropTexWsToCsID = Shader.PropertyToID("_PropTexWsToCs");
    this.PropTexCsToWsID = Shader.PropertyToID("_PropTexCsToWs");
    this.TopBorderHeightID = Shader.PropertyToID("_TopBorderHeight");
  }

  public void OnReset(object data = null)
  {
    this.lerpers = new TextureLerper[12];
    this.texturePagePool = new TexturePagePool();
    this.textureBuffers = new TextureBuffer[12];
    this.externallyUpdatedTextures = new Texture2D[12];
    for (int index1 = 0; index1 < 12; ++index1)
    {
      PropertyTextures.TextureProperties textureProperties = new PropertyTextures.TextureProperties()
      {
        textureFormat = TextureFormat.Alpha8,
        filterMode = FilterMode.Bilinear,
        blend = false,
        blendSpeed = 1f
      };
      for (int index2 = 0; index2 < this.textureProperties.Length; ++index2)
      {
        if ((PropertyTextures.Property) index1 == this.textureProperties[index2].simProperty)
          textureProperties = this.textureProperties[index2];
      }
      ref PropertyTextures.TextureProperties local = ref textureProperties;
      PropertyTextures.Property property = (PropertyTextures.Property) index1;
      string str = property.ToString();
      local.name = str;
      if ((UnityEngine.Object) this.externallyUpdatedTextures[index1] != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.externallyUpdatedTextures[index1]);
        this.externallyUpdatedTextures[index1] = (Texture2D) null;
      }
      Texture texture;
      if (textureProperties.updatedExternally)
      {
        this.externallyUpdatedTextures[index1] = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureUtil.TextureFormatToGraphicsFormat(textureProperties.textureFormat), TextureCreationFlags.None);
        texture = (Texture) this.externallyUpdatedTextures[index1];
      }
      else
      {
        TextureBuffer[] textureBuffers = this.textureBuffers;
        int index2 = index1;
        property = (PropertyTextures.Property) index1;
        TextureBuffer textureBuffer = new TextureBuffer(property.ToString(), Grid.WidthInCells, Grid.HeightInCells, textureProperties.textureFormat, textureProperties.filterMode, this.texturePagePool);
        textureBuffers[index2] = textureBuffer;
        texture = (Texture) this.textureBuffers[index1].texture;
      }
      if (textureProperties.blend)
      {
        TextureLerper[] lerpers = this.lerpers;
        int index2 = index1;
        Texture target_texture = texture;
        property = (PropertyTextures.Property) index1;
        string name = property.ToString();
        int filterMode = (int) texture.filterMode;
        int textureFormat = (int) textureProperties.textureFormat;
        TextureLerper textureLerper = new TextureLerper(target_texture, name, (FilterMode) filterMode, (TextureFormat) textureFormat);
        lerpers[index2] = textureLerper;
        this.lerpers[index1].Speed = textureProperties.blendSpeed;
      }
      string shaderPropertyName = this.GetShaderPropertyName((PropertyTextures.Property) index1);
      texture.name = shaderPropertyName;
      textureProperties.texturePropertyName = shaderPropertyName;
      Shader.SetGlobalTexture(shaderPropertyName, texture);
      this.allTextureProperties.Add(textureProperties);
    }
  }

  private void OnShadersReloaded()
  {
    for (int index = 0; index < 12; ++index)
    {
      TextureLerper lerper = this.lerpers[index];
      if (lerper != null)
        Shader.SetGlobalTexture(this.allTextureProperties[index].texturePropertyName, lerper.Update());
    }
  }

  public void Sim200ms(float dt)
  {
    if (this.lerpers == null || this.lerpers.Length == 0)
      return;
    for (int index = 0; index < this.lerpers.Length; ++index)
      this.lerpers[index]?.LongUpdate(dt);
  }

  private void UpdateTextureThreaded(
    TextureRegion texture_region,
    int x0,
    int y0,
    int x1,
    int y1,
    PropertyTextures.WorkItem.Callback update_texture_cb)
  {
    this.workItems.Reset((object) null);
    int num = 16;
    for (int y0_1 = y0; y0_1 <= y1; y0_1 += num)
    {
      int y1_1 = Math.Min(y0_1 + num - 1, y1);
      this.workItems.Add(new PropertyTextures.WorkItem(texture_region, x0, y0_1, x1, y1_1, update_texture_cb));
    }
    GlobalJobManager.Run((IWorkItemCollection) this.workItems);
  }

  private void UpdateProperty(
    ref PropertyTextures.TextureProperties p,
    int x0,
    int y0,
    int x1,
    int y1)
  {
    if (Game.Instance.IsLoading())
      return;
    int simProperty = (int) p.simProperty;
    if (!p.updatedExternally)
    {
      TextureRegion texture_region = this.textureBuffers[simProperty].Lock(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
      switch (p.simProperty)
      {
        case PropertyTextures.Property.StateChange:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateStateChange));
          break;
        case PropertyTextures.Property.GasPressure:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdatePressure));
          break;
        case PropertyTextures.Property.GasColour:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateGasColour));
          break;
        case PropertyTextures.Property.GasDanger:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateDanger));
          break;
        case PropertyTextures.Property.FogOfWar:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateFogOfWar));
          break;
        case PropertyTextures.Property.SolidDigAmount:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidDigAmount));
          break;
        case PropertyTextures.Property.SolidLiquidGasMass:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidLiquidGasMass));
          break;
        case PropertyTextures.Property.WorldLight:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateWorldLight));
          break;
        case PropertyTextures.Property.Temperature:
          this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateTemperature));
          break;
      }
      texture_region.Unlock();
    }
    else
    {
      switch (p.simProperty)
      {
        case PropertyTextures.Property.Flow:
          this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalFlowTex, 8 * Grid.WidthInCells * Grid.HeightInCells);
          break;
        case PropertyTextures.Property.Liquid:
          this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalLiquidTex, 4 * Grid.WidthInCells * Grid.HeightInCells);
          break;
        case PropertyTextures.Property.ExposedToSunlight:
          this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalExposedToSunlight, Grid.WidthInCells * Grid.HeightInCells);
          break;
      }
      this.externallyUpdatedTextures[simProperty].Apply();
    }
  }

  private void LateUpdate()
  {
    if (!Grid.IsInitialized())
      return;
    Shader.SetGlobalVector(this.WorldSizeID, new Vector4((float) Grid.WidthInCells, (float) Grid.HeightInCells, 1f / (float) Grid.WidthInCells, 1f / (float) Grid.HeightInCells));
    Shader.SetGlobalVector(this.PropTexWsToCsID, new Vector4(0.0f, 0.0f, 1f, 1f));
    Shader.SetGlobalVector(this.PropTexCsToWsID, new Vector4(0.0f, 0.0f, 1f, 1f));
    Shader.SetGlobalFloat(this.TopBorderHeightID, (float) Grid.TopBorderHeight);
    int x0;
    int y0;
    int x1;
    int y1;
    this.GetVisibleCellRange(out x0, out y0, out x1, out y1);
    Shader.SetGlobalFloat(this.FogOfWarScaleID, PropertyTextures.FogOfWarScale);
    int index1 = this.NextPropertyIdx++ % this.allTextureProperties.Count;
    for (PropertyTextures.TextureProperties allTextureProperty = this.allTextureProperties[index1]; allTextureProperty.updateEveryFrame; allTextureProperty = this.allTextureProperties[index1])
      index1 = this.NextPropertyIdx++ % this.allTextureProperties.Count;
    for (int index2 = 0; index2 < this.allTextureProperties.Count; ++index2)
    {
      PropertyTextures.TextureProperties allTextureProperty = this.allTextureProperties[index2];
      if (index1 == index2 || allTextureProperty.updateEveryFrame || GameUtil.IsCapturingTimeLapse())
        this.UpdateProperty(ref allTextureProperty, x0, y0, x1, y1);
    }
    for (int index2 = 0; index2 < 12; ++index2)
    {
      TextureLerper lerper = this.lerpers[index2];
      if (lerper != null)
      {
        if ((double) Time.timeScale == 0.0)
          lerper.LongUpdate(Time.unscaledDeltaTime);
        Shader.SetGlobalTexture(this.allTextureProperties[index2].texturePropertyName, lerper.Update());
      }
    }
  }

  private void GetVisibleCellRange(out int x0, out int y0, out int x1, out int y1)
  {
    int num = 16;
    Grid.GetVisibleExtents(out x0, out y0, out x1, out y1);
    x0 = Math.Max(0, x0 - num);
    y0 = Math.Max(0, y0 - num);
    x0 = Mathf.Min(x0, Grid.WidthInCells - 1);
    y0 = Mathf.Min(y0, Grid.HeightInCells - 1);
    x1 = Mathf.CeilToInt((float) (x1 + num));
    y1 = Mathf.CeilToInt((float) (y1 + num));
    x1 = Mathf.Max(x1, 0);
    y1 = Mathf.Max(y1, 0);
    x1 = Mathf.Min(x1, Grid.WidthInCells - 1);
    y1 = Mathf.Min(y1, Grid.HeightInCells - 1);
  }

  private static void UpdateFogOfWar(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    byte[] visible = Grid.Visible;
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        region.SetBytes(x, y, visible[cell]);
      }
    }
  }

  private static void UpdatePressure(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    Vector2 pressureRange = PropertyTextures.instance.PressureRange;
    float pressureVisibility = PropertyTextures.instance.MinPressureVisibility;
    float num1 = pressureRange.y - pressureRange.x;
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        float num2 = 0.0f;
        Element element = Grid.Element[cell];
        if (element.IsGas)
        {
          double num3 = (double) Grid.Pressure[cell];
          float b = num3 > 0.0 ? pressureVisibility : 0.0f;
          num2 = Mathf.Max(Mathf.Clamp01(((float) num3 - pressureRange.x) / num1), b);
        }
        else if (element.IsLiquid)
        {
          int index = Grid.CellAbove(cell);
          if (Grid.IsValidCell(index) && Grid.Element[index].IsGas)
          {
            double num3 = (double) Grid.Pressure[index];
            float b = num3 > 0.0 ? pressureVisibility : 0.0f;
            num2 = Mathf.Max(Mathf.Clamp01(((float) num3 - pressureRange.x) / num1), b);
          }
        }
        region.SetBytes(x, y, (byte) ((double) num2 * (double) byte.MaxValue));
      }
    }
  }

  private static void UpdateDanger(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        byte b0 = Grid.Element[cell].id == SimHashes.Oxygen ? (byte) 0 : byte.MaxValue;
        region.SetBytes(x, y, b0);
      }
    }
  }

  private static void UpdateStateChange(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    float stateChangeRange = PropertyTextures.instance.TemperatureStateChangeRange;
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        float a1 = 0.0f;
        Element element = Grid.Element[cell];
        if (!element.IsVacuum)
        {
          double num1 = (double) Grid.Temperature[cell];
          float num2 = element.lowTemp * stateChangeRange;
          float a2 = Mathf.Abs((float) num1 - element.lowTemp) / num2;
          float num3 = element.highTemp * stateChangeRange;
          float b = Mathf.Abs((float) num1 - element.highTemp) / num3;
          a1 = Mathf.Max(a1, 1f - Mathf.Min(a2, b));
        }
        region.SetBytes(x, y, (byte) ((double) a1 * (double) byte.MaxValue));
      }
    }
  }

  private static void UpdateGasColour(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        Element element = Grid.Element[cell];
        if (element.IsGas)
          region.SetBytes(x, y, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
        else if (element.IsLiquid)
        {
          if (Grid.IsValidCell(Grid.CellAbove(cell)))
            region.SetBytes(x, y, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
          else
            region.SetBytes(x, y, (byte) 0, (byte) 0, (byte) 0, (byte) 0);
        }
        else
          region.SetBytes(x, y, (byte) 0, (byte) 0, (byte) 0, (byte) 0);
      }
    }
  }

  private static void UpdateLiquid(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    for (int x = x0; x <= x1; ++x)
    {
      int cell1 = Grid.XYToCell(x, y1);
      Element element1 = Grid.Element[cell1];
      for (int y = y1; y >= y0; --y)
      {
        int cell2 = Grid.XYToCell(x, y);
        Element element2 = Grid.Element[cell2];
        if (element2.IsLiquid)
        {
          Color32 colour = element2.substance.colour;
          float liquidMaxMass = Lighting.Instance.Settings.LiquidMaxMass;
          float liquidAmountOffset = Lighting.Instance.Settings.LiquidAmountOffset;
          float num1;
          if (element1.IsLiquid || element1.IsSolid)
          {
            num1 = 1f;
          }
          else
          {
            float num2 = liquidAmountOffset + (1f - liquidAmountOffset) * Mathf.Min(Grid.Mass[cell2] / liquidMaxMass, 1f);
            num1 = Mathf.Pow(Mathf.Min(Grid.Mass[cell2] / liquidMaxMass, 1f), 0.45f);
          }
          region.SetBytes(x, y, (byte) ((double) num1 * (double) byte.MaxValue), colour.r, colour.g, colour.b);
        }
        else
          region.SetBytes(x, y, (byte) 0, (byte) 0, (byte) 0, (byte) 0);
        element1 = element2;
      }
    }
  }

  private static void UpdateSolidDigAmount(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    int elementIndex = ElementLoader.GetElementIndex(SimHashes.Void);
    for (int y = y0; y <= y1; ++y)
    {
      int cell1 = Grid.XYToCell(x0, y);
      int cell2 = Grid.XYToCell(x1, y);
      int i = cell1;
      int x = x0;
      while (i <= cell2)
      {
        byte b0 = 0;
        byte b1 = 0;
        byte b2 = 0;
        if ((int) Grid.ElementIdx[i] != elementIndex)
          b2 = byte.MaxValue;
        if (Grid.Solid[i])
        {
          b0 = byte.MaxValue;
          b1 = (byte) ((double) byte.MaxValue * (double) Grid.Damage[i]);
        }
        region.SetBytes(x, y, b0, b1, b2);
        ++i;
        ++x;
      }
    }
  }

  private static void UpdateSolidLiquidGasMass(
    TextureRegion region,
    int x0,
    int y0,
    int x1,
    int y1)
  {
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        Element element = Grid.Element[cell];
        byte b0 = 0;
        byte b1 = 0;
        byte b2 = 0;
        if (element.IsSolid)
          b0 = byte.MaxValue;
        else if (element.IsLiquid)
          b1 = byte.MaxValue;
        else if (element.IsGas || element.IsVacuum)
          b2 = byte.MaxValue;
        float num = Grid.Mass[cell];
        float b = Mathf.Min(1f, num / 2000f);
        if ((double) num > 0.0)
          b = Mathf.Max(0.003921569f, b);
        region.SetBytes(x, y, b0, b1, b2, (byte) ((double) b * (double) byte.MaxValue));
      }
    }
  }

  private static void GetTemperatureAlpha(
    float t,
    Vector2 cold_range,
    Vector2 hot_range,
    out byte cold_alpha,
    out byte hot_alpha)
  {
    cold_alpha = (byte) 0;
    hot_alpha = (byte) 0;
    if ((double) t <= (double) cold_range.y)
    {
      float num = Mathf.Clamp01((float) (((double) cold_range.y - (double) t) / ((double) cold_range.y - (double) cold_range.x)));
      cold_alpha = (byte) ((double) num * (double) byte.MaxValue);
    }
    else
    {
      if ((double) t < (double) hot_range.x)
        return;
      float num = Mathf.Clamp01((float) (((double) t - (double) hot_range.x) / ((double) hot_range.y - (double) hot_range.x)));
      hot_alpha = (byte) ((double) num * (double) byte.MaxValue);
    }
  }

  private static void UpdateTemperature(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    Vector2 coldRange = PropertyTextures.instance.coldRange;
    Vector2 hotRange = PropertyTextures.instance.hotRange;
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        float t = Grid.Temperature[cell];
        byte cold_alpha;
        byte hot_alpha;
        PropertyTextures.GetTemperatureAlpha(t, coldRange, hotRange, out cold_alpha, out hot_alpha);
        byte b2 = (byte) ((double) byte.MaxValue * (double) Mathf.Pow(Mathf.Clamp(t / 1000f, 0.0f, 1f), 0.45f));
        region.SetBytes(x, y, cold_alpha, hot_alpha, b2);
      }
    }
  }

  private static void UpdateWorldLight(TextureRegion region, int x0, int y0, int x1, int y1)
  {
    if (!PropertyTextures.instance.ForceLightEverywhere)
    {
      for (int y = y0; y <= y1; ++y)
      {
        int cell1 = Grid.XYToCell(x0, y);
        int cell2 = Grid.XYToCell(x1, y);
        int index = cell1;
        int x = x0;
        while (index <= cell2)
        {
          Color32 color32 = Grid.LightCount[index] > 0 ? Lighting.Instance.Settings.LightColour : new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
          region.SetBytes(x, y, color32.r, color32.g, color32.b, (int) color32.r + (int) color32.g + (int) color32.b > 0 ? byte.MaxValue : (byte) 0);
          ++index;
          ++x;
        }
      }
    }
    else
    {
      for (int y = y0; y <= y1; ++y)
      {
        for (int x = x0; x <= x1; ++x)
          region.SetBytes(x, y, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      }
    }
  }

  public enum Property
  {
    StateChange,
    GasPressure,
    GasColour,
    GasDanger,
    FogOfWar,
    Flow,
    SolidDigAmount,
    SolidLiquidGasMass,
    WorldLight,
    Liquid,
    Temperature,
    ExposedToSunlight,
    Num,
  }

  private struct TextureProperties
  {
    public string name;
    public PropertyTextures.Property simProperty;
    public TextureFormat textureFormat;
    public FilterMode filterMode;
    public bool updateEveryFrame;
    public bool updatedExternally;
    public bool blend;
    public float blendSpeed;
    public string texturePropertyName;
  }

  private struct WorkItem : IWorkItem<object>
  {
    private int x0;
    private int y0;
    private int x1;
    private int y1;
    private TextureRegion textureRegion;
    private PropertyTextures.WorkItem.Callback updateTextureCb;

    public WorkItem(
      TextureRegion texture_region,
      int x0,
      int y0,
      int x1,
      int y1,
      PropertyTextures.WorkItem.Callback update_texture_cb)
    {
      this.textureRegion = texture_region;
      this.x0 = x0;
      this.y0 = y0;
      this.x1 = x1;
      this.y1 = y1;
      this.updateTextureCb = update_texture_cb;
    }

    public void Run(object shared_data) => this.updateTextureCb(this.textureRegion, this.x0, this.y0, this.x1, this.y1);

    public delegate void Callback(TextureRegion texture_region, int x0, int y0, int x1, int y1);
  }
}
