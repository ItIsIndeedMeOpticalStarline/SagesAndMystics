using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SagesAndMystics.Common.Systems.Primitives
{
    public class NoTip : IRibbonTip
    {
        public int ExtraVertices => 0;

        public int ExtraIndices => 0;

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
            vertices = Array.Empty<VertexPositionColorTexture>();
            indices = Array.Empty<short>();
        }
    }
}
