using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using System.Linq;

namespace SupernovaMod.Content.Projectiles.BaseProjectiles
{
	public abstract class SupernovaLaserbeamProjectile : ModProjectile
	{
		public float RotationSpeed
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		public float Timer
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}
		public float Length
		{
			get => Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		public abstract Texture2D TextureLaserStart { get; }
		public abstract Texture2D TextureLaserMiddle { get; }
		public abstract Texture2D TextureLaserEnd { get; }

		public abstract float Lifetime { get; set; }
		public abstract float MaxScale { get; }
		public abstract float MaxLength { get; }

		public virtual float ExpansionSpeed => 4f;
		public virtual Color LightColor => Color.White;
		public virtual Color OverlayColor => Color.White * .8f;

		public virtual void DetermineScale()
		{
			Projectile.scale = (float)Math.Sin((double)(Timer / this.Lifetime * 3.1415927f)) * ExpansionSpeed * this.MaxScale;
			if (Projectile.scale > MaxScale)
			{
				Projectile.scale = MaxScale;
			}
		}
		public virtual float GetLaserLength() => MaxLength;
		public virtual float GetLaserLength_CollideWithTiles(int samplePointCount)
		{
			float[] laserLengthSamplePoints = new float[samplePointCount];
			Collision.LaserScan(Projectile.Center, Projectile.velocity, Projectile.scale, MaxLength, laserLengthSamplePoints);
			return Enumerable.Average(laserLengthSamplePoints);
		}

		public override void AI()
		{
			LaserbeamAI();
		}
		protected virtual void PreLaserbeamAI() { }
		protected virtual void LaserbeamAI()
		{
			PreLaserbeamAI();

			Projectile.velocity = Utils.SafeNormalize(Projectile.velocity, -Vector2.UnitY);
			Timer++;
			// Check if our lifetime is up
			//
			if (Timer >= Lifetime)
			{
				Projectile.Kill();
				return;
			}
			DetermineScale();

			// Update the laser movement
			UpdateLaserMovement();

			// 
			float idealLength = GetLaserLength();
			Length = MathHelper.Lerp(Length, idealLength, .9f);
			
			// Handle light
			//
			if (LightColor == Color.Transparent)
			{
				return;
			}
			Vector2 center = Projectile.Center;
			Vector2 vector = center + Projectile.velocity * Length;
			float num = Projectile.width * Projectile.scale;
			Utils.TileActionAttempt tileActionAttempt = new Utils.TileActionAttempt(DelegateMethods.CastLight);
			Utils.PlotLine(center.ToPoint(), vector.ToPoint(), tileActionAttempt);
		}
		protected virtual void UpdateLaserMovement()
		{
			float newDirection = Projectile.velocity.ToRotation() + RotationSpeed;
			Projectile.rotation = newDirection - 1.5707964f;
			Projectile.velocity = newDirection.ToRotationVector2();
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackMelee;
			Vector2 center = Projectile.Center;
			Vector2 vector = Projectile.Center + Projectile.velocity * Length;
			float num = Projectile.Size.Length() * Projectile.scale;
			Utils.TileActionAttempt tileActionAttempt = new Utils.TileActionAttempt(DelegateMethods.CutTiles);
			Utils.PlotTileLine(center, vector, num, tileActionAttempt);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.velocity != Vector2.Zero)
			{
				DrawBeam(OverlayColor, Projectile.scale, 0, 0, 0);
			}
			return false;
		}
		protected virtual void DrawBeam(Color beamColor, float scale, int startFrame = 0, int middleFrame = 0, int endFrame = 0)
		{
			Rectangle startFrameArea = Utils.Frame(TextureLaserStart, 1, Main.projFrames[Projectile.type], 0, startFrame, 0, 0);
			Rectangle middleFrameArea = Utils.Frame(TextureLaserMiddle, 1, Main.projFrames[Projectile.type], 0, middleFrame, 0, 0);
			Rectangle endFrameArea = Utils.Frame(TextureLaserEnd, 1, Main.projFrames[Projectile.type], 0, endFrame, 0, 0);
			Main.EntitySpriteDraw(TextureLaserStart, Projectile.Center - Main.screenPosition, new Rectangle?(startFrameArea), beamColor, Projectile.rotation, Utils.Size(TextureLaserStart) / 2f, scale, 0, 0f);
			
			float laserBodyLength = Length;
			laserBodyLength -= (float)(startFrameArea.Height / 2 + endFrameArea.Height) * scale;
			Vector2 centerOnLaser = Projectile.Center;
			centerOnLaser += Projectile.velocity * scale * (float)startFrameArea.Height / 2f;
			if (laserBodyLength > 0f)
			{
				float laserOffset = (float)middleFrameArea.Height * scale;
				float incrementalBodyLength = 0f;
				while (incrementalBodyLength + 1f < laserBodyLength)
				{
					Main.EntitySpriteDraw(TextureLaserMiddle, centerOnLaser - Main.screenPosition, new Rectangle?(middleFrameArea), beamColor, Projectile.rotation, (float)TextureLaserMiddle.Width * 0.5f * Vector2.UnitX, scale, 0, 0f);
					incrementalBodyLength += laserOffset;
					centerOnLaser += Projectile.velocity * laserOffset;
				}
			}
			if (Math.Abs(Length - GetLaserLength()) < 30f)
			{
				Vector2 laserEndCenter = centerOnLaser - Main.screenPosition;
				Main.EntitySpriteDraw(TextureLaserEnd, laserEndCenter, new Rectangle?(endFrameArea), beamColor, Projectile.rotation, Utils.Top(Utils.Frame(TextureLaserEnd, 1, 1, 0, 0, 0, 0)), scale, 0, 0f);
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projHitbox.Intersects(targetHitbox))
			{
				return new bool?(true);
			}
			float _ = 0f;
			return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), base.Projectile.Center, base.Projectile.Center + base.Projectile.velocity * this.Length, base.Projectile.Size.Length() * base.Projectile.scale, ref _));
			if (projHitbox.Intersects(targetHitbox))
			{
				return new bool?(true);
			}
			//float _ = 0;
			return new bool?(Collision.CheckAABBvLineCollision(
				Utils.TopLeft(targetHitbox),
				Utils.Size(targetHitbox),
				Projectile.Center,
				Projectile.velocity * Length,
				Projectile.Size.Length() * Projectile.scale,
				ref _
			));
		}
		public override bool ShouldUpdatePosition() => false;
	}
}
