﻿using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Common.Systems;
using Terraria.GameContent.ItemDropRules;
using SupernovaMod.Api;
using Terraria.GameContent.Bestiary;
using SupernovaMod.Common.ItemDropRules.DropConditions;
using SupernovaMod.Common;
using Terraria.GameContent;

namespace SupernovaMod.Content.Npcs.StormSovereign
{
    [AutoloadBossHead]
    public class StormSovereign : ModNPC
    {
		private const int DAMAGE_PROJECILE = 27;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Terror");
            Main.npcFrameCount[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 1;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                PortraitScale = .75f,
                Scale = .75f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of NPC NPC that is listed in the bestiary.
				//BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.,

				// Sets the description of NPC NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),
			});
		}

		public override void SetDefaults()
        {
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 5000; // The Max HP the boss has on Normal
            NPC.damage = 30; // The NPC damage value the boss has on Normal
            NPC.defense = 8; // The NPC defense on Normal
            NPC.knockBackResist = 0f; // No knockback
            NPC.width = 200;
            NPC.height = 100;
            NPC.value = 10000;
            NPC.npcSlots = 1f; // The higher the number, the more NPC slots NPC NPC takes.
            NPC.boss = true; // Is a boss
            NPC.lavaImmune = false; // Not hurt by lava
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.Boss1;
        }

        protected Player target;
		protected float velocity;
		protected float acceleration;
		protected Vector2 targetOffset;

		public bool SecondPhase { get; private set; } = false;

		private bool _initialized = false;
		public override void AI()
        {
			if (!_initialized)
			{
				SupernovaUtils.StartThunderStorm();
				_initialized = true;
			}
			Main.rainTime = 600;

			// Set/Reset all required values
			//
			float npcLifeRatio = NPC.life / (float)NPC.lifeMax;

            velocity = 12;
            acceleration = .05f;
			targetOffset = new Vector2(0, 250);

			ref float timer = ref NPC.ai[1];
            ref float attackPointer = ref NPC.ai[0];
			target = Main.player[NPC.target];

			_drawMotionBlur = true;

			// Handle AI
			//
			DespawnAI();
			timer++;

			if (npcLifeRatio > .5f)
			{
				if (attackPointer == 0 || attackPointer == 1)
				{
					if (timer % 120 == 0)
					{
						SpawnLightningStrike();
					}
					if (timer >= 340)
					{
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 2)
				{
					if (timer >= 80 && timer <= 120 && timer % 20 == 0)
					{
						SpawnLightningStrike();
					}
					if (timer >= 240)
					{
						timer = 0;
						attackPointer++;
					}
				}
				else
				{
					attackPointer = 0;
				}
			}
            //MovementAI(velocity, acceleration, targetOffset.X, targetOffset.Y);
        }

		#region AI Methods
		private void DespawnAI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}

			Player player = Main.player[NPC.target];
			if (NPC.ai[0] != 3f && (player.dead || !player.active))
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (player.dead || !player.active)
				{
					NPC.ai[0] = 0;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.timeLeft < 1800)
			{
				NPC.timeLeft = 1800;
			}
		}
		private void MovementAI(Vector2 targetPosition, float velocity, float acceleration)
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
			Vector2 distanceFromTarget = targetPosition - NPC.Center;
			SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);
		}
		private void MovementAI(float velocity, float acceleration, float targetOffsetX = 0, float targetOffsetY = 0)
		{
			NPC.spriteDirection = NPC.direction;

			ref Player target = ref Main.player[NPC.target];
			float gateValue = 100f;
			Vector2 distanceFromTarget = new Vector2(target.Center.X - targetOffsetX, target.Center.Y - targetOffsetY) - NPC.Center;
			SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);


			// Set direction
			//
			if (NPC.velocity.X < 0f)
			{
				NPC.direction = -1;
			}
			else
			{
				NPC.direction = 1;
			}
		}
		#endregion

		#region Attack Methods
		#endregion

		#region Helper Methods
		public void SpawnLightningStrike(int warnTime = 40)
		{
			int type = ModContent.ProjectileType<Projectiles.StormCloud>();
			Projectile.NewProjectile(NPC.GetSource_FromAI(), target.Center - new Vector2(0, 500), Vector2.Zero, type, 30, 1, Main.myPlayer, warnTime, 8);
		}
		private Vector2 _desiredDestination;
		private Vector2 GetRandFlyDestination()
		{
			if (target.position == Vector2.Zero)
			{
				return target.position;
			}

			if (_desiredDestination == Vector2.Zero ||
				Vector2.Distance(target.Center, _desiredDestination) >= 400 ||    // Make sure the boss will not get to far from the player
				Vector2.Distance(NPC.Center, _desiredDestination) >= 10			 //
			)
			{
				_desiredDestination = target.position + target.velocity;
				_desiredDestination.X += 350 * -NPC.direction;
				_desiredDestination.Y -= 100;
			}
			return _desiredDestination;
		}
		private void ShootToPlayer(int type, int damage, float velocityMulti = 1, float rotationMulti = 1)
        {
            //int type = ModContent.ProjectileType<TerrorProj>();
            SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

            Vector2 position = new Vector2(NPC.Center.X + (NPC.width / 2 + 25) * NPC.direction, NPC.Center.Y + NPC.height - 50);
			float rotation = (float)Math.Atan2(position.Y - (target.position.Y + target.height * 0.2f), position.X - (target.position.X + target.width * 0.15f));
			rotation *= rotationMulti;

            Vector2 velocity = new Vector2((float)-(Math.Cos(rotation) * 18) * .75f, (float)-(Math.Sin(rotation) * 18) * .75f) * velocityMulti;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, damage, 0f, 0);

            for (int x = 0; x < 5; x++)
            {
                int dust = Dust.NewDust(position, 50, 50, ModContent.DustType<Dusts.TerrorDust>(), velocity.X, velocity.Y, 80, default, Main.rand.NextFloat(.9f, 1.6f));
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= Main.rand.NextFloat(.3f, .5f);
            }
        }
        #endregion

		private bool _drawMotionBlur;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!_drawMotionBlur)
			{
				return true;
			}
			SpriteEffects spriteEffects = 0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color24 = NPC.GetAlpha(drawColor);
			Color color25 = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));
			Texture2D texture2D3 = TextureAssets.Npc[NPC.type].Value;
			int num156 = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
			int y3 = num156 * (int)NPC.frameCounter;
			Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			int num157 = 8;
			int num158 = 2;
			int num159 = 1;
			float num160 = 0f;
			int num161 = num159;
			spriteBatch.Draw(texture2D3, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), new Rectangle?(NPC.frame), color24, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 0f);
			while (num158 > 0 && num161 < num157 || num158 < 0 && num161 > num157)
			{
				Color color26 = NPC.GetAlpha(color25);
				float num162 = num157 - num161;
				if (num158 < 0)
				{
					num162 = num159 - num161;
				}
				color26 *= num162 / (NPCID.Sets.TrailCacheLength[NPC.type] * 1.5f);
				Vector2 value4 = NPC.oldPos[num161];
				float num163 = NPC.rotation;
				Main.spriteBatch.Draw(texture2D3, value4 + NPC.Size / 2f - screenPos + new Vector2(0f, NPC.gfxOffY), new Rectangle?(rectangle), color26, num163 + NPC.rotation * num160 * (num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, NPC.scale, spriteEffects, 0f);
				num161 += num158;
			}
			return NPC.IsABestiaryIconDummy;
		}
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1;
            NPC.frameCounter %= 30;

            int frame = (int)(NPC.frameCounter / 6.7);
            NPC.frame.Y = frame * frameHeight;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
		}
	}
}
