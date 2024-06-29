using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.Dusts
{
    public class WitchsCandleDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
            dust.velocity.X *= 0.3f;
            dust.scale *= 0.7f;
        }

        public override bool Update(Dust dust)
        {
            if (dust.customData != null && dust.customData is int i)
            {
                if (i == 0)
                {
                    if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
                    {
                        dust.scale *= 0.9f;
                        dust.velocity *= 0.25f;
                    }
                }
                else if (i == 1)
                {
                    dust.scale *= 0.98f;
                    dust.velocity.Y *= 0.98f;
                    if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
                    {
                        dust.scale *= 0.9f;
                        dust.velocity *= 0.25f;
                    }
                }
            }

            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.05f;
            }
            if (!dust.noLight && !dust.noLightEmittence)
            {
                float lightFade = dust.scale * 1.4f;

                if (lightFade > 0.6f)
                    lightFade = 0.6f;

                Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), lightFade * 0.82f, lightFade * 0.996f, lightFade * 0.008f);
            }

            return true;
        }
    }
}
