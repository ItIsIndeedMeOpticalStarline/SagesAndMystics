using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using SagesAndMystics.Content.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Weapons.PreHardmode.Ranged
{
    public class SagesWater : ModItem, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;

            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 3f;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.Ranged.SagesWater>();
            Item.shootSpeed = 9f;

            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ModContent.ItemType<Stardust>())
                .AddCondition(Condition.NearWater)
                .Register();
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX { get { return 10; } }

        public int GlowmaskOffsetY { get { return 0; } }

        public int GlowmaskAlpha { get { return 255; } }
    }
}
