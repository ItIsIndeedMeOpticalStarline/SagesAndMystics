using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using SagesAndMystics.Content.Projectiles.PreHardmode.Hostile.Bosses.Baku;
using SagesAndMystics.Common.Systems.Primitives;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SagesAndMystics.Content.NPCs.PreHardmode.Bosses.Baku
{
    public class Baku : ModNPC
    {
        private enum State
        {
            None,
            Follow,
            FollowAndFire,
            DreamNeedle,
            ExpertDreamOrbs,
            MasterCallNightmare
        }

        private bool Initalized
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value == true ? 1f : 0f;
        }
        private bool IntroDone
        {
            get => NPC.ai[1] == 1f;
            set => NPC.ai[1] = value == true ? 1f : 0f;
        }

        private bool Phase2Initalized
        {
            get => NPC.ai[2] == 1f;
            set => NPC.ai[2] = value == true ? 1f : 0f;
        }

        private uint StateTimer
        {
            get => (uint)NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        private State currentState, previousState;
        private int restrictedStates;
        private bool stateInitalized;

        private Vector2 acceleration, maxVelocity;

        private int[] seals;
        private Ribbon[] sealsEffects;
        private const int NumSealsEffectPoints = 30;
        private Vector2[] sealsEffectPoints;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type); // TODO: Add bestiary sprite

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 102;
            NPC.height = 64;
            NPC.alpha = 255;

            NPC.damage = 14;
            NPC.defense = 14;
            NPC.lifeMax = 1600;
            NPC.knockBackResist = 0f;

            NPC.aiStyle = -1;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.npcSlots = 5f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            Music = MusicID.Boss5;

            NPC.value = 0;

            NPC.boss = true;

            currentState = previousState = State.None;

            restrictedStates = int.MinValue;

            stateInitalized = false;

            acceleration = Vector2.Zero;
            maxVelocity = Vector2.Zero;

            seals = new int[3];
            sealsEffects = new Ribbon[seals.Length];
            sealsEffectPoints = new Vector2[seals.Length * NumSealsEffectPoints];
    }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];

            if (player.dead || !player.active)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                player = Main.player[NPC.target];

                if (player.dead || !player.active)
                {
                    NPC.velocity.Y -= 0.04f;

                    MessageSeals(BakuSeal.Action.None);

                    if (NPC.timeLeft > 10)
                        NPC.timeLeft = 10;

                    return;
                }
            }

            if (!Initalized)
            {
                for (int i = 0; i < sealsEffects.Length; i++)
                {
                    sealsEffects[i] = new Ribbon(Main.graphics.GraphicsDevice, NumSealsEffectPoints, null, null, f => new Color(251, 111, 255, 0));
                }

                if (Main.getGoodWorld)
                {
                    restrictedStates = 1;
                    acceleration = new Vector2(0.7f, 0.7f);
                    maxVelocity = new Vector2(4f);
                }
                else if (Main.masterMode)
                {
                    restrictedStates = 1;
                    acceleration = new Vector2(0.555f, 0.55f);
                    maxVelocity = new Vector2(4f);
                }
                else if (Main.expertMode)
                {
                    restrictedStates = 2;
                    acceleration = new Vector2(0.55f, 0.5f);
                    maxVelocity = new Vector2(4.5f);
                }
                else
                {
                    restrictedStates = 3;
                    acceleration = new Vector2(0.45f, 0.4f);
                    maxVelocity = new Vector2(3.5f);
                }

                NPC.dontTakeDamage = true;
                NPC.Center = player.Center + new Vector2(0f, player.height * -3);

                Initalized = true;
            }

            if (NPC.life < NPC.lifeMax / 2 && !Phase2Initalized)
            {
                restrictedStates -= 1;
                acceleration += new Vector2(0.5f);

                Phase2Initalized = true;
            }

            if (!IntroDone)
            {
                const int numParticles = 15;
                float radius = MathHelper.Max(NPC.width, NPC.height) * 1.5f;
                if (--NPC.alpha > 0)
                {
                    for (int i = 0; i < numParticles; i++)
                    {
                        float newRadius = (-1f + 2f / 255f * NPC.alpha) * radius;
                        Dust.NewDustPerfect(NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / numParticles * i + NPC.alpha) * newRadius, DustID.DungeonSpirit);
                    }
                }
                else
                {
                    NPC.dontTakeDamage = false;
                    IntroDone = true;
                }

                return;
            }

            if (currentState == State.None)
            {
                List<State> avalibleStates = [ State.Follow, State.FollowAndFire, State.DreamNeedle, State.ExpertDreamOrbs, State.MasterCallNightmare ];

                avalibleStates = avalibleStates[..^restrictedStates];

                avalibleStates.Remove(previousState);
                
                if (NPC.AnyNPCs(ModContent.NPCType<Nightmare>()))
                    avalibleStates.Remove(State.MasterCallNightmare);

                currentState = avalibleStates[Main.rand.Next(0, avalibleStates.Count)];

                StateTimer = 0;
                stateInitalized = false;
            }

            switch (currentState)
            {
                case State.Follow: Follow(); break;
                case State.FollowAndFire: FollowAndFire(); break;
                case State.DreamNeedle: DreamNeedle(); break;
                case State.ExpertDreamOrbs: DreamOrbs(); break;
                case State.MasterCallNightmare: CallNightmare(); break;
                default:
                {
                    currentState = State.None;
                } break;
            }
        }

        private void MessageSeals(BakuSeal.Action sealAI)
        {
            float radius = MathHelper.Max(NPC.width, NPC.height) * 1.5f;

            for (int i = 0; i < seals.Length; i++)
            {
                Projectile seal = Main.projectile[seals[i]];
                Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(360f / seals.Length * i + StateTimer)) * radius;
                seal.Center = pos;
                seal.ai[1] = (float)sealAI;
            }

            for (int i = 0; i < seals.Length; i++) // Link seals with ribbon
            {
                Projectile seal = Main.projectile[seals[i]];
                for (int j = 0; j < NumSealsEffectPoints; j++)
                {
                    sealsEffectPoints[i* NumSealsEffectPoints + j] = Vector2.Lerp(seal.Center, Main.projectile[seals[i + 1 >= seals.Length ? 0 : i + 1]].Center, j / (float)NumSealsEffectPoints);
                }

                sealsEffects[i].positions = sealsEffectPoints.Skip(i * NumSealsEffectPoints).Take(NumSealsEffectPoints).ToArray();
            }
        }

        private void RemoveState()
        {
            previousState = currentState;
            currentState = State.None;
        }

        // Chase player
        private void Follow()
        {
            Player player = Main.player[NPC.target];

            if (++StateTimer > 180)
                RemoveState();

            this.FlyTo(player.Center, acceleration, maxVelocity);
        }

        // Hover above and fire a line of projectiles
        private void FollowAndFire()
        {
            Player player = Main.player[NPC.target];

            if (++StateTimer > 420)
                RemoveState();

            this.FlyTo(player.Center - new Vector2(0f, player.height * 4), acceleration, maxVelocity);

            if (StateTimer % 150 == 0)
            {
                for (int i = -4; i < 5; i++)
                {
                    if (i != 0)
                    {
                        Vector2 pos = NPC.Center + new Vector2(i * NPC.width, 0f);
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<BakuBall>(), NPC.damage / 8, 1f);
                        Main.projectile[proj].ai[0] = NPC.whoAmI;
                        Main.projectile[proj].ai[1] = i;
                    }
                }
            }
        }

        // Teleport offscren then across firing needles up and down
        private void DreamNeedle()
        {
            Player player = Main.player[NPC.target];

            if (!stateInitalized)
            {
                float teleportMult = (float)Directions.DirectionRelativeTo(player.Center.X, NPC.Center.X);
                Vector2 endPos = player.Center - new Vector2(Main.screenWidth / 4 * teleportMult, player.height * 4);
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.DungeonSpirit);
                    Dust.NewDust(endPos, NPC.width, NPC.height, DustID.DungeonSpirit);
                }
                NPC.Center = endPos;
                NPC.velocity = new Vector2(teleportMult * 15f, 0f);
                stateInitalized = true;
            }

            if (++StateTimer > 60)
                RemoveState();

            NPC.direction = NPC.spriteDirection = (int)(NPC.velocity.X / MathF.Abs(NPC.velocity.X));

            if (StateTimer % 5 == 0)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0f, StateTimer % 2 == 0 ? 6f : -6f), ModContent.ProjectileType<BakuNeedle>(), NPC.damage / 8, 1f);
        }

        // Summon 3 seals which circle around firing needles
        private void DreamOrbs()
        {
            Player player = Main.player[NPC.target];

            float radius = MathHelper.Max(NPC.width, NPC.height) * 1.5f;
            if (!stateInitalized)
            {
                for (int i = 0; i < seals.Length; i++)
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(360f / seals.Length * i + StateTimer)) * radius;
                    seals[i] = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<BakuSeal>(), NPC.damage / 4, 3f);
                    Main.projectile[seals[i]].ai[0] = NPC.whoAmI;
                }
                stateInitalized = true;
            }

            BakuSeal.Action sealAI = BakuSeal.Action.None;
            if (++StateTimer > 420)
            {
                sealAI = BakuSeal.Action.KillYourself;
                RemoveState();
            }

            if (StateTimer % 20 == 0 && sealAI != BakuSeal.Action.KillYourself)
                sealAI = BakuSeal.Action.Shoot;

            this.FlyTo(player.Center, acceleration, maxVelocity / 2f);

            MessageSeals(sealAI);
        }

        // Create a nightmare portal to summon a nightmare
        private void CallNightmare()
        {
            if (Main.getGoodWorld || Main.masterMode || Main.expertMode)
                NPC.reflectsProjectiles = true;

            if (NPC.velocity == Vector2.Zero)
            {
                if (StateTimer == 0)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0f, NPC.height * 4f), Vector2.Zero, ModContent.ProjectileType<BakuNightmare>(), 0, 0f);

                if (++StateTimer > 180)
                {
                    NPC.reflectsProjectiles = false;
                    RemoveState();
                }
            }
            else
            {
                NPC.velocity *= 0.98f;
                this.StopIfSlow(0.1f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (currentState == State.ExpertDreamOrbs)
            {
                foreach (Ribbon sealEffect in sealsEffects)
                {
                    sealEffect.Device.Textures[0] = ModContent.Request<Texture2D>("SagesAndMystics/Assets/Ribbons/FireRibbon").Value;

                    Viewport viewport = sealEffect.Device.Viewport;
                    sealEffect.effect.World = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0));
                    sealEffect.effect.View = Main.GameViewMatrix.TransformationMatrix;
                    sealEffect.effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 10);

                    sealEffect.Render();
                }
            }

            return true;
        }
    }
}
