using SagesAndMystics.Common;
using SagesAndMystics.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Accessories.PreHardmode
{
    public class Headphones : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;

            Item.accessory = true;

            Item.value = Item.sellPrice(silver: 19, copper: 20);
            Item.rare = ItemRarityID.White;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SagesAndMysticsPlayer>().headphones = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 3)
                .AddIngredient(ItemID.Leather, 2)
                .AddIngredient(ModContent.ItemType<StarBunnyFur>(), 5)
                .AddTile(TileID.Tables)
                .Register();
        }
    }
}
