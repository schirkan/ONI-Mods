// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Writer.AbstractWriter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Writer
{
  public abstract class AbstractWriter
  {
    protected string _filename;
    protected BinaryWriter _writer;

    public string Filename
    {
      get => this._filename;
      set => this._filename = value;
    }

    public abstract void WriteFile();

    protected void OpenFile()
    {
      if (this._writer != null)
        return;
      if (File.Exists(this._filename))
      {
        try
        {
          File.Delete(this._filename);
        }
        catch (Exception ex)
        {
          throw new IOException("Unable to delete destination file", ex);
        }
      }
      BufferedStream bufferedStream;
      try
      {
        bufferedStream = new BufferedStream((Stream) new FileStream(this._filename, FileMode.Create));
      }
      catch (Exception ex)
      {
        throw new IOException("Unable to create destination file", ex);
      }
      this._writer = new BinaryWriter((Stream) bufferedStream);
    }

    protected void CloseFile()
    {
      try
      {
        this._writer.Flush();
        this._writer.Close();
        this._writer = (BinaryWriter) null;
      }
      catch (Exception ex)
      {
        throw new IOException("Unable to release stream", ex);
      }
    }
  }
}
