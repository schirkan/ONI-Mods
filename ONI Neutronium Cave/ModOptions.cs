using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Neutronium_Cave
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("https://github.com/schirkan/ONI-Mods")] // , "TODO"
    public class ModOptions
    {
        [JsonProperty]
        [Option("Enabled", "Enable or disable this mod temporarily. (Default: on)")]
        public bool Enabled { get; set; }

        [JsonProperty]
        [Option("Start biome with granite gaps", "Gaps in start biome borders are made of granite. (Default: on)")]
        public bool GraniteStartBiomeBorder { get; set; }

        [JsonProperty]
        [Option("Space biome with default border", "Space biome borders are made of the default border material. (Default: off)")]
        public bool DefaultSpaceBorder { get; set; }

        [JsonProperty]
        [Option("More gaps", "Double the amount of gaps in biome borders. (Default: off)")]
        public bool MoreEntrances { get; set; }

        [JsonProperty]
        [Option("Gap size", "Set the width of gaps in biome borders. (Default: 3)")]
        [Limit(2, 5)]
        public int GapWidth { get; set; }

        public ModOptions()
        {
            Enabled = true;
            GraniteStartBiomeBorder = true;
            DefaultSpaceBorder = false;
            MoreEntrances = false;
            GapWidth = 3;
        }
    }
}
