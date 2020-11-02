﻿// Decompiled with JetBrains decompiler
// Type: Timelapser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.IO;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Timelapser")]
public class Timelapser : KMonoBehaviour
{
  private bool screenshotActive;
  private bool screenshotPending;
  private bool previewScreenshot;
  private string previewSaveGamePath = "";
  private bool screenshotToday;
  private HashedString activeOverlay;
  private Camera freezeCamera;
  private RenderTexture bufferRenderTexture;
  private Vector3 camPosition;
  private float camSize;
  private bool debugScreenShot;
  private Vector2Int previewScreenshotResolution = new Vector2Int(Grid.WidthInCells * 2, Grid.HeightInCells * 2);
  private const int DEFAULT_SCREENSHOT_INTERVAL = 10;
  private int[] timelapseScreenshotCycles = new int[100]
  {
    1,
    2,
    3,
    4,
    5,
    6,
    7,
    8,
    9,
    10,
    11,
    12,
    13,
    14,
    15,
    16,
    17,
    18,
    19,
    20,
    21,
    22,
    23,
    24,
    25,
    26,
    27,
    28,
    29,
    30,
    31,
    32,
    33,
    34,
    35,
    36,
    37,
    38,
    39,
    40,
    41,
    42,
    43,
    44,
    45,
    46,
    47,
    48,
    49,
    50,
    55,
    60,
    65,
    70,
    75,
    80,
    85,
    90,
    95,
    100,
    110,
    120,
    130,
    140,
    150,
    160,
    170,
    180,
    190,
    200,
    210,
    220,
    230,
    240,
    250,
    260,
    270,
    280,
    290,
    200,
    310,
    320,
    330,
    340,
    350,
    360,
    370,
    380,
    390,
    400,
    410,
    420,
    430,
    440,
    450,
    460,
    470,
    480,
    490,
    500
  };

  public bool CapturingTimelapseScreenshot => this.screenshotActive;

  public Texture2D freezeTexture { get; private set; }

  protected override void OnPrefabInit()
  {
    this.RefreshRenderTextureSize();
    Game.Instance.Subscribe(75424175, new System.Action<object>(this.RefreshRenderTextureSize));
    this.freezeCamera = CameraController.Instance.timelapseFreezeCamera;
    if ((double) this.CycleTimeToScreenshot() > 0.0)
      this.OnNewDay();
    GameClock.Instance.Subscribe(631075836, new System.Action<object>(this.OnNewDay));
    this.OnResize();
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.StartCoroutine(this.Render());
  }

