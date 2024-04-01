using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.ModLoader.Utilities;
using SupernovaMod.Api.Effects;

namespace SupernovaMod.Content.Npcs.NormalNPCs
{
    public class Balancer : ModNPC
    {
        private float speed;
        private Player player;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.OnFire] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 70;
			NPC.damage = 54;
			NPC.defense = 28;
			NPC.lifeMax = 2000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 100f;
			NPC.knockBackResist = .1f;
			NPC.noGravity = true; // Not affected by gravity  
			NPC.noTileCollide = true; // Will not collide with the tiles.  
			NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
			AnimationType = NPCID.Harpy;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),
            });
        }

		bool awaitBuffer = false;
		public override void AI()
		{
			Target(); // Sets the Player Target  

			// Spawn new orbit projectile if we have less than 4. 
			// 
			if (NPC.ai[0] < 4)
			{
				if (!awaitBuffer)
				{
					NPC.ai[1]++;
					if (NPC.ai[1] >= 90)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position, Vector2.One, ModContent.ProjectileType<Projectiles.Hostile.BalancedOrb>(), (int)(30 * .7f), 4, Main.myPlayer, ai1: NPC.whoAmI);
						SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack);
						NPC.ai[1] = 0;
						NPC.ai[0]++;    // Add a projectile to the projectile counter  
						if (NPC.ai[0] >= 4) awaitBuffer = true;
					}
				}
				else if (NPC.ai[0] == 0) awaitBuffer = false;
			}

			DespawnHandler(); // Handles if the NPC should despawn.  

			Move(new Vector2(-0, -0f)); // Calls the Move Method  
		}

		private void Move(Vector2 offset)
		{
			speed = 0.9f; // Sets the max speed of the npc.  
			if (Main.netMode != 1)
			{
				Vector2 moveTo = player.Center + offset; // Gets the point that the npc will be moving to.  
				Vector2 move = moveTo - NPC.Center;

				float magnitude = Magnitude(move);
				if (magnitude > speed)
				{
					move *= speed / magnitude;
				}
				float turnResistance = 21f; // The larget the number the slower the npc will turn.  
				move = (NPC.velocity * turnResistance + move) / (turnResistance + 1f);
				magnitude = Magnitude(move);
				if (magnitude > speed)
				{
					move *= speed / magnitude;
				}
				NPC.velocity = move;
			}
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
		private void Target()
		{
			player = Main.player[NPC.target]; // This will get the player target.  
		}
		private float Magnitude(Vector2 mag) => (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
			NPC.frame.Y = frame * frameHeight;

			NPC.spriteDirection = NPC.direction;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Balancer>()) || spawnInfo.PlayerInTown)
            {
                return 0;
            }
            return SpawnCondition.HardmodeJungle.Chance * .1f;
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				DrawDust.MakeExplosion(NPC.Center, 4, DustID.JungleTorch, 12, noGravity: true);
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, hit.HitDirection, -1f, 0, default(Color), 1f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.JungleGrass, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.HelixStone>()));
            npcLoot.Add(ItemDropRule.Common(ItemID.StoneBlock, 3, 5, 10));

            base.ModifyNPCLoot(npcLoot);
        }
	}
}
