using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SagesAndMystics.Content.Tiles.Crafting
{
    public class Altar : ModTile
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(118, 66, 138), this.GetLocalization("MapEntry"));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Crafting.Altar>());

            Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = [ 16 ];
            TileObjectData.addTile(Type);

            DustType = DustID.WoodFurniture;
        }
    }
}
