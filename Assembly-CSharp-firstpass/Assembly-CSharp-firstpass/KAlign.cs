// Decompiled with JetBrains decompiler
// Type: KAlign
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
public class KAlign : MonoBehaviour
{
  public GameObject target;
  public KAlign.SourceLeftRight sourceHorizontal;
  public KAlign.SourceTopBottom sourceVertical;
  public KAlign.TargetLeftRight targetHorizontal;
  public KAlign.TargetTopBottom targetVertical;
  public Vector2 offset;

  public void SetTarget(GameObject newtarget)
  {
    this.target = newtarget;
    this.Update();
  }

  private void OnEnable() => this.Update();

  private void Update()
  {
    if (!((Object) this.target != (Object) null))
      return;
    RectTransform rectTransform = this.target.rectTransform();
    if (!((Object) rectTransform != (Object) null))
      return;
    Vector3[] fourCornersArray1 = new Vector3[4];
    rectTransform.GetWorldCorners(fourCornersArray1);
    Vector3 vector3_1 = fourCornersArray1[1];
    Vector3 vector3_2 = fourCornersArray1[3];
    Vector3 position = this.transform.GetPosition();
    Vector3[] fourCornersArray2 = new Vector3[4];
    this.rectTransform().GetWorldCorners(fourCornersArray2);
    Vector3 vector3_3 = fourCornersArray2[1];
    Vector3 vector3_4 = fourCornersArray2[3];
    float x = position.x;
    float y = position.y;
    if (this.targetHorizontal != KAlign.TargetLeftRight.None)
    {
      x = this.offset.x;
      if (this.sourceHorizontal == KAlign.SourceLeftRight.Left)
        x += position.x - vector3_3.x;
      else if (this.sourceHorizontal == KAlign.SourceLeftRight.Right)
        x += position.x - vector3_4.x;
      else if (this.sourceHorizontal == KAlign.SourceLeftRight.Middle)
        x += (float) ((double) vector3_3.x - (double) position.x + ((double) vector3_4.x - (double) vector3_3.x) / 2.0);
      if (this.targetHorizontal == KAlign.TargetLeftRight.Right)
        x += vector3_2.x;
      else if (this.targetHorizontal == KAlign.TargetLeftRight.Left)
        x += vector3_1.x;
      else if (this.targetHorizontal == KAlign.TargetLeftRight.Middle)
        x += vector3_1.x + (float) (((double) vector3_2.x - (double) vector3_1.x) / 2.0);
    }
    if (this.targetVertical != KAlign.TargetTopBottom.None)
    {
      y = this.offset.y;
      if (this.sourceVertical == KAlign.SourceTopBottom.Top)
        y += position.y - vector3_3.y;
      else if (this.sourceVertical == KAlign.SourceTopBottom.Bottom)
        y += position.y - vector3_4.y;
      else if (this.sourceVertical == KAlign.SourceTopBottom.Middle)
        y += (float) ((double) position.y - (double) vector3_3.y + ((double) vector3_3.y - (double) vector3_4.y) / 2.0);
      if (this.targetVertical == KAlign.TargetTopBottom.Top)
        y += vector3_1.y;
      else if (this.targetVertical == KAlign.TargetTopBottom.Bottom)
        y += vector3_2.y;
      else if (this.targetVertical == KAlign.TargetTopBottom.Middle)
        y += vector3_2.y + (float) (((double) vector3_1.y - (double) vector3_2.y) / 2.0);
    }
    position.x = x;
    position.y = y;
    this.transform.SetPosition(position);
  }

  public enum TargetLeftRight
  {
    None,
    Left,
    Middle,
    Right,
  }

  public enum TargetTopBottom
  {
    None,
    Top,
    Middle,
    Bottom,
  }

  public enum SourceLeftRight
  {
    Left,
    Middle,
    Right,
  }

  public enum SourceTopBottom
  {
    Top,
    Middle,
    Bottom,
  }
}
