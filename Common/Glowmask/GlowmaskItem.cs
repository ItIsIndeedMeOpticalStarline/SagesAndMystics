using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace SagesAndMystics.Common.Glowmask
{
    public class GlowmaskItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem is IGlowmask glowmask && glowmask.GlowmaskTexture != null;
        }

        public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            IGlowmask glowmaskItem = item.ModItem as IGlowmask;
            Texture2D texture = glowmaskItem.GlowmaskTexture;
            Rectangle frame = Main.itemAnimations[item.type]?.GetFrame(texture) ?? texture.Frame();
            Vector2 drawOrigin = frame.Size() / 2f;
            Vector2 drawOffset = new(item.width / 2 - drawOrigin.X, item.height - frame.Height);
            Vector2 drawPosition = item.position - Main.screenPosition + drawOrigin + drawOffset;

            spriteBatch.Draw(texture, drawPosition, frame, new Color(250, 250, 250, glowmaskItem.GlowmaskAlpha), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
    }
}
