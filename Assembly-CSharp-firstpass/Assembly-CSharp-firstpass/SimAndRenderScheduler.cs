﻿// Decompiled with JetBrains decompiler
// Type: SimAndRenderScheduler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

public class SimAndRenderScheduler
{
  private static SimAndRenderScheduler _instance;
  private Dictionary<string, SimAndRenderScheduler.Entry> bucketTable = new Dictionary<string, SimAndRenderScheduler.Entry>();
  public SimAndRenderScheduler.RenderEveryTickUpdater renderEveryTick = new SimAndRenderScheduler.RenderEveryTickUpdater();
  public SimAndRenderScheduler.Render200ms render200ms = new SimAndRenderScheduler.Render200ms();
  public SimAndRenderScheduler.Render1000msUpdater render1000ms = new SimAndRenderScheduler.Render1000msUpdater();
  public SimAndRenderScheduler.SimEveryTickUpdater simEveryTick = new SimAndRenderScheduler.SimEveryTickUpdater();
  public SimAndRenderScheduler.Sim33msUpdater sim33ms = new SimAndRenderScheduler.Sim33msUpdater();
  public SimAndRenderScheduler.Sim200msUpdater sim200ms = new SimAndRenderScheduler.Sim200msUpdater();
  public SimAndRenderScheduler.Sim1000msUpdater sim1000ms = new SimAndRenderScheduler.Sim1000msUpdater();
  public SimAndRenderScheduler.Sim4000msUpdater sim4000ms = new SimAndRenderScheduler.Sim4000msUpdater();
  private Dictionary<Type, UpdateRate[]> typeImplementedInterfaces = new Dictionary<Type, UpdateRate[]>();
  private Dictionary<Type, UpdateRate> availableInterfaces = new Dictionary<Type, UpdateRate>();

  public static SimAndRenderScheduler instance
  {
    get
    {
      if (SimAndRenderScheduler._instance == null)
        SimAndRenderScheduler._instance = new SimAndRenderScheduler();
      return SimAndRenderScheduler._instance;
    }
  }

  public static void DestroyInstance() => SimAndRenderScheduler._instance = (SimAndRenderScheduler) null;

  private SimAndRenderScheduler()
  {
    this.availableInterfaces[typeof (IRenderEveryTick)] = UpdateRate.RENDER_EVERY_TICK;
    this.availableInterfaces[typeof (IRender200ms)] = UpdateRate.RENDER_200ms;
    this.availableInterfaces[typeof (IRender1000ms)] = UpdateRate.RENDER_1000ms;
    this.availableInterfaces[typeof (ISimEveryTick)] = UpdateRate.SIM_EVERY_TICK;
    this.availableInterfaces[typeof (ISim33ms)] = UpdateRate.SIM_33ms;
    this.availableInterfaces[typeof (ISim200ms)] = UpdateRate.SIM_200ms;
    this.availableInterfaces[typeof (ISim1000ms)] = UpdateRate.SIM_1000ms;
    this.availableInterfaces[typeof (ISim4000ms)] = UpdateRate.SIM_4000ms;
  }

  private static string MakeBucketId(Type updater_type, UpdateRate update_rate) => string.Format("{0} {1}", (object) updater_type.Name, (object) update_rate.ToString());

  private UpdateRate[] GetImplementedInterfaces(Type type)
  {
    UpdateRate[] updateRateArray = (UpdateRate[]) null;
    if (!this.typeImplementedInterfaces.TryGetValue(type, out updateRateArray))
    {
      ListPool<UpdateRate, SimAndRenderScheduler>.PooledList pooledList = ListPool<UpdateRate, SimAndRenderScheduler>.Allocate();
      foreach (KeyValuePair<Type, UpdateRate> availableInterface in this.availableInterfaces)
      {
        if (availableInterface.Key.IsAssignableFrom(type))
          pooledList.Add(availableInterface.Value);
      }
      updateRateArray = pooledList.ToArray();
      pooledList.Recycle();
      this.typeImplementedInterfaces[type] = updateRateArray;
    }
    return updateRateArray;
  }

