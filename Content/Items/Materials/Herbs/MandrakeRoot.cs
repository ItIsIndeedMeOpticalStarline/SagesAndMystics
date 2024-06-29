using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Materials.Herbs
{
    public class MandrakeRoot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.AlchemyPlants;
        }

        public override void SetDefaults() 
        {
            Item.width = 36;
            Item.height = 34;

            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 20);
        }
    }
}
