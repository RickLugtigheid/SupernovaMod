using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Common;
using Terraria.GameContent;
using SupernovaMod.Content.Npcs.HarbingerOfAnnihilation.Projectiles;
using SupernovaMod.Api.Drawing;

namespace SupernovaMod.Content.Npcs.HarbingerOfAnnihilation
{
    public static class HoaArmAI
	{
		public const int Reset			= -1;
		public const int CircleHoa		= 0;
		public const int LaunchAtPlayer = 1;
		public const int SmashPlayer	= 2;
		public const int CirclePlayerAndShoot = 3;
		public const int GotoTarget		= 4;
		public const int CircleHoaAndShoot	  = 5;
		public const int CircleTarget	= 6;
		public const int ShootAtPlayer	= 7;
		public const int LightningLink  = 8;
	}
	public class HarbingerOfAnnihilation_Arm : ModProjectile
    {
		private readonly int _projIdMissile = ModContent.ProjectileType<HarbingerMissile>();

		public Vector2 customTarget = Vector2.Zero;
		public Vector2 customLookTarget = Vector2.Zero;
		public int customDuration = 1;

		public float AttackPointer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }

		protected HarbingerOfAnnihilation owner;
		private float _startDeg = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harbinger of Annihilation Arm");
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 72;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.light = 0.2f;
		}

		private Vector2 _targetPosition;
		public override void AI()
		{
			Projectile.timeLeft = 10;
			if (owner == null)
			{
				owner = (HarbingerOfAnnihilation)Main.npc[(int)Projectile.ai[0]].ModNPC;
				Projectile.ai[0] = 0;
				_startDeg = Projectile.ai[1]; //The degrees, you can multiply projectile.ai[0] to make it orbit faster, may be choppy depending on the value 
				Projectile.ai[1] = 0;
			}

			if (!CheckIfActive(owner))
			{
				Projectile.timeLeft = -1;
				return;
			}

			ref float timer = ref Projectile.localAI[1];
			ref float attackPointer = ref Projectile.ai[0];

			ref Player target = ref Main.player[owner.NPC.target];

			if (attackPointer == HoaArmAI.Reset)
			{
				if (ReturnToStartPosition())
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

					Projectile.velocity = Vector2.Zero;
					timer = 0;
					attackPointer = 0;
				}
			}
			else if (attackPointer == HoaArmAI.CircleHoa)
			{
				// Factors for calculations
				float deg = _startDeg;
				double rad = deg * (Math.PI / 180);    //Convert degrees to radians 
				rad += owner.NPC.rotation;
				double dist = 80;                           //Distance away from the owner 

				Projectile.rotation = MathHelper.ToRadians(deg - 90) + owner.NPC.rotation;

				/*Position the owner based on where the owner is, the Sin/Cos of the angle times the / 

				/ distance for the desired distance away from the owner minus the projectile's width   / 
				/ and height divided by two so the center of the projectile is at the right place.     */
				Projectile.position.X = owner.NPC.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
				Projectile.position.Y = owner.NPC.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
			}
			else if (attackPointer == HoaArmAI.LaunchAtPlayer)
			{
				timer++;

				if (timer <= 40)
				{
					float damping = .15f;

					Projectile.rotation = Mathf.Lerp(Projectile.rotation, Projectile.GetTargetLookRotation(target.position), damping);
					_targetPosition = target.position;
				}
				else if (timer == 41)
				{
					SoundEngine.PlaySound(SoundID.Item117, Projectile.Center);
					Vector2 velocity = (_targetPosition - Projectile.Center);
					velocity *= 25 / velocity.Magnitude();

					Projectile.velocity = velocity;
				}
				else if (timer >= 140 && ReturnToStartPosition())
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

					Projectile.velocity = Vector2.Zero;
					timer = 0;
					attackPointer = 0;
				}
			}
			else if (attackPointer == HoaArmAI.SmashPlayer)
			{
				timer++;

				if (timer <= 80)
				{
					Projectile.rotation = MathHelper.ToRadians(180);
					Vector2 targetPosition = new Vector2(target.Center.X + (target.velocity.X * target.width), target.Center.Y - 300);
					Vector2 distanceFromTarget = new Vector2(targetPosition.X, targetPosition.Y) - Projectile.Center;
					SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, 12, .35f);
				}
				else if (timer == 81)
				{
					SoundEngine.PlaySound(SoundID.Item117, Projectile.Center);
					Projectile.velocity = new Vector2(0, 10);
				}
				else if (timer >= 81 && timer < 140)
				{
					Projectile.velocity *= 1.01f;
				}
				else if (timer >= 140)
				{
					Projectile.velocity = Vector2.Zero;
					if (timer >= 180 && ReturnToStartPosition(6))
					{
						SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

						timer = 0;
						attackPointer = 0;
					}
				}
			}
			else if (attackPointer == HoaArmAI.CirclePlayerAndShoot)
			{
				timer++;

				ref float shootCooldown = ref Projectile.ai[1];
				ref float shootTime = ref Projectile.localAI[0];
				if (timer == 1)
				{
					shootTime = 90;
					shootCooldown = shootTime;
				}

				if (timer <= 280)
				{
					shootCooldown--;
					// Factors for calculations
					double rad = (_startDeg + timer) * (Math.PI / 180);     //Convert degrees to radians 
					double dist = 450;                                      //Distance away from the owner 

					Projectile.rotation = MathHelper.ToRadians((_startDeg + timer) + 90);

					/*Position the owner based on where the owner is, the Sin/Cos of the angle times the / 

					/ distance for the desired distance away from the owner minus the projectile's width   / 
					/ and height divided by two so the center of the projectile is at the right place.     */
					Projectile.position.X = target.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
					Projectile.position.Y = target.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

					if (shootCooldown == 0)
					{
						Vector2 velocity = (target.Center - Projectile.Center);
						velocity *= 10 / velocity.Magnitude();

						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, _projIdMissile, (int)(Projectile.damage * .75f), Projectile.knockBack, Main.myPlayer);

						if (owner.SecondPhase)
						{
							shootTime = 75;
							shootCooldown = shootTime;
							return;
						}
						if (shootTime == 90)
						{
							shootTime = 135;
						}
						else
						{
							shootTime = 90;
						}
						shootCooldown = shootTime;
					}
				}

				if (timer > 330 && ReturnToStartPosition(7))
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

					Projectile.velocity = Vector2.Zero;
					timer = 0;
					attackPointer = 0;
				}
			}
			else if (attackPointer == HoaArmAI.GotoTarget)
			{
				timer++;

				if (timer <= 180)
				{
					Projectile.rotation = Projectile.GetTargetLookRotation(customLookTarget);
					Vector2 distanceFromTarget = new Vector2(customTarget.X, customTarget.Y) - Projectile.Center;
					SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, 10, .1f);
				}
				else if (ReturnToStartPosition(7))
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

					Projectile.velocity = Vector2.Zero;
					timer = 0;
					attackPointer = 0;
				}
			}
			else if (attackPointer == HoaArmAI.CircleHoaAndShoot)
			{
				timer++;

				if (timer <= 10)
				{
					// Factors for calculations
					double rad = _startDeg * (Math.PI / 180);    //Convert degrees to radians 
					double dist = 80;                           //Distance away from the owner 

					rad = owner.NPC.GetTargetLookRotation(target.position) + MathHelper.ToRadians((_startDeg * .4f) + (45));
					Projectile.rotation = (float)rad - MathHelper.ToRadians(90);
					/*Position the owner based on where the owner is, the Sin/Cos of the angle times the / 

					/ distance for the desired distance away from the owner minus the projectile's width   / 
					/ and height divided by two so the center of the projectile is at the right place.     */
					Vector2 position = new(
						owner.NPC.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2,
						 owner.NPC.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2
					);
					//Projectile.position = position;
					Vector2 distanceFromTarget = position - Projectile.position;
					SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, 30, .01f);
				}

				if (timer <= customDuration)
				{
					if (timer % 10 == 0)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Vector2.One * 7).RotatedBy(Projectile.rotation + MathHelper.ToRadians(225 + Main.rand.Next(-2, 2))), _projIdMissile, (int)(Projectile.damage * .75f), Projectile.knockBack, Main.myPlayer);
					}
				}
				else if (ReturnToStartPosition(7))
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

					Projectile.velocity = Vector2.Zero;
					timer = 0;
					attackPointer = 0;
				}
			}
			else if (attackPointer == HoaArmAI.CircleTarget)
			{
				timer += 4;
				ref float shootCooldown = ref Projectile.localAI[0];
				ref float shootTime = ref Projectile.ai[1];

				if (timer == 1)
				{
					shootCooldown = 80;
					shootTime = shootCooldown;
				}
				shootTime--;
				if (timer <= (customDuration * 4))
				{
					// Factors for calculations
					double rad = (_startDeg + timer) * (Math.PI / 180);    //Convert degrees to radians 
					double dist = 80;                           //Distance away from the owner 

					Projectile.rotation = MathHelper.ToRadians((_startDeg + timer) + 90);

					/*Position the owner based on where the owner is, the Sin/Cos of the angle times the / 

					/ distance for the desired distance away from the owner minus the projectile's width   / 
					/ and height divided by two so the center of the projectile is at the right place.     */
					Projectile.position.X = customTarget.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
					Projectile.position.Y = customTarget.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;


					if (shootTime == 0)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Vector2.One * 5).RotatedBy(Projectile.rotation + MathHelper.ToRadians(225)), _projIdMissile, (int)(Projectile.damage * .75f), Projectile.knockBack, Main.myPlayer);

						if (shootCooldown == 40)
						{
							shootCooldown = 80;
						}
						else
						{
							shootCooldown = 40;
						}
						shootTime = shootCooldown;
					}
				}
				else if (ReturnToStartPosition())
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

					Projectile.velocity = Vector2.Zero;
					timer = 0;
					attackPointer = 0;
					shootCooldown = 0;
					shootTime = 0;
				}
			}
			else if (attackPointer == HoaArmAI.ShootAtPlayer)
			{
				timer++;
				if (timer <= customDuration)
				{
					Projectile.rotation = Projectile.GetTargetLookRotation(target.position);
					Vector2 distanceFromTarget = new Vector2(customTarget.X, customTarget.Y) - Projectile.Center;
					SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, 14, .05f);

					if (timer % 80 == 0)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Vector2.One * 5).RotatedBy(Projectile.rotation + MathHelper.ToRadians(225)), _projIdMissile, (int)(Projectile.damage * .75f), Projectile.knockBack, Main.myPlayer);
					}
				}
				else if (ReturnToStartPosition())
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

					Projectile.velocity = Vector2.Zero;
					timer = 0;
					attackPointer = 0;
				}
			}
			else if (attackPointer == HoaArmAI.LightningLink)
			{
				Projectile linkNode = Main.projectile[(int)Projectile.ai[1]];
				timer++;
				if (timer <= customDuration)
				{
					Projectile.rotation = Projectile.GetTargetLookRotation(linkNode.Center);
					Vector2 distanceFromTarget = new Vector2(customTarget.X, customTarget.Y) - Projectile.Center;
					SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, 6, .002f);

					Dust.NewDustPerfect(Projectile.Center - new Vector2(0, Projectile.width - 10).RotatedBy(Projectile.rotation), DustID.UndergroundHallowedEnemies, Vector2.One.RotatedByRandom(1));

					if (timer > 90 && (int)Projectile.ai[1] > Projectile.whoAmI) // Make sure only one arm is shooting lightning
					{
						// Link lightning to linkNode
						//
						_collideBetweenArms = true;
						Vector2 direction9 = linkNode.Center - Projectile.Center;
						int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
						direction9.Normalize();
						if (timer % 4 == 0 && distance < 1000 && linkNode.active)
						{
							DrawDust.Electricity(Projectile.Center + (Projectile.velocity * 4), linkNode.Center + (linkNode.velocity * 4), /*226*/ DustID.UndergroundHallowedEnemies, 0.6f, 60, Color.DeepPink);
						}
					}
				}
				else
				{
					_collideBetweenArms = false;
					if (ReturnToStartPosition(6))
					{
						SoundEngine.PlaySound(SoundID.MenuTick, Projectile.Center);

						Projectile.velocity = Vector2.Zero;
						timer = 0;
						attackPointer = 0;
						Projectile.ai[1] = 0;
					}
				}
			}
			else
			{
				attackPointer = 0;
			}
		}

		#region Helper Methods
		private bool CheckIfActive(HarbingerOfAnnihilation owner)
		{
			if (owner == null) return false;
			if (!owner.NPC.active) return false;
			return true;
		}
		/// <summary>
		/// Returns this arm projectile to the start position.
		/// </summary>
		/// <returns>If returned to that position</returns>
		private bool ReturnToStartPosition(float maxVelocity = 8)
		{
			// Factors for calculations
			double rad = _startDeg * (Math.PI / 180);    //Convert degrees to radians 
			double dist = 80;

			// Move to the target position
			//
			Vector2 targetPosition = new Vector2(
				owner.NPC.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2,
				owner.NPC.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2
			);

			Vector2 distanceFromTarget = new Vector2(targetPosition.X, targetPosition.Y) - Projectile.Center;
			SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, maxVelocity, .1f);

			// Rotate
			Projectile.rotation = Mathf.Lerp(Projectile.rotation, MathHelper.ToRadians(_startDeg - 90), .05f);

			// Check if at the target postion
			//
			bool atPosX = Projectile.Center.X <= targetPosition.X + Projectile.width && Projectile.Center.X >= targetPosition.X - Projectile.width;
			bool atPosY = Projectile.Center.Y <= targetPosition.Y + Projectile.height && Projectile.Center.Y >= targetPosition.Y - Projectile.height;
			return atPosX && atPosY;
		}
		#endregion

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (_collideBetweenArms && Main.rand.NextBool(2))
			{
				target.AddBuff(BuffID.Electrified, Main.rand.Next(2, 4) * 60);
			}
		}

		private bool _collideBetweenArms = false;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!_collideBetweenArms)
			{
				return base.Colliding(projHitbox, targetHitbox);
			}

			Vector2 lineStart = Projectile.Center;
			Projectile linkNode = Main.projectile[(int)Projectile.ai[1]];
			float collisionpoint = 0f;
			Vector2 lineEnd = linkNode.Center;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, Projectile.scale / 2, ref collisionpoint))
			{
				return true;
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = 0;
			if (Projectile.spriteDirection == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color24 = Projectile.GetAlpha(lightColor);
			Color color25 = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
			Texture2D texture2D3 = TextureAssets.Projectile[Projectile.type].Value;
			int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / 1;
			int y3 = num156 * (int)Projectile.frameCounter;
			Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			int num157 = 8;
			int num158 = 2;
			int num159 = 1;
			float num160 = 0f;
			int num161 = num159;
			Main.spriteBatch.Draw(texture2D3, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, Projectile.width, Projectile.height), color24, Projectile.rotation, new Vector2(Projectile.width, Projectile.height) / 2f, Projectile.scale, spriteEffects, 0f);
			while (num158 > 0 && num161 < num157 || num158 < 0 && num161 > num157)
			{
				Color color26 = Projectile.GetAlpha(color25);
				float num162 = num157 - num161;
				if (num158 < 0)
				{
					num162 = num159 - num161;
				}
				color26 *= num162 / (ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f);
				Vector2 value4 = Projectile.oldPos[num161];
				float num163 = Projectile.rotation;
				Main.spriteBatch.Draw(texture2D3, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rectangle), color26, num163 + Projectile.rotation * num160 * (num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, Projectile.scale, spriteEffects, 0f);
				num161 += num158;
			}
			return true;
		}
	}
}
