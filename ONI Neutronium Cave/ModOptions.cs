using Newtonsoft.Json;
using PeterHan.PLib;

namespace Neutronium_Cave
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("Neutronium Cave", "https://github.com/schirkan/ONI-Mods")] // , "TODO"
    public class ModOptions
    {
        [JsonProperty]
        [Option("Start biome with granite gaps", "Gaps in start biome borders are made of granite. (Default: on)")]
        public bool GraniteStartBiomeBorder { get; set; }

        [JsonProperty]
        [Option("Space biome with granite border", "Space biome borders are made of granite. (Default: off)")]
        public bool GraniteSpaceBorder { get; set; }

        [JsonProperty]
        [Option("More gaps", "Double the amount of gaps in biome borders. (Default: off)")]
        public bool MoreEntrances { get; set; }

        [JsonProperty]
        [Option("Gap size", "Set the width of gaps in biome borders. (Default: 2)")]
        [Limit(2, 5)]
        public int GapWidth { get; set; }

        public ModOptions()
        {
            GraniteStartBiomeBorder = true;
            GraniteSpaceBorder = false;
            MoreEntrances = false;
            GapWidth = 2;
        }
    }
}
