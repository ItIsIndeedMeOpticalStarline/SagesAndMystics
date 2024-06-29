using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Content.Dusts;
using Terraria.GameContent.Drawing;

namespace SagesAndMystics.Content.Tiles.Furniture
{
    public class WitchsCandle : ModTile
    {
        public override void SetStaticDefaults()
        {
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AddMapEntry(new Color(176, 172, 129), this.GetLocalization("MapEntry"));
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.Furniture.WitchsCandle>());

            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.CoordinateHeights = [ 16, 16 ];
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);

            DustType = ModContent.DustType<WitchsCandleDust>();
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.WitchsCandle>();
        }

        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18;
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
            Main.tile[i, topY].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
            NetMessage.SendTileSquare(-1, i, topY, 1);
            return true;
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18;
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
            Main.tile[i, topY].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
            NetMessage.SendTileSquare(-1, i, topY, 1);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0)
            {
                r = 0.82f;
                g = 0.996f;
                b = 0.008f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            if (!TileDrawing.IsVisible(tile))
                return;

            ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(ulong)i);
            Color color = new Color(198, 171, 108, 0);
            int frameX = tile.TileFrameX;
            int frameY = tile.TileFrameY;
            int width = 16;
            int offsetY = 0;
            int height = 16;
            int offsetX = 0;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
                zero = Vector2.Zero;

            for (int k = 0; k < 7; k++)
            {
                float x = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                float y = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
                Main.spriteBatch.Draw
                (
                    ModContent.Request<Texture2D>(Texture + "_Flame").Value, 
                    new Vector2(i * 16 - (int)Main.screenPosition.X + offsetX - (width - 16f) / 2f + x, j * 16 - (int)Main.screenPosition.Y + offsetY + y) + zero, 
                    new Rectangle(frameX, frameY, width, height), 
                    color, 
                    0f, 
                    default, 
                    1f,
                    SpriteEffects.None, 
                    0f
                );
            }
        }
    }
}
