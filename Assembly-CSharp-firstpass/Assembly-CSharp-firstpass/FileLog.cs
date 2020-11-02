// Decompiled with JetBrains decompiler
// Type: FileLog
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;
using System.IO;

public class FileLog
{
  private static FileLog instance;
  private StreamWriter writer;

  [Conditional("ENABLE_LOG")]
  public static void Initialize(string filename) => FileLog.instance = new FileLog(filename);

  [Conditional("ENABLE_LOG")]
  public static void Shutdown()
  {
    if (FileLog.instance.writer != null)
      FileLog.instance.writer.Close();
    FileLog.instance = (FileLog) null;
  }

  private FileLog(string filename)
  {
    this.writer = new StreamWriter(filename);
    this.writer.AutoFlush = true;
  }

  [Conditional("ENABLE_LOG")]
  public static void Log(params object[] objs) => FileLog.instance.LogObjs(objs);

  private void LogObjs(object[] objs) => this.writer.WriteLine(FileLog.BuildString(objs));

  private static string BuildString(object[] objs)
  {
    string str = "";
    if (objs.Length != 0)
    {
      str = objs[0] != null ? objs[0].ToString() : "null";
      for (int index = 1; index < objs.Length; ++index)
      {
        object obj = objs[index];
        str = str + " " + (obj != null ? obj.ToString() : "null");
      }
    }
    return str;
  }
}
