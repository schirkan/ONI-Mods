// Decompiled with JetBrains decompiler
// Type: BucketUpdater`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class BucketUpdater<DataType> : UpdateBucketWithUpdater<DataType>.IUpdater
{
  private System.Action<DataType, float> callback;

  public BucketUpdater(System.Action<DataType, float> callback) => this.callback = callback;

  public void Update(DataType data, float dt) => this.callback(data, dt);
}
