using Newtonsoft.Json;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SagesAndMystics.Common
{
    public class SagesAndMysticsConfig : ModConfig
    {
        [DefaultValue(true)]
        public bool sageRemembers;

        public static SagesAndMysticsConfig Instance => ModContent.GetInstance<SagesAndMysticsConfig>();

        [JsonIgnore]
        public static bool DisplayLastSeenSageTip => Instance.sageRemembers;

        public override ConfigScope Mode => ConfigScope.ClientSide;
    }
}
