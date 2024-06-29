using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;
using Terraria.Utilities;
using System;
using SagesAndMystics.Content.Items.Placeable.Crafting;
using Terraria.GameContent.UI;
using SagesAndMystics.Content.EmoteBubbles;
using SagesAndMystics.Common;
using SagesAndMystics.Content.Items.Consumables.Critters;
using SagesAndMystics.Content.Projectiles.PreHardmode.Ranged;
using static SagesAndMystics.Content.Directions;

namespace SagesAndMystics.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Sage : ModNPC
    {
        public bool newHelpTextAvailable;
        private int newHelpTextAvailableCounter;

        public bool pendingNewHelpTextCheck;

        public override void Load()
        {
            base.Load();
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;

            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 10;
            NPCID.Sets.HatOffsetY[Type] = 4;

            NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<SageEmote>();

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Direction = ToValue(Direction.Right)
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Love)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Wizard, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Mechanic, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Cyborg, AffectionLevel.Hate);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; // Sets NPC to be a Town NPC
            NPC.friendly = true; // NPC Will not attack player
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,

                new FlavorTextBestiaryInfoElement("Mods.SagesAndMystics.Bestiary.Sage")
            });
        }

        public override void PostAI()
        {
            if (Main.dedServ)
                return;

            if (newHelpTextAvailable)
                newHelpTextAvailableCounter++;
            else
                newHelpTextAvailableCounter = 0;

            if (pendingNewHelpTextCheck)
            {
                SagesAndMysticsPlayer player = Main.LocalPlayer.GetModPlayer<SagesAndMysticsPlayer>();

                if (NPC.downedSlimeKing && !player.unlockedKingSlimeTip)
                {
                    player.unlockedKingSlimeTip = true;
                    newHelpTextAvailable = true;
                }

                pendingNewHelpTextCheck = false;
                newHelpTextAvailableCounter = 0;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < (NPC.life > 0 ? 1 : 5); i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
            }

            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                int hatGore = NPC.GetPartyHatGore();
                int headGore = Mod.Find<ModGore>($"{Name}_Gore_Head").Type;
                int armGore = Mod.Find<ModGore>($"{Name}_Gore_Arm").Type;
                int legGore = Mod.Find<ModGore>($"{Name}_Gore_Leg").Type;

                if (hatGore > 0)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.ConsumedManaCrystals > 0)
                    return true;
            }

            return false;
        }

        public override ITownNPCProfile TownNPCProfile() => new SageProfile();

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Atlas",
            };
        }

        public override void FindFrame(int frameHeight) { }

        public override string GetChat()
        {
            helpOption = 0;

            if (newHelpTextAvailable)
            {
                newHelpTextAvailable = false;
                return Language.GetTextValue("Mods.SagesAndMystics.Dialogue.Sage.NewTextAvailable", Main.LocalPlayer.name);
            }

            WeightedRandom<string> chat = new ();

            int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
            if (witchDoctor >= 0)
                chat.Add(Language.GetTextValue("Mods.SagesAndMystics.Dialogue.Sage.WitchDoctor", Main.npc[witchDoctor].GivenName));

            int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
            if (cyborg >= 0)
                chat.Add(Language.GetTextValue("Mods.SagesAndMystics.Dialogue.Sage.Cyborg", Main.npc[cyborg].GivenName));
            
            chat.Add(Language.GetTextValue("Mods.SagesAndMystics.Dialogue.Sage.Greeting", Main.LocalPlayer.name));

            string chosenChat = chat;

            return chosenChat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (helpOption < 0)
                helpOption = 0;
            else if (helpOption > HelpOptionID.Count)
                helpOption = HelpOptionID.Count;

            button = helpOption == 0
                ? Language.GetTextValue("LegacyInterface.51")
                : helpOption > 1
                    ? Language.GetTextValue("Mods.SagesAndMystics.Dialogue.ChatOptions.Sage.PrevHelp")
                    : "";


            button2 = helpOption > 0 && helpOption < HelpOptionID.Count
                ? Language.GetTextValue("Mods.SagesAndMystics.Dialogue.ChatOptions.Sage.NextHelp")
                : "";
        }

        public static class HelpOptionID
        {
            public const int Rituals = 1;
            public const int SagesWater = 2;
            public const int SagesWorkbench = 3;
            public const int StarBunny = 4;

            public const int Count = 4;

            public static string GetHelpKey(int id)
            {
                string key = "Mods.SagesAndMystics.Dialogue.Sage.Help";

                return key + id + id switch
                {
                    _ => ""
                };
            }

            public static string GetHelpText(int id) => Language.GetTextValue(GetHelpKey(id));

            public static int GetHelpItem(int id)
            {
                return id switch
                {
                    SagesWater => ModContent.ItemType<Items.Weapons.PreHardmode.Ranged.SagesWater>(),
                    SagesWorkbench => ModContent.ItemType<SagesWorkbench>(),
                    StarBunny => ModContent.ItemType<StarBunny>(),
                    _ => 0
                };
            }
        }

        public int helpOption;

        public static readonly int[] helpOptionsByIndex = [
            HelpOptionID.Rituals,
            HelpOptionID.SagesWater,
            HelpOptionID.SagesWorkbench,
            HelpOptionID.StarBunny
        ];

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            ref int savedTip = ref Main.LocalPlayer.GetModPlayer<SagesAndMysticsPlayer>().sageHelpTip;

            bool wasHelpOptionUninitialized = false;
            if (helpOption == 0)
            {
                if (!SagesAndMysticsConfig.DisplayLastSeenSageTip)
                    savedTip = 0;

                helpOption = savedTip;
                wasHelpOptionUninitialized = true;
            }

            if (!wasHelpOptionUninitialized)
            {
                if (firstButton)
                    helpOption--;
                else
                    helpOption++;
            }

            if (helpOption > HelpOptionID.Count)
                helpOption = HelpOptionID.Count;
            else if (helpOption < 1)
                helpOption = 1;

            savedTip = helpOption;

            int option = helpOptionsByIndex[helpOption - 1];

            Main.npcChatText = HelpOptionID.GetHelpText(option);
            Main.npcChatCornerItem = HelpOptionID.GetHelpItem(option);
        }

        public override bool CanGoToStatue(bool toKingStatue) => true;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<SagesWater>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override int? PickEmote(Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
        {
            int type = 0;

            if (otherAnchor.entity is NPC { type: NPCID.Cyborg })
                type = EmoteID.EmotionAnger;

            for (int i = 0; i < 6; i++)
                emoteList.Add(type);

            return base.PickEmote(closestPlayer, emoteList, otherAnchor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (newHelpTextAvailable)
            {
                Texture2D exclamation = TextureAssets.Extra[48].Value;

                Rectangle source = exclamation.Frame(8, 39, newHelpTextAvailableCounter % 60 < 30 ? 6 : 7, 1);

                Vector2 center = NPC.Top - new Vector2(0, source.Height * 0.75f) - Main.screenPosition;

                double sin = (Math.Sin(newHelpTextAvailableCounter / 60d * MathHelper.TwoPi * 0.65) + 1) / 2;

                center.Y += (float)(-5 * Math.Sin(newHelpTextAvailableCounter / 60d * MathHelper.TwoPi * 0.4));

                float transparency = (float)(0.75 + 0.25 * sin);

                spriteBatch.Draw(exclamation, center, source, Color.White * transparency, 0, source.Size() / 2f, 1.5f, SpriteEffects.None, 0);
            }
        }
    }

    public class SageProfile : ITownNPCProfile
    {
        public int RollVariation() => 0;

        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>("SagesAndMystics/Content/NPCs/TownNPCs/Sage");

        public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("SagesAndMystics/Content/NPCs/TownNPCs/Sage_Head");
    }
}
