using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent.Drawing;
using Terraria.GameContent;

namespace SupernovaMod.Content.Projectiles.BaseProjectiles
{
	public abstract class SwordSlashProj : ModProjectile
	{
		protected virtual Color BackDarkColor { get; } = new Color(60, 160, 180); // Original Excalibur color: Color(180, 160, 60)
		protected virtual Color MiddleMediumColor { get; } = new Color(80, 255, 255); // Original Excalibur color: Color(255, 255, 80)
		protected virtual Color FrontLightColor { get; } = new Color(150, 240, 255); // Original Excalibur color: Color(255, 240, 150)

		protected virtual int DustType1 { get; } = DustID.FireworksRGB;
		protected virtual Color DustColor1 => Color.Lerp(Color.SkyBlue, Color.White, Main.rand.NextFloat() * 0.3f);
		protected virtual int DustType2 { get; } = DustID.TintableDustLighted;
		protected virtual Color DustColor2 { get; } = Color.SkyBlue;

		/// <summary>
		/// Use the Excalibur projectile sprite as default sprite.
		/// </summary>
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Excalibur;
		public override void SetStaticDefaults()
		{
			// If a Jellyfish is zapping and we attack it with this projectile, it will deal damage to us.
			// This set has the projectiles for the Night's Edge, Excalibur, Terra Blade (close range), and The Horseman's Blade (close range).
			// This set does not have the True Night's Edge, True Excalibur, or the long range Terra Beam projectiles.
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			Main.projFrames[Type] = 4; // This projectile has 4 frames
		}

		public override void SetDefaults()
		{
			// The width and height don't really matter here because we have custom collision.
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.penetrate = 3; // The projectile can hit 3 enemies.
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true; // A line of sight check so the projectile can't deal damage through tiles.
			Projectile.ownerHitCheckDistance = 300f; // The maximum range that the projectile can hit a target. 300 pixels is 18.75 tiles.
			Projectile.usesOwnerMeleeHitCD = true; // This will make the projectile apply the standard number of immunity frames as normal melee attacks.
												   // Normally, projectiles die after they have hit all the enemies they can.
												   // But, for this case, we want the projectile to continue to live so we can have the visuals of the swing.
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

			// We will be using custom AI for this projectile. The original Excalibur uses aiStyle 190.
			Projectile.aiStyle = -1;
			// Projectile.aiStyle = ProjAIStyleID.NightsEdge; // 190
			// AIType = ProjectileID.Excalibur;

			// If you are using custom AI, add this line. Otherwise, visuals from Flasks will spawn at the center of the projectile instead of around the arc.
			// We will spawn the visuals around the arc ourselves in the AI().
			Projectile.noEnchantmentVisuals = true;
		}

		public override void AI()
		{
			// In our item, we spawn the projectile with the direction, max time, and scale
			// Projectile.ai[0] == direction
			// Projectile.ai[1] == max time
			// Projectile.ai[2] == scale
			// Projectile.localAI[0] == current time

			// Terra Blade makes an extra sound when spawning.
			// if (Projectile.localAI[0] == 0f) {
			// 	SoundEngine.PlaySound(SoundID.Item60 with { Volume = 0.65f }, Projectile.position);
			// }

			Projectile.localAI[0]++; // Current time that the projectile has been alive.
			Player player = Main.player[Projectile.owner];
			float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1]; // The current time over the max time.
			float direction = Projectile.ai[0];
			float velocityRotation = Projectile.velocity.ToRotation();
			float adjustedRotation = MathHelper.Pi * direction * percentageOfLife + velocityRotation + direction * MathHelper.Pi + player.fullRotation;
			Projectile.rotation = adjustedRotation; // Set the rotation to our to the new rotation we calculated.

			float scaleMulti = 0.6f; // Excalibur, Terra Blade, and The Horseman's Blade is 0.6f; True Excalibur is 1f; default is 0.2f 
			float scaleAdder = 1f; // Excalibur, Terra Blade, and The Horseman's Blade is 1f; True Excalibur is 1.2f; default is 1f 

			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
			Projectile.scale = scaleAdder + percentageOfLife * scaleMulti;

