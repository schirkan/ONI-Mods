// Decompiled with JetBrains decompiler
// Type: JobManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class JobManager
{
  public static bool errorOccured;
  private List<JobManager.WorkerThread> threads = new List<JobManager.WorkerThread>();
  private Semaphore semaphore;
  private IWorkItemCollection workItems;
  private int nextWorkIndex = -1;
  private int workerThreadCount;
  private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
  private static bool runSingleThreaded;

  public bool isShuttingDown { get; private set; }

  private void Initialize()
  {
    this.semaphore = new Semaphore(0, CPUBudget.coreCount);
    for (int index = 0; index < CPUBudget.coreCount; ++index)
      this.threads.Add(new JobManager.WorkerThread(this.semaphore, this, string.Format("KWorker{0}", (object) index)));
  }

  public bool DoNextWorkItem()
  {
    int work_item_idx = Interlocked.Increment(ref this.nextWorkIndex);
    if (work_item_idx >= this.workItems.Count)
      return false;
    this.workItems.InternalDoWorkItem(work_item_idx);
    return true;
  }

  public void Cleanup()
  {
    this.isShuttingDown = true;
    this.semaphore.Release(this.threads.Count);
    foreach (JobManager.WorkerThread thread in this.threads)
      thread.Cleanup();
    this.threads.Clear();
  }

  public void Run(IWorkItemCollection work_items)
  {
    if (this.semaphore == null)
      this.Initialize();
    if (JobManager.runSingleThreaded || this.threads.Count == 0)
    {
      for (int work_item_idx = 0; work_item_idx < work_items.Count; ++work_item_idx)
        work_items.InternalDoWorkItem(work_item_idx);
    }
    else
    {
      this.workerThreadCount = this.threads.Count;
      this.nextWorkIndex = -1;
      this.workItems = work_items;
      Thread.MemoryBarrier();
      this.semaphore.Release(this.threads.Count);
      this.manualResetEvent.WaitOne();
      this.manualResetEvent.Reset();
      if (!JobManager.errorOccured)
        return;
      foreach (JobManager.WorkerThread thread in this.threads)
        thread.PrintExceptions();
    }
  }

  public void DecrementActiveWorkerThreadCount()
  {
    if (Interlocked.Decrement(ref this.workerThreadCount) != 0)
      return;
    this.manualResetEvent.Set();
  }

  private class WorkerThread
  {
    private Thread thread;
    private Semaphore semaphore;
    private JobManager jobManager;
    private List<Exception> exceptions;

    public WorkerThread(Semaphore semaphore, JobManager job_manager, string name)
    {
      this.semaphore = semaphore;
      this.thread = new Thread(new ParameterizedThreadStart(JobManager.WorkerThread.ThreadMain), 131072);
      Util.ApplyInvariantCultureToThread(this.thread);
      this.thread.Priority = ThreadPriority.AboveNormal;
      this.thread.Name = name;
      this.jobManager = job_manager;
      this.exceptions = new List<Exception>();
      this.thread.Start((object) this);
    }

    public void Run()
    {
      while (true)
      {
        this.semaphore.WaitOne();
        if (!this.jobManager.isShuttingDown)
        {
          try
          {
            bool flag = true;
            while (flag)
              flag = this.jobManager.DoNextWorkItem();
          }
          catch (Exception ex)
          {
            this.exceptions.Add(ex);
            JobManager.errorOccured = true;
            Debugger.Break();
          }
          this.jobManager.DecrementActiveWorkerThreadCount();
        }
        else
          break;
      }
    }

    public void PrintExceptions()
    {
      foreach (object exception in this.exceptions)
        Debug.LogError(exception);
    }

    public void Cleanup()
    {
    }

    public static void ThreadMain(object data) => ((JobManager.WorkerThread) data).Run();
  }
}
