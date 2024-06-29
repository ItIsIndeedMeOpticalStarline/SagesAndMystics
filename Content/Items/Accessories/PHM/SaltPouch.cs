using SagesAndMystics.Common;
using SagesAndMystics.Content.Items.Materials.Ores;
using SagesAndMystics.Content.Tiles.Crafting;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Accessories.PHM
{
    public class SaltPouch : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;

            Item.accessory = true;

            Item.value = Item.sellPrice(silver: 1, copper: 30);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SagesAndMysticsPlayer>().saltPouch = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Salt>(), 10)
                .AddIngredient(ItemID.Leather, 3)
                .AddTile(ModContent.TileType<SagesWorkbench>())
                .Register();
        }
    }
}
