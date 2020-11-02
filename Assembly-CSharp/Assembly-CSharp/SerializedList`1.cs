// Decompiled with JetBrains decompiler
// Type: SerializedList`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;

[SerializationConfig(MemberSerialization.OptIn)]
public class SerializedList<ItemType>
{
  [Serialize]
  private byte[] serializationBuffer;
  private List<ItemType> items = new List<ItemType>();

  public int Count => this.items.Count;

  public IEnumerator<ItemType> GetEnumerator() => (IEnumerator<ItemType>) this.items.GetEnumerator();

  public ItemType this[int idx] => this.items[idx];

  public void Add(ItemType item) => this.items.Add(item);

  public void Remove(ItemType item) => this.items.Remove(item);

  public void RemoveAt(int idx) => this.items.RemoveAt(idx);

  public bool Contains(ItemType item) => this.items.Contains(item);

  public void Clear() => this.items.Clear();

  [System.Runtime.Serialization.OnSerializing]
  private void OnSerializing()
  {
    MemoryStream memoryStream = new MemoryStream();
    BinaryWriter writer = new BinaryWriter((Stream) memoryStream);
    writer.Write(this.items.Count);
    foreach (ItemType itemType in this.items)
    {
      writer.WriteKleiString(itemType.GetType().FullName);
      long position1 = writer.BaseStream.Position;
      writer.Write(0);
      long position2 = writer.BaseStream.Position;
      Serializer.SerializeTypeless((object) itemType, writer);
      long position3 = writer.BaseStream.Position;
      long num = position3 - position2;
      writer.BaseStream.Position = position1;
      writer.Write((int) num);
      writer.BaseStream.Position = position3;
    }
    memoryStream.Flush();
    this.serializationBuffer = memoryStream.ToArray();
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (this.serializationBuffer == null)
      return;
    FastReader fastReader = new FastReader(this.serializationBuffer);
    int num = fastReader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      string typeName = fastReader.ReadKleiString();
      int length = fastReader.ReadInt32();
      int position = fastReader.Position;
      System.Type type = System.Type.GetType(typeName);
      if (type == (System.Type) null)
      {
        DebugUtil.LogWarningArgs((object) ("Type no longer exists: " + typeName));
        fastReader.SkipBytes(length);
      }
      else
      {
        ItemType itemType = !(typeof (ItemType) != type) ? default (ItemType) : (ItemType) Activator.CreateInstance(type);
        Deserializer.DeserializeTypeless((object) itemType, (IReader) fastReader);
        if (fastReader.Position != position + length)
        {
          DebugUtil.LogWarningArgs((object) "Expected to be at offset", (object) (position + length), (object) "but was only at offset", (object) fastReader.Position, (object) ". Skipping to catch up.");
          fastReader.SkipBytes(position + length - fastReader.Position);
        }
        this.items.Add(itemType);
      }
    }
  }
}
