// Decompiled with JetBrains decompiler
// Type: FMOD.StringHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FMOD
{
  internal static class StringHelper
  {
    private static List<StringHelper.ThreadSafeEncoding> encoders = new List<StringHelper.ThreadSafeEncoding>(1);

    public static StringHelper.ThreadSafeEncoding GetFreeHelper()
    {
      lock (StringHelper.encoders)
      {
        StringHelper.ThreadSafeEncoding threadSafeEncoding = (StringHelper.ThreadSafeEncoding) null;
        for (int index = 0; index < StringHelper.encoders.Count; ++index)
        {
          if (!StringHelper.encoders[index].InUse())
          {
            threadSafeEncoding = StringHelper.encoders[index];
            break;
          }
        }
        if (threadSafeEncoding == null)
        {
          threadSafeEncoding = new StringHelper.ThreadSafeEncoding();
          StringHelper.encoders.Add(threadSafeEncoding);
        }
        threadSafeEncoding.SetInUse();
        return threadSafeEncoding;
      }
    }

    public class ThreadSafeEncoding : IDisposable
    {
      private UTF8Encoding encoding = new UTF8Encoding();
      private byte[] encodedBuffer = new byte[128];
      private char[] decodedBuffer = new char[128];
      private bool inUse;

      public bool InUse() => this.inUse;

      public void SetInUse() => this.inUse = true;

      private int roundUpPowerTwo(int number)
      {
        int num = 1;
        while (num <= number)
          num *= 2;
        return num;
      }

      public byte[] byteFromStringUTF8(string s)
      {
        if (s == null)
          return (byte[]) null;
        if (this.encoding.GetMaxByteCount(s.Length) + 1 > this.encodedBuffer.Length)
        {
          int number = this.encoding.GetByteCount(s) + 1;
          if (number > this.encodedBuffer.Length)
            this.encodedBuffer = new byte[this.roundUpPowerTwo(number)];
        }
        this.encodedBuffer[this.encoding.GetBytes(s, 0, s.Length, this.encodedBuffer, 0)] = (byte) 0;
        return this.encodedBuffer;
      }

      public string stringFromNative(IntPtr nativePtr)
      {
        if (nativePtr == IntPtr.Zero)
          return "";
        int num = 0;
        while (Marshal.ReadByte(nativePtr, num) != (byte) 0)
          ++num;
        if (num == 0)
          return "";
        if (num > this.encodedBuffer.Length)
          this.encodedBuffer = new byte[this.roundUpPowerTwo(num)];
        Marshal.Copy(nativePtr, this.encodedBuffer, 0, num);
        if (this.encoding.GetMaxCharCount(num) > this.decodedBuffer.Length)
        {
          int charCount = this.encoding.GetCharCount(this.encodedBuffer, 0, num);
          if (charCount > this.decodedBuffer.Length)
            this.decodedBuffer = new char[this.roundUpPowerTwo(charCount)];
        }
        return new string(this.decodedBuffer, 0, this.encoding.GetChars(this.encodedBuffer, 0, num, this.decodedBuffer, 0));
      }

      public void Dispose()
      {
        lock (StringHelper.encoders)
          this.inUse = false;
      }
    }
  }
}
