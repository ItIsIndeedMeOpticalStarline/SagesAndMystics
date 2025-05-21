using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using SagesAndMystics.Common.Systems.Primitives;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Melee
{
    public class DreamSpirit : ModProjectile, IGlowmask
    {
        private Ribbon ribbon;
        private const int MaxPoints = 20;
        private List<Vector2> cache;

        public bool Initalized
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value == true ? 1f : 0f;
        }

        public bool Collided
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value == true ? 1f : 0f;
        }

        public int PostTileCollideKillDelay
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.light = 0.3f;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

            ribbon = new Ribbon(Main.graphics.GraphicsDevice, MaxPoints, null, factor => factor * 16f, null);
        }

        public override void AI()
        {
            if (!Initalized)
            {
                PostTileCollideKillDelay = 15;
                Initalized = true;
            }

            Projectile.rotation += 0.2f;

            if (!Main.dedServ)
            {
                if (Main.rand.NextBool())
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Pink, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
            }

            float range = 200f;
            bool foundTarget = false;
            Vector2 targetCenter = Vector2.Zero;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this) && Vector2.Distance(Projectile.Center, npc.Center) < range && Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1))
                {
                    float nRange = Math.Abs(Projectile.Center.X - npc.Center.X) + Math.Abs(Projectile.Center.Y - npc.Center.Y);
                    if (nRange < range)
                    {
                        range = nRange;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            if (foundTarget)
            {
                const float Speed = 8f;

                Vector2 toTarget = targetCenter - Projectile.Center;
                toTarget.Normalize();

                Projectile.velocity = (Projectile.velocity * 20f + toTarget * Speed) / 21f;
            }

            if (Collided)
            {
                PostTileCollideKillDelay--;

                if (PostTileCollideKillDelay <= 0)
                    Projectile.Kill();
            }

            const int FrameSpeed = 2;
            if (++Projectile.frameCounter >= FrameSpeed)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                if (cache == null)
                {
                    cache = new List<Vector2>();

                    for (int i = 0; i < MaxPoints; i++)
                        cache.Add(Projectile.Center);
                }

                cache.Add(Projectile.Center);

                while (cache.Count > MaxPoints)
                    cache.RemoveAt(0);

                ribbon.positions = cache.ToArray();
                ribbon.NextPosition = Projectile.Center + Projectile.velocity;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Pink, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            Collided = true;
            Projectile.tileCollide = false;

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            ribbon.Device.Textures[0] = ModContent.Request<Texture2D>("SagesAndMystics/Assets/Ribbons/SpiritRibbon").Value;

            Viewport viewport = ribbon.Device.Viewport;
            ribbon.effect.World = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0));
            ribbon.effect.View = Main.GameViewMatrix.TransformationMatrix;
            ribbon.effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 10);

            ribbon.Render();
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 0;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => Projectile.alpha;
    }
}
