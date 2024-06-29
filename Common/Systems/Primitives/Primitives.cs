using Microsoft.Xna.Framework.Graphics;

namespace SagesAndMystics.Common.Systems.Primitives
{
    public class Primitives
    {
        private readonly GraphicsDevice device;

        public GraphicsDevice Device { get => device; }

        private VertexPositionColorTexture[] vertices;

        private short[] indices;

        public Primitives(GraphicsDevice device, int maxVertices, int maxIndices)
        {
            this.device = device;
            vertices = new VertexPositionColorTexture[maxVertices];
            indices = new short[maxIndices];
        }

        public void Render(BasicEffect effect)
        {
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);
            }
        }

        public void SetVertices(VertexPositionColorTexture[] vertices) { this.vertices = vertices; }
        
        public void SetIndices(short[] indices) { this.indices = indices; }
    }
}
