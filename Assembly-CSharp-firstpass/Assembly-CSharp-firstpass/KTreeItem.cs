// Decompiled with JetBrains decompiler
// Type: KTreeItem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KTreeItem : MonoBehaviour
{
  [SerializeField]
  private bool childrenVisible;
  [SerializeField]
  private bool checkVisible;
  [SerializeField]
  private bool isChecked;
  [SerializeField]
  private Sprite spriteOpen;
  [SerializeField]
  private Sprite spriteClosed;
  [SerializeField]
  private Image openedImage;
  [SerializeField]
  private Text label;
  [SerializeField]
  private Toggle checkbox;
  [SerializeField]
  private GameObject childrenRoot;
  [NonSerialized]
  public object userData;
  private List<KTreeItem> childItems = new List<KTreeItem>();
  [NonSerialized]
  public KTreeItem parent;

  public event KTreeItem.StateChanged onOpenChanged;

  public event KTreeItem.StateChanged onCheckChanged;

  public string text
  {
    get => this.label.text;
    set
    {
      this.name = value;
      this.label.text = value;
    }
  }

  public bool checkboxEnabled
  {
    get => this.checkbox.gameObject.activeSelf;
    set => this.checkbox.gameObject.SetActive(value);
  }

  public bool checkboxChecked
  {
    get => this.checkbox.isOn;
    set => this.checkbox.isOn = value;
  }

  public bool opened
  {
    get => this.childrenVisible;
    set
    {
      this.childrenVisible = value;
      this.UpdateOpened();
    }
  }

  public IList<KTreeItem> children => (IList<KTreeItem>) this.childItems;

  private void Awake()
  {
    this.UpdateOpened();
    this.SetImageAlpha(0.0f);
  }

  private void SetImageAlpha(float a)
  {
    Color color = this.openedImage.color;
    color.a = a;
    this.openedImage.color = color;
  }

  public void AddChild(KTreeItem child)
  {
    this.childItems.Add(child);
    child.transform.SetParent(this.childrenRoot.transform, false);
    child.parent = this;
    this.SetImageAlpha(1f);
  }

  public void RemoveChild(KTreeItem child)
  {
    this.childItems.Remove(child);
    if (this.childItems.Count != 0)
      return;
    this.SetImageAlpha(0.0f);
  }

  public void ToggleOpened()
  {
    this.opened = !this.opened;
    this.UpdateOpened();
    if (this.onOpenChanged == null)
      return;
    this.onOpenChanged(this, this.opened);
  }

  public void ToggleChecked()
  {
    if (this.onCheckChanged == null)
      return;
    this.onCheckChanged(this, this.checkboxChecked);
  }

  private void UpdateOpened()
  {
    this.openedImage.sprite = this.opened ? this.spriteOpen : this.spriteClosed;
    this.childrenRoot.SetActive(this.opened);
  }

  public delegate void StateChanged(KTreeItem item, bool value);
}
