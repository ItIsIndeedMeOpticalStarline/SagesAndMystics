using SagesAndMystics.Content.Items.Materials.Ores;
using SagesAndMystics.Content.Projectiles.PreHardmode.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Weapons.PreHardmode.Ranged
{
    public class SaltShaker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;

            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 3f;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<SaltSpray>();
            Item.shootSpeed = 4f;

            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddRecipeGroup(RecipeGroupID.IronBar, 3)
                .AddIngredient(ModContent.ItemType<Salt>(), 10)
                .AddTile(TileID.Tables)
                .Register();
        }
    }
}
