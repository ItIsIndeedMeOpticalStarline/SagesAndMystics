using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesAndMystics.Common.Glowmask
{
    public interface IGlowmask
    {
        public bool PreDraw(ref PlayerDrawSet drawSet, Texture2D glowTexture);

        public Texture2D GlowmaskTexture
        {
            get
            {
                if (this is ModItem modItem)
                    return ModContent.Request<Texture2D>($"{modItem.Texture}_Glow", AssetRequestMode.ImmediateLoad).Value;

                return null;
            }
        }

        public int GlowmaskOffsetX { get; }

        public int GlowmaskOffsetY { get; }

        public int GlowmaskAlpha { get; }

        public static void Draw(ref PlayerDrawSet drawSet)
        {
            if (drawSet.shadow != 0)
                return;

            Player drawPlayer = drawSet.drawPlayer;
            Item heldItem = drawPlayer.HeldItem;

            if (heldItem.IsAir)
                return;


            if (heldItem is IGlowmask glowmaskItem)
            {
                Texture2D texture = glowmaskItem.GlowmaskTexture;
                Color color = new Color(255, 255, 255, glowmaskItem.GlowmaskAlpha);
                if (texture != null && drawPlayer.itemAnimation > 0)
                {
                    Vector2 basePosition = drawPlayer.itemLocation - Main.screenPosition;
                    basePosition = new Vector2((int)basePosition.X, (int)basePosition.Y) + (drawPlayer.RotatedRelativePoint(drawPlayer.MountedCenter) - drawPlayer.Center);
                    if (heldItem.useStyle == ItemUseStyleID.Shoot)
                    {
                        if (Item.staff[heldItem.type])
                        {
                            float rotationMod = MathHelper.PiOver4 * -drawPlayer.direction * drawPlayer.gravDir;
                            DrawData staffDraw = new DrawData(
                                texture,
                                basePosition,
                                default,
                                color,
                                drawPlayer.itemRotation - rotationMod,
                                new Vector2(drawPlayer.direction == -1 ? texture.Width : 0,
                                drawPlayer.gravDir == 1 ? texture.Height : 0),
                                drawPlayer.GetAdjustedItemScale(heldItem),
                                drawSet.itemEffect
                                );
                            drawSet.DrawDataCache.Add(staffDraw);

                        }
                        else
                        {
                            Vector2 offsetFix = new Vector2(texture.Width / 2, texture.Height / 2 + glowmaskItem.GlowmaskOffsetY * drawPlayer.gravDir);
                            int glowOffsetXInvert = -glowmaskItem.GlowmaskOffsetX;
                            Vector2 positionFix = new Vector2(drawPlayer.direction == -1 ? texture.Width - glowOffsetXInvert : glowOffsetXInvert, texture.Height / 2);

                            DrawData horizontalStaffDraw = new DrawData(
                                texture,
                                basePosition + offsetFix,
                                default,
                                color,
                                drawPlayer.itemRotation,
                                positionFix,
                                drawPlayer.GetAdjustedItemScale(heldItem),
                                drawSet.itemEffect
                                );
                            drawSet.DrawDataCache.Add(horizontalStaffDraw);
                        }
                    }
                    else
                    {
                        DrawData swingDraw = new DrawData(
                            texture,
                            basePosition,
                            default,
                            color,
                            drawPlayer.itemRotation,
                            new Vector2(drawPlayer.direction == -1 ? texture.Width : 0,
                            drawPlayer.gravDir == 1 ? texture.Height : 0),
                            drawPlayer.GetAdjustedItemScale(heldItem),
                            drawSet.itemEffect
                            );
                        drawSet.DrawDataCache.Add(swingDraw);
                    }
                }
            }
        }
    }

    public class GlowmaskPlayerDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawSet)
        {
            IGlowmask.Draw(ref drawSet);
        }
    }
}
