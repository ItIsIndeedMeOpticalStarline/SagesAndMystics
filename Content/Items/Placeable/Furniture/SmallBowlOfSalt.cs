using SagesAndMystics.Content.Items.Materials.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Placeable.Furniture
{
    public class SmallBowlOfSalt : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.createTile = ModContent.TileType<Tiles.Furniture.SmallBowlOfSalt>();

            Item.value = Item.sellPrice(copper: 52);
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SmallBowl>())
                .AddIngredient(ModContent.ItemType<Salt>(), 5)
                .Register();
        }
    }
}
