// Decompiled with JetBrains decompiler
// Type: Chunk
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.Runtime.Serialization;

[SerializationConfig(MemberSerialization.OptOut)]
public class Chunk
{
  public Chunk.State state;
  public Vector2I offset;
  public Vector2I size;
  public float[] data;
  public float[] overrides;
  public float[] density;
  public float[] heatOffset;
  public float[] defaultTemp;

  public Chunk()
  {
    this.state = Chunk.State.Unprocessed;
    this.data = (float[]) null;
    this.overrides = (float[]) null;
    this.density = (float[]) null;
    this.heatOffset = (float[]) null;
    this.defaultTemp = (float[]) null;
  }

  public Chunk(int x, int y, int width, int height)
  {
    this.offset = new Vector2I(x, y);
    this.size = new Vector2I(width, height);
  }

  [OnDeserializing]
  internal void OnDeserializingMethod()
  {
    int x = this.size.x;
    int y = this.size.y;
    this.data = new float[x * y];
    this.overrides = new float[x * y];
    this.density = new float[x * y];
    this.heatOffset = new float[x * y];
    this.defaultTemp = new float[x * y];
    this.state = Chunk.State.Loaded;
  }

  public enum State
  {
    Unprocessed,
    GeneratedNoise,
    Processed,
    Loaded,
  }
}
