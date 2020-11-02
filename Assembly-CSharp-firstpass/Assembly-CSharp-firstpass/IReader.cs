// Decompiled with JetBrains decompiler
// Type: IReader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public interface IReader
{
  byte ReadByte();

  sbyte ReadSByte();

  short ReadInt16();

  ushort ReadUInt16();

  int ReadInt32();

  uint ReadUInt32();

  long ReadInt64();

  ulong ReadUInt64();

  float ReadSingle();

  double ReadDouble();

  char[] ReadChars(int length);

  byte[] ReadBytes(int length);

  void SkipBytes(int length);

  string ReadKleiString();

  byte[] RawBytes();

  int Position { get; }

  bool IsFinished { get; }
}