  public static Type GetUpdateInterface(UpdateRate update_rate)
  {
    switch (update_rate)
    {
      case UpdateRate.RENDER_EVERY_TICK:
        return typeof (IRenderEveryTick);
      case UpdateRate.RENDER_200ms:
        return typeof (IRender200ms);
      case UpdateRate.RENDER_1000ms:
        return typeof (IRender1000ms);
      case UpdateRate.SIM_EVERY_TICK:
        return typeof (ISimEveryTick);
      case UpdateRate.SIM_33ms:
        return typeof (ISim33ms);
      case UpdateRate.SIM_200ms:
        return typeof (ISim200ms);
      case UpdateRate.SIM_1000ms:
        return typeof (ISim1000ms);
      case UpdateRate.SIM_4000ms:
        return typeof (ISim4000ms);
      default:
        return (Type) null;
    }
  }

  public UpdateRate GetUpdateRate(Type updater)
  {
    UpdateRate updateRate;
    if (!this.availableInterfaces.TryGetValue(updater, out updateRate))
      Debug.Assert(false, (object) "only call this with an update interface type");
    return updateRate;
  }

  public UpdateRate GetUpdateRate<T>() => this.GetUpdateRate(typeof (T));

  public void Add(object obj, bool load_balance = false)
  {
    foreach (int implementedInterface in this.GetImplementedInterfaces(obj.GetType()))
    {
      switch (implementedInterface)
      {
        case 0:
          this.renderEveryTick.Add((IRenderEveryTick) obj, load_balance);
          break;
        case 1:
          this.render200ms.Add((IRender200ms) obj, load_balance);
          break;
        case 2:
          this.render1000ms.Add((IRender1000ms) obj, load_balance);
          break;
        case 3:
          this.simEveryTick.Add((ISimEveryTick) obj, load_balance);
          break;
        case 4:
          this.sim33ms.Add((ISim33ms) obj, load_balance);
          break;
        case 5:
          this.sim200ms.Add((ISim200ms) obj, load_balance);
          break;
        case 6:
          this.sim1000ms.Add((ISim1000ms) obj, load_balance);
          break;
        case 7:
          this.sim4000ms.Add((ISim4000ms) obj, load_balance);
          break;
      }
    }
  }

  public void Remove(object obj)
  {
    foreach (int implementedInterface in this.GetImplementedInterfaces(obj.GetType()))
    {
      switch (implementedInterface)
      {
        case 0:
          this.renderEveryTick.Remove((IRenderEveryTick) obj);
          break;
        case 1:
          this.render200ms.Remove((IRender200ms) obj);
          break;
        case 2:
          this.render1000ms.Remove((IRender1000ms) obj);
          break;
        case 3:
          this.simEveryTick.Remove((ISimEveryTick) obj);
          break;
        case 4:
          this.sim33ms.Remove((ISim33ms) obj);
          break;
        case 5:
          this.sim200ms.Remove((ISim200ms) obj);
          break;
        case 6:
          this.sim1000ms.Remove((ISim1000ms) obj);
          break;
        case 7:
          this.sim4000ms.Remove((ISim4000ms) obj);
          break;
      }
    }
  }

  private SimAndRenderScheduler.Entry ManifestEntry<UpdateInterface>(
    string name,
    bool load_balance)
  {
    SimAndRenderScheduler.Entry entry1;
    if (this.bucketTable.TryGetValue(name, out entry1))
    {
      DebugUtil.DevAssertArgs((entry1.buckets.Length == (load_balance ? Singleton<StateMachineUpdater>.Instance.GetFrameCount(this.GetUpdateRate<UpdateInterface>()) : 1) ? 1 : 0) != 0, (object) "load_balance doesn't match previous registration...maybe load_balance erroneously on for a BatchUpdate type ", (object) name, (object) "?");
      return entry1;
    }
    SimAndRenderScheduler.Entry entry2 = new SimAndRenderScheduler.Entry();
    UpdateRate updateRate = this.GetUpdateRate<UpdateInterface>();
    int length = load_balance ? Singleton<StateMachineUpdater>.Instance.GetFrameCount(updateRate) : 1;
    entry2.buckets = new StateMachineUpdater.BaseUpdateBucket[length];
    for (int index = 0; index < length; ++index)
    {
      entry2.buckets[index] = (StateMachineUpdater.BaseUpdateBucket) new UpdateBucketWithUpdater<UpdateInterface>(name);
      Singleton<StateMachineUpdater>.Instance.AddBucket(updateRate, entry2.buckets[index]);
    }
    return entry2;
  }

