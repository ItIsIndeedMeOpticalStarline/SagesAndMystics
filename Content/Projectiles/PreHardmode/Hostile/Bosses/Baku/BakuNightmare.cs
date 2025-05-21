using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SagesAndMystics.Common.Glowmask;
using Terraria.Audio;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using SagesAndMystics.Content.NPCs.PreHardmode;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Hostile.Bosses.Baku
{
    public class BakuNightmare : ModProjectile, IGlowmask
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.damage = 0;
            Projectile.width = 124;
            Projectile.height = 124;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            float scaleFactor = (180f - Projectile.timeLeft) / 100f;
            Projectile.rotation += 0.1f + scaleFactor;
            Projectile.scale = 1f - scaleFactor / 2f;

            for (int i = 0; i < 5; i++)
            {
                Vector2 pos = Projectile.Center + new Vector2(MathHelper.Max(Projectile.width, Projectile.height) / 3f * 2f * Projectile.scale).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                Dust chargeDust = Dust.NewDustPerfect(pos, DustID.DungeonSpirit, (Projectile.Center - pos).SafeNormalize(-Vector2.UnitY) * 2f);
                chargeDust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14);
            NPC.NewNPC(Projectile.GetSource_Death(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<Nightmare>());
            for (int i = 0; i < 60; i++)
            {
                Dust chargeDust = Dust.NewDustPerfect(Projectile.Center, DustID.DungeonSpirit, Vector2.One.RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))).SafeNormalize(-Vector2.UnitY) * 10f);
                chargeDust.noGravity = true;
            }
        }

        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture) => true;

        public int GlowmaskOffsetX => 0;

        public int GlowmaskOffsetY => 0;

        public int GlowmaskAlpha => Projectile.alpha;
    }
}
