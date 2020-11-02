// Decompiled with JetBrains decompiler
// Type: FastReader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Text;

public class FastReader : IReader
{
  private int idx;
  private byte[] bytes;

  public FastReader(byte[] bytes) => this.bytes = bytes;

  public unsafe byte ReadByte()
  {
    int num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = (int) *numPtr;
    ++this.idx;
    return (byte) num;
  }

  public unsafe sbyte ReadSByte()
  {
    int num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = (int) (sbyte) *numPtr;
    ++this.idx;
    return (sbyte) num;
  }

  public unsafe ushort ReadUInt16()
  {
    int num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = (int) *(ushort*) numPtr;
    this.idx += 2;
    return (ushort) num;
  }

  public unsafe short ReadInt16()
  {
    int num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = (int) *(short*) numPtr;
    this.idx += 2;
    return (short) num;
  }

  public unsafe uint ReadUInt32()
  {
    int num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = (int) *(uint*) numPtr;
    this.idx += 4;
    return (uint) num;
  }

  public unsafe int ReadInt32()
  {
    int num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = *(int*) numPtr;
    this.idx += 4;
    return num;
  }

  public unsafe ulong ReadUInt64()
  {
    long num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = *(long*) numPtr;
    this.idx += 8;
    return (ulong) num;
  }

  public unsafe long ReadInt64()
  {
    long num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = *(long*) numPtr;
    this.idx += 8;
    return num;
  }

  public unsafe float ReadSingle()
  {
    double num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = (double) *(float*) numPtr;
    this.idx += 4;
    return (float) num;
  }

  public unsafe double ReadDouble()
  {
    double num;
    fixed (byte* numPtr = &this.bytes[this.idx])
      num = *(double*) numPtr;
    this.idx += 8;
    return num;
  }

  public char[] ReadChars(int length)
  {
    char[] chArray = new char[length];
    for (int index = 0; index < length; ++index)
      chArray[index] = (char) this.bytes[this.idx + index];
    this.idx += length;
    return chArray;
  }

  public byte[] ReadBytes(int length)
  {
    byte[] numArray = new byte[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = this.bytes[this.idx + index];
    this.idx += length;
    return numArray;
  }

  public string ReadKleiString()
  {
    int count = this.ReadInt32();
    string str = (string) null;
    if (count >= 0)
    {
      str = Encoding.UTF8.GetString(this.bytes, this.idx, count);
      this.idx += count;
    }
    return str;
  }

  public void SkipBytes(int length) => this.idx += length;

  public bool IsFinished => this.bytes == null || this.idx == this.bytes.Length;

  public byte[] RawBytes() => this.bytes;

  public int Position
  {
    get => this.idx;
    set => this.idx = value;
  }
}
