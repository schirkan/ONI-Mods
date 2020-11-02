// Decompiled with JetBrains decompiler
// Type: KTreeControl
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class KTreeControl : MonoBehaviour
{
  [SerializeField]
  private KTreeItem treeItemPrefab;
  [NonSerialized]
  public KTreeItem root;

  public void SetUserItemRoot(KTreeControl.UserItem rootItem)
  {
    if ((UnityEngine.Object) this.root != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.root);
    this.root = this.CreateItem(rootItem);
    this.root.transform.SetParent(this.transform, false);
  }

  private KTreeItem CreateItem(KTreeControl.UserItem userItem)
  {
    KTreeItem ktreeItem = UnityEngine.Object.Instantiate<KTreeItem>(this.treeItemPrefab);
    ktreeItem.text = userItem.text;
    ktreeItem.userData = userItem.userData;
    ktreeItem.onOpenChanged += new KTreeItem.StateChanged(this.OnOpenChanged);
    ktreeItem.onCheckChanged += new KTreeItem.StateChanged(this.OnCheckChanged);
    if (userItem.children != null)
    {
      for (int index = 0; index < userItem.children.Count; ++index)
      {
        KTreeItem child = this.CreateItem(userItem.children[index]);
        ktreeItem.AddChild(child);
      }
    }
    return ktreeItem;
  }

  private void OnOpenChanged(KTreeItem item, bool value)
  {
  }

  private void OnCheckChanged(KTreeItem item, bool isChecked)
  {
    if ((UnityEngine.Object) item.parent != (UnityEngine.Object) null)
    {
      bool isChecked1 = true;
      foreach (KTreeItem child in (IEnumerable<KTreeItem>) item.parent.children)
      {
        if (!child.checkboxChecked)
        {
          isChecked1 = false;
          break;
        }
      }
      item.parent.checkboxChecked = isChecked1;
      this.ChangeChecks(item.parent, isChecked1);
    }
    if (item.children == null)
      return;
    foreach (KTreeItem child in (IEnumerable<KTreeItem>) item.children)
    {
      child.checkboxChecked = isChecked;
      this.OnCheckChanged(child, isChecked);
    }
  }

  private void ChangeChecks(KTreeItem item, bool isChecked)
  {
    if (!((UnityEngine.Object) item.parent != (UnityEngine.Object) null))
      return;
    bool isChecked1 = true;
    foreach (KTreeItem child in (IEnumerable<KTreeItem>) item.parent.children)
    {
      if (!child.checkboxChecked)
      {
        isChecked1 = false;
        break;
      }
    }
    item.parent.checkboxChecked = isChecked1;
    this.ChangeChecks(item.parent, isChecked1);
  }

  public class UserItem
  {
    public string text;
    public object userData;
    public IList<KTreeControl.UserItem> children;
  }
}
