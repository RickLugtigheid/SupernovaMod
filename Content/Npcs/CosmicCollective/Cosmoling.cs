using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SupernovaMod.Content.Npcs.CosmicCollective
{
	public class Cosmoling : ModNPC
	{
		private float _speed = 13;
		private Player player;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			NPC.lifeMax = 60;
			NPC.defense = 9;
			NPC.width = 50;
			NPC.height = 40;
			NPC.damage = 42;
			NPC.knockBackResist = .5f;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath12;
			NPC.noGravity = true; // Not affected by gravity
			NPC.noTileCollide = true; // Can not collide with tiles
			NPC.scale = .8f;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)((float)NPC.lifeMax * 0.87f * balance);
        }

        public override void AI()
		{
			NPC.ai[0]++;

			Target(); // Sets the Player Target
			DespawnHandler(); // Handles if the NPC should despawn.


			// set the dust of the trail
			//
			int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.CrimsonPlants, NPC.velocity.X, NPC.velocity.Y, 15, default(Color), 1f);
			Main.dust[dust].noGravity = true; //this make so the dust has no gravity
			Main.dust[dust].velocity *= 0f;
			Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Blood, NPC.velocity.X * .05f, NPC.velocity.Y * .05f, 2, default(Color), .7f);

			LookToPlayer();
			Move();

			// Fix overlap with other npcs
			//
			float overlapVelocity = 0.12f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC other = Main.npc[i];

				if (i != NPC.whoAmI && other.active && Math.Abs(NPC.position.X - other.position.X) + Math.Abs(NPC.position.Y - other.position.Y) < NPC.width * 5)
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

		private void Move()
		{
			Vector2 moveTo = player.Center; // Gets the point that the npc will be moving to.
			Vector2 move = moveTo - NPC.Center;
			float magnitude = Magnitude(move);
			if (magnitude > _speed)
			{
				move *= _speed / magnitude;
			}
			float turnResistance = 24f; // The larger the number the slower the npc will turn.
			move = (NPC.velocity * turnResistance + move) / (turnResistance + 1f);
			magnitude = Magnitude(move);
			if (magnitude > _speed)
			{
				move *= _speed / magnitude;
			}
			NPC.velocity = move;
		}

		private void Target()
		{
			player = Main.player[NPC.target]; // This will get the player target.
		}


		private void LookToPlayer()
		{
			Vector2 look = Main.player[NPC.target].Center - NPC.Center;
			LookInDirection(look);
		}

		private void LookInDirection(Vector2 look)
		{
			float angle = 0.5f * (float)Math.PI;

			if (look.X != 0f)
			{
				angle = (float)Math.Atan(look.Y / look.X);
			}

			else if (look.Y < 0f)
			{
				angle += (float)Math.PI;
			}

			if (look.X < 0f)
			{
				angle += (float)Math.PI;
			}

			NPC.rotation = angle;
		}

		private float Magnitude(Vector2 mag)
		{
			return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
		}

		private void DespawnHandler()
		{
			if (!player.active || player.dead)
			{
				NPC.TargetClosest(false);
				player = Main.player[NPC.target];
				if (!player.active || player.dead)
				{
					NPC.velocity = new Vector2(0f, -10f);
					if (NPC.timeLeft > 10)
					{
						NPC.timeLeft = 10;
					}
					return;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
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
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1.2f);
				}
			}

			int i = 0;
			while ((double)i < hit.Damage / (double)NPC.lifeMax * 100.0)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1.5f);
				i++;
			}
		}
	}
}
