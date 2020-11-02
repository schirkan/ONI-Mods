// Decompiled with JetBrains decompiler
// Type: ProfilerBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

public class ProfilerBase
{
  private bool initialised;
  private int idx;
  protected StreamWriter proFile;
  private string category = "GAME";
  private string filePrefix;
  protected Dictionary<int, ProfilerBase.ThreadInfo> threadInfos;
  public Stopwatch sw;

  public static void StartLine(
    StringBuilder sb,
    string category,
    string region_name,
    int tid,
    Stopwatch sw,
    string ph)
  {
    sb.Append("{\"cat\":\"").Append(category).Append("\"");
    sb.Append(",\"name\":\"").Append(region_name).Append("\"");
    sb.Append(",\"pid\":0");
    sb.Append(",\"tid\":").Append(tid);
    long num = sw.ElapsedTicks * 1000000L / Stopwatch.Frequency;
    sb.Append(",\"ts\":").Append(num);
    sb.Append(",\"ph\":\"").Append(ph).Append("\"");
  }

  public static void WriteLine(
    StringBuilder sb,
    string category,
    string region_name,
    int tid,
    Stopwatch sw,
    string ph,
    string suffix)
  {
    ProfilerBase.StartLine(sb, category, region_name, tid, sw, ph);
    sb.Append(suffix).Append("\n");
  }

  protected bool IsRecording() => this.proFile != null;

  public ProfilerBase(string file_prefix)
  {
    this.filePrefix = file_prefix;
    this.threadInfos = new Dictionary<int, ProfilerBase.ThreadInfo>();
    this.sw = new Stopwatch();
  }

  public void Init() => this.proFile = (StreamWriter) null;

  public void Finalise()
  {
    if (!this.IsRecording())
      return;
    this.StopRecording();
  }

  public void ToggleRecording(string category = "GAME")
  {
    this.category = "G";
    if (!this.initialised)
    {
      this.initialised = true;
      this.Init();
    }
    if (this.IsRecording())
      this.StopRecording();
    else
      this.StartRecording();
  }

  public virtual void StartRecording()
  {
    foreach (KeyValuePair<int, ProfilerBase.ThreadInfo> threadInfo in this.threadInfos)
      threadInfo.Value.Reset();
    this.proFile = new StreamWriter(this.filePrefix + this.idx.ToString() + ".json");
    ++this.idx;
    if (this.proFile != null)
      this.proFile.WriteLine("{\"traceEvents\":[");
    this.sw.Start();
  }

  public virtual void StopRecording()
  {
    this.sw.Stop();
    if (this.proFile == null)
      return;
    foreach (KeyValuePair<int, ProfilerBase.ThreadInfo> threadInfo in this.threadInfos)
    {
      this.proFile.Write(threadInfo.Value.sb.ToString());
      threadInfo.Value.Reset();
    }
    ProfilerBase.ThreadInfo threadInfo1 = this.ManifestThreadInfo("Main");
    threadInfo1.WriteLine(this.category, "end", this.sw, "B", "},");
    threadInfo1.WriteLine(this.category, "end", this.sw, "E", "}]}");
    this.proFile.Write(threadInfo1.sb.ToString());
    threadInfo1.Reset();
    this.proFile.Close();
    this.proFile = (StreamWriter) null;
  }

  public virtual void BeginThreadProfiling(string threadGroupName, string threadName) => this.ManifestThreadInfo(threadName);

  public virtual void EndThreadProfiling()
  {
    if (this.proFile != null)
      this.proFile.Write(this.ManifestThreadInfo().sb.ToString());
    lock (this.threadInfos)
      this.threadInfos.Remove(Thread.CurrentThread.ManagedThreadId);
  }

  protected ProfilerBase.ThreadInfo ManifestThreadInfo(string name = null)
  {
    ProfilerBase.ThreadInfo threadInfo;
    if (!this.threadInfos.TryGetValue(Thread.CurrentThread.ManagedThreadId, out threadInfo))
    {
      threadInfo = new ProfilerBase.ThreadInfo(Thread.CurrentThread.ManagedThreadId);
      if (name != null)
        threadInfo.name = name;
      Debug.LogFormat("ManifestThreadInfo: {0}, {1}", (object) name, (object) Thread.CurrentThread.ManagedThreadId);
      lock (this.threadInfos)
        this.threadInfos.Add(Thread.CurrentThread.ManagedThreadId, threadInfo);
    }
    if (name != null && threadInfo.name != name)
    {
      Debug.LogFormat("ManifestThreadInfo: change name {0} to {1}, {2}", (object) name, (object) threadInfo.name, (object) Thread.CurrentThread.ManagedThreadId);
      threadInfo.name = name;
      lock (this.threadInfos)
        this.threadInfos[threadInfo.id] = threadInfo;
    }
    return threadInfo;
  }

  [Conditional("KPROFILER_VALIDATE_REGION_NAME")]
  private void ValidateRegionName(string region_name)
  {
    DebugUtil.Assert(!region_name.Contains("\""));
    region_name = "InvalidRegionName";
  }

  protected void Push(string region_name, string file, uint line)
  {
    if (!this.IsRecording())
      return;
    ProfilerBase.ThreadInfo threadInfo = this.ManifestThreadInfo();
    threadInfo.regionStack.Push(region_name);
    threadInfo.WriteLine(this.category, region_name, this.sw, "B", "},");
  }

  protected void Pop()
  {
    if (!this.IsRecording())
      return;
    ProfilerBase.ThreadInfo threadInfo = this.ManifestThreadInfo();
    if (threadInfo.regionStack.Count == 0)
      return;
    threadInfo.WriteLine(this.category, threadInfo.regionStack.Pop(), this.sw, "E", "},");
  }

  protected struct ThreadInfo
  {
    public Stack<string> regionStack;
    public StringBuilder sb;
    public int id;
    public string name;

    public ThreadInfo(int id)
    {
      this.regionStack = new Stack<string>();
      this.sb = new StringBuilder();
      this.id = id;
      this.name = string.Empty;
    }

    public void Reset()
    {
      this.regionStack.Clear();
      this.sb.Length = 0;
    }

    public void WriteLine(
      string category,
      string region_name,
      Stopwatch sw,
      string ph,
      string suffix)
    {
      ProfilerBase.WriteLine(this.sb, category, region_name, this.id, sw, ph, suffix);
    }

    public void StartLine(string category, string region_name, Stopwatch sw, string ph) => ProfilerBase.StartLine(this.sb, category, region_name, this.id, sw, ph);
  }
}
