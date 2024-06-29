using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace SagesAndMystics.Content.Tiles.Furniture
{
    public class StarBunnyCage : ModTile
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(122, 217, 232), this.GetLocalization("MapEntry"));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.StarBunnyCage>());

            TileID.Sets.CritterCageLidStyle[Type] = 2;

            Main.tileFrameImportant[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileLighted[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.Glass;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.816f;
            g = 0.78f;
            b = 0.945f;
        }

        private readonly int animationFrameHeight = 54;

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];
            Main.critterCage = true;
            int left = i - tile.TileFrameX / 18;
            int top = j - tile.TileFrameY / 18;
            int offset = left / 3 * (top / 3);
            offset %= Main.cageFrames;
            frameYOffset = Main.bunnyCageFrame[offset] * animationFrameHeight;
        }
    }
}
