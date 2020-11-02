// Decompiled with JetBrains decompiler
// Type: StateMachineSerializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;

public class StateMachineSerializer
{
  private static int serializerVersion = 12;
  private List<StateMachineSerializer.Entry> entries = new List<StateMachineSerializer.Entry>();

  public void Serialize(List<StateMachine.Instance> state_machines, BinaryWriter writer)
  {
    writer.Write(StateMachineSerializer.serializerVersion);
    long position1 = writer.BaseStream.Position;
    writer.Write(0);
    long position2 = writer.BaseStream.Position;
    try
    {
      int position3 = (int) writer.BaseStream.Position;
      int num = 0;
      writer.Write(num);
      foreach (StateMachine.Instance stateMachine in state_machines)
      {
        if (StateMachineSerializer.Entry.TrySerialize(stateMachine, writer))
          ++num;
      }
      int position4 = (int) writer.BaseStream.Position;
      writer.BaseStream.Position = (long) position3;
      writer.Write(num);
      writer.BaseStream.Position = (long) position4;
    }
    catch (Exception ex)
    {
      Debug.Log((object) "StateMachines: ");
      foreach (object stateMachine in state_machines)
        Debug.Log((object) stateMachine.ToString());
      Debug.LogError((object) ex);
    }
    long position5 = writer.BaseStream.Position;
    long num1 = position5 - position2;
    writer.BaseStream.Position = position1;
    writer.Write((int) num1);
    writer.BaseStream.Position = position5;
  }

