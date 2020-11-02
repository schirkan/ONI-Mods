// Decompiled with JetBrains decompiler
// Type: SymbolInstanceGpuData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SymbolInstanceGpuData
{
  public const int FLOATS_PER_SYMBOL_INSTANCE = 8;
  private SymbolInstanceGpuData.SymbolInstanceToByteConverter symbolInstancesConverter;
  private int symbolCount;

  private SymbolInstanceGpuData.SymbolInstance[] symbolInstances => this.symbolInstancesConverter.symbolInstances;

  public int version { get; private set; }

  public SymbolInstanceGpuData(int symbol_count)
  {
    this.symbolCount = symbol_count;
    this.symbolInstancesConverter = new SymbolInstanceGpuData.SymbolInstanceToByteConverter()
    {
      bytes = new byte[8 * symbol_count * 4]
    };
    for (int index = 0; index < symbol_count; ++index)
    {
      this.symbolInstances[index].isVisible = 1f;
      this.symbolInstances[index].symbolIndex = -1f;
      this.symbolInstances[index].scale = 1f;
      this.symbolInstances[index].unused = 1f;
      this.symbolInstances[index].color = Color.white;
    }
    this.MarkDirty();
  }

  private void MarkDirty() => ++this.version;

  public void SetVisible(int symbol_idx, bool is_visible)
  {
    DebugUtil.Assert(symbol_idx < this.symbolCount);
    float num = 0.0f;
    if (is_visible)
      num = 1f;
    if ((double) this.symbolInstances[symbol_idx].isVisible == (double) num)
      return;
    this.symbolInstances[symbol_idx].isVisible = num;
    this.MarkDirty();
  }

  public bool IsVisible(int symbol_idx)
  {
    DebugUtil.Assert(symbol_idx < this.symbolCount);
    return (double) this.symbolInstances[symbol_idx].isVisible > 0.5;
  }

  public void SetSymbolScale(int symbol_index, float scale)
  {
    DebugUtil.Assert(symbol_index < this.symbolCount);
    if ((double) this.symbolInstances[symbol_index].scale == (double) scale)
      return;
    this.symbolInstances[symbol_index].scale = scale;
    this.MarkDirty();
  }

  public void SetSymbolTint(int symbol_index, Color color)
  {
    DebugUtil.Assert(symbol_index < this.symbolCount);
    if (!(this.symbolInstances[symbol_index].color != color))
      return;
    this.symbolInstances[symbol_index].color = color;
    this.MarkDirty();
  }

  public void WriteToTexture(byte[] data, int data_idx, int instance_idx) => Buffer.BlockCopy((Array) this.symbolInstancesConverter.bytes, 0, (Array) data, data_idx, this.symbolCount * 8 * 4);

  [StructLayout(LayoutKind.Explicit)]
  public struct SymbolInstance
  {
    [FieldOffset(0)]
    public float symbolIndex;
    [FieldOffset(4)]
    public float isVisible;
    [FieldOffset(8)]
    public float scale;
    [FieldOffset(12)]
    public float unused;
    [FieldOffset(16)]
    public Color color;
  }

  [StructLayout(LayoutKind.Explicit)]
  public struct SymbolInstanceToByteConverter
  {
    [FieldOffset(0)]
    public byte[] bytes;
    [FieldOffset(0)]
    public SymbolInstanceGpuData.SymbolInstance[] symbolInstances;
  }
}
