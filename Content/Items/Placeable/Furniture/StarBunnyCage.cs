using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using SagesAndMystics.Content.Items.Consumables.Critters;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Placeable.Furniture
{
    public class StarBunnyCage : ModItem, IGlowmask
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BunnyCage);
            Item.createTile = ModContent.TileType<Tiles.Furniture.StarBunnyCage>();

            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Terrarium)
                .AddIngredient(ModContent.ItemType<StarBunny>())
                .Register();
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, new Vector3(0.816f, 0.78f, 0.945f));
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX { get { return 10; } }

        public int GlowmaskOffsetY { get { return 0; } }

        public int GlowmaskAlpha { get { return 255; } }
    }
}
