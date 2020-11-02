// Decompiled with JetBrains decompiler
// Type: DebugExtension
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using UnityEngine;

public static class DebugExtension
{
  public static void DebugPoint(
    Vector3 position,
    Color color,
    float scale = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    color = color == new Color() ? Color.white : color;
  }

  public static void DebugPoint(Vector3 position, float scale = 1f, float duration = 0.0f, bool depthTest = true) => DebugExtension.DebugPoint(position, Color.white, scale, duration, depthTest);

  public static void DebugBounds(Bounds bounds, Color color, float duration = 0.0f, bool depthTest = true)
  {
    double x = (double) bounds.extents.x;
    float y = bounds.extents.y;
    float z = bounds.extents.z;
    double num1 = (double) y;
    double num2 = (double) z;
    Vector3 center = bounds.center;
    Color color1 = color;
    double num3 = (double) duration;
    int num4 = depthTest ? 1 : 0;
    DebugExtension.DebugExtense((float) x, (float) num1, (float) num2, center, color1, (float) num3, num4 != 0);
  }

  public static void DebugExtense(
    float x,
    float y,
    float z,
    Vector3 center,
    Color color,
    float duration = 0.0f,
    bool depthTest = true)
  {
    Vector3 vector3_1 = center + new Vector3(x, y, z);
    Vector3 vector3_2 = center + new Vector3(x, y, -z);
    Vector3 vector3_3 = center + new Vector3(-x, y, z);
    Vector3 vector3_4 = center + new Vector3(-x, y, -z);
    Vector3 vector3_5 = center + new Vector3(x, -y, z);
    Vector3 vector3_6 = center + new Vector3(x, -y, -z);
    Vector3 vector3_7 = center + new Vector3(-x, -y, z);
    Vector3 vector3_8 = center + new Vector3(-x, -y, -z);
  }

  public static void DebugAABB(AABB3 bounds, Color color, float duration = 0.0f, bool depthTest = true)
  {
    Vector3 range = bounds.Range;
    DebugExtension.DebugExtense(range.x, range.y, range.z, bounds.Center, color, duration, depthTest);
  }

  public static void DebugAABB(
    Vector3 position,
    AABB3 bounds,
    Color color,
    float duration = 0.0f,
    bool depthTest = true)
  {
    Vector3 vector3 = bounds.Range * 0.5f;
    DebugExtension.DebugExtense(vector3.x, vector3.y, vector3.z, position, color, duration, depthTest);
  }

  public static void DebugRect(Rect rect, Color color, float duration = 0.0f, bool depthTest = true)
  {
  }

  public static void DebugBounds(Bounds bounds, float duration = 0.0f, bool depthTest = true) => DebugExtension.DebugBounds(bounds, Color.white, duration, depthTest);

  public static void DebugLocalCube(
    Transform transform,
    Vector3 size,
    Color color,
    Vector3 center = default (Vector3),
    float duration = 0.0f,
    bool depthTest = true)
  {
    transform.TransformPoint(center + -size * 0.5f);
    transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
    transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
    transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
    transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
    transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
    transform.TransformPoint(center + size * 0.5f);
    transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
  }

  public static void DebugLocalCube(
    Transform transform,
    Vector3 size,
    Vector3 center = default (Vector3),
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugLocalCube(transform, size, Color.white, center, duration, depthTest);
  }