  public void Deserialize(IReader reader)
  {
    int num = reader.ReadInt32();
    int length = reader.ReadInt32();
    if (num < 10)
    {
      Debug.LogWarning((object) ("State machine serializer version mismatch: " + (object) num + "!=" + (object) StateMachineSerializer.serializerVersion + "\nDiscarding data."));
      reader.SkipBytes(length);
    }
    else if (num < 12)
    {
      this.entries = StateMachineSerializer.OldEntryV11.DeserializeOldEntries(reader);
    }
    else
    {
      int capacity = reader.ReadInt32();
      this.entries = new List<StateMachineSerializer.Entry>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        StateMachineSerializer.Entry entry = StateMachineSerializer.Entry.Deserialize(reader);
        if (entry != null)
          this.entries.Add(entry);
      }
    }
  }

  private static string TrimAssemblyInfo(string type_name)
  {
    int length = type_name.IndexOf("[[");
    return length != -1 ? type_name.Substring(0, length) : type_name;
  }

  public bool Restore(StateMachine.Instance instance)
  {
    System.Type type = instance.GetType();
    for (int index = 0; index < this.entries.Count; ++index)
    {
      StateMachineSerializer.Entry entry = this.entries[index];
      if (entry.type == type)
      {
        this.entries.RemoveAt(index);
        return entry.Restore(instance);
      }
    }
    return false;
  }

  private class Entry
  {
    public int version;
    public System.Type type;
    public string currentState;
    public FastReader entryData;

    public static bool TrySerialize(StateMachine.Instance smi, BinaryWriter writer)
    {
      if (!smi.IsRunning())
        return false;
      int position1 = (int) writer.BaseStream.Position;
      writer.Write(smi.GetStateMachine().version);
      writer.WriteKleiString(smi.GetType().FullName);
      writer.WriteKleiString(smi.GetCurrentState().name);
      int position2 = (int) writer.BaseStream.Position;
      writer.Write(0);
      int position3 = (int) writer.BaseStream.Position;
      Serializer.SerializeTypeless((object) smi, writer);
      if (smi.GetStateMachine().serializable)
      {
        StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
        writer.Write(parameterContexts.Length);
        foreach (StateMachine.Parameter.Context context in parameterContexts)
        {
          long position4 = (long) (int) writer.BaseStream.Position;
          writer.Write(0);
          long position5 = (long) (int) writer.BaseStream.Position;
          writer.WriteKleiString(context.GetType().FullName);
          writer.WriteKleiString(context.parameter.name);
          context.Serialize(writer);
          long position6 = (long) (int) writer.BaseStream.Position;
          writer.BaseStream.Position = position4;
          long num = position6 - position5;
          writer.Write((int) num);
          writer.BaseStream.Position = position6;
        }
      }
      int num1 = (int) writer.BaseStream.Position - position3;
      if (num1 > 0)
      {
        int position4 = (int) writer.BaseStream.Position;
        writer.BaseStream.Position = (long) position2;
        writer.Write(num1);
        writer.BaseStream.Position = (long) position4;
        return true;
      }
      writer.BaseStream.Position = (long) position1;
      writer.BaseStream.SetLength((long) position1);
      return false;
    }

    public static StateMachineSerializer.Entry Deserialize(IReader reader)
    {
      StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
      entry.version = reader.ReadInt32();
      string typeName = reader.ReadKleiString();
      entry.type = System.Type.GetType(typeName);
      entry.currentState = reader.ReadKleiString();
      int length = reader.ReadInt32();
      entry.entryData = new FastReader(reader.ReadBytes(length));
      return entry.type == (System.Type) null ? (StateMachineSerializer.Entry) null : entry;
    }

    public bool Restore(StateMachine.Instance smi)
    {
      if (this.version != smi.GetStateMachine().version)
        return false;
      if (Manager.HasDeserializationMapping(smi.GetType()))
        Deserializer.DeserializeTypeless((object) smi, (IReader) this.entryData);
      if (!smi.GetStateMachine().serializable)
        return false;
      StateMachine.BaseState state = smi.GetStateMachine().GetState(this.currentState);
      if (state == null)
        return false;
      StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
      int num1 = this.entryData.ReadInt32();
      for (int index = 0; index < num1; ++index)
      {
        int num2 = this.entryData.ReadInt32();
        int position = this.entryData.Position;
        string str1 = this.entryData.ReadKleiString().Replace("Version=2.0.0.0", "Version=4.0.0.0");
        string str2 = this.entryData.ReadKleiString();
        foreach (StateMachine.Parameter.Context context in parameterContexts)
        {
          if (context.parameter.name == str2 && context.GetType().FullName == str1)
          {
            context.Deserialize((IReader) this.entryData);
            break;
          }
        }
        this.entryData.SkipBytes(num2 - (this.entryData.Position - position));
      }
      smi.GoTo(state);
      return true;
    }
  }

  private class OldEntryV11
  {
    public int version;
    public int dataPos;
    public System.Type type;
    public string currentState;

    public OldEntryV11(int version, int data_pos, System.Type type, string current_state)
    {
      this.version = version;
      this.dataPos = data_pos;
      this.type = type;
      this.currentState = current_state;
    }

    public static List<StateMachineSerializer.Entry> DeserializeOldEntries(
      IReader reader)
    {
      List<StateMachineSerializer.OldEntryV11> oldEntryV11List = StateMachineSerializer.OldEntryV11.ReadEntries(reader);
      byte[] bytes = StateMachineSerializer.OldEntryV11.ReadEntryData(reader);
      List<StateMachineSerializer.Entry> entryList = new List<StateMachineSerializer.Entry>(oldEntryV11List.Count);
      foreach (StateMachineSerializer.OldEntryV11 oldEntryV11 in oldEntryV11List)
      {
        StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
        entry.version = oldEntryV11.version;
        entry.type = oldEntryV11.type;
        entry.currentState = oldEntryV11.currentState;
        entry.entryData = new FastReader(bytes);
        entry.entryData.SkipBytes(oldEntryV11.dataPos);
        entryList.Add(entry);
      }
      return entryList;
    }

    private static StateMachineSerializer.OldEntryV11 Deserialize(
      IReader reader)
    {
      int version = reader.ReadInt32();
      int data_pos = reader.ReadInt32();
      string typeName = reader.ReadKleiString();
      string current_state = reader.ReadKleiString();
      System.Type type = System.Type.GetType(typeName);
      return type == (System.Type) null ? (StateMachineSerializer.OldEntryV11) null : new StateMachineSerializer.OldEntryV11(version, data_pos, type, current_state);
    }

    private static List<StateMachineSerializer.OldEntryV11> ReadEntries(
      IReader reader)
    {
      List<StateMachineSerializer.OldEntryV11> oldEntryV11List = new List<StateMachineSerializer.OldEntryV11>();
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
      {
        StateMachineSerializer.OldEntryV11 oldEntryV11 = StateMachineSerializer.OldEntryV11.Deserialize(reader);
        if (oldEntryV11 != null)
          oldEntryV11List.Add(oldEntryV11);
      }
      return oldEntryV11List;
    }

    private static byte[] ReadEntryData(IReader reader)
    {
      int length = reader.ReadInt32();
      return reader.ReadBytes(length);
    }
  }
}
