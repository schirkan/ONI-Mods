// Decompiled with JetBrains decompiler
// Type: TreeFilterableSideScreenElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenElement")]
public class TreeFilterableSideScreenElement : KMonoBehaviour
{
  [SerializeField]
  private LocText elementName;
  [SerializeField]
  private KToggle checkBox;
  [SerializeField]
  private KImage elementImg;
  private KImage checkBoxImg;
  private Tag elementTag;
  private TreeFilterableSideScreen parent;
  private bool initialized;

  public Tag GetElementTag() => this.elementTag;

  public bool IsSelected => this.checkBox.isOn;

  public event System.Action<Tag, bool> OnSelectionChanged;

  public KToggle GetCheckboxToggle() => this.checkBox;

  public TreeFilterableSideScreen Parent
  {
    get => this.parent;
    set => this.parent = value;
  }

  private void Initialize()
  {
    if (this.initialized)
      return;
    this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
    this.checkBox.onClick += new System.Action(this.CheckBoxClicked);
    this.initialized = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Initialize();
  }

  public Sprite GetStorageObjectSprite(Tag t)
  {
    Sprite sprite = (Sprite) null;
    GameObject prefab = Assets.GetPrefab(t);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0]);
    }
    return sprite;
  }

  public void SetSprite(Tag t)
  {
    Element element = ElementLoader.GetElement(t);
    Sprite sprite = element != null ? Def.GetUISpriteFromMultiObjectAnim(element.substance.anim) : this.GetStorageObjectSprite(t);
    this.elementImg.sprite = sprite;
    this.elementImg.enabled = (UnityEngine.Object) sprite != (UnityEngine.Object) null;
  }

  public void SetTag(Tag newTag)
  {
    this.Initialize();
    this.elementTag = newTag;
    string str = this.elementTag.ProperName();
    if (this.parent.IsStorage)
    {
      float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
      str = str + ": " + GameUtil.GetFormattedMass(amountInStorage);
    }
    this.elementName.text = str;
  }

  private void CheckBoxClicked() => this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));

  public void SetCheckBox(bool checkBoxState)
  {
    this.checkBox.isOn = checkBoxState;
    this.checkBoxImg.enabled = checkBoxState;
    if (this.OnSelectionChanged == null)
      return;
    this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
  }
}