  public SimAndRenderScheduler.Handle Schedule<SimUpdateType>(
    string name,
    UpdateBucketWithUpdater<SimUpdateType>.IUpdater bucket_updater,
    UpdateRate update_rate,
    SimUpdateType updater,
    bool load_balance = false)
  {
    SimAndRenderScheduler.Entry entry = this.ManifestEntry<SimUpdateType>(name, load_balance);
    UpdateBucketWithUpdater<SimUpdateType> bucket = (UpdateBucketWithUpdater<SimUpdateType>) entry.buckets[entry.nextBucketIdx];
    SimAndRenderScheduler.Handle handle = new SimAndRenderScheduler.Handle();
    handle.handle = bucket.Add(updater, Singleton<StateMachineUpdater>.Instance.GetFrameTime(update_rate, bucket.frame), bucket_updater);
    handle.bucket = (StateMachineUpdater.BaseUpdateBucket) bucket;
    entry.nextBucketIdx = (entry.nextBucketIdx + 1) % entry.buckets.Length;
    this.bucketTable[name] = entry;
    return handle;
  }

  public void Reset() => SimAndRenderScheduler._instance = (SimAndRenderScheduler) null;

  public void RegisterBatchUpdate<UpdateInterface, T>(
    UpdateBucketWithUpdater<UpdateInterface>.BatchUpdateDelegate batch_update)
  {
    string str = SimAndRenderScheduler.MakeBucketId(typeof (T), this.GetUpdateRate<UpdateInterface>());
    SimAndRenderScheduler.Entry entry = this.ManifestEntry<UpdateInterface>(str, false);
    DebugUtil.DevAssert(((IEnumerable<UpdateRate>) this.GetImplementedInterfaces(typeof (T))).Contains<UpdateRate>(this.GetUpdateRate<UpdateInterface>()), "T does not implement the UpdateInterface it is registering for BatchUpdate under");
    DebugUtil.DevAssert(entry.buckets.Length == 1, "don't do a batch update with load balancing because load balancing will produce many small batches which is inefficient");
    ((UpdateBucketWithUpdater<UpdateInterface>) entry.buckets[0]).batch_update_delegate = batch_update;
    this.bucketTable[str] = entry;
  }

  public struct Handle
  {
    public HandleVector<int>.Handle handle;
    public StateMachineUpdater.BaseUpdateBucket bucket;

    public bool IsValid() => this.bucket != null;

    public void Release()
    {
      if (this.bucket == null)
        return;
      this.bucket.Remove(this.handle);
      this.bucket = (StateMachineUpdater.BaseUpdateBucket) null;
    }
  }

  private struct Entry
  {
    public StateMachineUpdater.BaseUpdateBucket[] buckets;
    public int nextBucketIdx;
  }

  public class BaseUpdaterManager
  {
    public UpdateRate updateRate { get; private set; }

    protected BaseUpdaterManager(UpdateRate update_rate) => this.updateRate = update_rate;
  }

  public class UpdaterManager<UpdaterType> : SimAndRenderScheduler.BaseUpdaterManager
  {
    private Dictionary<UpdaterType, SimAndRenderScheduler.Handle> updaterHandles = new Dictionary<UpdaterType, SimAndRenderScheduler.Handle>();
    private Dictionary<Type, string> bucketIds = new Dictionary<Type, string>();

    public UpdaterManager(UpdateRate update_rate)
      : base(update_rate)
    {
    }

