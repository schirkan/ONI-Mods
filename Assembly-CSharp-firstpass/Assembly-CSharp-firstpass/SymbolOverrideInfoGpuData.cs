// Decompiled with JetBrains decompiler
// Type: SymbolOverrideInfoGpuData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SymbolOverrideInfoGpuData
{
  public const int FLOATS_PER_SYMBOL_OVERRIDE_INFO = 12;
  private SymbolOverrideInfoGpuData.SymbolOverrideInfoToByteConverter symbolOverrideInfoConverter;
  private int symbolCount;

  private SymbolOverrideInfoGpuData.SymbolOverrideInfo[] symbolOverrideInfos => this.symbolOverrideInfoConverter.symbolOverrideInfos;

  public int version { get; private set; }

  public SymbolOverrideInfoGpuData(int symbol_count)
  {
    this.symbolCount = symbol_count;
    this.symbolOverrideInfoConverter = new SymbolOverrideInfoGpuData.SymbolOverrideInfoToByteConverter()
    {
      bytes = new byte[12 * symbol_count * 4]
    };
    for (int index = 0; index < symbol_count; ++index)
      this.symbolOverrideInfos[index].atlas = 0.0f;
    this.MarkDirty();
  }

  private void MarkDirty() => ++this.version;

  public void SetSymbolOverrideInfo(
    int symbol_idx,
    KAnim.Build.SymbolFrameInstance symbol_frame_instance)
  {
    if (symbol_idx >= this.symbolCount)
      DebugUtil.Assert(false);
    ref SymbolOverrideInfoGpuData.SymbolOverrideInfo local = ref this.symbolOverrideInfos[symbol_idx];
    local.atlas = (float) symbol_frame_instance.buildImageIdx;
    local.isoverriden = 1f;
    local.bboxMin = symbol_frame_instance.symbolFrame.bboxMin;
    local.bboxMax = symbol_frame_instance.symbolFrame.bboxMax;
    local.uvMin = symbol_frame_instance.symbolFrame.uvMin;
    local.uvMax = symbol_frame_instance.symbolFrame.uvMax;
    this.MarkDirty();
  }

  public void WriteToTexture(byte[] data, int data_idx, int instance_idx)
  {
    DebugUtil.Assert(instance_idx * this.symbolCount * 12 * 4 == data_idx);
    Buffer.BlockCopy((Array) this.symbolOverrideInfoConverter.bytes, 0, (Array) data, data_idx, this.symbolCount * 12 * 4);
  }

  [StructLayout(LayoutKind.Explicit)]
  public struct SymbolOverrideInfo
  {
    [FieldOffset(0)]
    public float atlas;
    [FieldOffset(4)]
    public float isoverriden;
    [FieldOffset(8)]
    public float unused1;
    [FieldOffset(12)]
    public float unused2;
    [FieldOffset(16)]
    public Vector2 bboxMin;
    [FieldOffset(24)]
    public Vector2 bboxMax;
    [FieldOffset(32)]
    public Vector2 uvMin;
    [FieldOffset(40)]
    public Vector2 uvMax;
  }

  [StructLayout(LayoutKind.Explicit)]
  public struct SymbolOverrideInfoToByteConverter
  {
    [FieldOffset(0)]
    public byte[] bytes;
    [FieldOffset(0)]
    public SymbolOverrideInfoGpuData.SymbolOverrideInfo[] symbolOverrideInfos;
  }
}
