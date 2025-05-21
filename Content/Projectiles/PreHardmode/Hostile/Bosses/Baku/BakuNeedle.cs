using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Hostile.Bosses.Baku
{
    public class BakuNeedle : ModProjectile, IGlowmask
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            Projectile.width = 16;
            Projectile.height = 28;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.light = 0.1f;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 15;
            else
                Projectile.alpha = 0;

            if (Projectile.timeLeft < 30)
                Projectile.alpha += 9;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            const int FrameSpeed = 2;
            if (++Projectile.frameCounter >= FrameSpeed)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.ToRadians(90f * i));
                Vector2 velocity = offset;
                velocity.Normalize();
                velocity *= 8f;
                Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.DungeonSpirit, velocity.X, velocity.Y);
            }
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 0;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => Projectile.alpha;
    }
}
