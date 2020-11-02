// Decompiled with JetBrains decompiler
// Type: StateMachineUpdater
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;

public class StateMachineUpdater : Singleton<StateMachineUpdater>
{
  private List<StateMachineUpdater.BucketGroup> bucketGroups = new List<StateMachineUpdater.BucketGroup>();
  private List<StateMachineUpdater.BucketGroup> simBucketGroups = new List<StateMachineUpdater.BucketGroup>();
  private List<StateMachineUpdater.BucketGroup> renderBucketGroups = new List<StateMachineUpdater.BucketGroup>();
  private List<StateMachineUpdater.BucketGroup> renderEveryTickBucketGroups = new List<StateMachineUpdater.BucketGroup>();

  public StateMachineUpdater() => this.Initialize();

  private void Initialize()
  {
    this.bucketGroups = new List<StateMachineUpdater.BucketGroup>();
    this.simBucketGroups = new List<StateMachineUpdater.BucketGroup>();
    this.renderBucketGroups = new List<StateMachineUpdater.BucketGroup>();
    this.renderEveryTickBucketGroups = new List<StateMachineUpdater.BucketGroup>();
    this.CreateBucketGroup(1, 0.01666667f, UpdateRate.RENDER_EVERY_TICK, this.renderEveryTickBucketGroups);
    this.CreateBucketGroup(12, 0.01666667f, UpdateRate.RENDER_200ms, this.renderBucketGroups);
    this.CreateBucketGroup(60, 0.01666667f, UpdateRate.RENDER_1000ms, this.renderBucketGroups);
    this.CreateBucketGroup(1, 0.01666667f, UpdateRate.SIM_EVERY_TICK, this.simBucketGroups);
    this.CreateBucketGroup(2, 0.01666667f, UpdateRate.SIM_33ms, this.simBucketGroups);
    this.CreateBucketGroup(12, 0.01666667f, UpdateRate.SIM_200ms, this.simBucketGroups);
    this.CreateBucketGroup(60, 0.01666667f, UpdateRate.SIM_1000ms, this.simBucketGroups);
    this.CreateBucketGroup(240, 0.01666667f, UpdateRate.SIM_4000ms, this.simBucketGroups);
  }

  private void CreateBucketGroup(
    int sub_tick_count,
    float seconds_per_sub_tick,
    UpdateRate update_rate,
    List<StateMachineUpdater.BucketGroup> sub_group)
  {
    StateMachineUpdater.BucketGroup bucketGroup = new StateMachineUpdater.BucketGroup(sub_tick_count, seconds_per_sub_tick, update_rate);
    this.bucketGroups.Add(bucketGroup);
    sub_group.Add(bucketGroup);
  }

  public void AdvanceOneSimSubTick()
  {
    foreach (StateMachineUpdater.BucketGroup simBucketGroup in this.simBucketGroups)
    {
      float dt = (float) simBucketGroup.subTickCount * simBucketGroup.secondsPerSubTick;
      simBucketGroup.AdvanceOneSubTick(dt);
    }
  }

  public void Render(float dt)
  {
    foreach (StateMachineUpdater.BucketGroup renderBucketGroup in this.renderBucketGroups)
      renderBucketGroup.Advance(dt);
  }

  public void RenderEveryTick(float dt)
  {
    foreach (StateMachineUpdater.BucketGroup everyTickBucketGroup in this.renderEveryTickBucketGroups)
      everyTickBucketGroup.AdvanceOneSubTick(dt);
  }

  public int GetFrameCount(UpdateRate update_rate) => this.bucketGroups[(int) update_rate].subTickCount;

  public void AddBucket(UpdateRate update_rate, StateMachineUpdater.BaseUpdateBucket bucket) => this.bucketGroups[(int) update_rate].AddBucket(bucket);

  public float GetFrameTime(UpdateRate update_rate, int frame) => this.bucketGroups[(int) update_rate].GetFrameTime(frame);

  public void Clear() => this.Initialize();

  public class BucketGroup
  {
    private List<List<StateMachineUpdater.BaseUpdateBucket>> bucketFrames = new List<List<StateMachineUpdater.BaseUpdateBucket>>();
    private string name;
    public float accumulatedTime;
    private int nextUpdateIndex;
    private int nextBucketFrame;

    public float secondsPerSubTick { get; private set; }

    public UpdateRate updateRate { get; private set; }

    public int subTickCount => this.bucketFrames.Count;

    public BucketGroup(int frame_count, float seconds_per_sub_tick, UpdateRate update_rate)
    {
      for (int index = 0; index < frame_count; ++index)
        this.bucketFrames.Add(new List<StateMachineUpdater.BaseUpdateBucket>());
      this.secondsPerSubTick = seconds_per_sub_tick;
      this.updateRate = update_rate;
      this.name = "BucketGroup-" + update_rate.ToString();
    }

    private void InternalAdvance(float dt)
    {
      this.accumulatedTime += dt;
      float dt1 = (float) this.subTickCount * this.secondsPerSubTick;
      for (; (double) this.accumulatedTime >= (double) this.secondsPerSubTick; this.accumulatedTime -= this.secondsPerSubTick)
        this.AdvanceOneSubTick(dt1);
    }

    public void AdvanceOneSubTick(float dt)
    {
      List<StateMachineUpdater.BaseUpdateBucket> bucketFrame = this.bucketFrames[this.nextUpdateIndex];
      int count = bucketFrame.Count;
      for (int index = 0; index < count; ++index)
      {
        StateMachineUpdater.BaseUpdateBucket baseUpdateBucket = bucketFrame[index];
        if (baseUpdateBucket.count != 0)
          baseUpdateBucket.Update(dt);
      }
      this.nextUpdateIndex = (this.nextUpdateIndex + 1) % this.bucketFrames.Count;
    }

    public void Advance(float dt) => this.InternalAdvance(dt);

    public void AddBucket(StateMachineUpdater.BaseUpdateBucket bucket)
    {
      this.bucketFrames[this.nextBucketFrame].Add(bucket);
      bucket.frame = this.nextBucketFrame;
      this.nextBucketFrame = (this.nextBucketFrame + 1) % this.bucketFrames.Count;
    }

    public float GetFrameTime(int frame)
    {
      int num = this.nextUpdateIndex - 1 - frame;
      if (num <= 0)
        num += this.bucketFrames.Count;
      return (float) num * this.secondsPerSubTick;
    }
  }

  [DebuggerDisplay("{name}")]
  public abstract class BaseUpdateBucket
  {
    public int frame;

    public string name { get; private set; }

    public abstract int count { get; }

    public BaseUpdateBucket(string name) => this.name = name;

    public abstract void Update(float dt);

    public abstract void Remove(HandleVector<int>.Handle handle);
  }
}
