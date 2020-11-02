// Decompiled with JetBrains decompiler
// Type: Bouncer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
  private bool m_bouncing;
  public float durationSecs = 0.3f;
  public float height = 20f;

  public void Bounce()
  {
    if (!this.gameObject.activeInHierarchy || this.m_bouncing)
      return;
    this.StartCoroutine(this.DoBounce());
  }

  public bool IsBouncing() => this.m_bouncing;

  private IEnumerator DoBounce()
  {
    Bouncer bouncer = this;
    bouncer.m_bouncing = true;
    float completion = 0.0f;
    Vector3 position = new Vector3(bouncer.gameObject.transform.position.x, bouncer.gameObject.transform.position.y, bouncer.gameObject.transform.position.z);
    float startPos = bouncer.gameObject.transform.position.y;
    while ((double) completion < 1.0)
    {
      completion = Mathf.Min(completion + Time.unscaledDeltaTime / bouncer.durationSecs, 1f);
      position.y = startPos + Bouncer.BounceSpline(completion) * bouncer.height;
      bouncer.gameObject.transform.position = position;
      yield return (object) new WaitForEndOfFrame();
    }
    position.y = startPos;
    bouncer.gameObject.transform.position = position;
    bouncer.m_bouncing = false;
  }

  private static float BounceSpline(float k) => (double) k < 0.5 ? Bouncer.QuadOut(k * 2f) : 1f - Bouncer.QuadIn((float) ((double) k * 2.0 - 1.0));

  private static float QuadOut(float k) => k * (2f - k);

  private static float QuadIn(float k) => k * k;
}
