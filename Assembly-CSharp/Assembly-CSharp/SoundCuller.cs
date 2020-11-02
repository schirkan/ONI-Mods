// Decompiled with JetBrains decompiler
// Type: SoundCuller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct SoundCuller
{
  private Vector2 min;
  private Vector2 max;
  private Vector2 cameraPos;
  private float zoomScaler;

  public bool IsAudible(Vector2 pos) => this.min.LessEqual(pos) && pos.LessEqual(this.max);

  public bool IsAudibleNoCameraScaling(Vector2 pos, float falloff_distance_sq) => ((double) pos.x - (double) this.cameraPos.x) * ((double) pos.x - (double) this.cameraPos.x) + ((double) pos.y - (double) this.cameraPos.y) * ((double) pos.y - (double) this.cameraPos.y) < (double) falloff_distance_sq;

  public bool IsAudible(Vector2 pos, float falloff_distance_sq)
  {
    pos = (Vector2) this.GetVerticallyScaledPosition((Vector3) pos);
    return this.IsAudibleNoCameraScaling(pos, falloff_distance_sq);
  }

  public bool IsAudible(Vector2 pos, HashedString sound_path) => sound_path.IsValid && this.IsAudible(pos, KFMOD.GetSoundEventDescription(sound_path).falloffDistanceSq);

  public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
  {
    float num1 = 1f;
    float num2;
    if ((double) pos.y > (double) this.max.y)
      num2 = Mathf.Abs(pos.y - this.max.y);
    else if ((double) pos.y < (double) this.min.y)
    {
      num2 = Mathf.Abs(pos.y - this.min.y);
      num1 = -1f;
    }
    else
      num2 = 0.0f;
    float extraYrange = TuningData<SoundCuller.Tuning>.Get().extraYRange;
    float num3 = (double) num2 < (double) extraYrange ? num2 : extraYrange;
    float num4 = (float) ((double) num3 * (double) num3 / (4.0 * (double) this.zoomScaler)) * num1;
    Vector3 vector3 = new Vector3(pos.x, pos.y + num4, 0.0f);
    if (objectIsSelectedAndVisible)
      vector3.z = pos.z;
    return vector3;
  }

  public static SoundCuller CreateCuller()
  {
    SoundCuller soundCuller = new SoundCuller();
    Camera main = Camera.main;
    Vector3 worldPoint1 = main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
    Vector3 worldPoint2 = main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.GetPosition().z));
    soundCuller.min = (Vector2) new Vector3(worldPoint2.x, worldPoint2.y, 0.0f);
    soundCuller.max = (Vector2) new Vector3(worldPoint1.x, worldPoint1.y, 0.0f);
    soundCuller.cameraPos = (Vector2) main.transform.GetPosition();
    Audio audio = Audio.Get();
    float num = (double) (CameraController.Instance.cameras[0].orthographicSize / (audio.listenerReferenceZ - audio.listenerMinZ)) > 0.0 ? 1f : 2f;
    soundCuller.zoomScaler = num;
    return soundCuller;
  }

  public class Tuning : TuningData<SoundCuller.Tuning>
  {
    public float extraYRange;
  }
}
