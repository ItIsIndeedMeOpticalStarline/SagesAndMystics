using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using SagesAndMystics.Content.Dusts;
using SagesAndMystics.Content.Items.Materials.Ores;
using SagesAndMystics.Content.Tiles.Crafting;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Placeable.Furniture
{
    public class WitchsCandle : ModItem, IGlowmask
    {
        private static readonly Vector2 Offset = new Vector2(12f, -14f);

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Candle);
            Item.createTile = ModContent.TileType<Tiles.Furniture.WitchsCandle>();

            Item.value = Item.sellPrice(gold: 2, silver: 16);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Candle)
                .AddIngredient(ItemID.Emerald)
                .AddIngredient(ModContent.ItemType<Salt>(), 10)
                .AddTile(ModContent.TileType<SagesWorkbench>())
                .Register();
        }

        public override void HoldItem(Player player)
        {
            if (player.wet)
                return;

            if (Main.rand.NextBool(player.itemAnimation > 0 ? 7 : 30))
            {
                Dust dust = Dust.NewDustDirect
                (
                    new Vector2(player.itemLocation.X + (player.direction == -1 ? -16f : 6f), player.itemLocation.Y + 2f * player.gravDir), 
                    4, 
                    4,
                    ModContent.DustType<WitchsCandleDust>(), 
                    0f, 
                    0f, 
                    100
                );

                if (!Main.rand.NextBool(3))
                    dust.noGravity = true;

                dust.velocity *= 0.3f;
                dust.velocity.Y -= 1.5f;
                dust.position = player.RotatedRelativePoint(dust.position);
            }


            Lighting.AddLight
            (
                new Vector2(player.itemLocation.X + Offset.X * player.direction + player.velocity.X, player.itemLocation.Y + Offset.Y + player.velocity.Y), 
                new Vector3(0.82f, 0.996f, 0.008f)
            );
        }

        public override void PostUpdate()
        {
            if (!Item.wet)
                Lighting.AddLight(Item.Center, new Vector3(0.82f, 0.996f, 0.008f));
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX { get { return 10; } }

        public int GlowmaskOffsetY { get { return 0; } }

        public int GlowmaskAlpha { get { return 255; } }
    }
}
