// Decompiled with JetBrains decompiler
// Type: IComponentManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public interface IComponentManager
{
  void Spawn();

  void RenderEveryTick(float dt);

  void FixedUpdate(float dt);

  void Sim200ms(float dt);

  void CleanUp();

  void Clear();

  bool Has(object go);

  int Count { get; }

  string Name { get; }
}