    public void Add(UpdaterType updater, bool load_balance = false)
    {
      if (this.Contains(updater))
        return;
      string name = "";
      if (!this.bucketIds.TryGetValue(updater.GetType(), out name))
      {
        name = SimAndRenderScheduler.MakeBucketId(updater.GetType(), this.updateRate);
        this.bucketIds[updater.GetType()] = name;
      }
      SimAndRenderScheduler.Handle handle = SimAndRenderScheduler.instance.Schedule<UpdaterType>(name, (UpdateBucketWithUpdater<UpdaterType>.IUpdater) this, this.updateRate, updater, load_balance);
      this.updaterHandles[updater] = handle;
    }

    public void Remove(UpdaterType updater)
    {
      SimAndRenderScheduler.Handle handle;
      if (!this.updaterHandles.TryGetValue(updater, out handle))
        return;
      handle.Release();
      this.updaterHandles.Remove(updater);
    }

    public bool Contains(UpdaterType updater) => this.updaterHandles.ContainsKey(updater);
  }

  public class RenderEveryTickUpdater : SimAndRenderScheduler.UpdaterManager<IRenderEveryTick>, UpdateBucketWithUpdater<IRenderEveryTick>.IUpdater
  {
    public RenderEveryTickUpdater()
      : base(UpdateRate.RENDER_EVERY_TICK)
    {
    }

    public void Update(IRenderEveryTick updater, float dt) => updater.RenderEveryTick(dt);
  }

  public class Render200ms : SimAndRenderScheduler.UpdaterManager<IRender200ms>, UpdateBucketWithUpdater<IRender200ms>.IUpdater
  {
    public Render200ms()
      : base(UpdateRate.RENDER_200ms)
    {
    }

    public void Update(IRender200ms updater, float dt) => updater.Render200ms(dt);
  }

  public class Render1000msUpdater : SimAndRenderScheduler.UpdaterManager<IRender1000ms>, UpdateBucketWithUpdater<IRender1000ms>.IUpdater
  {
    public Render1000msUpdater()
      : base(UpdateRate.RENDER_1000ms)
    {
    }

    public void Update(IRender1000ms updater, float dt) => updater.Render1000ms(dt);
  }

  public class SimEveryTickUpdater : SimAndRenderScheduler.UpdaterManager<ISimEveryTick>, UpdateBucketWithUpdater<ISimEveryTick>.IUpdater
  {
    public SimEveryTickUpdater()
      : base(UpdateRate.SIM_EVERY_TICK)
    {
    }

    public void Update(ISimEveryTick updater, float dt) => updater.SimEveryTick(dt);
  }

  public class Sim33msUpdater : SimAndRenderScheduler.UpdaterManager<ISim33ms>, UpdateBucketWithUpdater<ISim33ms>.IUpdater
  {
    public Sim33msUpdater()
      : base(UpdateRate.SIM_33ms)
    {
    }

    public void Update(ISim33ms updater, float dt) => updater.Sim33ms(dt);
  }

  public class Sim200msUpdater : SimAndRenderScheduler.UpdaterManager<ISim200ms>, UpdateBucketWithUpdater<ISim200ms>.IUpdater
  {
    public Sim200msUpdater()
      : base(UpdateRate.SIM_200ms)
    {
    }

    public void Update(ISim200ms updater, float dt) => updater.Sim200ms(dt);
  }

  public class Sim1000msUpdater : SimAndRenderScheduler.UpdaterManager<ISim1000ms>, UpdateBucketWithUpdater<ISim1000ms>.IUpdater
  {
    public Sim1000msUpdater()
      : base(UpdateRate.SIM_1000ms)
    {
    }

    public void Update(ISim1000ms updater, float dt) => updater.Sim1000ms(dt);
  }

  public class Sim4000msUpdater : SimAndRenderScheduler.UpdaterManager<ISim4000ms>, UpdateBucketWithUpdater<ISim4000ms>.IUpdater
  {
    public Sim4000msUpdater()
      : base(UpdateRate.SIM_4000ms)
    {
    }

    public void Update(ISim4000ms updater, float dt) => updater.Sim4000ms(dt);
  }
}
