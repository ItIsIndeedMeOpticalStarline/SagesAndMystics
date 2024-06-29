using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria;

namespace SagesAndMystics.Content.NPCs
{
    public static partial class NPCProjectiles
    {
        /// <summary>
        /// Fires [numProjectiles] projectiles, evenly spaced across an arc of [degrees] degrees
        /// </summary>
        /// <param name="source">Source of the projectiles</param>
        /// <param name="pos">Position where the projectiles spawn</param>
        /// <param name="dest">Target the projectiles are fired at if [aimed] == true</param>
        /// <param name="speed">Speed of the projectiles</param>
        /// <param name="type">Type of projectiles</param>
        /// <param name="damage">Damage of projectiles</param>
        /// <param name="knockback">Knockback of projectiles</param>
        /// <param name="degrees">The central angle of the arc</param>
        /// <param name="numProjectiles">The number of projectiles spread across the arc</param>
        /// <param name="owner">The projectile's owner</param>
        /// <param name="aimed">Whether or not the fan of projectiles is fired at [dest]</param>
        /// <param name="ai0">Default value for the spawned projectile's AI[0]</param>
        /// <param name="ai1">Default value for the spawned projectile's AI[0]</param>
        /// <returns>An array of size [numProjectiles] containing the indices of the projectiles in the Main.projectile array</returns>
        public static int[] MultiProjectile
        (
            IEntitySource source, 
            Vector2 pos, 
            Vector2 dest, 
            float speed, 
            int type, 
            int damage, 
            int knockback, 
            float degrees, 
            int numProjectiles, 
            bool aimed = true, 
            int owner = -1, 
            float ai0 = 0f, 
            float ai1 = 0f, 
            float ai2 = 0f
        )
        {
            float offset;
            Vector2 rotation;

            if (aimed)
            {
                if (numProjectiles % 2 == 0)
                    offset = 0f;
                else
                    offset = degrees / numProjectiles / 2f;

                rotation = (dest - pos).SafeNormalize(-Vector2.UnitY);
            }
            else
            {
                offset = degrees / numProjectiles / 2f;
                rotation = Vector2.One.SafeNormalize(-Vector2.UnitY);
            }

            int[] projs = new int[numProjectiles];
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector2 velocity = rotation.RotatedBy(MathHelper.ToRadians(degrees / 2f * -1f + degrees / numProjectiles * i + offset)) * speed;
                projs[i] = Projectile.NewProjectile(source, pos, velocity, type, damage, knockback, owner, ai0, ai1, ai2);
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, NetworkText.Empty, projs[i]);
                }
            }

            return projs;
        }
    }
}
