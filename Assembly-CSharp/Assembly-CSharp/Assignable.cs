// Decompiled with JetBrains decompiler
// Type: Assignable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
  public string slotID;
  private AssignableSlot _slot;
  public IAssignableIdentity assignee;
  [Serialize]
  protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();
  [Serialize]
  private string assignee_groupID = "";
  public AssignableSlot[] subSlots;
  public bool canBePublic;
  [Serialize]
  private bool canBeAssigned = true;
  private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
  private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

  public AssignableSlot slot
  {
    get
    {
      if (this._slot == null)
        this._slot = Db.Get().AssignableSlots.Get(this.slotID);
      return this._slot;
    }
  }

  public bool CanBeAssigned => this.canBeAssigned;

  public event System.Action<IAssignableIdentity> OnAssign;

  [System.Runtime.Serialization.OnDeserialized]
  internal void OnDeserialized()
  {
  }

  private void RestoreAssignee()
  {
    IAssignableIdentity savedAssignee = this.GetSavedAssignee();
    if (savedAssignee == null)
      return;
    this.Assign(savedAssignee);
  }

  private IAssignableIdentity GetSavedAssignee()
  {
    if ((UnityEngine.Object) this.assignee_identityRef.Get() != (UnityEngine.Object) null)
      return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
    return this.assignee_groupID != "" ? (IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID] : (IAssignableIdentity) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RestoreAssignee();
    Game.Instance.assignmentManager.Add(this);
    if (this.assignee != null || !this.canBePublic)
      return;
    this.Assign((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"]);
  }

  protected override void OnCleanUp()
  {
    this.Unassign();
    Game.Instance.assignmentManager.Remove(this);
    base.OnCleanUp();
  }

  public bool CanAutoAssignTo(IAssignableIdentity identity)
  {
    MinionAssignablesProxy assignablesProxy = identity as MinionAssignablesProxy;
    if ((UnityEngine.Object) assignablesProxy == (UnityEngine.Object) null)
      return true;
    if (!this.CanAssignTo((IAssignableIdentity) assignablesProxy))
      return false;
    foreach (Func<MinionAssignablesProxy, bool> autoassignmentPrecondition in this.autoassignmentPreconditions)
    {
      if (!autoassignmentPrecondition(assignablesProxy))
        return false;
    }
    return true;
  }

  public bool CanAssignTo(IAssignableIdentity identity)
  {
    MinionAssignablesProxy assignablesProxy = identity as MinionAssignablesProxy;
    if ((UnityEngine.Object) assignablesProxy == (UnityEngine.Object) null)
      return true;
    foreach (Func<MinionAssignablesProxy, bool> assignmentPrecondition in this.assignmentPreconditions)
    {
      if (!assignmentPrecondition(assignablesProxy))
        return false;
    }
    return true;
  }

  public bool IsAssigned() => this.assignee != null;

  public bool IsAssignedTo(IAssignableIdentity identity)
  {
    Debug.Assert(identity != null, (object) "IsAssignedTo identity is null");
    Ownables soleOwner = identity.GetSoleOwner();
    Debug.Assert((UnityEngine.Object) soleOwner != (UnityEngine.Object) null, (object) "IsAssignedTo identity sole owner is null");
    if (this.assignee != null)
    {
      foreach (Ownables owner in this.assignee.GetOwners())
      {
        Debug.Assert((bool) (UnityEngine.Object) owner, (object) "Assignable owners list contained null");
        if ((UnityEngine.Object) owner.gameObject == (UnityEngine.Object) soleOwner.gameObject)
          return true;
      }
    }
    return false;
  }

  public virtual void Assign(IAssignableIdentity new_assignee)
  {
    if (new_assignee == this.assignee)
      return;
    switch (new_assignee)
    {
      case KMonoBehaviour _:
        if (!this.CanAssignTo(new_assignee))
          return;
        this.assignee_identityRef.Set((KMonoBehaviour) new_assignee);
        this.assignee_groupID = "";
        break;
      case AssignmentGroup _:
        this.assignee_identityRef.Set((KMonoBehaviour) null);
        this.assignee_groupID = ((AssignmentGroup) new_assignee).id;
        break;
    }
    this.GetComponent<KPrefabID>().AddTag(GameTags.Assigned);
    this.assignee = new_assignee;
    if (this.slot != null)
    {
      switch (new_assignee)
      {
        case MinionIdentity _:
        case StoredMinionIdentity _:
        case MinionAssignablesProxy _:
          Ownables soleOwner = new_assignee.GetSoleOwner();
          if ((UnityEngine.Object) soleOwner != (UnityEngine.Object) null)
            soleOwner.GetSlot(this.slot)?.Assign(this);
          Equipment component = soleOwner.GetComponent<Equipment>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            AssignableSlotInstance slot = component.GetSlot(this.slot);
            if (slot != null)
            {
              slot.Assign(this);
              break;
            }
            break;
          }
          break;
      }
    }
    if (this.OnAssign != null)
      this.OnAssign(new_assignee);
    this.Trigger(684616645, (object) new_assignee);
  }

  public virtual void Unassign()
  {
    if (this.assignee == null)
      return;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
    if (this.slot != null)
    {
      Ownables soleOwner = this.assignee.GetSoleOwner();
      if ((bool) (UnityEngine.Object) soleOwner)
      {
        soleOwner.GetSlot(this.slot)?.Unassign();
        Equipment component = soleOwner.GetComponent<Equipment>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.GetSlot(this.slot)?.Unassign();
      }
    }
    this.assignee = (IAssignableIdentity) null;
    if (this.canBePublic)
      this.Assign((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"]);
    this.assignee_identityRef.Set((KMonoBehaviour) null);
    this.assignee_groupID = "";
    if (this.OnAssign != null)
      this.OnAssign((IAssignableIdentity) null);
    this.Trigger(684616645, (object) null);
  }

  public void SetCanBeAssigned(bool state) => this.canBeAssigned = state;

  public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition) => this.assignmentPreconditions.Add(precondition);

  public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition) => this.autoassignmentPreconditions.Add(precondition);

  public int GetNavigationCost(Navigator navigator)
  {
    int num = -1;
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    IApproachable component = this.GetComponent<IApproachable>();
    foreach (CellOffset offset in component != null ? component.GetOffsets() : new CellOffset[1])
    {
      int cell2 = Grid.OffsetCell(cell1, offset);
      int navigationCost = navigator.GetNavigationCost(cell2);
      if (navigationCost != -1 && (num == -1 || navigationCost < num))
        num = navigationCost;
    }
    return num;
  }
}
