// Decompiled with JetBrains decompiler
// Type: KPrefabID
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Plugins/KPrefabID")]
public class KPrefabID : KMonoBehaviour, ISaveLoadable
{
  public const int InvalidInstanceID = -1;
  private static int nextUniqueID = 0;
  [ReadOnly]
  public Tag SaveLoadTag;
  public Tag PrefabTag;
  private TagBits tagBits;
  private bool initialized;
  private bool dirtyTagBits = true;
  [Serialize]
  public int InstanceID;
  public int defaultLayer;
  public List<Descriptor> AdditionalRequirements;
  public List<Descriptor> AdditionalEffects;
  [Serialize]
  private HashSet<Tag> serializedTags = new HashSet<Tag>();
  private HashSet<Tag> tags = new HashSet<Tag>();
  private static readonly EventSystem.IntraObjectHandler<KPrefabID> OnObjectDestroyedDelegate = new EventSystem.IntraObjectHandler<KPrefabID>((System.Action<KPrefabID, object>) ((component, data) => component.OnObjectDestroyed(data)));

  public static int NextUniqueID
  {
    get => KPrefabID.nextUniqueID;
    set => KPrefabID.nextUniqueID = value;
  }

  public event KPrefabID.PrefabFn instantiateFn;

  public event KPrefabID.PrefabFn prefabInitFn;

  public event KPrefabID.PrefabFn prefabSpawnFn;

  public bool pendingDestruction { get; private set; }

  public bool conflicted { get; private set; }

  public HashSet<Tag> Tags
  {
    get
    {
      this.InitializeTags(true);
      return this.tags;
    }
  }

  public void CopyTags(KPrefabID other)
  {
    foreach (Tag tag in other.tags)
      this.tags.Add(tag);
  }

  public void CopyInitFunctions(KPrefabID other)
  {
    this.instantiateFn = other.instantiateFn;
    this.prefabInitFn = other.prefabInitFn;
    this.prefabSpawnFn = other.prefabSpawnFn;
  }

  public void RunInstantiateFn()
  {
    if (this.instantiateFn == null)
      return;
    this.instantiateFn(this.gameObject);
    this.instantiateFn = (KPrefabID.PrefabFn) null;
  }

  private void ValidateTags()
  {
    DebugUtil.Assert(this.PrefabTag.IsValid);
    Debug.Assert(this.tags.Contains(this.PrefabTag), (object) string.Format("PrefabTag {0} is not contained in tags", (object) this.PrefabTag));
    foreach (Tag serializedTag in this.serializedTags)
      Debug.Assert(this.tags.Contains(serializedTag), (object) string.Format("serialized tag {0} is not contained in tags", (object) serializedTag));
  }

  public void InitializeTags(bool force_initialize = false)
  {
    if (this.initialized && !force_initialize)
      return;
    DebugUtil.Assert(this.PrefabTag.IsValid);
    if (this.tags.Add(this.PrefabTag))
      this.dirtyTagBits = true;
    foreach (Tag serializedTag in this.serializedTags)
    {
      if (this.tags.Add(serializedTag))
        this.dirtyTagBits = true;
    }
    this.initialized = true;
  }

  public void UpdateSaveLoadTag() => this.SaveLoadTag = new Tag(this.PrefabTag.Name);

  public Tag GetSaveLoadTag() => this.SaveLoadTag;

  private void LaunderTagBits()
  {
    if (!this.dirtyTagBits)
      return;
    this.tagBits.ClearAll();
    foreach (Tag tag in this.tags)
      this.tagBits.SetTag(tag);
    this.dirtyTagBits = false;
  }

  public void UpdateTagBits()
  {
    this.InitializeTags();
    this.LaunderTagBits();
  }