  private void OnResize()
  {
    if ((UnityEngine.Object) this.freezeTexture != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.freezeTexture);
    this.freezeTexture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight, TextureFormat.ARGB32, false);
  }

  private void RefreshRenderTextureSize(object data = null)
  {
    if (this.previewScreenshot)
    {
      this.bufferRenderTexture = new RenderTexture(this.previewScreenshotResolution.x, this.previewScreenshotResolution.y, 32, RenderTextureFormat.ARGB32);
    }
    else
    {
      if (!this.timelapseUserEnabled)
        return;
      this.bufferRenderTexture = new RenderTexture(SaveGame.Instance.TimelapseResolution.x, SaveGame.Instance.TimelapseResolution.y, 32, RenderTextureFormat.ARGB32);
    }
  }

  private bool timelapseUserEnabled => SaveGame.Instance.TimelapseResolution.x > 0;

  private void OnNewDay(object data = null)
  {
    int cycle = GameClock.Instance.GetCycle();
    if (cycle > this.timelapseScreenshotCycles[this.timelapseScreenshotCycles.Length - 1])
    {
      if (cycle % 10 != 0)
        return;
      this.screenshotToday = true;
    }
    else
    {
      for (int index = 0; index < this.timelapseScreenshotCycles.Length; ++index)
      {
        if (cycle == this.timelapseScreenshotCycles[index])
          this.screenshotToday = true;
      }
    }
  }

  private void Update()
  {
    if (!this.screenshotToday || (double) this.CycleTimeToScreenshot() > 0.0)
      return;
    if (!this.timelapseUserEnabled)
    {
      this.screenshotToday = false;
    }
    else
    {
      if (PlayerController.Instance.IsDragging())
        return;
      CameraController.Instance.ForcePanningState(false);
      this.screenshotToday = false;
      this.SaveScreenshot();
    }
  }

  private float CycleTimeToScreenshot() => (float) (300.0 - (double) GameClock.Instance.GetTime() % 600.0);

  private IEnumerator Render()
  {
    WaitForEndOfFrame wait = new WaitForEndOfFrame();
    while (true)
    {
      do
      {
        yield return (object) wait;
      }
      while (!this.screenshotPending);
      if (!this.freezeCamera.enabled)
      {
        this.freezeTexture.ReadPixels(new Rect(0.0f, 0.0f, (float) Camera.main.pixelWidth, (float) Camera.main.pixelHeight), 0, 0);
        this.freezeTexture.Apply();
        this.freezeCamera.gameObject.GetComponent<FillRenderTargetEffect>().SetFillTexture((Texture) this.freezeTexture);
        this.freezeCamera.enabled = true;
        this.screenshotActive = true;
        this.RefreshRenderTextureSize();
        this.SetPostionAndOrtho();
        DebugHandler.SetTimelapseMode(true);
        this.activeOverlay = OverlayScreen.Instance.mode;
        OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
      }
      else
      {
        this.RenderAndPrint();
        this.freezeCamera.enabled = false;
        DebugHandler.SetTimelapseMode(false);
        this.screenshotPending = false;
        this.previewScreenshot = false;
        this.screenshotActive = false;
        this.debugScreenShot = false;
        this.previewSaveGamePath = "";
        OverlayScreen.Instance.ToggleOverlay(this.activeOverlay, false);
      }
    }
  }

  public void SaveScreenshot() => this.screenshotPending = true;

  public void SaveColonyPreview(string saveFileName)
  {
    this.previewSaveGamePath = saveFileName;
    this.previewScreenshot = true;
    this.SaveScreenshot();
  }

  private void SetPostionAndOrtho()
  {
    float num1 = 0.0f;
    GameObject telepad = GameUtil.GetTelepad();
    if ((UnityEngine.Object) telepad == (UnityEngine.Object) null)
      return;
    Vector3 position1 = telepad.transform.GetPosition();
    foreach (KMonoBehaviour kmonoBehaviour in Components.BuildingCompletes.Items)
    {
      Vector3 position2 = kmonoBehaviour.transform.GetPosition();
      float num2 = (float) this.bufferRenderTexture.width / (float) this.bufferRenderTexture.height;
      Vector3 vector3 = position1 - position2;
      num1 = Mathf.Max(num1, vector3.x / num2, vector3.y);
    }
    float size = Mathf.Max(num1 + 10f, 18f);
    this.camSize = CameraController.Instance.overlayCamera.orthographicSize;
    CameraController.Instance.SetOrthographicsSize(size);
    this.camPosition = CameraController.Instance.transform.position;
    CameraController.Instance.SetPosition(new Vector3(telepad.transform.position.x, telepad.transform.position.y, CameraController.Instance.transform.position.z));
    CameraController.Instance.SetTargetPos(new Vector3(telepad.transform.position.x, telepad.transform.position.y, CameraController.Instance.transform.position.z), this.camSize, false);
  }

  private void RenderAndPrint()
  {
    GameObject telepad = GameUtil.GetTelepad();
    if ((UnityEngine.Object) telepad == (UnityEngine.Object) null)
    {
      Debug.Log((object) "No telepad present, aborting screenshot.");
    }
    else
    {
      RenderTexture active = RenderTexture.active;
      RenderTexture.active = this.bufferRenderTexture;
      CameraController.Instance.SetPosition(new Vector3(telepad.transform.position.x, telepad.transform.position.y, CameraController.Instance.transform.position.z));
      CameraController.Instance.RenderForTimelapser(ref this.bufferRenderTexture);
      this.WriteToPng(this.bufferRenderTexture);
      CameraController.Instance.SetOrthographicsSize(this.camSize);
      CameraController.Instance.SetPosition(this.camPosition);
      CameraController.Instance.SetTargetPos(this.camPosition, this.camSize, false);
      RenderTexture.active = active;
    }
  }

  public void WriteToPng(RenderTexture renderTex)
  {
    Texture2D tex = new Texture2D(renderTex.width, renderTex.height, TextureFormat.ARGB32, false);
    tex.ReadPixels(new Rect(0.0f, 0.0f, (float) renderTex.width, (float) renderTex.height), 0, 0);
    tex.Apply();
    byte[] png = tex.EncodeToPNG();
    UnityEngine.Object.Destroy((UnityEngine.Object) tex);
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    string str1 = System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
    if (!Directory.Exists(str1))
      Directory.CreateDirectory(str1);
    string path2 = RetireColonyUtility.StripInvalidCharacters(SaveGame.Instance.BaseName);
    if (!this.previewScreenshot)
    {
      string str2 = System.IO.Path.Combine(str1, path2);
      if (!Directory.Exists(str2))
        Directory.CreateDirectory(str2);
      string str3 = System.IO.Path.Combine(str2, path2);
      DebugUtil.LogArgs((object) "Saving screenshot to", (object) str3);
      string format = "0000.##";
      string str4 = str3 + "_cycle_" + GameClock.Instance.GetCycle().ToString(format);
      if (this.debugScreenShot)
      {
        object[] objArray = new object[11]
        {
          (object) str4,
          (object) "_",
          (object) System.DateTime.Now.Day,
          (object) "-",
          (object) System.DateTime.Now.Month,
          (object) "_",
          null,
          null,
          null,
          null,
          null
        };
        System.DateTime now = System.DateTime.Now;
        objArray[6] = (object) now.Hour;
        objArray[7] = (object) "-";
        now = System.DateTime.Now;
        objArray[8] = (object) now.Minute;
        objArray[9] = (object) "-";
        now = System.DateTime.Now;
        objArray[10] = (object) now.Second;
        str4 = string.Concat(objArray);
      }
      File.WriteAllBytes(str4 + ".png", png);
    }
    else
    {
      string path = System.IO.Path.ChangeExtension(this.previewSaveGamePath, ".png");
      DebugUtil.LogArgs((object) "Saving screenshot to", (object) path);
      File.WriteAllBytes(path, png);
    }
  }
}
