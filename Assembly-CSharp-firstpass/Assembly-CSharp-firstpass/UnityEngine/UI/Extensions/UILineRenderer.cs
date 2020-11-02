// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Extensions.UILineRenderer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
  [AddComponentMenu("UI/Extensions/Primitives/UILineRenderer")]
  public class UILineRenderer : UIPrimitiveBase
  {
    private const float MIN_MITER_JOIN = 0.2617994f;
    private const float MIN_BEVEL_NICE_JOIN = 0.5235988f;
    private static readonly Vector2 UV_TOP_LEFT = Vector2.zero;
    private static readonly Vector2 UV_BOTTOM_LEFT = new Vector2(0.0f, 1f);
    private static readonly Vector2 UV_TOP_CENTER = new Vector2(0.5f, 0.0f);
    private static readonly Vector2 UV_BOTTOM_CENTER = new Vector2(0.5f, 1f);
    private static readonly Vector2 UV_TOP_RIGHT = new Vector2(1f, 0.0f);
    private static readonly Vector2 UV_BOTTOM_RIGHT = new Vector2(1f, 1f);
    private static readonly Vector2[] startUvs = new Vector2[4]
    {
      UILineRenderer.UV_TOP_LEFT,
      UILineRenderer.UV_BOTTOM_LEFT,
      UILineRenderer.UV_BOTTOM_CENTER,
      UILineRenderer.UV_TOP_CENTER
    };
    private static readonly Vector2[] middleUvs = new Vector2[4]
    {
      UILineRenderer.UV_TOP_CENTER,
      UILineRenderer.UV_BOTTOM_CENTER,
      UILineRenderer.UV_BOTTOM_CENTER,
      UILineRenderer.UV_TOP_CENTER
    };
    private static readonly Vector2[] endUvs = new Vector2[4]
    {
      UILineRenderer.UV_TOP_CENTER,
      UILineRenderer.UV_BOTTOM_CENTER,
      UILineRenderer.UV_BOTTOM_RIGHT,
      UILineRenderer.UV_TOP_RIGHT
    };
    [SerializeField]
    private Rect m_UVRect = new Rect(0.0f, 0.0f, 1f, 1f);
    [SerializeField]
    private Vector2[] m_points;
    public float LineThickness = 2f;
    public bool UseMargins;
    public Vector2 Margin;
    public bool relativeSize;
    public bool LineList;
    public bool LineCaps;
    public UILineRenderer.JoinType LineJoins;
    public UILineRenderer.BezierType BezierMode;
    public int BezierSegmentsPerCurve = 10;

    public Rect uvRect
    {
      get => this.m_UVRect;
      set
      {
        if (this.m_UVRect == value)
          return;
        this.m_UVRect = value;
        this.SetVerticesDirty();
      }
    }

    public Vector2[] Points
    {
      get => this.m_points;
      set
      {
        if (this.m_points == value)
          return;
        this.m_points = value;
        this.SetAllDirty();
      }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
      if (this.m_points == null)
        return;
      Vector2[] newControlPoints = this.m_points;
      if (this.BezierMode != UILineRenderer.BezierType.None && this.m_points.Length > 3)
      {
        BezierPath bezierPath = new BezierPath();
        bezierPath.SetControlPoints(newControlPoints);
        bezierPath.SegmentsPerCurve = this.BezierSegmentsPerCurve;
        List<Vector2> vector2List;
        switch (this.BezierMode)
        {
          case UILineRenderer.BezierType.Basic:
            vector2List = bezierPath.GetDrawingPoints0();
            break;
          case UILineRenderer.BezierType.Improved:
            vector2List = bezierPath.GetDrawingPoints1();
            break;
          default:
            vector2List = bezierPath.GetDrawingPoints2();
            break;
        }
        newControlPoints = vector2List.ToArray();
      }
      float num1 = this.rectTransform.rect.width;
      float num2 = this.rectTransform.rect.height;
      float num3 = -this.rectTransform.pivot.x * this.rectTransform.rect.width;
      float num4 = -this.rectTransform.pivot.y * this.rectTransform.rect.height;
      if (!this.relativeSize)
      {
        num1 = 1f;
        num2 = 1f;
      }
      if (this.UseMargins)
      {
        num1 -= this.Margin.x;
        num2 -= this.Margin.y;
        num3 += this.Margin.x / 2f;
        num4 += this.Margin.y / 2f;
      }
      vh.Clear();
      List<UIVertex[]> uiVertexArrayList = new List<UIVertex[]>();
      if (this.LineList)
      {
        for (int index = 1; index < newControlPoints.Length; index += 2)
        {
          Vector2 start = newControlPoints[index - 1];
          Vector2 end = newControlPoints[index];
          start = new Vector2(start.x * num1 + num3, start.y * num2 + num4);
          end = new Vector2(end.x * num1 + num3, end.y * num2 + num4);
          if (this.LineCaps)
            uiVertexArrayList.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.Start));
          uiVertexArrayList.Add(this.CreateLineSegment(start, end, UILineRenderer.SegmentType.Middle));
          if (this.LineCaps)
            uiVertexArrayList.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.End));
        }
      }
      else
      {
        for (int index = 1; index < newControlPoints.Length; ++index)
        {
          Vector2 start = newControlPoints[index - 1];
          Vector2 end = newControlPoints[index];
          start = new Vector2(start.x * num1 + num3, start.y * num2 + num4);
          end = new Vector2(end.x * num1 + num3, end.y * num2 + num4);
          if (this.LineCaps && index == 1)
            uiVertexArrayList.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.Start));
          uiVertexArrayList.Add(this.CreateLineSegment(start, end, UILineRenderer.SegmentType.Middle));
          if (this.LineCaps && index == newControlPoints.Length - 1)
            uiVertexArrayList.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.End));
        }
      }
      for (int index = 0; index < uiVertexArrayList.Count; ++index)
      {
        if (!this.LineList && index < uiVertexArrayList.Count - 1)
        {
          Vector3 vector3_1 = uiVertexArrayList[index][1].position - uiVertexArrayList[index][2].position;
          Vector3 vector3_2 = uiVertexArrayList[index + 1][2].position - uiVertexArrayList[index + 1][1].position;
          float num5 = Vector2.Angle((Vector2) vector3_1, (Vector2) vector3_2) * ((float) Math.PI / 180f);
          float num6 = Mathf.Sign(Vector3.Cross(vector3_1.normalized, vector3_2.normalized).z);
          float num7 = this.LineThickness / (2f * Mathf.Tan(num5 / 2f));
          Vector3 vector3_3 = uiVertexArrayList[index][2].position - vector3_1.normalized * num7 * num6;
          Vector3 vector3_4 = uiVertexArrayList[index][3].position + vector3_1.normalized * num7 * num6;
          UILineRenderer.JoinType joinType = this.LineJoins;
          if (joinType == UILineRenderer.JoinType.Miter)
          {
            if ((double) num7 < (double) vector3_1.magnitude / 2.0 && (double) num7 < (double) vector3_2.magnitude / 2.0 && (double) num5 > 0.261799395084381)
            {
              uiVertexArrayList[index][2].position = vector3_3;
              uiVertexArrayList[index][3].position = vector3_4;
              uiVertexArrayList[index + 1][0].position = vector3_4;
              uiVertexArrayList[index + 1][1].position = vector3_3;
            }
            else
              joinType = UILineRenderer.JoinType.Bevel;
          }
          if (joinType == UILineRenderer.JoinType.Bevel)
          {
            if ((double) num7 < (double) vector3_1.magnitude / 2.0 && (double) num7 < (double) vector3_2.magnitude / 2.0 && (double) num5 > 0.523598790168762)
            {
              if ((double) num6 < 0.0)
              {
                uiVertexArrayList[index][2].position = vector3_3;
                uiVertexArrayList[index + 1][1].position = vector3_3;
              }
              else
              {
                uiVertexArrayList[index][3].position = vector3_4;
                uiVertexArrayList[index + 1][0].position = vector3_4;
              }
            }
            UIVertex[] verts = new UIVertex[4]
            {
              uiVertexArrayList[index][2],
              uiVertexArrayList[index][3],
              uiVertexArrayList[index + 1][0],
              uiVertexArrayList[index + 1][1]
            };
            vh.AddUIVertexQuad(verts);
          }
        }
        vh.AddUIVertexQuad(uiVertexArrayList[index]);
      }
    }

    private UIVertex[] CreateLineCap(
      Vector2 start,
      Vector2 end,
      UILineRenderer.SegmentType type)
    {
      if (type == UILineRenderer.SegmentType.Start)
        return this.CreateLineSegment(start - (end - start).normalized * this.LineThickness / 2f, start, UILineRenderer.SegmentType.Start);
      if (type == UILineRenderer.SegmentType.End)
      {
        Vector2 end1 = end + (end - start).normalized * this.LineThickness / 2f;
        return this.CreateLineSegment(end, end1, UILineRenderer.SegmentType.End);
      }
      Debug.LogError((object) "Bad SegmentType passed in to CreateLineCap. Must be SegmentType.Start or SegmentType.End");
      return (UIVertex[]) null;
    }

    private UIVertex[] CreateLineSegment(
      Vector2 start,
      Vector2 end,
      UILineRenderer.SegmentType type)
    {
      Vector2[] uvs = UILineRenderer.middleUvs;
      switch (type)
      {
        case UILineRenderer.SegmentType.Start:
          uvs = UILineRenderer.startUvs;
          break;
        case UILineRenderer.SegmentType.End:
          uvs = UILineRenderer.endUvs;
          break;
      }
      Vector2 vector2 = new Vector2(start.y - end.y, end.x - start.x).normalized * this.LineThickness / 2f;
      return this.SetVbo(new Vector2[4]
      {
        start - vector2,
        start + vector2,
        end + vector2,
        end - vector2
      }, uvs);
    }

    private enum SegmentType
    {
      Start,
      Middle,
      End,
    }

    public enum JoinType
    {
      Bevel,
      Miter,
    }

    public enum BezierType
    {
      None,
      Quick,
      Basic,
      Improved,
    }
  }
}
