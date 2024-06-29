using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Placeable.Crafting
{
    public class Altar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 16;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.createTile = ModContent.TileType<Tiles.Crafting.Altar>();

            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodenTable, 1)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.Book)
                .AddTile(ModContent.TileType<Tiles.Crafting.SagesWorkbench>())
                .Register();
        }
    }
}
