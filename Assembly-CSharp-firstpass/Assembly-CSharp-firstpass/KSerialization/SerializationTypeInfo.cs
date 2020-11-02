// Decompiled with JetBrains decompiler
// Type: KSerialization.SerializationTypeInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace KSerialization
{
  public enum SerializationTypeInfo : byte
  {
    UserDefined = 0,
    SByte = 1,
    Byte = 2,
    Boolean = 3,
    Int16 = 4,
    UInt16 = 5,
    Int32 = 6,
    UInt32 = 7,
    Int64 = 8,
    UInt64 = 9,
    Single = 10, // 0x0A
    Double = 11, // 0x0B
    String = 12, // 0x0C
    Enumeration = 13, // 0x0D
    Vector2I = 14, // 0x0E
    Vector2 = 15, // 0x0F
    Vector3 = 16, // 0x10
    Array = 17, // 0x11
    Pair = 18, // 0x12
    Dictionary = 19, // 0x13
    List = 20, // 0x14
    HashSet = 21, // 0x15
    Queue = 22, // 0x16
    Colour = 23, // 0x17
    VALUE_MASK = 63, // 0x3F
    IS_VALUE_TYPE = 64, // 0x40
    IS_GENERIC_TYPE = 128, // 0x80
  }
}
