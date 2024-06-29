using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SagesAndMystics.Content.Tiles.Crafting
{
    public class SagesWorkbench : ModTile
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(62, 57, 97), this.GetLocalization("MapEntry"));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Crafting.SagesWorkbench>());

            Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.CoordinateHeights = [ 18 ];
            TileObjectData.addTile(Type);

            DustType = ModContent.DustType<Dusts.SagesWaterDust>();
        }
    }
}
