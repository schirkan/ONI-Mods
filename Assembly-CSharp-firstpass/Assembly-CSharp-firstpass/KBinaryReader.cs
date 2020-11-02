// Decompiled with JetBrains decompiler
// Type: KBinaryReader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using System.Text;

public class KBinaryReader : BinaryReader, IReader
{
  public KBinaryReader(Stream stream)
    : base(stream)
  {
  }

  public void SkipBytes(int length) => this.ReadBytes(length);

  public bool IsFinished => this.BaseStream.Position == this.BaseStream.Length;

  public int Position => (int) this.BaseStream.Position;

  public string ReadKleiString()
  {
    string str = (string) null;
    int count = this.ReadInt32();
    if (count >= 0)
      str = Encoding.UTF8.GetString(this.ReadBytes(count), 0, count);
    return str;
  }

  public byte[] RawBytes()
  {
    long position = this.BaseStream.Position;
    this.BaseStream.Position = 0L;
    byte[] numArray = this.ReadBytes((int) this.BaseStream.Length);
    this.BaseStream.Position = position;
    return numArray;
  }
}
