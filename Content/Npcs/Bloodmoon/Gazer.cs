using Microsoft.Xna.Framework;
using SupernovaMod.Api.Helpers;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Bloodmoon
{
    public class Gazer : ModNPC
    {
		private const float ProjectileExpertDamageMultiplier = .6f;
		public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gazer");
			Main.npcFrameCount[NPC.type] = 5;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = -20,
                // Influences how the NPC looks in the Bestiary
                Velocity = 1 // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,

				// Sets the description of this NPC that is listed in the bestiary.
				//new FlavorTextBestiaryInfoElement("Impish terrors to all excavators beneath the surface. These unique creatures have evolved and adapted to coexist with the harsh circumstances of the underground. But something's hinting towards them being more than just lifeforms..."),
            });
        }

        int timer;
        int ShootDamage;
        int shootTimer;
        public override void SetDefaults()
        {
            NPC.lifeMax = 36;
            NPC.defense = 6;
            ShootDamage = 12;
            shootTimer = 140;

            NPC.width = 54;
            NPC.height = 54;

            NPC.damage = 12;
            NPC.value = 0;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 44;
            AIType = NPCID.Harpy;  //npc behavior
            //AnimationType = NPCID.Harpy;
            NPC.HitSound = SoundID.NPCHit9;
            NPC.DeathSound = SoundID.NPCDeath11;
        }

        public override void FindFrame(int frameHeight)
        {
			/*NPC.frameCounter -= .8f; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;*/

			// This is a simple "loop through all frames from top to bottom" animation
			NPC.frameCounter++;

			if (NPC.frameCounter > 5)
			{
				NPC.frameCounter = 0;
				NPC npc = NPC;
				npc.frame.Y = npc.frame.Y + npc.height;

				// Check if the frame exceeds the height of our sprite sheet.
				// If so we reset to 0.
				//
				if (NPC.frame.Y >= NPC.height * Main.npcFrameCount[NPC.type])
				{
					NPC.frame.Y = 0;
				}
			}
            NPC.spriteDirection = NPC.direction;
		}

        public override void AI()
        {
            int radius = 625;
            if (!NPC.confused && Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) <= radius)
            {
                timer++;
                if (timer >= shootTimer)
                {
                    Shoot();
                    timer = 0;
                }
            }

			// Fix overlap with other npcs
			//
			float overlapVelocity = 0.05f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC other = Main.npc[i];

				if (i != NPC.whoAmI && other.active && Math.Abs(NPC.position.X - other.position.X) + Math.Abs(NPC.position.Y - other.position.Y) < NPC.width * 2)
				{
					if (NPC.position.X < other.position.X)
					{
						NPC.velocity.X -= overlapVelocity;
					}
					else
					{
						NPC.velocity.X += overlapVelocity;
					}

					if (NPC.position.Y < other.position.Y)
					{
						NPC.velocity.Y -= overlapVelocity;
					}
					else
					{
						NPC.velocity.Y += overlapVelocity;
					}
				}
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		void Shoot()
        {
            SoundEngine.PlaySound(SoundID.Item12, NPC.Center);

			// Only spawn projectiles as Server or singleplayer client
			//
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

            int type = ModContent.ProjectileType<Projectiles.Hostile.BloodBoltHostile>();
            Vector2 Velocity = Mathf.VelocityFPTP(NPC.Center, Main.player[NPC.target].Center, 10);
            int Spread = 1;
            float SpreadMult = 0.15f;
            Velocity.X = Velocity.X + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
            Velocity.Y = Velocity.Y + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
            int i = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Velocity.X, Velocity.Y, type, (int)(ShootDamage * ProjectileExpertDamageMultiplier), 1.75f);
            Main.projectile[i].hostile = true;
            Main.projectile[i].friendly = false;
            Main.projectile[i].tileCollide = true;
        }
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)((float)NPC.lifeMax * .5f * balance);
			NPC.damage = (int)((double)NPC.damage * .8f);
		}
	}
}