// Decompiled with JetBrains decompiler
// Type: MathUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class MathUtil
{
  public static float Clamp(float min, float max, float val) => Mathf.Max(min, Mathf.Min(max, val));

  public static int Clamp(int min, int max, int val) => Mathf.Max(min, Mathf.Min(max, val));

  public static float ReRange(float val, float in_a, float in_b, float out_a, float out_b) => (float) (((double) val - (double) in_a) / ((double) in_b - (double) in_a) * ((double) out_b - (double) out_a)) + out_a;

  public static float Wrap(float min, float max, float val)
  {
    while ((double) val < (double) min)
      val += max - min;
    while ((double) val > (double) max)
      val -= max - min;
    return val;
  }

  public static float ApproachConstant(float target, float current, float speed)
  {
    float num = target - current;
    if ((double) num > (double) speed)
      return current + speed;
    return (double) num < -(double) speed ? current - speed : target;
  }

  public static Vector3 ApproachConstant(Vector3 target, Vector3 current, float speed)
  {
    Vector3 vector3 = target - current;
    return (double) vector3.magnitude > (double) speed ? current + vector3.normalized * speed : target;
  }

  public static Vector3 Round(this Vector3 v) => new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));

  public static Vector3 Min(this Vector3 a, Vector3 b) => new Vector3(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));

  public static Vector3 Max(this Vector3 a, Vector3 b) => new Vector3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));

  public static Vector3[] RaySphereIntersection(
    Ray ray,
    Vector3 sphereCenter,
    float sphereRadius)
  {
    ray.direction.Normalize();
    Vector3 vector3 = sphereCenter - ray.origin;
    float num1 = Vector3.Dot(ray.direction, vector3);
    float num2 = Vector3.Dot(vector3, vector3);
    float f = (float) ((double) num1 * (double) num1 - (double) num2 + (double) sphereRadius * (double) sphereRadius);
    if ((double) f < 0.0)
      return new Vector3[0];
    return (double) f == 0.0 ? new Vector3[1]
    {
      num1 * ray.direction + ray.origin
    } : new Vector3[2]
    {
      (num1 - Mathf.Sqrt(f)) * ray.direction + ray.origin,
      (num1 + Mathf.Sqrt(f)) * ray.direction + ray.origin
    };
  }

  public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) => Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;

  public static float GetClosestPointBetweenPointAndLineSegment(
    MathUtil.Pair<Vector2, Vector2> segment,
    Vector2 point,
    ref float closest_point)
  {
    float num1 = (float) (((double) segment.Second.x - (double) segment.First.x) * ((double) segment.Second.x - (double) segment.First.x) + ((double) segment.Second.y - (double) segment.First.y) * ((double) segment.Second.y - (double) segment.First.y));
    if ((double) num1 <= 0.0)
    {
      closest_point = 0.0f;
      return Vector2.Distance(segment.First, point);
    }
    float num2 = (float) (((double) point.x - (double) segment.First.x) * ((double) segment.Second.x - (double) segment.First.x) + ((double) point.y - (double) segment.First.y) * ((double) segment.Second.y - (double) segment.First.y));
    closest_point = Mathf.Max(0.0f, Mathf.Min(1f, num2 / num1));
    return Vector2.Distance(segment.First + (segment.Second - segment.First) * closest_point, point);
  }

  public struct MinMax
  {
    public float min { get; private set; }

    public float max { get; private set; }

    public MinMax(float min, float max)
    {
      this.min = min;
      this.max = max;
    }

    public float Get(SeededRandom rnd) => rnd.RandomRange(this.min, this.max);

    public float Get() => Random.Range(this.min, this.max);

    public float Lerp(float t) => Mathf.Lerp(this.min, this.max, t);

    public override string ToString() => string.Format("[{0}:{1}]", (object) this.min, (object) this.max);
  }

  public class Pair<T, U>
  {
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
      this.First = first;
      this.Second = second;
    }

    public T First { get; set; }

    public U Second { get; set; }
  }
}
