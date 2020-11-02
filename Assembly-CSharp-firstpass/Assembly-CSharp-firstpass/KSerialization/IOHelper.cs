// Decompiled with JetBrains decompiler
// Type: KSerialization.IOHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace KSerialization
{
  public static class IOHelper
  {
    private static byte[] s_stringBuffer = new byte[1024];
    private static byte[] s_singleBuffer = new byte[4];

    public static void WriteKleiString(this BinaryWriter writer, string str)
    {
      if (str != null)
      {
        Encoding utF8 = Encoding.UTF8;
        int byteCount = utF8.GetByteCount(str);
        writer.Write(byteCount);
        if (byteCount < IOHelper.s_stringBuffer.Length)
        {
          utF8.GetBytes(str, 0, str.Length, IOHelper.s_stringBuffer, 0);
          writer.Write(IOHelper.s_stringBuffer, 0, byteCount);
        }
        else
        {
          Debug.LogWarning((object) string.Format("Writing large string {0} of {1} bytes", (object) str, (object) byteCount));
          writer.Write(utF8.GetBytes(str));
        }
      }
      else
        writer.Write(-1);
    }

    public static unsafe void WriteSingleFast(this BinaryWriter writer, float value)
    {
      byte* numPtr = (byte*) &value;
      if (BitConverter.IsLittleEndian)
      {
        IOHelper.s_singleBuffer[0] = *numPtr;
        IOHelper.s_singleBuffer[1] = numPtr[1];
        IOHelper.s_singleBuffer[2] = numPtr[2];
        IOHelper.s_singleBuffer[3] = numPtr[3];
      }
      else
      {
        IOHelper.s_singleBuffer[0] = numPtr[3];
        IOHelper.s_singleBuffer[1] = numPtr[2];
        IOHelper.s_singleBuffer[2] = numPtr[1];
        IOHelper.s_singleBuffer[3] = *numPtr;
      }
      writer.Write(IOHelper.s_singleBuffer);
    }

    [Conditional("DEBUG_VALIDATE")]
    public static void WriteBoundaryTag(this BinaryWriter writer, object tag) => writer.Write((uint) tag);

    [Conditional("DEBUG_VALIDATE")]
    public static void CheckBoundaryTag(this IReader reader, object expected)
    {
      uint num = reader.ReadUInt32();
      if ((int) (uint) expected == (int) num)
        return;
      Debug.LogError((object) string.Format("Expected Tag {0}(0x{1:X}) but got 0x{2:X} instead", (object) expected.ToString(), (object) (uint) expected, (object) num));
    }

    [Conditional("DEBUG_VALIDATE")]
    public static void Assert(bool condition) => DebugUtil.Assert(condition);

    public static Vector2I ReadVector2I(this IReader reader)
    {
      Vector2I vector2I;
      vector2I.x = reader.ReadInt32();
      vector2I.y = reader.ReadInt32();
      return vector2I;
    }

    public static Vector2 ReadVector2(this IReader reader)
    {
      Vector2 vector2;
      vector2.x = reader.ReadSingle();
      vector2.y = reader.ReadSingle();
      return vector2;
    }

    public static Vector3 ReadVector3(this IReader reader)
    {
      Vector3 vector3;
      vector3.x = reader.ReadSingle();
      vector3.y = reader.ReadSingle();
      vector3.z = reader.ReadSingle();
      return vector3;
    }

    public static Color ReadColour(this IReader reader)
    {
      byte num1 = reader.ReadByte();
      byte num2 = reader.ReadByte();
      byte num3 = reader.ReadByte();
      byte num4 = reader.ReadByte();
      Color color;
      color.r = (float) num1 / (float) byte.MaxValue;
      color.g = (float) num2 / (float) byte.MaxValue;
      color.b = (float) num3 / (float) byte.MaxValue;
      color.a = (float) num4 / (float) byte.MaxValue;
      return color;
    }
  }
}
