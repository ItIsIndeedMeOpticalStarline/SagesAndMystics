using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Placeable.Furniture
{
    public class SmallBowlOfWater : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 8;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.createTile = ModContent.TileType<Tiles.Furniture.SmallBowlOfWater>();

            Item.value = Item.sellPrice(copper: 2);
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SmallBowl>())
                .AddCondition(Condition.NearWater)
                .Register();
        }
    }
}
