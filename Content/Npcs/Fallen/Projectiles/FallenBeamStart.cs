using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Fallen.Projectiles
{
	public class FallenBeamStart : SupernovaLaserbeamProjectile
	{
		public override Texture2D TextureLaserStart => ModContent.Request<Texture2D>(Texture, AssetRequestMode.AsyncLoad).Value;
		public override Texture2D TextureLaserMiddle => TextureLaserStart;
		public override Texture2D TextureLaserEnd => TextureLaserStart;
		//public override Texture2D TextureLaserMiddle => ModContent.Request<Texture2D>("SupernovaMod/Assets/ExtraTextures/Lasers/FallenBeamMiddle", AssetRequestMode.ImmediateLoad).Value;
		//public override Texture2D TextureLaserEnd => ModContent.Request<Texture2D>("SupernovaMod/Assets/ExtraTextures/Lasers/FallenBeamEnd", AssetRequestMode.ImmediateLoad).Value;

		public override string LocalizationCategory => "Projectiles.Boss";

		public bool useMoveAI;
		public virtual int Owner => (int)Projectile.ai[1];
		public virtual bool IsValidOwner
		{
			get
			{
				return Main.npc[Owner].active
					&& Main.npc[Owner].type == ModContent.NPCType<Fallen>();
			}
		}

		public override float MaxScale => 1;
		public override float MaxLength => 2100;
		public override float Lifetime { get; set; } = 150;
		public override Color OverlayColor => Color.DarkRed;// new Color(250, 250, 250, 100);
		public override Color LightColor => Color.Red * .9f;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
			ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = (int)Lifetime;
			CooldownSlot = 1;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
			writer.Write(useMoveAI);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = reader.ReadSingle();
			Projectile.localAI[1] = reader.ReadSingle();
			useMoveAI = reader.ReadBoolean();
		}

		protected override void PreLaserbeamAI()
		{
			if (!IsValidOwner)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					Projectile.Kill();
				}	
				return;
			}
			if (useMoveAI)
			{
				Vector2 fireFromPosition = Main.npc[Owner].Center + Vector2.UnitY * Main.npc[Owner].gfxOffY;
				fireFromPosition += Projectile.velocity.SafeNormalize(Vector2.UnitY);
				Projectile.Center = fireFromPosition;
			}
			else
			{
				Vector2 fireFromPosition = Main.npc[Owner].Center + Vector2.UnitY * Main.npc[Owner].gfxOffY;
				fireFromPosition += Projectile.velocity.SafeNormalize(Vector2.UnitY);
				Projectile.Center = fireFromPosition;
			}
		}
		protected override void UpdateLaserMovement()
		{
			if (useMoveAI)
			{
				base.UpdateLaserMovement();
			}
			else
			{
				float newDirection = Projectile.velocity.ToRotation();
				Projectile.rotation = newDirection - 1.5707964f;
			}
        }

		/*protected override void UpdateLaserMovement()
		{
			Player target = Main.player[Main.npc[Owner].target];
			//float newDirection = Projectile.velocity.GetTargetLookRotation() + RotationSpeed;

			float newDirection = Mathf.LerpAngle(Projectile.rotation, Projectile.GetTargetLookRotation(target.position) - 1.5707964f, RotationSpeed);
			Projectile.rotation = newDirection;
			Projectile.velocity = newDirection.ToRotationVector2();
		}*/
		public override float GetLaserLength()
		{
			return GetLaserLength_CollideWithTiles(10);
		}

		public override void PostAI()
		{
			if (!IsValidOwner)
			{
				return;
			}
			int dustType = DustID.LifeDrain;
			Vector2 dustPosition = Projectile.Center + Projectile.velocity * (Length - 14f);
			for (int i = 0;  i < 2; i++)
			{
				Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(Projectile.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 4f);
				Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1f);
				dust.noGravity = true;
				dust.scale = 1.7f;
			}
			if (Main.rand.NextBool(5))
			{
				Vector2 dustSpawnOffset = Utils.RotatedBy(Projectile.velocity, 1.5707963705062866, default(Vector2)) * Utils.NextFloatDirection(Main.rand) * (float)Projectile.width * 0.5f;
				Dust redFlame = Dust.NewDustDirect(dustPosition + dustSpawnOffset - Vector2.One * 4f, 8, 8, dustType, 0f, 0f, 100, default(Color), 1.5f);
				redFlame.velocity *= 0.5f;
				redFlame.velocity.Y = -Math.Abs(redFlame.velocity.Y);
			}

			// Play laser sound
			SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);

			//
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 5f == 0f)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
			}

			// This Vector2 stores the beam's hitbox statistics. X = beam length. Y = beam width.
			Vector2 beamDims = new Vector2(Projectile.velocity.Length() * Length, Projectile.width * Projectile.scale);

			// If the game is rendering (i.e. isn't a dedicated server), make the beam disturb water.
			if (Main.netMode != NetmodeID.Server)
			{
				ProduceWaterRipples(beamDims);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.Red;
			if (!IsValidOwner)
			{
				return false;
			}
			if (useMoveAI && base.Projectile.velocity == Vector2.Zero)
			{
				return false;
			}
			if (base.Projectile.scale < 0.001f)
			{
				return false;
			}
			Color beamColor = OverlayColor;
			Rectangle startFrameArea = Utils.Frame(TextureLaserStart, 1, Main.projFrames[base.Projectile.type], 0, base.Projectile.frame, 0, 0);
			Rectangle middleFrameArea = Utils.Frame(TextureLaserMiddle, 1, Main.projFrames[base.Projectile.type], 0, base.Projectile.frame, 0, 0);
			Rectangle endFrameArea = Utils.Frame(TextureLaserEnd, 1, Main.projFrames[base.Projectile.type], 0, base.Projectile.frame, 0, 0);
			Main.EntitySpriteDraw(TextureLaserStart, base.Projectile.Center - Main.screenPosition, new Rectangle?(startFrameArea), beamColor, Projectile.rotation, Utils.Size(TextureLaserStart) / 2f, base.Projectile.scale, 0, 0f);
			float laserBodyLength = Length + (float)middleFrameArea.Height;
			Vector2 centerOnLaser = base.Projectile.Center;
			if (laserBodyLength > 0f)
			{
				float laserOffset = (float)middleFrameArea.Height * base.Projectile.scale;
				float incrementalBodyLength = 0f;
				while (incrementalBodyLength + 1f < laserBodyLength)
				{
					Main.EntitySpriteDraw(TextureLaserMiddle, centerOnLaser - Main.screenPosition, new Rectangle?(middleFrameArea), beamColor, Projectile.rotation, Utils.Size(TextureLaserMiddle) * 0.5f, base.Projectile.scale, 0, 0f);
					incrementalBodyLength += laserOffset;
					centerOnLaser += base.Projectile.velocity * laserOffset;
					middleFrameArea.Y += TextureLaserMiddle.Height / Main.projFrames[base.Projectile.type];
					if (middleFrameArea.Y + middleFrameArea.Height > TextureLaserMiddle.Height)
					{
						middleFrameArea.Y = 0;
					}
				}
			}
			Vector2 laserEndCenter = centerOnLaser - Main.screenPosition;
			Main.EntitySpriteDraw(TextureLaserEnd, laserEndCenter, new Rectangle?(endFrameArea), beamColor, Projectile.rotation, Utils.Size(TextureLaserEnd) * 0.5f, base.Projectile.scale, 0, 0f);
			return false;
		}

		private void ProduceWaterRipples(Vector2 beamDims)
		{
			WaterShaderData shaderData = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();

			// A universal time-based sinusoid which updates extremely rapidly. GlobalTime is 0 to 3600, measured in seconds.
			float waveSine = 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
			Vector2 ripplePos = Projectile.position + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(Projectile.rotation);

			// WaveData is encoded as a Color. Not really sure why.
			Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
			shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, Projectile.rotation);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (info.Damage > 0)
			{
				target.AddBuff(BuffID.OnFire3, 360, true, false);
			}
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x00322894 File Offset: 0x00320A94
		public override bool CanHitPlayer(Player target)
		{
			return IsValidOwner && Projectile.scale >= 0.5f;
		}
	}
}
