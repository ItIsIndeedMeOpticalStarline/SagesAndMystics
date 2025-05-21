using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SagesAndMystics.Content.Dusts;
using SagesAndMystics.Content.Items.Placeable.Crafting;
using System;
using Terraria.Audio;
using SagesAndMystics.Content.Items.Materials.Seeds;

namespace SagesAndMystics.Content.Projectiles.PreHardmode.Ranged
{
    public class SagesWater : ModProjectile
    {
        public bool Exploded
        {
            get => Projectile.localAI[1] == 1f;
            set => Projectile.localAI[1] = value == true ? 1f : 0f;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 2;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

        }

        private bool CloseEnoughToTransmute(Vector2 pos1, Vector2 pos2)
        {
            if (Math.Abs(pos1.X) - Math.Abs(pos2.X) + Math.Abs(pos2.Y) - Math.Abs(pos2.Y) < 30f)
                return true;

            return false;
        }

        private void Explode()
        {
            if (!Exploded)
            {
                Projectile.tileCollide = false;
                Projectile.timeLeft = 3;
                Projectile.Resize(Projectile.width * 6, Projectile.height * 6);

                Exploded = true;
            }
        }

        private void SpawnTransmutationDust(Vector2 pos, int width, int height)
        {
            for (int j = 0; j < 15; j++)
            {
                Dust.NewDust(pos, width, height, DustID.Enchanted_Gold);
            }
        }

        private void TransmuteTo(Item baseItem, int resultItemType)
        {
            Item.NewItem(Projectile.GetSource_FromAI(), baseItem.position, resultItemType, baseItem.stack, false, 0, true);
            SpawnTransmutationDust(baseItem.position, baseItem.width, baseItem.height);
            baseItem.active = false;
        }

        private bool TryToTransmuteItem(Item item)
        {
            if (!item.active)
                return false;

            switch (item.type)
            {
                case ItemID.WorkBench:
                    {
                        if (CloseEnoughToTransmute(Projectile.Center, item.Center))
                        {
                            TransmuteTo(item, ModContent.ItemType<SagesWorkbench>());
                            return true;
                        }
                    }
                    break;
                case ItemID.GrassSeeds:
                    {
                        if (CloseEnoughToTransmute(Projectile.Center, item.Center))
                        {
                            TransmuteTo(item, ModContent.ItemType<MandrakeRootSeeds>());
                            return true;
                        }
                    }
                    break;
                case ItemID.Bunny:
                    {
                        if (CloseEnoughToTransmute(Projectile.Center, item.Center))
                        {
                            TransmuteTo(item, ModContent.ItemType<Items.Consumables.Critters.StarBunny>());
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        private void TransmuteTo(NPC baseNPC, int resultNPCType)
        {
            NPC.NewNPC(Projectile.GetSource_FromAI(), (int)baseNPC.Center.X, (int)baseNPC.Center.Y, resultNPCType);
            SpawnTransmutationDust(baseNPC.position, baseNPC.width, baseNPC.height);
            baseNPC.dontTakeDamage = true;
            baseNPC.active = false;
        }

        private bool TryToTransmuteNPC(NPC npc)
        {
            if (!npc.active)
                return false;

            switch (npc.type)
            {
                case NPCID.Bunny:
                    if (CloseEnoughToTransmute(Projectile.Center, npc.Center))
                    {
                        TransmuteTo(npc, ModContent.NPCType<NPCs.Critters.StarBunny>());
                        return true;
                    }
                    break;
                case NPCID.Worm:
                    if (CloseEnoughToTransmute(Projectile.Center, npc.Center))
                    {
                        TransmuteTo(npc, NPCID.EnchantedNightcrawler);
                        return true;
                    }
                    break;
            }

            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Explode();

            TryToTransmuteNPC(target);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Main.maxItems; i++)
            {
                ref Item item = ref Main.item[i];

                TryToTransmuteItem(item);
            }

            SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Glass);
            }
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SagesWaterDust>(), 0f, -2f, 0, default, 1.1f)];

                dust.alpha = 100;
                dust.velocity.X *= 1.5f;
                dust.velocity *= 3f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Exploded)
                return false;

            return true;
        }
    }
}
