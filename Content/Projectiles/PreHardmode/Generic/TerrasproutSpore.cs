using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Generic
{
    public class TerrasproutSpore : ModProjectile
    {
        public override string Texture => "SagesAndMystics/Assets/Textures/EmptyTexture";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = 1;
            Projectile.penetrate = 1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Terra);
            dust.scale = 0.3f;
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }
    }
}
