﻿// Decompiled with JetBrains decompiler
// Type: GlobalResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using UnityEngine;

public class GlobalResources : ScriptableObject
{
  public Material AnimMaterial;
  public Material AnimUIMaterial;
  public Material AnimPlaceMaterial;
  public Material AnimMaterialUIDesaturated;
  public Material AnimSimpleMaterial;
  public Material AnimOverlayMaterial;
  public Texture2D WhiteTexture;
  [EventRef]
  public string ConduitOverlaySoundLiquid;
  [EventRef]
  public string ConduitOverlaySoundGas;
  [EventRef]
  public string ConduitOverlaySoundSolid;
  [EventRef]
  public string AcousticDisturbanceSound;
  [EventRef]
  public string AcousticDisturbanceBubbleSound;
  [EventRef]
  public string WallDamageLayerSound;
  public Sprite sadDupeAudio;
  public Sprite sadDupe;
  private static GlobalResources _Instance;

  public static GlobalResources Instance()
  {
    if ((Object) GlobalResources._Instance == (Object) null)
      GlobalResources._Instance = Resources.Load<GlobalResources>(nameof (GlobalResources));
    return GlobalResources._Instance;
  }
}
