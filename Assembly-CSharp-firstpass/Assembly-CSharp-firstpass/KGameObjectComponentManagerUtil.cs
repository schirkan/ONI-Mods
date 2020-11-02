// Decompiled with JetBrains decompiler
// Type: KGameObjectComponentManagerUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.IO;
using UnityEngine;

public static class KGameObjectComponentManagerUtil
{
  public static void Serialize<MgrType, DataType>(MgrType mgr, GameObject go, BinaryWriter writer)
    where MgrType : KGameObjectComponentManager<DataType>
    where DataType : new()
  {
    long position1 = writer.BaseStream.Position;
    writer.Write(0);
    long position2 = writer.BaseStream.Position;
    HandleVector<int>.Handle handle = mgr.GetHandle(go);
    Serializer.SerializeTypeless((object) mgr.GetData(handle), writer);
    long position3 = writer.BaseStream.Position;
    long num = position3 - position2;
    writer.BaseStream.Position = position1;
    writer.Write((int) num);
    writer.BaseStream.Position = position3;
  }

  public static void Deserialize<MgrType, DataType>(MgrType mgr, GameObject go, IReader reader)
    where MgrType : KGameObjectComponentManager<DataType>
    where DataType : new()
  {
    HandleVector<int>.Handle handle = mgr.GetHandle(go);
    object result;
    Deserializer.Deserialize(typeof (DataType), reader, out result);
    mgr.SetData(handle, (DataType) result);
  }

  public static void Serialize<MgrType, Header, Payload>(
    MgrType mgr,
    GameObject go,
    BinaryWriter writer)
    where MgrType : KGameObjectSplitComponentManager<Header, Payload>
    where Header : new()
    where Payload : new()
  {
    long position1 = writer.BaseStream.Position;
    writer.Write(0);
    long position2 = writer.BaseStream.Position;
    HandleVector<int>.Handle handle = mgr.GetHandle(go);
    Header header;
    Payload payload;
    mgr.GetData(handle, out header, out payload);
    Serializer.SerializeTypeless((object) header, writer);
    Serializer.SerializeTypeless((object) payload, writer);
    long position3 = writer.BaseStream.Position;
    long num = position3 - position2;
    writer.BaseStream.Position = position1;
    writer.Write((int) num);
    writer.BaseStream.Position = position3;
  }

  public static void Deserialize<MgrType, Header, Payload>(
    MgrType mgr,
    GameObject go,
    IReader reader)
    where MgrType : KGameObjectSplitComponentManager<Header, Payload>
    where Header : new()
    where Payload : new()
  {
    HandleVector<int>.Handle handle = mgr.GetHandle(go);
    object result1;
    Deserializer.Deserialize(typeof (Header), reader, out result1);
    object result2;
    Deserializer.Deserialize(typeof (Payload), reader, out result2);
    Payload new_data1 = (Payload) result2;
    mgr.SetData(handle, (Header) result1, ref new_data1);
  }
}