			// The other sword projectiles that use AI Style 190 have different effects.
			// This example only includes the Excalibur.
			// Look at AI_190_NightsEdge() in Projectile.cs for the others.

			// Here we spawn some dust inside the arc of the swing.
			float dustRotation = Projectile.rotation + Main.rand.NextFloatDirection() * MathHelper.PiOver2 * 0.7f;
			Vector2 dustPosition = Projectile.Center + dustRotation.ToRotationVector2() * 84f * Projectile.scale;
			Vector2 dustVelocity = (dustRotation + Projectile.ai[0] * MathHelper.PiOver2).ToRotationVector2();
			if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
			{
				// Original Excalibur color: Color.Gold, Color.White
				Dust coloredDust = Dust.NewDustPerfect(Projectile.Center + dustRotation.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), DustType1, dustVelocity * 1f, 100, DustColor1, 0.4f);
				coloredDust.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
				coloredDust.noGravity = true;
			}

			if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
			{
				// Original Excalibur color: Color.White
				Dust.NewDustPerfect(dustPosition, DustType2, dustVelocity, 100, DustColor2 * Projectile.Opacity, 1.2f * Projectile.Opacity);
				//Dust.NewDustPerfect(dustPosition, Dust2.Type, dustVelocity, 100, Dust2.Color * Projectile.Opacity, 1.2f * Projectile.Opacity);
			}

			Projectile.scale *= Projectile.ai[2]; // Set the scale of the projectile to the scale of the item.

			// If the projectile is as old as the max animation time, kill the projectile.
			if (Projectile.localAI[0] >= Projectile.ai[1])
			{
				Projectile.Kill();
			}

