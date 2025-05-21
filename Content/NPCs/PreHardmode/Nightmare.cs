using Microsoft.Xna.Framework;
using SagesAndMystics.Content.Projectiles.PreHardmode.Hostile.Bosses.Baku;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static SagesAndMystics.Content.Directions;

namespace SagesAndMystics.Content.NPCs.PreHardmode
{
    public class Nightmare : ModNPC
    {
        private enum State // State machine is sloppily implemented but I don't need it to be fancy for this guy
        {
            NONE,
            SWOOP_ATTACK,
            PROJECTILE_ATTACK,
        }

        private State currState;

        private Direction swoopFrom;

        private bool ProjectileInitalized
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value == true ? 1f : 0f;
        }

        private bool SwoopPrepared
        {
            get => NPC.ai[1] == 1f;
            set => NPC.ai[1] = value == true ? 1f : 0f;
        }

        private float SwoopTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        private uint ProjectileTimer
        {
            get => (uint)NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        private bool projectileStarted;

        private bool swoopStarted;

        private Vector2 acceleration, maxVelocity;

        private Vector2 projectilePosition;

        private Vector2 swoopPosition, swoopDims;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.alpha = 0;
            NPC.defense = 2;
            NPC.damage = 12;
            NPC.knockBackResist = 0f;
            NPC.lifeMax = 400;
            NPC.width = 26;
            NPC.height = 48;

            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;

            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.HitSound = SoundID.NPCHit1;

            currState = State.SWOOP_ATTACK;

            projectileStarted = true;
            swoopFrom = Main.rand.NextBool() ? Direction.Right : Direction.Left;
            swoopStarted = false;

            acceleration = Vector2.Zero;
            maxVelocity = Vector2.Zero;
            projectilePosition = Vector2.Zero;
            swoopPosition = Vector2.Zero;
            swoopDims = new Vector2(300f, 300f);
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (player.dead || !player.active)
            {
                NPC.TargetClosest(true);
                player = Main.player[NPC.target];

                if (player.dead || !player.active)
                {
                    NPC.velocity *= 0.98f;
                    this.StopIfSlow(0.1f);
                    NPC.alpha++;
                    if (NPC.alpha == 255)
                    {
                        NPC.position = new Vector2(float.MinValue);
                        NPC.timeLeft = 0;
                    }
                }
            }

            if (Main.masterMode)
            {
                acceleration = new Vector2(0.4f);
                maxVelocity = new Vector2(8f);
            }
            else
            {
                acceleration = new Vector2(0.3f);
                maxVelocity = new Vector2(6f);
            }

            switch (currState)
            {
                case State.SWOOP_ATTACK: SwoopAttack(); break;
                case State.PROJECTILE_ATTACK: ProjectileAttack(); break;
                default:
                    {
                        Main.NewText("Unusable AI state for ModNPC Baku");
                        currState = State.SWOOP_ATTACK;
                    }
                    break;
            }
        }

        private Vector2 NewSwoopStart()
        {
            Player player = Main.player[NPC.target];
            return player.Center + new Vector2(swoopDims.X * (float)swoopFrom, -swoopDims.Y);
        }

        private void SwoopAttack()
        {
            if (swoopStarted)
            {
                SwoopTimer += MathF.Tau / 90f; // 1.5 sec for full cycle

                Vector2 nextPos = new Vector2(swoopPosition.X + swoopDims.X * ((float)swoopFrom * 2f), swoopPosition.Y + swoopDims.Y / 2f + MathF.Cos(SwoopTimer) * -(swoopDims.Y / 2f));

                NPC.position.X = MathHelper.Lerp(swoopPosition.X, nextPos.X, SwoopTimer / MathF.Tau);
                NPC.position.Y = nextPos.Y;

                if (SwoopTimer >= MathF.Tau)
                {
                    swoopFrom = Flip(swoopFrom);
                    swoopPosition = NewSwoopStart();
                    SwoopTimer = 0;
                    swoopStarted = false;
                    if (Main.rand.NextBool(3))
                    {
                        currState = State.PROJECTILE_ATTACK;
                    }
                }
            }
            else
            {
                Player player = Main.player[NPC.target];

                if (Vector2.Distance(player.Center, swoopPosition) > 150f)
                    swoopPosition.X = Main.player[NPC.target].Center.X + swoopDims.X * (float)Flip(swoopFrom);

                swoopPosition.Y = player.Center.Y - swoopDims.Y;

                if (Vector2.Distance(NPC.Center, swoopPosition) < 5f)
                    SwoopPrepared = true;

                if (SwoopPrepared)
                {
                    NPC.velocity *= 0.8f; // Ease to stop to prevent sudden velocity changes
                    this.StopIfSlow(0.1f);
                    if (NPC.velocity == Vector2.Zero)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Vector2 velocity = new Vector2(Main.rand.NextFloat(6f), Main.rand.NextFloat(6f)).RotatedByRandom(Math.Tau);
                            Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.DungeonSpirit, velocity.X, velocity.Y);
                        }

                        swoopStarted = true;
                        swoopPosition = NPC.Center; // Set start position to NPC center to avoid the NPC snapping to the swoop position
                        SwoopPrepared = false;
                    }
                }
                else
                {
                    this.FlyTo(swoopPosition, acceleration, maxVelocity);
                }
            }
        }

        private void ProjectileAttack()
        {
            Player player = Main.player[NPC.target];

            if (ProjectileInitalized == false)
            {
                projectilePosition = player.Center + new Vector2(0f, -300f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20f, 20f)));
                ProjectileInitalized = true;
            }

            if (Vector2.Distance(NPC.Center, projectilePosition) < 5f)
                projectileStarted = true;

            if (projectileStarted)
            {
                NPC.velocity *= 0.8f; // Ease to stop to prevent sudden velocity changes
                this.StopIfSlow(0.1f);
                if (NPC.velocity == Vector2.Zero)
                    ProjectileTimer++;

                if (ProjectileTimer == 30)
                    NPCProjectiles.MultiProjectile(NPC.GetSource_FromAI(), NPC.Center, player.Center, 8f, ModContent.ProjectileType<BakuNeedle>(), NPC.damage / 4, 1, 15f, 3);

                if (ProjectileTimer >= 60)
                {
                    ProjectileInitalized = false;
                    projectileStarted = false;
                    ProjectileTimer = 0;
                    currState = State.SWOOP_ATTACK;
                }
            }
            else
            {
                this.FlyTo(projectilePosition, acceleration, maxVelocity);
            }
        }
    }
}
