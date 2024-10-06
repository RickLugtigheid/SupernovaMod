using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Api;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Fallen.Projectiles
{
	public class AshBall : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.ImmuneToAllBuffs[NPC.type] = true;
		}
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.CultistBossFireBallClone}";

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.ChaosBall);
			NPC.lifeMax = 250;
			NPC.damage = 46;
			NPC.defense = 24;
			NPC.knockBackResist = 1;
			NPC.width = 34;
			NPC.height = 40;
			NPC.aiStyle = -1;
			AIType = 0;
		}

		private Vector2 _targetPosition;
		public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 10000)
			{
				NPC.TargetClosest(true);
			}
			Player target = Main.player[NPC.target];

			// Dusts
			//
			if (Main.rand.NextBool(4))
			{
				for (int m = 0; m < 1; m++)
				{
					Vector2 value18 = -Vector2.UnitX.RotatedByRandom(0.19634954631328583).RotatedBy((double)NPC.velocity.ToRotation(), default);
					int num306 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
					Dust obj7 = Main.dust[num306];
					obj7.velocity *= 0.1f;
					Main.dust[num306].position = NPC.Center + value18 * NPC.width / 2f;
					Main.dust[num306].fadeIn = 0.9f;
				}
			}
			if (Main.rand.NextBool(16))
			{
				for (int n = 0; n < 1; n++)
				{
					Vector2 value19 = -Vector2.UnitX.RotatedByRandom(0.39269909262657166).RotatedBy((double)NPC.velocity.ToRotation(), default);
					int num317 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 155, default, 0.8f);
					Dust obj8 = Main.dust[num317];
					obj8.velocity *= 0.3f;
					Main.dust[num317].position = NPC.Center + value19 * NPC.width / 2f;
					if (Main.rand.NextBool(2))
					{
						Main.dust[num317].fadeIn = 1.4f;
					}
				}
			}
			//if (Main.rand.NextBool(2))
			{
				for (int num320 = 0; num320 < 2; num320++)
				{
					Vector2 value20 = -Vector2.UnitX.RotatedByRandom(0.78539818525314331).RotatedBy((double)NPC.velocity.ToRotation(), default);
					int num326 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Ash, 0f, 0f, 0, default, 1);
					Dust obj9 = Main.dust[num326];
					obj9.velocity *= 0.3f;
					Main.dust[num326].noGravity = true;
					Main.dust[num326].position = NPC.Center + value20 * NPC.width / 2f;
					if (Main.rand.NextBool(2))
					{
						Main.dust[num326].fadeIn = 1.4f;
					}
				}
			}

			// Move
			_targetPosition = target.position;
			Vector2 distanceFromTarget = new Vector2(_targetPosition.X, _targetPosition.Y) - NPC.Center;
			SupernovaUtils.MoveNPCSmooth(NPC, 100, distanceFromTarget, 8, .1f, true);
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

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
			target.AddBuff(ModContent.BuffType<Buffs.DamageOverTime.BlackFlames>(), 140);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			drawColor = new Color(78, 78, 76);
			return base.PreDraw(spriteBatch, screenPos, drawColor);
		}
	}
}
