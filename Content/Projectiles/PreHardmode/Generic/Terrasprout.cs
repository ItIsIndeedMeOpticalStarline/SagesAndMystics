using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Generic
{
    public class Terrasprout : ModProjectile
    {
        public const int FrameHeight = 20;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = FrameHeight;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            const int FrameSpeed = 8;
            if (++Projectile.frameCounter >= FrameSpeed)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile
                (
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    -Vector2.UnitY.RotatedBy(-MathHelper.PiOver4 + (i * MathHelper.PiOver2 / 3)),
                    ModContent.ProjectileType<TerrasproutSpore>(),
                    Projectile.damage / 2,
                    1f,
                    Projectile.owner
                );
            }
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 0;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => Projectile.alpha;
    }
}
