// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Writer.Heightmap8RawWriter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Renderer;
using System;
using System.IO;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Writer
{
  public class Heightmap8RawWriter : AbstractWriter
  {
    protected Heightmap8 _heightmap;

    public Heightmap8 Heightmap
    {
      get => this._heightmap;
      set => this._heightmap = value;
    }

    public override void WriteFile()
    {
      if (this._heightmap == null)
        throw new ArgumentException("An heightmap must be provided");
      this.OpenFile();
      try
      {
        this._writer.Write(this._heightmap.Share());
      }
      catch (Exception ex)
      {
        throw new IOException("Unknown IO exception", ex);
      }
      this.CloseFile();
    }
  }
}