			// This for loop spawns the visuals when using Flasks (weapon imbues)
			for (float i = -MathHelper.PiOver4; i <= MathHelper.PiOver4; i += MathHelper.PiOver2)
			{
				Rectangle rectangle = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + i).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
				Projectile.EmitEnchantmentVisualsAt(rectangle.TopLeft(), rectangle.Width, rectangle.Height);
			}
		}

		// Here is where we have our custom collision.
		// This collision will only run if the projectile is within range of target with the range being Projectile.ownerHitCheckDistance
		// Or if the projectile hasn't already hit all of the targets it can with Projectile.penetrate
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// This is how large the circumference is, aka how big the range is. Vanilla uses 94f to match it to the size of the texture.
			float coneLength = 94f * Projectile.scale;
			// This number affects how much the start and end of the collision will be rotated.
			// Bigger Pi numbers will rotate the collision counter clockwise.
			// Smaller Pi numbers will rotate the collision clockwise.
			// (Projectile.ai[0] is the direction)
			float collisionRotation = MathHelper.Pi * 2f / 25f * Projectile.ai[0];
			float maximumAngle = MathHelper.PiOver4; // The maximumAngle is used to limit the rotation to create a dead zone.
			float coneRotation = Projectile.rotation + collisionRotation;

			// Uncomment this line for a visual representation of the cone. The dusts are not perfect, but it gives a general idea.
			// Dust.NewDustPerfect(Projectile.Center + coneRotation.ToRotationVector2() * coneLength, DustID.Pixie, Vector2.Zero);
			// Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy, new Vector2((float)Math.Cos(maximumAngle) * Projectile.ai[0], (float)Math.Sin(maximumAngle)) * 5f); // Assumes collisionRotation was not changed

			// First, we check to see if our first cone intersects the target.
			if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation, maximumAngle))
			{
				return true;
			}

			// The first cone isn't the entire swinging arc, though, so we need to check a second cone for the back of the arc.
			float backOfTheSwing = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
			if (backOfTheSwing > 0f)
			{
				float coneRotation2 = coneRotation - MathHelper.PiOver4 * Projectile.ai[0] * backOfTheSwing;

				// Uncomment this line for a visual representation of the cone. The dusts are not perfect, but it gives a general idea.
				// Dust.NewDustPerfect(Projectile.Center + coneRotation2.ToRotationVector2() * coneLength, DustID.Enchanted_Pink, Vector2.Zero);
				// Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy, new Vector2((float)Math.Cos(backOfTheSwing) * -Projectile.ai[0], (float)Math.Sin(backOfTheSwing)) * 5f); // Assumes collisionRotation was not changed

				if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation2, maximumAngle))
				{
					return true;
				}
			}

			return false;
		}

		public override void CutTiles()
		{
			// Here we calculate where the projectile can destroy grass, pots, Queen Bee Larva, etc.
			Vector2 starting = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
			Vector2 ending = (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
			float width = 60f * Projectile.scale;
			Utils.PlotTileLine(Projectile.Center + starting, Projectile.Center + ending, width, DelegateMethods.CutTiles);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			// Vanilla has several particles that can easily be used anywhere.
			// The particles from the Particle Orchestra are predefined by vanilla and most can not be customized that much.
			// Use auto complete to see the other ParticleOrchestraType types there are.
			// Here we are spawning the Excalibur particle randomly inside of the target's hitbox.
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
				Projectile.owner);

			// You could also spawn dusts at the enemy position. Here is simple an example:
			// Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, ModContent.DustType<Content.Dusts.Sparkle>());

			// Set the target's hit direction to away from the player so the knockback is in the correct direction.
			hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
				Projectile.owner);

			info.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
		}

		// Taken from Main.DrawProj_Excalibur()
		// Look at the source code for the other sword types.
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 position = Projectile.Center - Main.screenPosition;
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle sourceRectangle = texture.Frame(1, 4); // The sourceRectangle says which frame to use.
			Vector2 origin = sourceRectangle.Size() / 2f;
			float scale = Projectile.scale * 1.1f;
			SpriteEffects spriteEffects = ((!(Projectile.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None); // Flip the sprite based on the direction it is facing.
			float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1]; // The current time over the max time.
			float lerpTime = Utils.Remap(percentageOfLife, 0f, 0.6f, 0f, 1f) * Utils.Remap(percentageOfLife, 0.6f, 1f, 1f, 0f);
			float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
			lightingColor = Utils.Remap(lightingColor, 0.2f, 1f, 0f, 1f);

			Color whiteTimesLerpTime = Color.White * lerpTime * 0.5f;
			whiteTimesLerpTime.A = (byte)(whiteTimesLerpTime.A * (1f - lightingColor));
			Color faintLightingColor = whiteTimesLerpTime * lightingColor * 0.5f;
			faintLightingColor.G = (byte)(faintLightingColor.G * lightingColor);
			faintLightingColor.B = (byte)(faintLightingColor.R * (0.25f + lightingColor * 0.75f));

			// Back part
			Main.EntitySpriteDraw(texture, position, sourceRectangle, BackDarkColor * lightingColor * lerpTime, Projectile.rotation + Projectile.ai[0] * MathHelper.PiOver4 * -1f * (1f - percentageOfLife), origin, scale, spriteEffects, 0f);
			// Very faint part affected by the light color
			Main.EntitySpriteDraw(texture, position, sourceRectangle, faintLightingColor * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, scale, spriteEffects, 0f);
			// Middle part
			Main.EntitySpriteDraw(texture, position, sourceRectangle, MiddleMediumColor * lightingColor * lerpTime * 0.3f, Projectile.rotation, origin, scale, spriteEffects, 0f);
			// Front part
			Main.EntitySpriteDraw(texture, position, sourceRectangle, FrontLightColor * lightingColor * lerpTime * 0.5f, Projectile.rotation, origin, scale * 0.975f, spriteEffects, 0f);
			// Thin top line (final frame)
			Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.6f * lerpTime, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, scale, spriteEffects, 0f);
			// Thin middle line (final frame)
			Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.5f * lerpTime, Projectile.rotation + Projectile.ai[0] * -0.05f, origin, scale * 0.8f, spriteEffects, 0f);
			// Thin bottom line (final frame)
			Main.EntitySpriteDraw(texture, position, texture.Frame(1, 4, 0, 3), Color.White * 0.4f * lerpTime, Projectile.rotation + Projectile.ai[0] * -0.1f, origin, scale * 0.6f, spriteEffects, 0f);

			// This draws some sparkles around the circumference of the swing.
			for (float i = 0f; i < 8f; i += 1f)
			{
				float edgeRotation = Projectile.rotation + Projectile.ai[0] * i * (MathHelper.Pi * -2f) * 0.025f + Utils.Remap(percentageOfLife, 0f, 1f, 0f, MathHelper.PiOver4) * Projectile.ai[0];
				Vector2 drawpos = position + edgeRotation.ToRotationVector2() * ((float)texture.Width * 0.5f - 6f) * scale;
				DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * lerpTime * (i / 9f), MiddleMediumColor, percentageOfLife, 0f, 0.5f, 0.5f, 1f, edgeRotation, new Vector2(0f, Utils.Remap(percentageOfLife, 0f, 1f, 3f, 0f)) * scale, Vector2.One * scale);
			}

			// This draws a large star sparkle at the front of the projectile.
			Vector2 drawpos2 = position + (Projectile.rotation + Utils.Remap(percentageOfLife, 0f, 1f, 0f, MathHelper.PiOver4) * Projectile.ai[0]).ToRotationVector2() * ((float)texture.Width * 0.5f - 4f) * scale;
			DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos2, new Color(255, 255, 255, 0) * lerpTime * 0.5f, MiddleMediumColor, percentageOfLife, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(percentageOfLife, 0f, 1f, 4f, 1f)) * scale, Vector2.One * scale);

			// DEBUG: this line for a visual representation of the projectile's size.
			//
			if (Supernova.DebugMode)
			{
				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, position, sourceRectangle, Color.Orange * 0.75f, 0f, origin, scale, spriteEffects);
			}

			return false;
		}

		private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
		{
			Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
			Color bigColor = shineColor * opacity * 0.5f;
			bigColor.A = 0;
			Vector2 origin = sparkleTexture.Size() / 2f;
			Color smallColor = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			bigColor *= lerpValue;
			smallColor *= lerpValue;
			// Bright, large part
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
			// Dim, small part
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
		}
	}
}
	/*public abstract class SwordSlashProj : ModProjectile
	{
		public abstract Color color1 { get; } // new Color(139, 42, 156); // purpley
		public abstract Color color2 { get; } // new Color(236, 200, 19); // yellow
		public abstract Color color3 { get; } // = new Color(179, 179, 179); // light gray
		public abstract Color SparkleColor { get; } // = LightGoldenrodYellow
		public abstract Color BigSparkleColor { get; } // = LightGoldenrodYellow
		public abstract int Dust1 { get; }
		public abstract Color Dust1Color { get; }
		public abstract int Dust2 { get; }
		public abstract float scalemod { get; }

		public abstract bool CanCutTile { get; }
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.scale = 2f;
			Projectile.ownerHitCheck = true;
			Projectile.ownerHitCheckDistance = 300f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override void AI()
		{
			Projectile.localAI[0]++;
			Player player = Main.player[Projectile.owner];
			float num = Projectile.localAI[0] / Projectile.ai[1];
			float num2 = Projectile.ai[0];
			float num3 = Projectile.velocity.ToRotation();
			Projectile.rotation = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation;
			float num5 = 1f;
			float num6 = 1.2f;

			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
			Projectile.scale = num6 + num * num5 * Main.player[Projectile.owner].GetAdjustedItemScale(Main.player[Projectile.owner].HeldItem);

			if (!Projectile.noEnchantmentVisuals)
			{
				UpdateEnchantmentVisuals();
			}

			float num8 = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
			Vector2 vector2 = Projectile.Center + num8.ToRotationVector2() * 84f * Projectile.scale;
			Vector2 vector3 = (num8 + Projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();
			//if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
			{
				Dust dust2 = Dust.NewDustPerfect(Projectile.Center + num8.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), 278, vector3 * 1f, Dust1, Dust1Color, 0.4f);
				dust2.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
				dust2.noGravity = true;
			}
			//if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
			{
				Dust.NewDustPerfect(vector2, Dust2, vector3 * 1f, 100, Color.White * Projectile.Opacity, 1.2f * Projectile.Opacity);
			}
			//Projectile.scale *= Projectile.ai[2];
			if (Projectile.localAI[0] >= Projectile.ai[1])
			{
				Projectile.Kill();
			}
		}
		public override void CutTiles()
		{
			Vector2 vector2 = (Projectile.rotation - (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
			Vector2 vector3 = (Projectile.rotation + (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
			float num2 = 60f * Projectile.scale;
			Utils.PlotTileLine(Projectile.Center + vector2, Projectile.Center + vector3, num2, DelegateMethods.CutTiles);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			DrawProj_Excalibur(Projectile);
			return false;
		}
		public override bool? CanCutTiles() => true;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
			ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
			particleOrchestraSettings.PositionInWorld = positionInWorld;
			ParticleOrchestraSettings settings = particleOrchestraSettings;
			ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.Keybrand, settings, Projectile.owner);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float coneLength2 = 94f * Projectile.scale * (scalemod * 0.9f);
			float num3 = (float)Math.PI * 2f / 25f * Projectile.ai[0];
			float maximumAngle2 = (float)Math.PI / 4f;
			float num4 = Projectile.rotation + num3;
			if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, num4, maximumAngle2))
			{
				return true;
			}
			float num5 = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
			if (num5 > 0f)
			{
				float coneRotation2 = num4 - (float)Math.PI / 4f * Projectile.ai[0] * num5;
				if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, coneRotation2, maximumAngle2))
				{
					return true;
				}
			}
			return false;
		}
		private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, Color SparkleColor, Color BigSparkleColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
		{
			Texture2D value = ModContent.Request<Texture2D>(Supernova.GetTexturePath("SlashSpark")).Value;
			Color color = shineColor * opacity * 0.5f;
			color.A = 0;
			Vector2 origin = value.Size() / 2f;
			Color color2 = drawColor * 0.5f;
			float num = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * num;
			Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * num;
			color *= num;
			color2 *= num;
			Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, (float)Math.PI / 2f + rotation, origin, vector, dir, 0);
			Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, 0f + rotation, origin, vector2, dir, 0);
			Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir, 0);
			Main.EntitySpriteDraw(value, drawpos, null, SparkleColor, 0f + rotation, origin, vector2 * 0.6f, dir, 0);
		}
		public void DrawProj_Excalibur(Projectile proj)
		{
			Vector2 vector = proj.Center - Main.screenPosition;
			Texture2D val = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle rectangle = val.Frame(1, 4);
			Vector2 origin = rectangle.Size() / 2f;
			float num = proj.scale * 1.1f;
			SpriteEffects effects = ((!(proj.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
			float num2 = proj.localAI[0] / proj.ai[1];
			float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f);
			float num4 = 0.975f;
			float fromValue = Lighting.GetColor(proj.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
			fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
			Main.spriteBatch.Draw(val, vector, rectangle, color1 * fromValue * num3, proj.rotation + proj.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - num2), origin, num, effects, 0f);
			Color questionmarkcolor = Color.White * num3 * 0.5f;
			questionmarkcolor.A = (byte)(questionmarkcolor.A * (1f - fromValue));
			Color color5 = questionmarkcolor * fromValue * 0.5f;
			color5.G = (byte)(color5.G * fromValue);
			color5.B = (byte)(color5.R * (0.25f + fromValue * 0.75f));
			Main.spriteBatch.Draw(val, vector, rectangle, color5 * 0.15f, proj.rotation + proj.ai[0] * 0.01f, origin, num * scalemod, effects, 0f);
			Main.spriteBatch.Draw(val, vector, rectangle, color3 * 0.4f, proj.rotation, origin, num * scalemod, effects, 0f);
			Main.spriteBatch.Draw(val, vector, rectangle, color2 * 0.4f, proj.rotation, origin, num * num4 * scalemod, effects, 0f);
			Main.spriteBatch.Draw(val, vector, val.Frame(1, 4, 0, 3), Color.White * 0.6f * num3, proj.rotation + proj.ai[0] * 0.01f, origin, num * scalemod, effects, 0f);
			Main.spriteBatch.Draw(val, vector, val.Frame(1, 4, 0, 3), Color.White * 0.5f * num3, proj.rotation + proj.ai[0] * -0.05f, origin, num * 0.8f * scalemod, effects, 0f);
			Main.spriteBatch.Draw(val, vector, val.Frame(1, 4, 0, 3), Color.White * 0.4f * num3, proj.rotation + proj.ai[0] * -0.1f, origin, num * 0.6f * scalemod, effects, 0f);
			for (float num5 = 0f; num5 < 8f; num5++)
			{
				float num6 = proj.rotation + proj.ai[0] * num5 * ((float)Math.PI * -2f) * 0.025f + Utils.Remap(num2, 0f, 1f, 0f, (float)Math.PI / 4f) * proj.ai[0];
				Vector2 drawpos = vector + num6.ToRotationVector2() * (val.Width * 0.5f - 6f) * num * scalemod;
				float num7 = num5 / 9f;
				DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * num3 * num7, color3, SparkleColor, BigSparkleColor, num2, 0f, 0.5f, 0.5f, 1f, num6, new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num, Vector2.One * num);
			}
			Vector2 drawpos2 = vector + (proj.rotation + Utils.Remap(num2, 0f, 1f, 0f, (float)Math.PI / 4f) * proj.ai[0]).ToRotationVector2() * (val.Width * 0.5f - 4f) * num * scalemod;
			DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos2, new Color(255, 255, 255, 0) * num3 * 0.5f, color3, BigSparkleColor, BigSparkleColor, num2, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(num2, 0f, 1f, 4f, 1f)) * num, Vector2.One * num);
		}
		protected void UpdateEnchantmentVisuals()
		{
			if (Projectile.npcProj)
			{
				return;
			}
			Vector2 boxPosition = Projectile.position;
			int boxWidth = Projectile.width;
			int boxHeight = Projectile.height;
			for (float num = -(float)Math.PI / 4f; num <= (float)Math.PI / 4f; num += (float)Math.PI / 2f)
			{
				Rectangle r = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + num).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
				EmitEnchantmentVisualsAt(r.TopLeft(), r.Width, r.Height);
			}
		}
		protected void EmitEnchantmentVisualsAt(Vector2 boxPosition, int boxWidth, int boxHeight)
		{
			Player player = Main.player[Projectile.owner];
			if (player.frostBurn && (Projectile.DamageType == DamageClass.Melee || Projectile.DamageType == DamageClass.Ranged) && Projectile.friendly && !Projectile.hostile && !Projectile.noEnchantments && Main.rand.Next(2 * (1 + Projectile.extraUpdates)) == 0)
			{
				int num = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.IceTorch, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= 0.7f;
				Main.dust[num].velocity.Y -= 0.5f;
			}
			if (Projectile.DamageType == DamageClass.Melee && player.magmaStone && !Projectile.noEnchantments && !Main.rand.NextBool(3))
			{
				int num2 = Dust.NewDust(new Vector2(boxPosition.X - 4f, boxPosition.Y - 4f), boxWidth + 8, boxHeight + 8, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
				if (Main.rand.NextBool(2))
				{
					Main.dust[num2].scale = 1.5f;
				}
				Main.dust[num2].noGravity = true;
				Main.dust[num2].velocity.X *= 2f;
				Main.dust[num2].velocity.Y *= 2f;
			}
			if (Projectile.DamageType != DamageClass.Melee || player.meleeEnchant <= 0 || Projectile.noEnchantments)
			{
				return;
			}
			if (player.meleeEnchant == 1 && Main.rand.NextBool(3))
			{
				int num3 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.Venom, 0f, 0f, 100);
				Main.dust[num3].noGravity = true;
				Main.dust[num3].fadeIn = 1.5f;
				Main.dust[num3].velocity *= 0.25f;
			}
			if (player.meleeEnchant == 1)
			{
				if (Main.rand.NextBool(3))
				{
					int num4 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.Venom, 0f, 0f, 100);
					Main.dust[num4].noGravity = true;
					Main.dust[num4].fadeIn = 1.5f;
					Main.dust[num4].velocity *= 0.25f;
				}
			}
			else if (player.meleeEnchant == 2)
			{
				if (Main.rand.NextBool(2))
				{
					int num5 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.CursedTorch, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
					Main.dust[num5].noGravity = true;
					Main.dust[num5].velocity *= 0.7f;
					Main.dust[num5].velocity.Y -= 0.5f;
				}
			}
			else if (player.meleeEnchant == 3)
			{
				if (Main.rand.NextBool(2))
				{
					int num6 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.Torch, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity *= 0.7f;
					Main.dust[num6].velocity.Y -= 0.5f;
				}
			}
			else if (player.meleeEnchant == 4)
			{
				int num7 = 0;
				if (Main.rand.NextBool(2))
				{
					num7 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.Enchanted_Gold, Projectile.velocity.X * 0.2f + (float)(Projectile.direction * 3), Projectile.velocity.Y * 0.2f, 100, default(Color), 1.1f);
					Main.dust[num7].noGravity = true;
					Main.dust[num7].velocity.X /= 2f;
					Main.dust[num7].velocity.Y /= 2f;
				}
			}
			else if (player.meleeEnchant == 5)
			{
				if (Main.rand.NextBool(2))
				{
					int num8 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.IchorTorch, 0f, 0f, 100);
					Main.dust[num8].velocity.X += Projectile.direction;
					Main.dust[num8].velocity.Y += 0.2f;
					Main.dust[num8].noGravity = true;
				}
			}
			else if (player.meleeEnchant == 6)
			{
				if (Main.rand.NextBool(2))	
				{
					int num9 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.IceTorch, 0f, 0f, 100);
					Main.dust[num9].velocity.X += Projectile.direction;
					Main.dust[num9].velocity.Y += 0.2f;
					Main.dust[num9].noGravity = true;
				}
			}
			else if (player.meleeEnchant == 7)
			{
				Vector2 vector = Projectile.velocity;
				if (vector.Length() > 4f)
				{
					vector *= 4f / vector.Length();
				}
				if (Main.rand.NextBool(20))
				{
					int num10 = Main.rand.Next(139, 143);
					int num11 = Dust.NewDust(boxPosition, boxWidth, boxHeight, num10, vector.X, vector.Y, 0, default(Color), 1.2f);
					Main.dust[num11].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
					Main.dust[num11].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
					Main.dust[num11].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
					Main.dust[num11].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
					Main.dust[num11].scale *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
				}
				if (Main.rand.NextBool(40))
				{
					int num12 = Main.rand.Next(276, 283);
					int num13 = Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.position, vector, num12);
					Main.gore[num13].velocity.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
					Main.gore[num13].velocity.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.01f;
					Main.gore[num13].scale *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
					Main.gore[num13].velocity.X += (float)Main.rand.Next(-50, 51) * 0.05f;
					Main.gore[num13].velocity.Y += (float)Main.rand.Next(-50, 51) * 0.05f;
				}
			}
			else if (player.meleeEnchant == 8 && Main.rand.NextBool(4))
			{
				int num14 = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.Poisoned, 0f, 0f, 100);
				Main.dust[num14].noGravity = true;
				Main.dust[num14].fadeIn = 1.5f;
				Main.dust[num14].velocity *= 0.25f;
			}
		}
	}*/
