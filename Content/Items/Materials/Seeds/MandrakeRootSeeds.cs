using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Materials.Seeds
{
    public class MandrakeRootSeeds : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;

            Item.ResearchUnlockCount = 25;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.AlchemySeeds;
        }

        public override void SetDefaults() 
        {
            Item.width = 22;
            Item.height = 18;

            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.createTile = ModContent.TileType<Tiles.Herbs.MandrakeRoot>();

            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 16);
        }
    }
}
