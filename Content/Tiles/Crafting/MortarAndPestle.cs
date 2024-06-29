using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SagesAndMystics.Content.Tiles.Crafting
{
    public class MortarAndPestle : ModTile
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(146, 81, 68), this.GetLocalization("MapEntry"));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Crafting.MortarAndPestle>());

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.Width = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.Clay;
        }
    }
}
