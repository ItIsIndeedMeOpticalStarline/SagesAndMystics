using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SagesAndMystics.Content.Tiles.Furniture
{
    public class SmallBowlOfWater : ModTile
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(146, 81, 68), this.GetLocalization("MapEntry"));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.SmallBowlOfWater>());

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.addTile(Type);

            DustType = DustID.Clay;
        }
    }
}
