// Decompiled with JetBrains decompiler
// Type: GlobalJobManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public static class GlobalJobManager
{
  private static JobManager jobManager = new JobManager();

  public static void Run(IWorkItemCollection work_items) => GlobalJobManager.jobManager.Run(work_items);

  public static void Cleanup()
  {
    if (GlobalJobManager.jobManager != null)
      GlobalJobManager.jobManager.Cleanup();
    GlobalJobManager.jobManager = (JobManager) null;
  }
}
