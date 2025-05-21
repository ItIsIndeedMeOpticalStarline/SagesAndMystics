using Microsoft.Xna.Framework;
using SagesAndMystics.Content.Items.Accessories.PreHardmode;
using SagesAndMystics.Content.Projectiles.PreHardmode.Generic;
using SagesAndMystics.Content.Projectiles.PreHardmode.Hostile;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SagesAndMystics.Common
{
    public class SagesAndMysticsPlayer : ModPlayer
    {
        internal bool unlockedKingSlimeTip;
        internal int sageHelpTip;

        internal const int SAVE_VERSION = 1;

        // Accessories

        public bool flaresproutBoots = false;
        public Item flaresproutBootsItemInstance = null;

        public bool headphones = false;
        public bool saltPouch = false;

        public override void ResetEffects()
        {
            flaresproutBoots = false;
            flaresproutBootsItemInstance = null;

            headphones = false;
            saltPouch = false;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["sage"] = sageHelpTip;

            BitsByte unlocked = new BitsByte(unlockedKingSlimeTip);
            tag["unlocked"] = (byte)unlocked;

            tag["version"] = SAVE_VERSION;
        }

        public override void LoadData(TagCompound tag)
        {
            sageHelpTip = tag.GetInt("version") == SAVE_VERSION ? tag.GetInt("sage") : 0;

            BitsByte unlocked = tag.GetByte("unlocked");
            unlocked.Retrieve(ref unlockedKingSlimeTip);
        }

        public override void PostUpdateEquips()
        {
            if (flaresproutBoots && flaresproutBootsItemInstance != null &&
                Player.miscCounter % 10 == 0 &&
                Player.velocity.Y == 0f && Player.grappling[0] == -1 && Player.velocity.X != 0f &&
                Main.myPlayer == Player.whoAmI &&
                !Player.wet)
            {
                int projectileType = ModContent.ProjectileType<Flaresprout>();
                int damage = 30;
                int frameHeight = Flaresprout.FrameHeight;
                if (flaresproutBootsItemInstance.type == ModContent.ItemType<TerrasproutTreads>())
                {
                    projectileType = ModContent.ProjectileType<Terrasprout>();
                    damage += 10;
                    frameHeight = Terrasprout.FrameHeight;
                }

                Projectile.NewProjectile
                (
                    Player.GetSource_Accessory(flaresproutBootsItemInstance),
                    new Vector2(Player.Center.X, Player.position.Y + (Player.height - frameHeight / 2)),
                    Vector2.Zero,
                    projectileType,
                    damage,
                    1f,
                    Main.myPlayer
                );
            }
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (headphones && proj.type == ModContent.ProjectileType<MandrakeCry>())
                return false;

            return true;
        }
    }
}
