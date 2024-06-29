using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Materials.Ores
{
    public class Salt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)10520;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.consumable = true;
            Item.maxStack = 9999;

            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.createTile = ModContent.TileType<Tiles.Ores.Salt>();

            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.rare = ItemRarityID.White;
        }
    }
}
