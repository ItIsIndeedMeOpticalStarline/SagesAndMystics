using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common;
using SagesAndMystics.Common.Glowmask;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Items.Accessories.PreHardmode
{
    public class TerrasproutTreads : ModItem, IGlowmask
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 28;

            Item.accessory = true;

            Item.value = Item.sellPrice(gold: 13);
            Item.rare = ItemRarityID.Lime;
        }

        // Visual effect for the custom boots
        // TODO: IL edit this in...
        private void SpawnTerrasproutFastRunParticles(Player player)
        {
            for (int j = 0; j < 2; j++)
            {
                Dust dust = (j != 0) ? Dust.NewDustDirect
                                       (
                                           new Vector2(player.Center.X, player.position.Y + player.height + player.gfxOffY),
                                           player.width / 2, 
                                           6, 
                                           DustID.Terra, 
                                           Scale: 1.35f
                                       ) :
                                       Dust.NewDustDirect
                                       (
                                           new Vector2(player.position.X, player.position.Y + player.height + player.gfxOffY), 
                                           player.width / 2, 
                                           6,
                                           DustID.Terra,
                                           Scale: 1.35f
                                       );

                dust.scale *= 1f + Main.rand.NextFloat(0.2f, 0.4f);
                dust.noGravity = true;
                dust.noLight = false;
                dust.velocity *= 0.001f;
                dust.velocity.Y -= 0.003f;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SagesAndMysticsPlayer>().flaresproutBoots = true;
            player.GetModPlayer<SagesAndMysticsPlayer>().flaresproutBootsItemInstance = Item;

            player.accRunSpeed = 6f;
            player.desertBoots = true;
            player.jumpSpeedBoost += 1.6f;
            player.autoJump = true;
            player.extraFall = 10;
            player.rocketBoots = 4;
            player.vanityRocketBoots = 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<DesertFrogBoots>()
                .AddIngredient<FlaresproutBoots>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public int GlowmaskOffsetX => 0;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => 255;

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;
    }
}
