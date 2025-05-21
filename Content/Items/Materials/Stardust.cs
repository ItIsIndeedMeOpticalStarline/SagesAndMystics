using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Materials
{
    public class Stardust : ModItem, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;

            Item.maxStack = Item.CommonMaxStack;

            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddIngredient(ItemID.FallenStar)
                .Register();
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 10;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => 255;
    }
}
