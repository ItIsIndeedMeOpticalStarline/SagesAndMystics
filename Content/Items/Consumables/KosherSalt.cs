using SagesAndMystics.Content.Items.Materials.Ores;
using SagesAndMystics.Content.Projectiles.PreHardmode.Ranged;
using SagesAndMystics.Content.Tiles.Crafting;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Consumables
{
    public class KosherSalt : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<SaltSpray>();
            Item.shootSpeed = 4f;

            Item.value = Item.sellPrice(copper: 30);
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddIngredient(ModContent.ItemType<Salt>(), 3)
                .AddTile(ModContent.TileType<MortarAndPestle>())
                .Register();
        }
    }
}
