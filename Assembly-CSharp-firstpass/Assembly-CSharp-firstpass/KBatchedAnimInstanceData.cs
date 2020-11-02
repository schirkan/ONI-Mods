﻿// Decompiled with JetBrains decompiler
// Type: KBatchedAnimInstanceData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class KBatchedAnimInstanceData
{
  public const int SIZE_IN_BYTES = 112;
  public const int SIZE_IN_FLOATS = 28;
  private KAnimConverter.IAnimConverter target;
  private bool isTransformOverriden;
  private KBatchedAnimInstanceData.AnimInstanceDataToByteConverter converter;

  public KBatchedAnimInstanceData(KAnimConverter.IAnimConverter target)
  {
    this.target = target;
    this.converter = new KBatchedAnimInstanceData.AnimInstanceDataToByteConverter()
    {
      bytes = new byte[112]
    };
    KBatchedAnimInstanceData.AnimInstanceData animInstanceData = this.converter.animInstanceData[0];
    animInstanceData.tintColour = Color.white;
    animInstanceData.highlightColour = Color.black;
    animInstanceData.overlayColour = Color.white;
    this.converter.animInstanceData[0] = animInstanceData;
  }

  public void SetClipRadius(float x, float y, float dist_sq, bool do_clip) => this.converter.animInstanceData[0].clipParameters = new Vector4(x, y, dist_sq, do_clip ? 1f : 0.0f);

  public void SetBlend(float amt) => this.converter.animInstanceData[0].blend = amt;

  public Color GetOverlayColour() => this.converter.animInstanceData[0].overlayColour;

  public bool SetOverlayColour(Color color)
  {
    if (!(color != this.converter.animInstanceData[0].overlayColour))
      return false;
    this.converter.animInstanceData[0].overlayColour = color;
    return true;
  }

  public Color GetTintColour() => this.converter.animInstanceData[0].tintColour;

  public bool SetTintColour(Color color)
  {
    if (!(color != this.converter.animInstanceData[0].tintColour))
      return false;
    this.converter.animInstanceData[0].tintColour = color;
    return true;
  }

  public Color GetHighlightcolour() => this.converter.animInstanceData[0].highlightColour;

  public bool SetHighlightColour(Color color)
  {
    if (!(color != this.converter.animInstanceData[0].highlightColour))
      return false;
    this.converter.animInstanceData[0].highlightColour = color;
    return true;
  }

  public void WriteToTexture(byte[] output_bytes, int output_index, int this_index)
  {
    KBatchedAnimInstanceData.AnimInstanceData animInstanceData = this.converter.animInstanceData[0];
    animInstanceData.curAnimFrameIndex = (float) this.target.GetCurrentFrameIndex();
    animInstanceData.thisIndex = (float) this_index;
    animInstanceData.currentAnimNumFrames = this.target.IsVisible() ? (float) this.target.GetCurrentNumFrames() : 0.0f;
    animInstanceData.currentAnimFirstFrameIdx = (float) this.target.GetFirstFrameIndex();
    if (!this.isTransformOverriden)
      animInstanceData.transformMatrix = this.target.GetTransformMatrix();
    this.converter.animInstanceData[0] = animInstanceData;
    Buffer.BlockCopy((Array) this.converter.bytes, 0, (Array) output_bytes, output_index, 112);
  }

  public void SetOverrideTransformMatrix(Matrix2x3 transform_matrix)
  {
    this.isTransformOverriden = true;
    this.converter.animInstanceData[0].transformMatrix = transform_matrix;
  }

  public void ClearOverrideTransformMatrix() => this.isTransformOverriden = false;

  [StructLayout(LayoutKind.Explicit)]
  public struct AnimInstanceData
  {
    [FieldOffset(0)]
    public float curAnimFrameIndex;
    [FieldOffset(4)]
    public float thisIndex;
    [FieldOffset(8)]
    public float currentAnimNumFrames;
    [FieldOffset(12)]
    public float currentAnimFirstFrameIdx;
    [FieldOffset(16)]
    public Matrix2x3 transformMatrix;
    [FieldOffset(40)]
    public float blend;
    [FieldOffset(44)]
    public float unused;
    [FieldOffset(48)]
    public Color highlightColour;
    [FieldOffset(64)]
    public Color tintColour;
    [FieldOffset(80)]
    public Color overlayColour;
    [FieldOffset(96)]
    public Vector4 clipParameters;
  }

  [StructLayout(LayoutKind.Explicit)]
  public struct AnimInstanceDataToByteConverter
  {
    [FieldOffset(0)]
    public byte[] bytes;
    [FieldOffset(0)]
    public KBatchedAnimInstanceData.AnimInstanceData[] animInstanceData;
  }
}
