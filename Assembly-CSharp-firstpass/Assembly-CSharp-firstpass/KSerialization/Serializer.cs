// Decompiled with JetBrains decompiler
// Type: KSerialization.Serializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;

namespace KSerialization
{
  public class Serializer
  {
    private BinaryWriter writer;

    public Serializer(BinaryWriter writer) => this.writer = writer;

    public void Serialize(object obj) => Serializer.Serialize(obj, this.writer);

    public static void Serialize(object obj, BinaryWriter writer)
    {
      SerializationTemplate serializationTemplate = Manager.GetSerializationTemplate(obj.GetType());
      string ktypeString = obj.GetType().GetKTypeString();
      writer.WriteKleiString(ktypeString);
      object obj1 = obj;
      BinaryWriter writer1 = writer;
      serializationTemplate.SerializeData(obj1, writer1);
    }

    public static void SerializeTypeless(object obj, BinaryWriter writer) => Manager.GetSerializationTemplate(obj.GetType()).SerializeData(obj, writer);
  }
}
