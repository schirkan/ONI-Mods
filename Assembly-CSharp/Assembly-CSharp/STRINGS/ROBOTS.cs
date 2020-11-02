// Decompiled with JetBrains decompiler
// Type: STRINGS.ROBOTS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace STRINGS
{
  public class ROBOTS
  {
    public static LocString CATEGORY_NAME = (LocString) "Robots";

    public class STATUSITEMS
    {
      public class CANTREACHSTATION
      {
        public static LocString NAME = (LocString) "Can't Reach Sweepy Dock";
        public static LocString DESC = (LocString) "Something is blocking this robot from reaching its base station";
        public static LocString TOOLTIP = (LocString) "Something is blocking this robot from reaching its base station";
      }

      public class MOVINGTOCHARGESTATION
      {
        public static LocString NAME = (LocString) "Travelling to Sweepy Dock";
        public static LocString DESC = (LocString) "This robot is on its way to a battery recharge";
        public static LocString TOOLTIP = (LocString) "This robot is on its way to a battery recharge";
      }

      public class LOWBATTERY
      {
        public static LocString NAME = (LocString) "Low Battery";
        public static LocString DESC = (LocString) "This robot's battery is low and needs to recharge";
        public static LocString TOOLTIP = (LocString) "This robot's battery is low and needs to recharge";
      }

      public class DUSTBINFULL
      {
        public static LocString NAME = (LocString) "Dust Bin Full";
        public static LocString DESC = (LocString) "This robot must return to its base station to unload";
        public static LocString TOOLTIP = (LocString) "This robot must return to its base station to unload";
      }

      public class WORKING
      {
        public static LocString NAME = (LocString) "Working";
        public static LocString DESC = (LocString) "This robot is working diligently";
        public static LocString TOOLTIP = (LocString) "This robot is working diligently";
      }

      public class UNLOADINGSTORAGE
      {
        public static LocString NAME = (LocString) "Unloading";
        public static LocString DESC = (LocString) "This robot is unloading its storage";
        public static LocString TOOLTIP = (LocString) "This robot unloading its storage";
      }

      public class CHARGING
      {
        public static LocString NAME = (LocString) "Charging";
        public static LocString DESC = (LocString) "This robot is charging its battery";
        public static LocString TOOLTIP = (LocString) "This robot is charging its battery";
      }

      public class REACTPOSITIVE
      {
        public static LocString NAME = (LocString) "Positive Reaction";
        public static LocString DESC = (LocString) "This robot is reacting positively to something";
        public static LocString TOOLTIP = (LocString) "This robot is reacting positively to something";
      }

      public class REACTNEGATIVE
      {
        public static LocString NAME = (LocString) "Negative Reaction";
        public static LocString DESC = (LocString) "This robot is reacting negatively to something";
        public static LocString TOOLTIP = (LocString) "This robot is reacting negatively to something";
      }
    }

    public class MODELS
    {
      public class SWEEPBOT
      {
        public static LocString NAME = (LocString) "Sweepy";
        public static LocString DESC = (LocString) ("An automated sweeping robot.\n\nPicks up both " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " and " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " debris and stores it in a " + UI.FormatAsLink("Sweepy Dock", "SWEEPBOTSTATION") + ".");
      }
    }
  }
}
