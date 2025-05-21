using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Hostile.Bosses.Baku
{
    class BakuBall : ModProjectile, IGlowmask
    {
        private NPC Parent
        {
            get => Main.npc[(int)Projectile.ai[0]];
        }

        private int SiblingID
        {
            get => (int)Projectile.ai[1];
        }

        private bool Initialized
        {
            get => Projectile.ai[2] == 1f;
            set => Projectile.ai[2] = value == true ? 1f : 0f;
        }

        private bool introDone;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.light = 0.1f;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            introDone = false;
        }

        public override void AI()
        {
            Player player = Main.player[Parent.target];

            if (!Initialized)
            {
                Projectile.timeLeft = 120 + (int)MathF.Abs(SiblingID * 15f) + 17; // Compensate for animation (17 ticks)
                Initialized = true;
            }

            if (!introDone)
            {
                Projectile.alpha -= 15;
                if (!player.dead && player.active && Parent.active)
                {
                    if (Projectile.timeLeft > 90)
                        Projectile.Center = Parent.Center + new Vector2(SiblingID * 48f, 17f - (255f - Projectile.alpha) / 15f); // lock on to parent and offset
                }

                if (Projectile.alpha <= 0)
                {
                    Projectile.alpha = 0;
                    introDone = true;
                }

                return;
            }

            float speed;
            if (Main.masterMode)
                speed = 12f;
            else
                speed = 10f;

            if (Projectile.timeLeft < 30)
                Projectile.alpha += 9;

            if (!player.dead && player.active && Parent.active)
            {
                if (Projectile.timeLeft > 90)
                    Projectile.Center = Parent.Center + new Vector2(SiblingID * 48f, 0f);
                else if (Projectile.timeLeft == 90)
                {
                    SoundEngine.PlaySound(SoundID.Item43);
                    Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * speed;
                }
            }

            if (!Parent.active && Projectile.timeLeft > 90) // Dont delete if not locked to the parent
                Projectile.Kill();

            Projectile.rotation += 0.05f;

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
            for (int i = 0; i < 3; i++)
                Dust.NewDustPerfect(Projectile.Center + new Vector2(Main.rand.Next(Projectile.width, Projectile.height)), DustID.DungeonSpirit);
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 0;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => Projectile.alpha;
    }
}
