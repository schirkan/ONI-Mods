// Decompiled with JetBrains decompiler
// Type: GenericMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class GenericMessage : Message
{
  [Serialize]
  private string title;
  [Serialize]
  private string tooltip;
  [Serialize]
  private string body;
  [Serialize]
  private Ref<KMonoBehaviour> clickFocus = new Ref<KMonoBehaviour>();

  public GenericMessage(string _title, string _body, string _tooltip, KMonoBehaviour click_focus = null)
  {
    this.title = _title;
    this.body = _body;
    this.tooltip = _tooltip;
    this.clickFocus.Set(click_focus);
  }

  public GenericMessage()
  {
  }

  public override string GetSound() => (string) null;

  public override string GetMessageBody() => this.body;

  public override string GetTooltip() => this.tooltip;

  public override string GetTitle() => this.title;

  public override void OnClick()
  {
    KMonoBehaviour kmonoBehaviour = this.clickFocus.Get();
    if ((Object) kmonoBehaviour == (Object) null)
      return;
    Transform transform = kmonoBehaviour.transform;
    if ((Object) transform == (Object) null)
      return;
    Vector3 position = transform.GetPosition();
    position.z = -40f;
    CameraController.Instance.SetTargetPos(position, 8f, true);
    if (!((Object) transform.GetComponent<KSelectable>() != (Object) null))
      return;
    SelectTool.Instance.Select(transform.GetComponent<KSelectable>());
  }
}
