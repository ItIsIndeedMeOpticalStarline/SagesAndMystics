using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SagesAndMystics.Common.Glowmask
{
    public class GlowmaskProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.ModProjectile is IGlowmask glowmask && glowmask.GlowmaskTexture != null;
        }

        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            IGlowmask glowmaskProjectile = projectile.ModProjectile as IGlowmask;
            Texture2D texture = glowmaskProjectile.GlowmaskTexture;
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
            Vector2 frameOrigin = frame.Size() / 2f;
            Main.EntitySpriteDraw
            (
                texture,
                projectile.Center + new Vector2(glowmaskProjectile.GlowmaskOffsetX, glowmaskProjectile.GlowmaskOffsetY) - Main.screenPosition,
                frame,
                new Color(250, 250, 250, glowmaskProjectile.GlowmaskAlpha),
                projectile.rotation,
                frameOrigin,
                projectile.scale,
                SpriteEffects.None
            );
        }
    }
}
