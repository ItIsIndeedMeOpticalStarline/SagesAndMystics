using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SagesAndMystics.Content.Projectiles.PreHardmode.Hostile;

namespace SagesAndMystics.Content.NPCs.PreHardmode
{
    public class MandrakeRoot : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 32;

            NPC.damage = 14;
            NPC.defense = 6;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0.5f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;

            NPC.value = 0;
            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;

            AnimationType = NPCID.Zombie;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.SagesAndMystics.Bestiary.MandrakeRoot"))
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.Herbs.MandrakeRoot>()));
        }

        public override void AI()
        {
            if (Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) <= 200f)
            {
                if (Main.netMode != NetmodeID.Server && !Terraria.Graphics.Effects.Filters.Scene["Shockwave"].IsActive())
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MandrakeCry>(), NPC.damage, 3.0f, default, NPC.whoAmI);
            }
        }
    }
}