  public void AndTagBits(ref TagBits rhs)
  {
    this.UpdateTagBits();
    rhs.And(ref this.tagBits);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<KPrefabID>(1969584890, KPrefabID.OnObjectDestroyedDelegate);
    this.InitializeTags(true);
    if (this.prefabInitFn != null)
    {
      this.prefabInitFn(this.gameObject);
      this.prefabInitFn = (KPrefabID.PrefabFn) null;
    }
    this.GetComponent<IStateMachineControllerHack>()?.CreateSMIS();
  }

  protected override void OnSpawn()
  {
    this.InitializeTags(true);
    this.GetComponent<IStateMachineControllerHack>()?.StartSMIS();
    if (this.prefabSpawnFn == null)
      return;
    this.prefabSpawnFn(this.gameObject);
    this.prefabSpawnFn = (KPrefabID.PrefabFn) null;
  }

  protected override void OnCmpEnable() => this.InitializeTags(true);

  public void AddTag(Tag tag, bool serialize = false)
  {
    DebugUtil.Assert(tag.IsValid);
    if (this.Tags.Add(tag))
    {
      this.dirtyTagBits = true;
      this.Trigger(-1582839653, (object) null);
    }
    if (!serialize)
      return;
    this.serializedTags.Add(tag);
  }

  public void RemoveTag(Tag tag)
  {
    if (this.Tags.Remove(tag))
    {
      this.dirtyTagBits = true;
      this.Trigger(-1582839653, (object) null);
    }
    this.serializedTags.Remove(tag);
  }

  public void SetTag(Tag tag, bool set)
  {
    if (set)
      this.AddTag(tag);
    else
      this.RemoveTag(tag);
  }

  public bool HasTag(Tag tag) => this.Tags.Contains(tag);

  public bool HasAnyTags(List<Tag> search_tags)
  {
    this.InitializeTags();
    foreach (Tag searchTag in search_tags)
    {
      if (this.tags.Contains(searchTag))
        return true;
    }
    return false;
  }

  public bool HasAnyTags(Tag[] search_tags)
  {
    this.InitializeTags();
    foreach (Tag searchTag in search_tags)
    {
      if (this.tags.Contains(searchTag))
        return true;
    }
    return false;
  }

  public bool HasAnyTags(ref TagBits search_tags)
  {
    this.UpdateTagBits();
    return this.tagBits.HasAny(ref search_tags);
  }

  public bool HasAllTags(ref TagBits search_tags)
  {
    this.UpdateTagBits();
    return this.tagBits.HasAll(ref search_tags);
  }

  public bool HasAnyTags_AssumeLaundered(ref TagBits search_tags) => this.tagBits.HasAny(ref search_tags);

  public bool HasAllTags_AssumeLaundered(ref TagBits search_tags) => this.tagBits.HasAll(ref search_tags);

  public override bool Equals(object o)
  {
    KPrefabID kprefabId = o as KPrefabID;
    return (UnityEngine.Object) kprefabId != (UnityEngine.Object) null && this.PrefabTag == kprefabId.PrefabTag;
  }

  public override int GetHashCode() => this.PrefabTag.GetHashCode();

  public static int GetUniqueID() => KPrefabID.NextUniqueID++;

  public string GetDebugName() => this.name + "(" + (object) this.InstanceID + ")";

  protected override void OnCleanUp()
  {
    this.pendingDestruction = true;
    if (this.InstanceID != -1)
      KPrefabIDTracker.Get().Unregister(this);
    this.Trigger(1969584890, (object) null);
  }

  [OnDeserialized]
  internal void OnDeserializedMethod()
  {
    this.InitializeTags(true);
    KPrefabIDTracker kprefabIdTracker = KPrefabIDTracker.Get();
    if ((bool) (UnityEngine.Object) kprefabIdTracker.GetInstance(this.InstanceID))
      this.conflicted = true;
    kprefabIdTracker.Register(this);
  }

  private void OnObjectDestroyed(object data) => this.pendingDestruction = true;

  public delegate void PrefabFn(GameObject go);
}
