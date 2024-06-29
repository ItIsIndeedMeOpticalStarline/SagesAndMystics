using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using SagesAndMystics.Content.Items.Materials;
using SagesAndMystics.Content.Tiles.Crafting;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Accessories.PHM
{
    public class LuckyRabbitsFoot : ModItem, IGlowmask
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;

            Item.accessory = true;

            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.luck += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StarBunnyFur>(), 5)
                .AddTile(ModContent.TileType<SagesWorkbench>())
                .Register();
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX { get { return 10; } }

        public int GlowmaskOffsetY { get { return 0; } }

        public int GlowmaskAlpha { get { return 255; } }
    }
}
