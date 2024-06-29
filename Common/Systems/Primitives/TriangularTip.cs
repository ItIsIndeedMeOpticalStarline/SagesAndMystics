using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace SagesAndMystics.Common.Systems.Primitives
{
    public class TriangularTip : IRibbonTip
    {
        private const float BaseWidth = 30f;

        private readonly float length;
        
        public int ExtraVertices => 3;

        public int ExtraIndices => 3;

        public TriangularTip(float length)
        {
            this.length = length;
        }

        public void GenerateMesh
        (
            Vector2 ribbonTipPosition, 
            Vector2 ribbonTipNormal, 
            int startFromIndex, 
            out VertexPositionColorTexture[] vertices, 
            out short[] indices, 
            WidthFunction widthModifier, 
            ColorFunction colorModifier
        )
        {
            Vector2 normalPerpendicular = ribbonTipNormal.RotatedBy(MathHelper.PiOver2);

            float width = widthModifier?.Invoke(1f) ?? BaseWidth;

            Vector2 leftPoint = ribbonTipPosition + normalPerpendicular * width;
            Vector2 rightPoint = ribbonTipPosition - normalPerpendicular * width;
            Vector2 upperPoint = ribbonTipPosition + ribbonTipNormal * length;

            Vector2 leftTexCoord = Vector2.UnitX;
            Vector2 rightTexCoord = Vector2.One;
            Vector2 upperTexCoord = new Vector2(1f, 0.5f);

            Color leftColor = colorModifier?.Invoke(leftTexCoord) ?? new Color(1f, 1f, 1f, 0f);
            Color rightColor = colorModifier?.Invoke(rightTexCoord) ?? new Color(1f, 1f, 1f, 0f);
            Color upperColor = colorModifier?.Invoke(upperTexCoord) ?? new Color(1f, 1f, 1f, 0f);

            vertices =
            [
                new(new Vector3(leftPoint, 0f), leftColor, leftTexCoord),
                new(new Vector3(rightPoint, 0f), rightColor, rightTexCoord),
                new(new Vector3(upperPoint, 0f), upperColor, upperTexCoord),
            ];

            indices =
            [
                (short)startFromIndex,
                (short)(startFromIndex + 1),
                (short)(startFromIndex + 2)
            ];
        }
    }
}
