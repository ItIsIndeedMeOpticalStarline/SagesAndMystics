using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using SagesAndMystics.Common.Glowmask;
using SagesAndMystics.Content.Projectiles.PreHardmode.Melee;

namespace SagesAndMystics.Content.Items.Weapons.PreHardmode.Melee
{
    public class DreamKatana : ModItem, IGlowmask
    {
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 66;

            Item.damage = 11;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<SpiritNeedle>();
            Item.shootSpeed = 16f;

            Item.value = Item.sellPrice(0, 0, 20);
            Item.rare = ItemRarityID.Blue;
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 10;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => 255;
    }
}
