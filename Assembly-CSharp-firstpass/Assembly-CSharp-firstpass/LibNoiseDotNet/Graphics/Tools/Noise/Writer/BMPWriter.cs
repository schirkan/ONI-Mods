// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Writer.BMPWriter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Renderer;
using System;
using System.IO;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Writer
{
  public class BMPWriter : AbstractWriter
  {
    public const int BMP_HEADER_SIZE = 54;
    protected Image _image;

    public Image Image
    {
      get => this._image;
      set => this._image = value;
    }

    public override void WriteFile()
    {
      int width = this._image != null ? this._image.Width : throw new ArgumentException("An image map must be provided");
      int height = this._image.Height;
      int length = this.CalcWidthByteCount(width);
      int num1 = length * height;
      byte[] buffer1 = new byte[length];
      this.OpenFile();
      byte[] buffer2 = new byte[4];
      byte[] buffer3 = new byte[2]{ (byte) 66, (byte) 77 };
      try
      {
        this._writer.Write(buffer3);
        this._writer.Write(Libnoise.UnpackLittleUint32(num1 + 54, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(0, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(54, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(40, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(width, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(height, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint16((short) 1, ref buffer3));
        this._writer.Write(Libnoise.UnpackLittleUint16((short) 24, ref buffer3));
        this._writer.Write(Libnoise.UnpackLittleUint32(0, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(num1, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(2834, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(2834, ref buffer2));
        this._writer.Write(Libnoise.UnpackLittleUint32(0, ref buffer2));
        this._writer.Write(buffer2);
        for (int y = 0; y < height; ++y)
        {
          int num2 = 0;
          Array.Clear((Array) buffer1, 0, buffer1.Length);
          for (int x = 0; x < width; ++x)
          {
            Color color = this._image.GetValue(x, y);
            byte[] numArray1 = buffer1;
            int index1 = num2;
            int num3 = index1 + 1;
            int blue = (int) color.Blue;
            numArray1[index1] = (byte) blue;
            byte[] numArray2 = buffer1;
            int index2 = num3;
            int num4 = index2 + 1;
            int green = (int) color.Green;
            numArray2[index2] = (byte) green;
            byte[] numArray3 = buffer1;
            int index3 = num4;
            num2 = index3 + 1;
            int red = (int) color.Red;
            numArray3[index3] = (byte) red;
          }
          this._writer.Write(buffer1);
        }
      }
      catch (Exception ex)
      {
        throw new IOException("Unknown IO exception", ex);
      }
      this.CloseFile();
    }

    protected int CalcWidthByteCount(int width) => width * 3 + 3 & -4;
  }
}
