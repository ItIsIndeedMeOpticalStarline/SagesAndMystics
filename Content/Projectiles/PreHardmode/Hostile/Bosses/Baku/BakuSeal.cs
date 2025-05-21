using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using Terraria.Audio;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Hostile.Bosses.Baku
{
    public class BakuSeal : ModProjectile, IGlowmask
    {
        public enum Action
        {
            None,
            Shoot,
            KillYourself
        }

        private NPC Parent
        {
            get => Main.npc[(int)Projectile.ai[0]];
        }

        private Action CurrentAction
        {
            get => (Action)Projectile.ai[1];
        }

        private bool IntroDone
        {
            get => Projectile.ai[2] == 1f;
            set => Projectile.ai[2] = value == true ? 1f : 0f;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.light = 0.2f;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.timeLeft = 600;

            if (!IntroDone)
            {
                Projectile.alpha -= 15;
                if (Projectile.alpha <= 0)
                {
                    Projectile.alpha = 0;
                    IntroDone = true;
                }
                else
                    return;
            }

            switch (CurrentAction)
            {
                case Action.None: break;
                case Action.Shoot:
                {
                    Vector2 vel = (Projectile.Center - Parent.Center).SafeNormalize(-Vector2.UnitY) * 8f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<BakuNeedle>(), Projectile.damage / 2, Projectile.knockBack);
                } break;
                case Action.KillYourself:
                {
                    Projectile.Kill();
                } break;
            }

            if (Projectile.timeLeft < 30)
                Projectile.alpha += 9;

            if (!Parent.active)
                Projectile.Kill();

            Projectile.rotation += 0.1f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14);
            for (int i = 0; i < 16; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.ToRadians(22.5f * i));
                Vector2 velocity = offset;
                velocity.Normalize();
                velocity *= -8f;
                Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.DungeonSpirit, velocity.X, velocity.Y);
            }
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 0;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => Projectile.alpha;
    }
}
