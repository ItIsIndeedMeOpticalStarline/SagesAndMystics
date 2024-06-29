using SagesAndMystics.Content.WorldGeneration.Passes;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace SagesAndMystics.Content.WorldGeneration
{
    public class GenSystem : ModSystem
    {
        public static LocalizedText WorldGenSaltPassMessage { get; private set; }

        public override void SetStaticDefaults()
        {
            WorldGenSaltPassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(WorldGenSaltPassMessage)}"));
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int index = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

            if (index != -1)
                tasks.Insert(index + 1, new OreGenPreHardmode("Salt", 100f));
        }
    }
}
