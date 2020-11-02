// Decompiled with JetBrains decompiler
// Type: KSnap
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
public class KSnap : MonoBehaviour
{
  public GameObject target;
  public KSnap.LeftRight horizontal;
  public KSnap.TopBottom vertical;
  public Vector2 offset;
  [SerializeField]
  private bool keepOnScreen;
  private Vector3[] corners = new Vector3[4];

  public void SetTarget(GameObject newtarget)
  {
    this.target = newtarget;
    this.Update();
  }

  private void Update()
  {
    if (!((Object) this.target != (Object) null))
      return;
    RectTransform rectTransform = this.target.rectTransform();
    if (!((Object) rectTransform != (Object) null))
      return;
    rectTransform.GetWorldCorners(this.corners);
    Vector3 corner1 = this.corners[2];
    Vector3 corner2 = this.corners[0];
    Vector3 position = this.transform.GetPosition();
    if (this.horizontal == KSnap.LeftRight.Left)
      position.x = corner2.x + this.offset.x;
    else if (this.horizontal == KSnap.LeftRight.Right)
      position.x = corner1.x + this.offset.x;
    else if (this.horizontal == KSnap.LeftRight.Middle)
      position.x = corner1.x + (float) (((double) corner2.x - (double) corner1.x) / 2.0) + this.offset.x;
    if (this.vertical == KSnap.TopBottom.Top)
      position.y = corner1.y + this.offset.y;
    else if (this.vertical == KSnap.TopBottom.Bottom)
      position.y = corner2.y + this.offset.y;
    else if (this.vertical == KSnap.TopBottom.Middle)
      position.y = corner2.y + (float) (((double) corner1.y - (double) corner2.y) / 2.0) + this.offset.y;
    this.transform.SetPosition(position);
    this.KeepOnScreen();
  }

  private void KeepOnScreen()
  {
    if (!this.keepOnScreen)
      return;
    this.transform.rectTransform().GetWorldCorners(this.corners);
    Vector3 zero = Vector3.zero;
    foreach (Vector3 corner in this.corners)
    {
      if ((double) corner.x < 0.0)
        zero.x = Mathf.Max(zero.x, -corner.x);
      if ((double) corner.x > (double) Screen.width)
        zero.x = Mathf.Min(zero.x, (float) Screen.width - corner.x);
      if ((double) corner.y < 0.0)
        zero.y = Mathf.Max(zero.y, -corner.y);
      if ((double) corner.y > (double) Screen.height)
        zero.y = Mathf.Min(zero.y, (float) Screen.height - corner.y);
    }
    this.transform.SetPosition(this.transform.GetPosition() + zero);
  }

  public enum LeftRight
  {
    None,
    Left,
    Middle,
    Right,
  }

  public enum TopBottom
  {
    None,
    Top,
    Middle,
    Bottom,
  }
}
