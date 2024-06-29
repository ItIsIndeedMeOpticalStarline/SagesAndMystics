using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;

namespace SagesAndMystics.Content.NPCs
{
    public static partial class NPCMovement
    {
        public static void FlyTo(this ModNPC npc, Vector2 destination, in Vector2 acceleration, in Vector2 maxVelocity)
        {
            Vector2 desiredVelocity = npc.NPC.DirectionTo(destination) * maxVelocity;

            if (npc.NPC.velocity.X < desiredVelocity.X)
            {
                npc.NPC.velocity.X += acceleration.X;

                if (npc.NPC.velocity.X < 0f && desiredVelocity.X > 0f)
                    npc.NPC.velocity.X += acceleration.X;
            }
            else if (npc.NPC.velocity.X > desiredVelocity.X)
            {
                npc.NPC.velocity.X -= acceleration.X;

                if (npc.NPC.velocity.X > 0f && desiredVelocity.X < 0f)
                    npc.NPC.velocity.X -= acceleration.X;
            }
            if (npc.NPC.velocity.Y < desiredVelocity.Y)
            {
                npc.NPC.velocity.Y += acceleration.Y;

                if (npc.NPC.velocity.Y < 0f && desiredVelocity.Y > 0f)
                    npc.NPC.velocity.Y += acceleration.Y;
            }
            else if (npc.NPC.velocity.Y > desiredVelocity.Y)
            {
                npc.NPC.velocity.Y -= acceleration.Y;

                if (npc.NPC.velocity.Y > 0f && desiredVelocity.Y < 0f)
                    npc.NPC.velocity.Y -= acceleration.Y;
            }
        }

        public static void StopIfSlow(this ModNPC npc, float threshold)
        {
            if (MathF.Abs(npc.NPC.velocity.X) <= threshold)
                npc.NPC.velocity.X = 0;
            if (MathF.Abs(npc.NPC.velocity.Y) <= threshold)
                npc.NPC.velocity.Y = 0;
        }

        public static void StopIfSlow(this ModNPC npc, Vector2 threshold)
        {
            if (MathF.Abs(npc.NPC.velocity.X) <= threshold.X)
                npc.NPC.velocity.X = 0;
            if (MathF.Abs(npc.NPC.velocity.Y) <= threshold.Y)
                npc.NPC.velocity.Y = 0;
        }
    }
}
