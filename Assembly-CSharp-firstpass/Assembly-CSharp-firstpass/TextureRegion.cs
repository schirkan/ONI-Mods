// Decompiled with JetBrains decompiler
// Type: TextureRegion
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

public struct TextureRegion
{
  public int x;
  public int y;
  public int bytesPerPixel;
  public byte[] bytes;
  public int width;
  public TexturePage page;
  public TextureBuffer buffer;
  public TextureRegion.ByteToFloatConverter floatConverter;

  public TextureRegion(int x, int y, TexturePage page, TextureBuffer buffer)
  {
    this.x = x;
    this.y = y;
    this.page = page;
    this.buffer = buffer;
    this.width = page.width;
    this.bytesPerPixel = TextureUtil.GetBytesPerPixel(page.format);
    this.bytes = page.bytes;
    this.floatConverter = new TextureRegion.ByteToFloatConverter()
    {
      bytes = page.bytes
    };
  }

  private int GetByteIdx(int x, int y) => (x - this.x + (y - this.y) * this.width) * this.bytesPerPixel;

  public void SetBytes(int x, int y, byte b0) => this.bytes[this.GetByteIdx(x, y)] = b0;

  public void SetBytes(int x, int y, byte b0, byte b1)
  {
    int byteIdx = this.GetByteIdx(x, y);
    this.bytes[byteIdx] = b0;
    this.bytes[byteIdx + 1] = b1;
  }

  public void SetBytes(int x, int y, byte b0, byte b1, byte b2)
  {
    int byteIdx = this.GetByteIdx(x, y);
    this.bytes[byteIdx] = b0;
    this.bytes[byteIdx + 1] = b1;
    this.bytes[byteIdx + 2] = b2;
  }

  public void SetBytes(int x, int y, byte b0, byte b1, byte b2, byte b3)
  {
    int byteIdx = this.GetByteIdx(x, y);
    this.bytes[byteIdx] = b0;
    this.bytes[byteIdx + 1] = b1;
    this.bytes[byteIdx + 2] = b2;
    this.bytes[byteIdx + 3] = b3;
  }

  public void SetBytes(int x, int y, float v0, float v1)
  {
    int index = this.GetByteIdx(x, y) / 4;
    this.floatConverter.floats[index] = v0;
    this.floatConverter.floats[index + 1] = v1;
  }

  public void Unlock() => this.buffer.Unlock(this);

  [StructLayout(LayoutKind.Explicit)]
  public struct ByteToFloatConverter
  {
    [FieldOffset(0)]
    public byte[] bytes;
    [FieldOffset(0)]
    public float[] floats;
  }
}
