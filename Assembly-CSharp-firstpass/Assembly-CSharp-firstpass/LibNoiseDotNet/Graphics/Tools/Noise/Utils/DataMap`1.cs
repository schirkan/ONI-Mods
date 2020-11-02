// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Utils.DataMap`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Utils
{
  public abstract class DataMap<T>
  {
    protected T _borderValue;
    protected int _width;
    protected int _height;
    private int _stride;
    protected int _memoryUsage;
    protected int _cellsCount;
    protected T[] _data;
    protected bool _hasMaxDimension;
    protected int _maxWidth;
    protected int _maxHeight;

    public int Width => this._width;

    public int Height => this._height;

    public int Stride
    {
      get => this._stride;
      set => this._stride = value;
    }

    public T BorderValue
    {
      get => this._borderValue;
      set => this._borderValue = value;
    }

    public int MemoryUsage => this._memoryUsage;

    public float MemoryUsageKb => (float) this.MemoryUsage / 8192f;

    public float MemoryUsageMo => (float) this.MemoryUsage / 8388608f;

    public DataMap() => this.AllocateBuffer();

    public DataMap(int width, int height) => this.AllocateBuffer(width, height);

    public DataMap(DataMap<T> copy) => this.CopyFrom(copy);

    public T[] GetSlab(int y)
    {
      T[] objArray = new T[this._stride];
      if (this._data != null && y >= 0 && y < this._height)
      {
        Array.Copy((Array) this._data, y * this._stride, (Array) objArray, 0, this._stride);
      }
      else
      {
        for (int index = 0; index < objArray.Length; ++index)
          objArray[index] = this._borderValue;
      }
      return objArray;
    }

    public T GetValue(int x, int y) => this._data != null && x >= 0 && (x < this._width && y >= 0) && y < this._height ? this._data[y * this._stride + x] : this._borderValue;

    public void SetValue(int x, int y, T value)
    {
      if (this._data == null || x < 0 || (x >= this._width || y < 0) || y >= this._height)
        return;
      this._data[y * this._stride + x] = value;
    }

    public void SetSize(int width, int height)
    {
      if (width < 0 || height < 0)
        throw new ArgumentException("Map dimension must be greater or equal 0");
      if (this._hasMaxDimension && (width > this._maxWidth || height > this._maxHeight))
        throw new ArgumentException(string.Format("Map dimension must be lower than {0} * {1}", (object) this._maxWidth, (object) this._maxHeight));
      this.AllocateBuffer(width, height);
    }

    public void CopyFrom(DataMap<T> source)
    {
      this.AllocateBuffer(source._width, source._height);
      if (this._cellsCount > 0)
        Array.Copy((Array) source._data, 0, (Array) this._data, 0, this._cellsCount);
      this._borderValue = source._borderValue;
    }

    public void CopyTo(DataMap<T> dest)
    {
      if (dest == null)
        throw new ArgumentNullException("Dest is null");
      dest.CopyFrom(this);
    }

    public void CopyTo(ref T[] buffer)
    {
      if (this._data == null)
        return;
      if (buffer == null)
        buffer = new T[this._cellsCount];
      int length = this._data.Length > buffer.Length ? buffer.Length : this._data.Length;
      Array.Copy((Array) this._data, 0, (Array) buffer, 0, length);
    }

    public T[] Share() => this._data != null ? this._data : throw new NullReferenceException("The internal buffer is null");

    public void Reset() => this.AllocateBuffer(0, 0);

    public void DeleteAndReset()
    {
      this._data = (T[]) null;
      this.AllocateBuffer(0, 0);
    }

    public void ReclaimMemory()
    {
      if (this._data == null || this._data.Length <= this._cellsCount)
        return;
      Array.Resize<T>(ref this._data, this._cellsCount);
    }

    public void Clear(T value)
    {
      if (this._data == null)
        return;
      for (int index = 0; index <= this._cellsCount; ++index)
        this._data[index] = value;
    }

    public void Clear()
    {
      if (this._data == null)
        return;
      Array.Clear((Array) this._data, 0, this._cellsCount);
    }

    protected abstract int SizeofT();

    protected abstract T MinvalofT();

    protected abstract T MaxvalofT();

    protected void AllocateBuffer()
    {
      this._cellsCount = this._width * this._height;
      this._stride = this._width;
      this._memoryUsage = this._cellsCount * this.SizeofT();
      if (this._cellsCount == 0)
        return;
      if (this._data == null)
      {
        this._data = new T[this._cellsCount];
      }
      else
      {
        if (this._data.Length >= this._cellsCount)
          return;
        Array.Resize<T>(ref this._data, this._cellsCount);
      }
    }

    protected void AllocateBuffer(int width, int height)
    {
      this._width = width;
      this._height = height;
      this.AllocateBuffer();
    }
  }
}
