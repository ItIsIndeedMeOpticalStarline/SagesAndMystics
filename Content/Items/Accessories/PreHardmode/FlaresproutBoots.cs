using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common;
using SagesAndMystics.Common.Glowmask;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Accessories.PreHardmode
{
    public class FlaresproutBoots : ModItem, IGlowmask
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;

            Item.accessory = true;

            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SagesAndMysticsPlayer>().flaresproutBoots = true;
            player.GetModPlayer<SagesAndMysticsPlayer>().flaresproutBootsItemInstance = Item;

            player.accRunSpeed = 6f;
            player.rocketBoots = 5;
            player.vanityRocketBoots = 5;

            if (!hideVisual)
            {
                player.CancelAllBootRunVisualEffects();

                player.hellfireTreads = true;

                if (!player.mount.Active || player.mount.Type != MountID.WallOfFleshGoat)
                    player.DoBootsEffect(player.DoBootsEffect_PlaceFlamesOnTile);
            }
        }

        public override void UpdateVanity(Player player)
        {
            player.vanityRocketBoots = 5;

            player.CancelAllBootRunVisualEffects();

            player.hellfireTreads = true;

            if (!player.mount.Active || player.mount.Type != MountID.WallOfFleshGoat)
                player.DoBootsEffect(player.DoBootsEffect_PlaceFlamesOnTile);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBoots)
                .AddIngredient(ItemID.FlowerBoots)
                .AddIngredient(ItemID.FlameWakerBoots)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.FairyBoots)
                .AddIngredient(ItemID.FlameWakerBoots)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.HellfireTreads)
                .AddIngredient(ItemID.FlowerBoots)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public int GlowmaskOffsetX => 10;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => 255;

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;
    }
}
