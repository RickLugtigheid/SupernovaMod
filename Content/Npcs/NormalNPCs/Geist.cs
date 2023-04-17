using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using SupernovaMod.Common;

namespace SupernovaMod.Content.Npcs.NormalNPCs
{
    public class Geist : ModNPC
    {
        private float _velocity;
		private float _acceleration;
		private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Geist");
			Main.npcFrameCount[NPC.type] = 3;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 10;
            NPC.defense = 6;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 1200f;
            NPC.knockBackResist = .5f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles.
            NPC.aiStyle = -1; 

            _velocity = 5;
            _acceleration = .01f;

			NPC.buffImmune[BuffID.Confused] = true;
		}

		#region AI
		public override void AI()
        {
			player = Main.player[NPC.target]; // Sets the Player Target

			DespawnHandler(); // Handles if the NPC should despawn.
            MovementAI(player, _velocity, _acceleration);

            // Make sure the npc only uses it's fade attack after comming into contact with the player for the first time.
            // We do this so the npc does not teleport to the player whan spawned.
            //
            if (NPC.localAI[0] == 0)
            {
                if (Vector2.Distance(NPC.Center, player.Center) > 500)
                {
                    NPC.localAI[0] = 1;
				}
                return;
            }

            if (Vector2.Distance(NPC.Center, player.Center) > 500)
            { 
                NPC.alpha += 2;
				if (NPC.alpha > 250)
				{
                    NPC.alpha = 255;
					NPC.Center = player.Center + new Vector2(150 * -NPC.direction, 0);
				}
			}
            else if (NPC.alpha > 0)
            {
				NPC.alpha -= 5;
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
        private void MovementAI(Player target, float velocity, float acceleration)
        {
			// Set direction
			//
			if (NPC.Center.X < target.Center.X - 2f)
			{
				NPC.direction = 1;
			}
			if (NPC.Center.X > target.Center.X + 2f)
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;
			NPC.rotation = (NPC.rotation + (NPC.velocity.X * .25f) % 20) / 10f;

			NPC.rotation = MathHelper.ToRadians(NPC.velocity.X % 15);

			float gateValue = 100f;
			Vector2 distanceFromTarget = target.Center - NPC.Center;
			SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);
		}
		#endregion

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
            if (Main.rand.NextBool(4))
            {
				target.AddBuff(BuffID.Darkness, Main.rand.Next(2, 6) * 60);
			}
		}

		public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter += 1;
			NPC.frameCounter %= 30;

			int frame = (int)(NPC.frameCounter / 6.7);
			if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
			NPC.frame.Y = frame * frameHeight;
		}

        //public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneOverworldHeight == true && spawnInfo.Player.ZoneSnow == true && NPC.downedQueenBee == true ? 0.06f : 0;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneGraveyard)
            {
                return 0.05f;
            }

            if (Main.dayTime || spawnInfo.PlayerSafe || spawnInfo.Player.ZoneDungeon || spawnInfo.PlayerInTown || spawnInfo.Player.ZoneOldOneArmy || Main.snowMoon || Main.pumpkinMoon)
            {
                return 0f;
            }
            return 0.065f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

        }
    }
}
