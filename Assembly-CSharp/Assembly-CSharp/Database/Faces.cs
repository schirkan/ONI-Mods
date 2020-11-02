// Decompiled with JetBrains decompiler
// Type: Database.Faces
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Database
{
  public class Faces : ResourceSet<Face>
  {
    public Face Neutral;
    public Face Happy;
    public Face Uncomfortable;
    public Face Cold;
    public Face Hot;
    public Face Tired;
    public Face Sleep;
    public Face Hungry;
    public Face Angry;
    public Face Suffocate;
    public Face Dead;
    public Face Sick;
    public Face SickSpores;
    public Face Zombie;
    public Face SickFierySkin;
    public Face SickCold;
    public Face Productive;
    public Face Determined;
    public Face Sticker;
    public Face Balloon;
    public Face Sparkle;
    public Face Tickled;

    public Faces()
    {
      this.Neutral = this.Add(new Face(nameof (Neutral)));
      this.Happy = this.Add(new Face(nameof (Happy)));
      this.Uncomfortable = this.Add(new Face(nameof (Uncomfortable)));
      this.Cold = this.Add(new Face(nameof (Cold)));
      this.Hot = this.Add(new Face(nameof (Hot)));
      this.Tired = this.Add(new Face(nameof (Tired)));
      this.Sleep = this.Add(new Face(nameof (Sleep)));
      this.Hungry = this.Add(new Face(nameof (Hungry)));
      this.Angry = this.Add(new Face(nameof (Angry)));
      this.Suffocate = this.Add(new Face(nameof (Suffocate)));
      this.Sick = this.Add(new Face(nameof (Sick)));
      this.SickSpores = this.Add(new Face("Spores"));
      this.Zombie = this.Add(new Face(nameof (Zombie)));
      this.SickFierySkin = this.Add(new Face("Fiery"));
      this.SickCold = this.Add(new Face(nameof (Cold)));
      this.Dead = this.Add(new Face("Death"));
      this.Productive = this.Add(new Face(nameof (Productive)));
      this.Determined = this.Add(new Face(nameof (Determined)));
      this.Sticker = this.Add(new Face(nameof (Sticker)));
      this.Sparkle = this.Add(new Face(nameof (Sparkle)));
      this.Balloon = this.Add(new Face(nameof (Balloon)));
      this.Tickled = this.Add(new Face(nameof (Tickled)));
    }
  }
}
