using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Content.Dusts;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.CosmicCollective
{
	public class CosmicCollectiveEye : ModNPC
	{
		private const float ExpertDamageMultiplier = .7f;

		public Player target;
		public CosmicCollective Owner => Main.npc[(int)NPC.ai[0]].ModNPC as CosmicCollective;
		public float OffsetX => NPC.ai[1];
		public float OffsetY => NPC.ai[2];
		private float Timer
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public int extraShootDelay = 0;

		public override void SetStaticDefaults()
		{
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
			NPCID.Sets.TeleportationImmune[NPC.type] = true;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)((float)NPC.lifeMax * 0.85f * balance);
            NPC.damage = (int)((double)NPC.damage * ExpertDamageMultiplier);
        }

        public override void SetDefaults()
		{
			NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
			NPC.lifeMax = 1500; // The Max HP the boss has on Normal
			NPC.damage = 24; // The base damage value the boss has on Normal
			NPC.defense = 17; // The base defense on Normal
			NPC.knockBackResist = 0f; // No knockback
			NPC.width = 34;
			NPC.height = 34;
			NPC.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
			NPC.lavaImmune = true; // Not hurt by lava
			NPC.noGravity = true; // Not affected by gravity
			NPC.noTileCollide = true; // Will not collide with the tiles. 
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath60;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(extraShootDelay);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			extraShootDelay = reader.ReadInt32();
		}

		public override void AI()
		{
			AI_Setup();
			if (AI_Despawn())
			{
				return;
			}

			// Set NPC position
			NPC.position = Owner.NPC.Center - new Vector2(OffsetX, OffsetY);

			// Shoot
			//
			int timeTilNextShot = (Owner.State == CosmicCollective.AIState.Phase2 ? 160 : 180) + extraShootDelay;
            if (Main.expertMode || Main.masterMode)
            {
                timeTilNextShot -= 10;
            }
            if (Owner.EyesActive < 5)
            {
                timeTilNextShot -= 10;
            }
            if (Owner.EyesActive < 4)
            {
                timeTilNextShot -= 20;
            }
            if (Owner.EyesActive < 3)
            {
                timeTilNextShot -= 35;
            }
            if (Owner.EyesActive < 2)
            {
                timeTilNextShot -= 50;
            }

            if (Timer % timeTilNextShot == 0)
			{
				ShootToPlayer(ProjectileID.EyeLaser, 45);
			}
		}

		private void AI_Setup()
		{
			target = Main.player[NPC.target];
			Timer++;
		}
		private bool AI_Despawn()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}
			if (!Owner.NPC.active)
			{
				if (NPC.timeLeft > 8)
				{
					NPC.timeLeft = 8;
				}
				NPC.ai[0] = 0;
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
				NPC.ai[3] = 0f;
				NPC.netUpdate = true;
				return true;
			}
			return false;
		}

		public override void OnKill()
		{
            float sin = 1f + (float)Math.Sin(((ModNPC)this).NPC.timeLeft * 10);
            float cos = 1f + (float)Math.Cos(((ModNPC)this).NPC.timeLeft * 10);
            Color color = new Color(0.5f + cos * 0.2f, 0.8f, 0.5f + sin * 0.2f);
            for (int i = 0; i < 18; i++)
            {
                Dust.NewDustPerfect(NPC.Center + Utils.RotatedByRandom(Vector2.One, 6.28000020980835) * Utils.NextFloat(Main.rand, 4f), 5, (Vector2?)(Utils.RotatedByRandom(Vector2.One, 6.28000020980835) * Utils.NextFloat(Main.rand, 2f, 4f)), 0, color, Utils.NextFloat(Main.rand, 0.85f, 1.6f)).customData = Utils.NextFloat(Main.rand, 0.5f, 1f);
                Dust.NewDustPerfect(NPC.Center + Utils.RotatedByRandom(Vector2.One, 6.28000020980835) * Utils.NextFloat(Main.rand, 4f), ModContent.DustType<BloodDust>(), (Vector2?)(Utils.RotatedByRandom(Vector2.One, 6.28000020980835) * Utils.NextFloat(Main.rand, 0.7f, 1.25f)), 0, color, 0.5f);
            }

            Owner.OnEyeKilled(this);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return base.PreDraw(spriteBatch, screenPos, drawColor);
		}

		private void ShootToPlayer(int type, int damage, float velocityMulti = .7f, float rotationMulti = 1)
		{
            float sin = 1f + (float)Math.Sin(((ModNPC)this).NPC.timeLeft * 10);
            float cos = 1f + (float)Math.Cos(((ModNPC)this).NPC.timeLeft * 10);
            Color color = new Color(0.5f + cos * 0.2f, 0.8f, 0.5f + sin * 0.2f);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(NPC.Center + Utils.RotatedByRandom(Vector2.One, 6.28000020980835) * Utils.NextFloat(Main.rand, 4f), 71, (Vector2?)(Utils.RotatedByRandom(Vector2.One, 6.28000020980835) * Utils.NextFloat(Main.rand, 1.5f, 3f)), 0, color, Utils.NextFloat(Main.rand, 0.7f, 1.2f)).customData = Utils.NextFloat(Main.rand, 0.5f, 1f);
            }

            Vector2 position = NPC.Center;
			float rotation = (float)Math.Atan2(position.Y - (target.position.Y + target.height * 0.2f), position.X - (target.position.X + target.width * 0.15f));
			rotation *= rotationMulti;

			Vector2 velocity = new Vector2((float)-(Math.Cos(rotation) * 18) * .75f, (float)-(Math.Sin(rotation) * 18) * .75f) * velocityMulti;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, (int)(damage * ExpertDamageMultiplier), 0f, 0);
		}
	}
}
