using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SagesAndMystics.Common.Systems.Primitives
{
    public interface IRibbonTip
    {
        int ExtraVertices { get; }

        int ExtraIndices { get; }

        void GenerateMesh
        (
            Vector2 ribbonTipPosition,
            Vector2 ribbonTipNormal,
            int startFromIndex,
            out VertexPositionColorTexture[] vertices,
            out short[] indices,
            WidthFunction widthModifier,
            ColorFunction colorModifier
        );
    }
}
