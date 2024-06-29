using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SagesAndMystics.Common.Systems.Primitives
{
    public delegate float WidthFunction(float factorAlongRibbon);

    public delegate Color ColorFunction(Vector2 textureCoordinates);

    public class Ribbon
    {
        private readonly Primitives primitives;

        private readonly int maxPoints;

        private readonly IRibbonTip tip;

        private readonly WidthFunction widthModifier;

        private readonly ColorFunction colorModifier;
        private const float BaseWidth = 30f;

        public Vector2[] positions;

        public Vector2 NextPosition { get; set; }

        public BasicEffect effect;

        public GraphicsDevice Device { get => primitives.Device; }

        public Ribbon(GraphicsDevice device, int maxPoints, IRibbonTip tip, WidthFunction widthModifier, ColorFunction colorModifier)
        {
            this.tip = tip ?? new NoTip();
            this.maxPoints = maxPoints;
            this.widthModifier = widthModifier;
            this.colorModifier = colorModifier;

            Main.RunOnMainThread(() =>
            {
                effect = new BasicEffect(device);
                effect.VertexColorEnabled = true;
                effect.TextureEnabled = true;
            });

            /* A---B---C
             * |  /|  /|
             * D / E / F
             * |/  |/  |
             * G---H---I
             * 
             * Let D, E, F, etc. be the set of n points that define the trail.
             * Since each point generates 2 vertices, there are 2n vertices, plus the tip's count.
             * 
             * As for indices - in the region between 2 defining points there are 2 triangles.
             * The amount of regions in the whole trail are given by n - 1, so there are 2(n - 1) triangles for n points.
             * Finally, since each triangle is defined by 3 indices, there are 6(n - 1) indices, plus the tip's count.
             */

            primitives = new Primitives(device, maxPoints * 2 + this.tip.ExtraVertices, 6 * (maxPoints - 1) + this.tip.ExtraIndices);
        }

        private void GenerateMesh(out VertexPositionColorTexture[] vertices, out short[] indices, out int nextAvailableIndex)
        {
            VertexPositionColorTexture[] calculatedVertices = new VertexPositionColorTexture[maxPoints * 2];
            short[] calculatedIndices = new short[(maxPoints - 1) * 6];

            for (int i = 0; i < positions.Length; i++)
            {
                float factorAlongRibbon = (float)i / (positions.Length - 1);

                float width = widthModifier?.Invoke(factorAlongRibbon) ?? BaseWidth;

                Vector2 curr = positions[i];
                Vector2 next = i == positions.Length - 1 ? positions[^1] + (positions[^1] - positions[^2]) : positions[i + 1];

                //if (curr != Vector2.Zero && next != Vector2.Zero)
                //{
                    Vector2 toNext = (next - curr).SafeNormalize(Vector2.Zero);
                    Vector2 toNextPerpendicular = toNext.RotatedBy(MathHelper.PiOver2);

                    Vector2 upperPoint = curr + toNextPerpendicular * width;
                    Vector2 lowerPoint = curr - toNextPerpendicular * width;

                    Vector2 upperTexCoord = new Vector2(factorAlongRibbon, 0);
                    Vector2 lowerTexCoord = new Vector2(factorAlongRibbon, 1);

                    Color upperColor = colorModifier?.Invoke(upperTexCoord) ?? new Color(1f, 1f, 1f, 0f);
                    Color lowerColor = colorModifier?.Invoke(lowerTexCoord) ?? new Color(1f, 1f, 1f, 0f);

                    calculatedVertices[i] = new VertexPositionColorTexture(new Vector3(upperPoint, 0f), upperColor, upperTexCoord);
                    calculatedVertices[i + maxPoints] = new VertexPositionColorTexture(new Vector3(lowerPoint, 0f), lowerColor, lowerTexCoord);
                //}
            }

            for (short i = 0; i < maxPoints - 1; i++)
            {
                /* 0---1
                 * |  /|
                 * A / B
                 * |/  |
                 * 2---3
                 * 
                 * This illustration is the most basic set of points (where n = 2).
                 * In this, we want to make triangles (2, 3, 1) and (1, 0, 2).
                 * Generalising this, if we consider A to be k = 0 and B to be k = 1, then the indices we want are going to be (k + n, k + n + 1, k + 1) and (k + 1, k, k + n)
                 */

                calculatedIndices[i * 6] = (short)(i + maxPoints);
                calculatedIndices[i * 6 + 1] = (short)(i + maxPoints + 1);
                calculatedIndices[i * 6 + 2] = (short)(i + 1);
                calculatedIndices[i * 6 + 3] = (short)(i + 1);
                calculatedIndices[i * 6 + 4] = i;
                calculatedIndices[i * 6 + 5] = (short)(i + maxPoints);
            }

            nextAvailableIndex = calculatedVertices.Length;

            vertices = calculatedVertices;

            indices = calculatedIndices;
        }

        public void Render()
        {
            if (positions == null)
                return;

            GenerateMesh(out VertexPositionColorTexture[] generatedVertices, out short[] generatedIndices, out int nextAvailableIndex);

            Vector2 toNext = (NextPosition - positions[^1]).SafeNormalize(Vector2.Zero);

            tip.GenerateMesh(positions[^1], toNext, nextAvailableIndex, out VertexPositionColorTexture[] tipVertices, out short[] tipIndices, widthModifier, colorModifier);

            VertexPositionColorTexture[] combinedVertices = new VertexPositionColorTexture[generatedVertices.Length + tipVertices.Length];
            Array.Copy(generatedVertices, combinedVertices, generatedVertices.Length);
            Array.Copy(tipVertices, 0, combinedVertices, generatedVertices.Length, tipVertices.Length);

            short[] combinedIndices = new short[generatedIndices.Length + tipIndices.Length];
            Array.Copy(generatedIndices, combinedIndices, generatedIndices.Length);
            Array.Copy(tipIndices, 0, combinedIndices, generatedIndices.Length, tipIndices.Length);

            primitives.SetVertices(combinedVertices);
            primitives.SetIndices(combinedIndices);

            primitives.Render(effect);
        }
    }
}
