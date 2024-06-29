using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Hostile
{
    public class MandrakeCry : ModProjectile
    {
        private ref NPC Parent
        {
            get => ref Main.npc[(int)Projectile.ai[0]];
        }

        private bool Initalized
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value == true ? 1f : 0f;
        }

        private int maxTimeLeft = 180;

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;

            Projectile.aiStyle = -1;

            Projectile.hostile = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.penetrate = -1;

            Projectile.timeLeft = maxTimeLeft;

            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (!Initalized)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyScream, Projectile.Center);

                const float rippleCount = 3f, rippleSize = 1500f, rippleSpeed = 60f;
                if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
                    Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(Projectile.Center);

                Initalized = true;
            }

            if (Projectile.timeLeft == maxTimeLeft / 2)
                Projectile.hostile = false;

            if (Parent.active)
                Projectile.Center = Parent.Center;

            const float distortStrength = 100f;
            if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
            {
                float progress = (180f - Projectile.timeLeft) / 60f;
                Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f)).UseTargetPosition(Projectile.Center);
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                Filters.Scene["Shockwave"].Deactivate();
        }
    }
}
