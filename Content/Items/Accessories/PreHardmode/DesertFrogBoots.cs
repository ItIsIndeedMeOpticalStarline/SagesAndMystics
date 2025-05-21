using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Accessories.PreHardmode
{
    public class DesertFrogBoots : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 28;

            Item.accessory = true;

            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.accRunSpeed = 6f;
            player.desertBoots = true;
            player.jumpSpeedBoost += 1.6f;
            player.autoJump = true;
            player.extraFall = 10;
            
            if (!hideVisual)
            {
                player.CancelAllBootRunVisualEffects();

                player.sailDash = true;
            }
        }

        public override void UpdateVanity(Player player)
        {
            player.CancelAllBootRunVisualEffects();

            player.sailDash = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SandBoots)
                .AddIngredient(ItemID.AmphibianBoots)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
