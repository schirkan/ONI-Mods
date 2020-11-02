// Decompiled with JetBrains decompiler
// Type: KAnimConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class KAnimConverter
{
  public interface IAnimConverter
  {
    int GetMaxVisible();

    HashedString GetBatchGroupID(bool isEditorWindow = false);

    KAnimBatch GetBatch();

    void SetBatch(KAnimBatch id);

    Vector2I GetCellXY();

    float GetZ();

    int GetLayer();

    string GetName();

    bool IsActive();

    bool IsVisible();

    int GetCurrentNumFrames();

    int GetFirstFrameIndex();

    int GetCurrentFrameIndex();

    Matrix2x3 GetTransformMatrix();

    KBatchedAnimInstanceData GetBatchInstanceData();

    SymbolInstanceGpuData symbolInstanceGpuData { get; }

    SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; }

    KAnimBatchGroup.MaterialType GetMaterialType();

    bool ApplySymbolOverrides();
  }
}
