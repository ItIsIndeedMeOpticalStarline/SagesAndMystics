using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Placeable.Crafting
{
    public class SagesWorkbench : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.createTile = ModContent.TileType<Tiles.Crafting.SagesWorkbench>();

            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
