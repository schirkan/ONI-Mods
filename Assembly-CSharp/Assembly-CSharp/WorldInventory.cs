// Decompiled with JetBrains decompiler
// Type: WorldInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/WorldInventory")]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  private HashSet<Tag> Discovered = new HashSet<Tag>();
  [Serialize]
  private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();
  private Dictionary<Tag, HashSet<Pickupable>> Inventory = new Dictionary<Tag, HashSet<Pickupable>>();
  private MinionGroupProber Prober;
  private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();
  private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>((System.Action<WorldInventory, object>) ((component, data) => component.GenerateInventoryReport(data)));
  private int accessibleUpdateIndex;
  private bool firstUpdate = true;

  public static WorldInventory Instance { get; private set; }

  public event System.Action<Tag, Tag> OnDiscover;

  protected override void OnPrefabInit()
  {
    WorldInventory.Instance = this;
    this.Subscribe(Game.Instance.gameObject, -1588644844, new System.Action<object>(this.OnAddedFetchable));
    this.Subscribe(Game.Instance.gameObject, -1491270284, new System.Action<object>(this.OnRemovedFetchable));
    this.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
  }

  private void GenerateInventoryReport(object data)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (object brain in Components.Brains)
    {
      CreatureBrain cmp = brain as CreatureBrain;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
      {
        if (cmp.HasTag(GameTags.Creatures.Wild))
        {
          ++num1;
          ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, cmp.GetProperName(), cmp.GetProperName());
        }
        else
        {
          ++num2;
          ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, cmp.GetProperName(), cmp.GetProperName());
        }
      }
    }
    foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
    {
      if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
        ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName);
    }
  }

  protected override void OnSpawn()
  {
    this.Prober = MinionGroupProber.Get();
    this.StartCoroutine(this.InitialRefresh());
  }

  private IEnumerator InitialRefresh()
  {
    for (int i = 0; i < 1; ++i)
      yield return (object) null;
    for (int idx = 0; idx < Components.Pickupables.Count; ++idx)
    {
      Pickupable pickupable = Components.Pickupables[idx];
      if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
        pickupable.GetSMI<ReachabilityMonitor.Instance>()?.UpdateReachability();
    }
  }

  public bool IsReachable(Pickupable pickupable) => this.Prober.IsReachable((Workable) pickupable);

  public float GetTotalAmount(Tag tag)
  {
    float num = 0.0f;
    this.accessibleAmounts.TryGetValue(tag, out num);
    return num;
  }

  public ICollection<Pickupable> GetPickupables(Tag tag)
  {
    HashSet<Pickupable> pickupableSet = (HashSet<Pickupable>) null;
    this.Inventory.TryGetValue(tag, out pickupableSet);
    return (ICollection<Pickupable>) pickupableSet;
  }

  public List<Pickupable> CreatePickupablesList(Tag tag)
  {
    HashSet<Pickupable> source = (HashSet<Pickupable>) null;
    this.Inventory.TryGetValue(tag, out source);
    return source == null ? (List<Pickupable>) null : source.ToList<Pickupable>();
  }

  public List<Tag> GetPickupableTagsFromCategoryTag(Tag t)
  {
    List<Tag> tagList = new List<Tag>();
    ICollection<Pickupable> pickupables = this.GetPickupables(t);
    if (pickupables != null && pickupables.Count > 0)
    {
      foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
        tagList.AddRange((IEnumerable<Tag>) pickupable.KPrefabID.Tags);
    }
    return tagList;
  }

  public float GetAmount(Tag tag) => Mathf.Max(this.GetTotalAmount(tag) - MaterialNeeds.Instance.GetAmount(tag), 0.0f);

  public void Discover(Tag tag, Tag categoryTag)
  {
    int num = this.Discovered.Add(tag) ? 1 : 0;
    this.DiscoverCategory(categoryTag, tag);
    if (num == 0 || this.OnDiscover == null)
      return;
    this.OnDiscover(categoryTag, tag);
  }

  private void DiscoverCategory(Tag category_tag, Tag item_tag)
  {
    HashSet<Tag> tagSet;
    if (!this.DiscoveredCategories.TryGetValue(category_tag, out tagSet))
    {
      tagSet = new HashSet<Tag>();
      this.DiscoveredCategories[category_tag] = tagSet;
    }
    tagSet.Add(item_tag);
  }

  public HashSet<Tag> GetDiscovered() => this.Discovered;

  public bool IsDiscovered(Tag tag) => this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);

  public bool AnyDiscovered(ICollection<Tag> tags)
  {
    foreach (Tag tag in (IEnumerable<Tag>) tags)
    {
      if (this.IsDiscovered(tag))
        return true;
    }
    return false;
  }

  public bool Contains(Recipe.Ingredient[] ingredients)
  {
    bool flag = true;
    foreach (Recipe.Ingredient ingredient in ingredients)
    {
      if ((double) this.GetAmount(ingredient.tag) < (double) ingredient.amount)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources) => this.DiscoveredCategories.TryGetValue(tag, out resources);

  public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
  {
    HashSet<Tag> tagSet;
    return this.DiscoveredCategories.TryGetValue(tag, out tagSet) ? tagSet : new HashSet<Tag>();
  }

  public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(
    TagSet tagSet)
  {
    Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
    foreach (Tag tag in tagSet)
    {
      HashSet<Tag> tagSet1;
      if (this.DiscoveredCategories.TryGetValue(tag, out tagSet1))
        dictionary[tag] = tagSet1;
    }
    return dictionary;
  }

  private void Update()
  {
    int num1 = 0;
    Dictionary<Tag, HashSet<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
    while (enumerator.MoveNext())
    {
      KeyValuePair<Tag, HashSet<Pickupable>> current = enumerator.Current;
      if (num1 == this.accessibleUpdateIndex || this.firstUpdate)
      {
        Tag key = current.Key;
        HashSet<Pickupable> pickupableSet = current.Value;
        float num2 = 0.0f;
        foreach (Pickupable cmp in (IEnumerable<Pickupable>) pickupableSet)
        {
          if ((UnityEngine.Object) cmp != (UnityEngine.Object) null && !cmp.HasTag(GameTags.StoredPrivate))
            num2 += cmp.TotalAmount;
        }
        this.accessibleAmounts[key] = num2;
        this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
        break;
      }
      ++num1;
    }
    this.firstUpdate = false;
  }

  protected override void OnLoadLevel()
  {
    base.OnLoadLevel();
    WorldInventory.Instance = (WorldInventory) null;
  }

  public static Tag GetCategoryForTags(HashSet<Tag> tags)
  {
    Tag tag1 = Tag.Invalid;
    foreach (Tag tag2 in tags)
    {
      if (GameTags.AllCategories.Contains(tag2))
      {
        tag1 = tag2;
        break;
      }
    }
    return tag1;
  }

  public static Tag GetCategoryForEntity(KPrefabID entity)
  {
    ElementChunk component = entity.GetComponent<ElementChunk>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetComponent<PrimaryElement>().Element.materialCategory : WorldInventory.GetCategoryForTags(entity.Tags);
  }

  private void OnAddedFetchable(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject.GetComponent<Navigator>() != (UnityEngine.Object) null)
      return;
    Pickupable component1 = gameObject.GetComponent<Pickupable>();
    KPrefabID component2 = component1.GetComponent<KPrefabID>();
    Tag tag1 = component2.PrefabID();
    if (!this.Inventory.ContainsKey(tag1))
    {
      Tag categoryForEntity = WorldInventory.GetCategoryForEntity(component2);
      DebugUtil.DevAssertArgs((categoryForEntity.IsValid ? 1 : 0) != 0, (object) component1.name, (object) "was found by worldinventory but doesn't have a category! Add it to the element definition.");
      this.Discover(tag1, categoryForEntity);
    }
    foreach (Tag tag2 in component2.Tags)
    {
      HashSet<Pickupable> pickupableSet;
      if (!this.Inventory.TryGetValue(tag2, out pickupableSet))
      {
        pickupableSet = new HashSet<Pickupable>();
        this.Inventory[tag2] = pickupableSet;
      }
      pickupableSet.Add(component1);
    }
  }

  private void OnRemovedFetchable(object data)
  {
    Pickupable component = ((GameObject) data).GetComponent<Pickupable>();
    foreach (Tag tag in component.GetComponent<KPrefabID>().Tags)
    {
      HashSet<Pickupable> pickupableSet;
      if (this.Inventory.TryGetValue(tag, out pickupableSet))
        pickupableSet.Remove(component);
    }
  }

  public Dictionary<Tag, float> GetAccessibleAmounts() => this.accessibleAmounts;
}
