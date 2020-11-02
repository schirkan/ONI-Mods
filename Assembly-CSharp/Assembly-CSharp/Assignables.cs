// Decompiled with JetBrains decompiler
// Type: Assignables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Assignables")]
public class Assignables : KMonoBehaviour
{
  protected List<AssignableSlotInstance> slots = new List<AssignableSlotInstance>();
  private static readonly EventSystem.IntraObjectHandler<Assignables> OnDeadTagChangedDelegate = GameUtil.CreateHasTagHandler<Assignables>(GameTags.Dead, (System.Action<Assignables, object>) ((component, data) => component.OnDeath(data)));

  public List<AssignableSlotInstance> Slots => this.slots;

  protected IAssignableIdentity GetAssignableIdentity()
  {
    MinionIdentity component = this.GetComponent<MinionIdentity>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? (IAssignableIdentity) component.assignableProxy.Get() : (IAssignableIdentity) this.GetComponent<MinionAssignablesProxy>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameUtil.SubscribeToTags<Assignables>(this, Assignables.OnDeadTagChangedDelegate);
  }

  private void OnDeath(object data)
  {
    foreach (AssignableSlotInstance slot in this.slots)
      slot.Unassign();
  }

  public void Add(AssignableSlotInstance slot_instance) => this.slots.Add(slot_instance);

  public Assignable GetAssignable(AssignableSlot slot) => this.GetSlot(slot)?.assignable;

  public AssignableSlotInstance GetSlot(AssignableSlot slot)
  {
    Debug.Assert(this.slots.Count > 0, (object) "GetSlot called with no slots configured");
    if (slot == null)
      return (AssignableSlotInstance) null;
    foreach (AssignableSlotInstance slot1 in this.slots)
    {
      if (slot1.slot == slot)
        return slot1;
    }
    return (AssignableSlotInstance) null;
  }

  public Assignable AutoAssignSlot(AssignableSlot slot)
  {
    Assignable assignable1 = this.GetAssignable(slot);
    if ((UnityEngine.Object) assignable1 != (UnityEngine.Object) null)
      return assignable1;
    GameObject targetGameObject = this.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    if ((UnityEngine.Object) targetGameObject == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "AutoAssignSlot failed, proxy game object was null.");
      return (Assignable) null;
    }
    Navigator component = targetGameObject.GetComponent<Navigator>();
    IAssignableIdentity assignableIdentity = this.GetAssignableIdentity();
    int num = int.MaxValue;
    foreach (Assignable assignable2 in Game.Instance.assignmentManager)
    {
      if (!((UnityEngine.Object) assignable2 == (UnityEngine.Object) null) && !assignable2.IsAssigned() && (assignable2.slot == slot && assignable2.CanAutoAssignTo(assignableIdentity)))
      {
        int navigationCost = assignable2.GetNavigationCost(component);
        if (navigationCost != -1 && navigationCost < num)
        {
          num = navigationCost;
          assignable1 = assignable2;
        }
      }
    }
    if ((UnityEngine.Object) assignable1 != (UnityEngine.Object) null)
      assignable1.Assign(assignableIdentity);
    return assignable1;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    foreach (AssignableSlotInstance slot in this.slots)
      slot.Unassign();
  }
}
