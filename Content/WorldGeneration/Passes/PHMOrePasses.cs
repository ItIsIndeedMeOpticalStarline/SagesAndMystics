using SagesAndMystics.Content.Tiles.Ores;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace SagesAndMystics.Content.WorldGeneration.Passes
{
    public class OreGenPreHardmode : GenPass
    {
        public OreGenPreHardmode(string name, double loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 0.001); i++)
            {
                WorldGen.TileRunner(
                    WorldGen.genRand.Next(0, Main.maxTilesX), 
                    WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY), 
                    WorldGen.genRand.Next(3, 6), 
                    WorldGen.genRand.Next(2, 6), 
                    ModContent.TileType<Salt>());
            }
        }
    }
}
