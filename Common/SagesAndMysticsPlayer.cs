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

        public bool headphones = false;
        public bool saltPouch = false;

        public override void ResetEffects()
        {
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

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (headphones && proj.type == ModContent.ProjectileType<MandrakeCry>())
                return false;

            return true;
        }
    }
}
