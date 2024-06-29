using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using SagesAndMystics.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace SagesAndMystics.Content.NPCs.Critters
{
    public class StarBunny : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];

            Main.npcCatchable[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;

            NPCID.Sets.TownCritter[Type] = true;
            NPCID.Sets.CountsAsCritter[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bunny);
            AnimationType = NPCID.Bunny;
            AIType = NPCID.Bunny;

            NPC.lifeMax = 30;
            NPC.friendly = false;

            NPC.catchItem = ModContent.ItemType<Items.Consumables.Critters.StarBunny>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.SagesAndMystics.Bestiary.StarBunny"))
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
                return;

            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity.RotatedByRandom(MathHelper.Pi / 16), Mod.Find<ModGore>($"{Name}_Gore_Head").Type);
            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity.RotatedByRandom(MathHelper.Pi / 16), Mod.Find<ModGore>($"{Name}_Gore_Rump").Type);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Stardust>(), default, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarBunnyFur>(), 2, 1, 2));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //Vanilla draw code
            float someNumModdedNPCsArentUsing = 0f;
            float drawOffsetY = Main.NPCAddHeight(NPC);
            Asset<Texture2D> asset = ModContent.Request<Texture2D>(Texture + "_Glow");
            Texture2D texture = asset.Value;

            Vector2 halfSize = new Vector2(asset.Width() / 1 / 2, asset.Height() / Main.npcFrameCount[NPC.type] / 2);

            SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 halfSizeOff = halfSize * NPC.scale;
            Vector2 textureOff = new Vector2(asset.Width() * NPC.scale / 1 / 2f, asset.Height() * NPC.scale / Main.npcFrameCount[NPC.type]);
            Vector2 drawPosition = new Vector2(NPC.Center.X, NPC.Bottom.Y + drawOffsetY + someNumModdedNPCsArentUsing + NPC.gfxOffY + 4f);
            spriteBatch.Draw(texture, drawPosition + halfSizeOff - textureOff - screenPos, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, halfSize, NPC.scale, effects, 0f);

            Lighting.AddLight(NPC.Center, new Vector3(0.816f, 0.78f, 0.945f));
        }
    }
}
