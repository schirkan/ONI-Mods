// Decompiled with JetBrains decompiler
// Type: TextureAtlas
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TextureAtlas : ScriptableObject
{
  public Texture2D texture;
  public float vertexScale = 1f;
  public TextureAtlas.Item[] items;

  public void Import(TextAsset data, Texture2D texture)
  {
    this.texture = texture;
    float width = (float) texture.width;
    float height = (float) texture.height;
    TextureAtlas.AtlasData atlasData = JsonConvert.DeserializeObject<TextureAtlas.AtlasData>(data.text);
    float num1 = (float) atlasData.meta.size["w"];
    float num2 = (float) atlasData.meta.size["h"];
    this.items = new TextureAtlas.Item[atlasData.frames.Count];
    for (int index1 = 0; index1 < atlasData.frames.Count; ++index1)
    {
      TextureAtlas.AtlasData.Frame frame = atlasData.frames[index1];
      this.items[index1].name = frame.filename;
      this.items[index1].uvBox.x = (float) frame.frame["x"] / num1;
      this.items[index1].uvBox.y = (float) (1.0 - (double) frame.frame["y"] / (double) num2);
      this.items[index1].uvBox.z = this.items[index1].uvBox.x + (float) frame.frame["w"] / num1;
      this.items[index1].uvBox.w = this.items[index1].uvBox.y - (float) frame.frame["h"] / num2;
      if (frame.vertices != null)
      {
        Vector3[] vector3Array = new Vector3[frame.vertices.Count];
        Vector2[] vector2Array = new Vector2[frame.verticesUV.Count];
        int[] numArray = new int[frame.triangles.Count * 3];
        for (int index2 = 0; index2 < frame.vertices.Count; ++index2)
          vector3Array[index2] = new Vector3((float) frame.vertices[index2][0], (float) (frame.frame["h"] - frame.vertices[index2][1]), 0.0f) / this.vertexScale;
        for (int index2 = 0; index2 < frame.verticesUV.Count; ++index2)
          vector2Array[index2] = new Vector2((float) frame.verticesUV[index2][0] / width, (float) (1.0 - (double) frame.verticesUV[index2][1] / (double) height));
        for (int index2 = 0; index2 < frame.triangles.Count; ++index2)
        {
          numArray[index2 * 3] = frame.triangles[index2][0];
          numArray[index2 * 3 + 1] = frame.triangles[index2][1];
          numArray[index2 * 3 + 2] = frame.triangles[index2][2];
        }
        this.items[index1].vertices = vector3Array;
        this.items[index1].uvs = vector2Array;
        this.items[index1].indices = numArray;
      }
    }
  }

  [Serializable]
  public struct Item
  {
    public string name;
    public Vector4 uvBox;
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] indices;
  }

  public class AtlasData
  {
    public List<TextureAtlas.AtlasData.Frame> frames;
    public TextureAtlas.AtlasData.Meta meta;

    public class Frame
    {
      public string filename;
      public Dictionary<string, int> frame;
      public List<int[]> vertices;
      public List<int[]> verticesUV;
      public List<int[]> triangles;
    }

    public struct Meta
    {
      public Dictionary<string, int> size;
    }
  }
}