  public static void DebugLocalCube(
    Matrix4x4 space,
    Vector3 size,
    Color color,
    Vector3 center = default (Vector3),
    float duration = 0.0f,
    bool depthTest = true)
  {
    color = color == new Color() ? Color.white : color;
    space.MultiplyPoint3x4(center + -size * 0.5f);
    space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
    space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
    space.MultiplyPoint3x4(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
    space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
    space.MultiplyPoint3x4(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
    space.MultiplyPoint3x4(center + size * 0.5f);
    space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
  }

  public static void DebugLocalCube(
    Matrix4x4 space,
    Vector3 size,
    Vector3 center = default (Vector3),
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugLocalCube(space, size, Color.white, center, duration, depthTest);
  }

  public static void DebugCircle(
    Vector3 position,
    Vector3 up,
    Color color,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true,
    float jumpPerSegment = 4f)
  {
    Vector3 vector3_1 = up.normalized * radius;
    Vector3 rhs = Vector3.Slerp(vector3_1, -vector3_1, 0.5f);
    Vector3 vector3_2 = Vector3.Cross(vector3_1, rhs).normalized * radius;
    Matrix4x4 matrix4x4 = new Matrix4x4();
    matrix4x4[0] = vector3_2.x;
    matrix4x4[1] = vector3_2.y;
    matrix4x4[2] = vector3_2.z;
    matrix4x4[4] = vector3_1.x;
    matrix4x4[5] = vector3_1.y;
    matrix4x4[6] = vector3_1.z;
    matrix4x4[8] = rhs.x;
    matrix4x4[9] = rhs.y;
    matrix4x4[10] = rhs.z;
    Vector3 vector3_3 = position + matrix4x4.MultiplyPoint3x4(new Vector3(Mathf.Cos(0.0f), 0.0f, Mathf.Sin(0.0f)));
    Vector3 point = Vector3.zero;
    color = color == new Color() ? Color.white : color;
    for (int index = 0; (double) index < 364.0 / (double) jumpPerSegment; ++index)
    {
      point.x = Mathf.Cos((float) ((double) index * (double) jumpPerSegment * (Math.PI / 180.0)));
      point.z = Mathf.Sin((float) ((double) index * (double) jumpPerSegment * (Math.PI / 180.0)));
      point.y = 0.0f;
      point = position + matrix4x4.MultiplyPoint3x4(point);
    }
  }

  public static void DebugCircle(
    Vector3 position,
    Color color,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugCircle(position, Vector3.up, color, radius, duration, depthTest);
  }

  public static void DebugCircle2d(
    Vector2 position,
    Color color,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true,
    float jumpPerSegment = 4f)
  {
    DebugExtension.DebugCircle((Vector3) position, Vector3.forward, color, radius, duration, depthTest, jumpPerSegment);
  }

  public static void DebugCircle(
    Vector3 position,
    Vector3 up,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugCircle(position, up, Color.white, radius, duration, depthTest);
  }

  public static void DebugCircle(Vector3 position, float radius = 1f, float duration = 0.0f, bool depthTest = true) => DebugExtension.DebugCircle(position, Vector3.up, Color.white, radius, duration, depthTest);

  public static void DebugWireSphere(
    Vector3 position,
    Color color,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    float num = 10f;
    Vector3 vector3_1 = new Vector3(position.x, position.y + radius * Mathf.Sin(0.0f), position.z + radius * Mathf.Cos(0.0f));
    Vector3 vector3_2 = new Vector3(position.x + radius * Mathf.Cos(0.0f), position.y, position.z + radius * Mathf.Sin(0.0f));
    Vector3 vector3_3 = new Vector3(position.x + radius * Mathf.Cos(0.0f), position.y + radius * Mathf.Sin(0.0f), position.z);
    for (int index = 1; index < 37; ++index)
    {
      Vector3 vector3_4 = new Vector3(position.x, position.y + radius * Mathf.Sin((float) ((double) num * (double) index * (Math.PI / 180.0))), position.z + radius * Mathf.Cos((float) ((double) num * (double) index * (Math.PI / 180.0))));
      Vector3 vector3_5 = new Vector3(position.x + radius * Mathf.Cos((float) ((double) num * (double) index * (Math.PI / 180.0))), position.y, position.z + radius * Mathf.Sin((float) ((double) num * (double) index * (Math.PI / 180.0))));
      Vector3 vector3_6 = new Vector3(position.x + radius * Mathf.Cos((float) ((double) num * (double) index * (Math.PI / 180.0))), position.y + radius * Mathf.Sin((float) ((double) num * (double) index * (Math.PI / 180.0))), position.z);
    }
  }

  public static void DebugWireSphere(
    Vector3 position,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugWireSphere(position, Color.white, radius, duration, depthTest);
  }

  public static void DebugCylinder(
    Vector3 start,
    Vector3 end,
    Color color,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    Vector3 vector3_1 = (end - start).normalized * radius;
    Vector3 rhs = Vector3.Slerp(vector3_1, -vector3_1, 0.5f);
    Vector3 vector3_2 = Vector3.Cross(vector3_1, rhs).normalized * radius;
    DebugExtension.DebugCircle(start, vector3_1, color, radius, duration, depthTest);
    DebugExtension.DebugCircle(end, -vector3_1, color, radius, duration, depthTest);
    DebugExtension.DebugCircle((start + end) * 0.5f, vector3_1, color, radius, duration, depthTest);
  }

  public static void DebugCylinder(
    Vector3 start,
    Vector3 end,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugCylinder(start, end, Color.white, radius, duration, depthTest);
  }

  public static void DebugCone(
    Vector3 position,
    Vector3 direction,
    Color color,
    float angle = 45f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    float magnitude = direction.magnitude;
    Vector3 vector3_1 = direction;
    Vector3 vector3_2 = Vector3.Slerp(vector3_1, -vector3_1, 0.5f);
    Vector3 vector3_3 = Vector3.Cross(vector3_1, vector3_2).normalized * magnitude;
    direction = direction.normalized;
    Vector3 direction1 = Vector3.Slerp(vector3_1, vector3_2, angle / 90f);
    float enter;
    new Plane(-direction, position + vector3_1).Raycast(new Ray(position, direction1), out enter);
    DebugExtension.DebugCircle(position + vector3_1, direction, color, (vector3_1 - direction1.normalized * enter).magnitude, duration, depthTest);
    DebugExtension.DebugCircle(position + vector3_1 * 0.5f, direction, color, (vector3_1 * 0.5f - direction1.normalized * (enter * 0.5f)).magnitude, duration, depthTest);
  }

  public static void DebugCone(
    Vector3 position,
    Vector3 direction,
    float angle = 45f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugCone(position, direction, Color.white, angle, duration, depthTest);
  }

  public static void DebugCone(
    Vector3 position,
    Color color,
    float angle = 45f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugCone(position, Vector3.up, color, angle, duration, depthTest);
  }

  public static void DebugCone(Vector3 position, float angle = 45f, float duration = 0.0f, bool depthTest = true) => DebugExtension.DebugCone(position, Vector3.up, Color.white, angle, duration, depthTest);

  public static void DebugArrow(
    Vector3 position,
    Vector3 direction,
    Color color,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugCone(position + direction, -direction * 0.333f, color, 15f, duration, depthTest);
  }

  public static void DebugArrow(
    Vector3 position,
    Vector3 direction,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugArrow(position, direction, Color.white, duration, depthTest);
  }

  public static void DebugCapsule(
    Vector3 start,
    Vector3 end,
    Color color,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    Vector3 vector3_1 = (end - start).normalized * radius;
    Vector3 rhs = Vector3.Slerp(vector3_1, -vector3_1, 0.5f);
    Vector3 vector3_2 = Vector3.Cross(vector3_1, rhs).normalized * radius;
    float num1 = Mathf.Max(0.0f, (start - end).magnitude * 0.5f - radius);
    Vector3 vector3_3 = (end + start) * 0.5f;
    start = vector3_3 + (start - vector3_3).normalized * num1;
    end = vector3_3 + (end - vector3_3).normalized * num1;
    DebugExtension.DebugCircle(start, vector3_1, color, radius, duration, depthTest);
    DebugExtension.DebugCircle(end, -vector3_1, color, radius, duration, depthTest);
    int num2 = 1;
    while (num2 < 26)
      ++num2;
  }

  public static void DebugCapsule(
    Vector3 start,
    Vector3 end,
    float radius = 1f,
    float duration = 0.0f,
    bool depthTest = true)
  {
    DebugExtension.DebugCapsule(start, end, Color.white, radius, duration, depthTest);
  }

  public static void DrawPoint(Vector3 position, Color color, float scale = 1f)
  {
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    Gizmos.DrawRay(position + Vector3.up * (scale * 0.5f), -Vector3.up * scale);
    Gizmos.DrawRay(position + Vector3.right * (scale * 0.5f), -Vector3.right * scale);
    Gizmos.DrawRay(position + Vector3.forward * (scale * 0.5f), -Vector3.forward * scale);
    Gizmos.color = color1;
  }

  public static void DrawPoint(Vector3 position, float scale = 1f) => DebugExtension.DrawPoint(position, Color.white, scale);

  public static void DrawBounds(Bounds bounds, Color color)
  {
    Vector3 center = bounds.center;
    float x = bounds.extents.x;
    float y = bounds.extents.y;
    float z = bounds.extents.z;
    Vector3 from = center + new Vector3(x, y, z);
    Vector3 vector3_1 = center + new Vector3(x, y, -z);
    Vector3 vector3_2 = center + new Vector3(-x, y, z);
    Vector3 vector3_3 = center + new Vector3(-x, y, -z);
    Vector3 vector3_4 = center + new Vector3(x, -y, z);
    Vector3 to = center + new Vector3(x, -y, -z);
    Vector3 vector3_5 = center + new Vector3(-x, -y, z);
    Vector3 vector3_6 = center + new Vector3(-x, -y, -z);
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    Gizmos.DrawLine(from, vector3_2);
    Gizmos.DrawLine(from, vector3_1);
    Gizmos.DrawLine(vector3_2, vector3_3);
    Gizmos.DrawLine(vector3_1, vector3_3);
    Gizmos.DrawLine(from, vector3_4);
    Gizmos.DrawLine(vector3_1, to);
    Gizmos.DrawLine(vector3_2, vector3_5);
    Gizmos.DrawLine(vector3_3, vector3_6);
    Gizmos.DrawLine(vector3_4, vector3_5);
    Gizmos.DrawLine(vector3_4, to);
    Gizmos.DrawLine(vector3_5, vector3_6);
    Gizmos.DrawLine(vector3_6, to);
    Gizmos.color = color1;
  }

  public static void DrawBounds(Bounds bounds) => DebugExtension.DrawBounds(bounds, Color.white);

  public static void DrawLocalCube(Transform transform, Vector3 size, Color color, Vector3 center = default (Vector3))
  {
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    Vector3 vector3_1 = transform.TransformPoint(center + -size * 0.5f);
    Vector3 vector3_2 = transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
    Vector3 vector3_3 = transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
    Vector3 vector3_4 = transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
    Vector3 vector3_5 = transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
    Vector3 vector3_6 = transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
    Vector3 vector3_7 = transform.TransformPoint(center + size * 0.5f);
    Vector3 vector3_8 = transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
    Gizmos.DrawLine(vector3_1, vector3_2);
    Gizmos.DrawLine(vector3_2, vector3_3);
    Gizmos.DrawLine(vector3_3, vector3_4);
    Gizmos.DrawLine(vector3_4, vector3_1);
    Gizmos.DrawLine(vector3_5, vector3_6);
    Gizmos.DrawLine(vector3_6, vector3_7);
    Gizmos.DrawLine(vector3_7, vector3_8);
    Gizmos.DrawLine(vector3_8, vector3_5);
    Gizmos.DrawLine(vector3_1, vector3_5);
    Gizmos.DrawLine(vector3_2, vector3_6);
    Gizmos.DrawLine(vector3_3, vector3_7);
    Gizmos.DrawLine(vector3_4, vector3_8);
    Gizmos.color = color1;
  }

  public static void DrawLocalCube(Transform transform, Vector3 size, Vector3 center = default (Vector3)) => DebugExtension.DrawLocalCube(transform, size, Color.white, center);

  public static void DrawLocalCube(Matrix4x4 space, Vector3 size, Color color, Vector3 center = default (Vector3))
  {
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    Vector3 vector3_1 = space.MultiplyPoint3x4(center + -size * 0.5f);
    Vector3 vector3_2 = space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
    Vector3 vector3_3 = space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
    Vector3 vector3_4 = space.MultiplyPoint3x4(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
    Vector3 vector3_5 = space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
    Vector3 vector3_6 = space.MultiplyPoint3x4(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
    Vector3 vector3_7 = space.MultiplyPoint3x4(center + size * 0.5f);
    Vector3 vector3_8 = space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
    Gizmos.DrawLine(vector3_1, vector3_2);
    Gizmos.DrawLine(vector3_2, vector3_3);
    Gizmos.DrawLine(vector3_3, vector3_4);
    Gizmos.DrawLine(vector3_4, vector3_1);
    Gizmos.DrawLine(vector3_5, vector3_6);
    Gizmos.DrawLine(vector3_6, vector3_7);
    Gizmos.DrawLine(vector3_7, vector3_8);
    Gizmos.DrawLine(vector3_8, vector3_5);
    Gizmos.DrawLine(vector3_1, vector3_5);
    Gizmos.DrawLine(vector3_2, vector3_6);
    Gizmos.DrawLine(vector3_3, vector3_7);
    Gizmos.DrawLine(vector3_4, vector3_8);
    Gizmos.color = color1;
  }

  public static void DrawLocalCube(Matrix4x4 space, Vector3 size, Vector3 center = default (Vector3)) => DebugExtension.DrawLocalCube(space, size, Color.white, center);

  public static void DrawCircle(Vector3 position, Vector3 up, Color color, float radius = 1f)
  {
    up = (up == Vector3.zero ? Vector3.up : up).normalized * radius;
    Vector3 rhs = Vector3.Slerp(up, -up, 0.5f);
    Vector3 vector3_1 = Vector3.Cross(up, rhs).normalized * radius;
    Matrix4x4 matrix4x4 = new Matrix4x4();
    matrix4x4[0] = vector3_1.x;
    matrix4x4[1] = vector3_1.y;
    matrix4x4[2] = vector3_1.z;
    matrix4x4[4] = up.x;
    matrix4x4[5] = up.y;
    matrix4x4[6] = up.z;
    matrix4x4[8] = rhs.x;
    matrix4x4[9] = rhs.y;
    matrix4x4[10] = rhs.z;
    Vector3 from = position + matrix4x4.MultiplyPoint3x4(new Vector3(Mathf.Cos(0.0f), 0.0f, Mathf.Sin(0.0f)));
    Vector3 vector3_2 = Vector3.zero;
    Color color1 = Gizmos.color;
    Gizmos.color = color == new Color() ? Color.white : color;
    for (int index = 0; index < 91; ++index)
    {
      vector3_2.x = Mathf.Cos((float) (index * 4) * ((float) Math.PI / 180f));
      vector3_2.z = Mathf.Sin((float) (index * 4) * ((float) Math.PI / 180f));
      vector3_2.y = 0.0f;
      vector3_2 = position + matrix4x4.MultiplyPoint3x4(vector3_2);
      Gizmos.DrawLine(from, vector3_2);
      from = vector3_2;
    }
    Gizmos.color = color1;
  }

  public static void DrawCircleNoGizmo(Vector3 position, Vector3 up, Color color, float radius = 1f)
  {
    up = (up == Vector3.zero ? Vector3.up : up).normalized * radius;
    Vector3 rhs = Vector3.Slerp(up, -up, 0.5f);
    Vector3 vector3_1 = Vector3.Cross(up, rhs).normalized * radius;
    Matrix4x4 matrix4x4 = new Matrix4x4();
    matrix4x4[0] = vector3_1.x;
    matrix4x4[1] = vector3_1.y;
    matrix4x4[2] = vector3_1.z;
    matrix4x4[4] = up.x;
    matrix4x4[5] = up.y;
    matrix4x4[6] = up.z;
    matrix4x4[8] = rhs.x;
    matrix4x4[9] = rhs.y;
    matrix4x4[10] = rhs.z;
    Vector3 vector3_2 = position + matrix4x4.MultiplyPoint3x4(new Vector3(Mathf.Cos(0.0f), 0.0f, Mathf.Sin(0.0f)));
    Vector3 point = Vector3.zero;
    for (int index = 0; index < 91; ++index)
    {
      point.x = Mathf.Cos((float) (index * 4) * ((float) Math.PI / 180f));
      point.z = Mathf.Sin((float) (index * 4) * ((float) Math.PI / 180f));
      point.y = 0.0f;
      point = position + matrix4x4.MultiplyPoint3x4(point);
    }
  }

  public static void DrawCircle(Vector3 position, Color color, float radius = 1f) => DebugExtension.DrawCircle(position, Vector3.up, color, radius);

  public static void DrawCircleNoGizmo(Vector2 position, Color color, float radius = 1f) => DebugExtension.DrawCircleNoGizmo((Vector3) position, Vector3.forward, color, radius);

  public static void DrawCircle(Vector3 position, Vector3 up, float radius = 1f) => DebugExtension.DrawCircle(position, position, Color.white, radius);

  public static void DrawCircle(Vector3 position, float radius = 1f) => DebugExtension.DrawCircle(position, Vector3.up, Color.white, radius);

  public static void DrawCylinder(Vector3 start, Vector3 end, Color color, float radius = 1f)
  {
    Vector3 vector3_1 = (end - start).normalized * radius;
    Vector3 rhs = Vector3.Slerp(vector3_1, -vector3_1, 0.5f);
    Vector3 vector3_2 = Vector3.Cross(vector3_1, rhs).normalized * radius;
    DebugExtension.DrawCircle(start, vector3_1, color, radius);
    DebugExtension.DrawCircle(end, -vector3_1, color, radius);
    DebugExtension.DrawCircle((start + end) * 0.5f, vector3_1, color, radius);
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    Gizmos.DrawLine(start + vector3_2, end + vector3_2);
    Gizmos.DrawLine(start - vector3_2, end - vector3_2);
    Gizmos.DrawLine(start + rhs, end + rhs);
    Gizmos.DrawLine(start - rhs, end - rhs);
    Gizmos.DrawLine(start - vector3_2, start + vector3_2);
    Gizmos.DrawLine(start - rhs, start + rhs);
    Gizmos.DrawLine(end - vector3_2, end + vector3_2);
    Gizmos.DrawLine(end - rhs, end + rhs);
    Gizmos.color = color1;
  }

  public static void DrawCylinder(Vector3 start, Vector3 end, float radius = 1f) => DebugExtension.DrawCylinder(start, end, Color.white, radius);

  public static void DrawCone(Vector3 position, Vector3 direction, Color color, float angle = 45f)
  {
    float magnitude = direction.magnitude;
    Vector3 vector3_1 = direction;
    Vector3 vector3_2 = Vector3.Slerp(vector3_1, -vector3_1, 0.5f);
    Vector3 b = Vector3.Cross(vector3_1, vector3_2).normalized * magnitude;
    direction = direction.normalized;
    Vector3 direction1 = Vector3.Slerp(vector3_1, vector3_2, angle / 90f);
    float enter;
    new Plane(-direction, position + vector3_1).Raycast(new Ray(position, direction1), out enter);
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    Gizmos.DrawRay(position, direction1.normalized * enter);
    Gizmos.DrawRay(position, Vector3.Slerp(vector3_1, -vector3_2, angle / 90f).normalized * enter);
    Gizmos.DrawRay(position, Vector3.Slerp(vector3_1, b, angle / 90f).normalized * enter);
    Gizmos.DrawRay(position, Vector3.Slerp(vector3_1, -b, angle / 90f).normalized * enter);
    DebugExtension.DrawCircle(position + vector3_1, direction, color, (vector3_1 - direction1.normalized * enter).magnitude);
    DebugExtension.DrawCircle(position + vector3_1 * 0.5f, direction, color, (vector3_1 * 0.5f - direction1.normalized * (enter * 0.5f)).magnitude);
    Gizmos.color = color1;
  }

  public static void DrawCone(Vector3 position, Vector3 direction, float angle = 45f) => DebugExtension.DrawCone(position, direction, Color.white, angle);

  public static void DrawCone(Vector3 position, Color color, float angle = 45f) => DebugExtension.DrawCone(position, Vector3.up, color, angle);

  public static void DrawCone(Vector3 position, float angle = 45f) => DebugExtension.DrawCone(position, Vector3.up, Color.white, angle);

  public static void DrawArrow(Vector3 position, Vector3 direction, Color color)
  {
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    Gizmos.DrawRay(position, direction);
    DebugExtension.DrawCone(position + direction, -direction * 0.333f, color, 15f);
    Gizmos.color = color1;
  }

  public static void DrawArrow(Vector3 position, Vector3 direction) => DebugExtension.DrawArrow(position, direction, Color.white);

  public static void DrawCapsule(Vector3 start, Vector3 end, Color color, float radius = 1f)
  {
    Vector3 vector3_1 = (end - start).normalized * radius;
    Vector3 vector3_2 = Vector3.Slerp(vector3_1, -vector3_1, 0.5f);
    Vector3 a = Vector3.Cross(vector3_1, vector3_2).normalized * radius;
    Color color1 = Gizmos.color;
    Gizmos.color = color;
    float num = Mathf.Max(0.0f, (start - end).magnitude * 0.5f - radius);
    Vector3 vector3_3 = (end + start) * 0.5f;
    start = vector3_3 + (start - vector3_3).normalized * num;
    end = vector3_3 + (end - vector3_3).normalized * num;
    DebugExtension.DrawCircle(start, vector3_1, color, radius);
    DebugExtension.DrawCircle(end, -vector3_1, color, radius);
    Gizmos.DrawLine(start + a, end + a);
    Gizmos.DrawLine(start - a, end - a);
    Gizmos.DrawLine(start + vector3_2, end + vector3_2);
    Gizmos.DrawLine(start - vector3_2, end - vector3_2);
    for (int index = 1; index < 26; ++index)
    {
      Gizmos.DrawLine(Vector3.Slerp(a, -vector3_1, (float) index / 25f) + start, Vector3.Slerp(a, -vector3_1, (float) (index - 1) / 25f) + start);
      Gizmos.DrawLine(Vector3.Slerp(-a, -vector3_1, (float) index / 25f) + start, Vector3.Slerp(-a, -vector3_1, (float) (index - 1) / 25f) + start);
      Gizmos.DrawLine(Vector3.Slerp(vector3_2, -vector3_1, (float) index / 25f) + start, Vector3.Slerp(vector3_2, -vector3_1, (float) (index - 1) / 25f) + start);
      Gizmos.DrawLine(Vector3.Slerp(-vector3_2, -vector3_1, (float) index / 25f) + start, Vector3.Slerp(-vector3_2, -vector3_1, (float) (index - 1) / 25f) + start);
      Gizmos.DrawLine(Vector3.Slerp(a, vector3_1, (float) index / 25f) + end, Vector3.Slerp(a, vector3_1, (float) (index - 1) / 25f) + end);
      Gizmos.DrawLine(Vector3.Slerp(-a, vector3_1, (float) index / 25f) + end, Vector3.Slerp(-a, vector3_1, (float) (index - 1) / 25f) + end);
      Gizmos.DrawLine(Vector3.Slerp(vector3_2, vector3_1, (float) index / 25f) + end, Vector3.Slerp(vector3_2, vector3_1, (float) (index - 1) / 25f) + end);
      Gizmos.DrawLine(Vector3.Slerp(-vector3_2, vector3_1, (float) index / 25f) + end, Vector3.Slerp(-vector3_2, vector3_1, (float) (index - 1) / 25f) + end);
    }
    Gizmos.color = color1;
  }

  public static void DrawCapsule(Vector3 start, Vector3 end, float radius = 1f) => DebugExtension.DrawCapsule(start, end, Color.white, radius);

  public static string MethodsOfObject(object obj, bool includeInfo = false)
  {
    string str = "";
    MethodInfo[] methods = obj.GetType().GetMethods();
    for (int index = 0; index < methods.Length; ++index)
      str = !includeInfo ? str + methods[index].Name + "\n" : str + (object) methods[index] + "\n";
    return str;
  }

  public static string MethodsOfType(System.Type type, bool includeInfo = false)
  {
    string str = "";
    MethodInfo[] methods = type.GetMethods();
    for (int index = 0; index < methods.Length; ++index)
      str = !includeInfo ? str + methods[index].Name + "\n" : str + (object) methods[index] + "\n";
    return str;
  }
}
