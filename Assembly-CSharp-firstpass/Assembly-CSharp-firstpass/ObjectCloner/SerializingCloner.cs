// Decompiled with JetBrains decompiler
// Type: ObjectCloner.SerializingCloner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ObjectCloner
{
  public static class SerializingCloner
  {
    public static T Copy<T>(T obj)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) memoryStream, (object) obj);
        memoryStream.Position = 0L;
        return (T) binaryFormatter.Deserialize((Stream) memoryStream);
      }
    }
  }
}
