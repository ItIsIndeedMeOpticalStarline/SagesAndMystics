using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SagesAndMystics.Common.Glowmask;
using SagesAndMystics.Content.Projectiles.PreHardmode.Melee;
using static SagesAndMystics.Content.Directions;

namespace SagesAndMystics.Content.Items.Weapons.PreHardmode.Melee
{
    public class DreamBroadsword : ModItem, IGlowmask
    {
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;

            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;

            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<DreamSpirit>();
            Item.shootSpeed = 8f;

            Item.value = Item.sellPrice(0, 0, 20);
            Item.rare = ItemRarityID.Blue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int ProjectileCount = 2;
            for (int i = 0; i < ProjectileCount; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.TwoPi / 16f * i * -player.direction), type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 10;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => 255;
    }
}
