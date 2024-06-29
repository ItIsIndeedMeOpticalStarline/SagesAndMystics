using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Placeable.Crafting
{
    public class MortarAndPestle : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 16;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.createTile = ModContent.TileType<Tiles.Crafting.MortarAndPestle>();

            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ClayBlock, 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}
