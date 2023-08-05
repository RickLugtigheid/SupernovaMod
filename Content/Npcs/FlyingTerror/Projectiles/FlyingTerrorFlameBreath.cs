using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.FlyingTerror.Projectiles
{
	public class FlyingTerrorFlameBreath : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.DD2BetsyFlameBreath}";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fire Breath");
			Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.DD2BetsyFlameBreath];
			Main.projFrames[Projectile.type] = 7;
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2BetsyFlameBreath);
			//Projectile.aiStyle = ProjAIStyleID.DD2BetsysBreath;
			Projectile.aiStyle = -1;
			//AIType = ProjectileID.DD2BetsyFlameBreath;	
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			target.AddBuff(24, 60 * Main.rand.Next(7, 11), true, false);
		}

		public override void AI()
		{
			if (Projectile.ai[1] < 0f || Projectile.ai[1] > 200f)
			{
				Projectile.Kill();
				return;
			}
			NPC npc = Main.npc[(int)Projectile.ai[1]];
			float num = -8f;
			Vector2 center = npc.Center + new Vector2((110f + num) * (float)npc.spriteDirection, 30f).RotatedBy((double)npc.rotation, default(Vector2));
			Projectile.Center = center;
			Projectile.rotation = npc.DirectionTo(Projectile.Center).ToRotation();
			DelegateMethods.v3_1 = new Vector3(1.2f, 1f, 0.3f);
			float num2 = Projectile.ai[0] / 40f;
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			float num3 = (Projectile.ai[0] - 38f) / 40f;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			Utils.PlotTileLine(Projectile.Center + Projectile.rotation.ToRotationVector2() * 400f * num3, Projectile.Center + Projectile.rotation.ToRotationVector2() * 400f * num2, 16f, new Utils.TileActionAttempt(DelegateMethods.CastLight));
			Utils.PlotTileLine(Projectile.Center + Projectile.rotation.ToRotationVector2().RotatedBy(0.19634954631328583, default(Vector2)) * 400f * num3, Projectile.Center + Projectile.rotation.ToRotationVector2().RotatedBy(0.19634954631328583, default(Vector2)) * 400f * num2, 16f, new Utils.TileActionAttempt(DelegateMethods.CastLight));
			Utils.PlotTileLine(Projectile.Center + Projectile.rotation.ToRotationVector2().RotatedBy(-0.19634954631328583, default(Vector2)) * 400f * num3, Projectile.Center + Projectile.rotation.ToRotationVector2().RotatedBy(-0.19634954631328583, default(Vector2)) * 400f * num2, 16f, new Utils.TileActionAttempt(DelegateMethods.CastLight));
			if (num3 == 0f && num2 > 0.1f)
			{
				for (int i = 0; i < 3; i++)
				{
					// > Changed dustId 6 to 27 (Shadowflame)
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 0, default(Color), 1f);
					dust.fadeIn = 1.5f;
					dust.velocity = Projectile.rotation.ToRotationVector2().RotatedBy((double)(Main.rand.NextFloatDirection() * 0.2617994f), default(Vector2)) * (0.5f + Main.rand.NextFloat() * 2.5f) * 15f;
					dust.velocity += npc.velocity * 2f;
					dust.noLight = true;
					dust.noGravity = true;
					dust.alpha = 200;
				}
			}
			if (Main.rand.NextBool(5)&& Projectile.ai[0] >= 15f)
			{
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.rotation.ToRotationVector2() * 300f - Utils.RandomVector2(Main.rand, -20f, 20f), Vector2.Zero, 61 + Main.rand.Next(3), 0.5f);
				gore.velocity *= 0.3f;
				gore.velocity += Projectile.rotation.ToRotationVector2() * 4f;
			}
			for (int j = 0; j < 1; j++)
			{
				Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1f);
				dust2.fadeIn = 1.5f;
				dust2.scale = 0.4f;
				dust2.velocity = Projectile.rotation.ToRotationVector2().RotatedBy((double)(Main.rand.NextFloatDirection() * 0.2617994f), default(Vector2)) * (0.5f + Main.rand.NextFloat() * 2.5f) * 15f;
				dust2.velocity += npc.velocity * 2f;
				dust2.velocity *= 0.3f;
				dust2.noLight = true;
				dust2.noGravity = true;
				float num4 = Main.rand.NextFloat();
				dust2.position = Vector2.Lerp(Projectile.Center + Projectile.rotation.ToRotationVector2() * 400f * num3, Projectile.Center + Projectile.rotation.ToRotationVector2() * 400f * num2, num4);
				dust2.position += Projectile.rotation.ToRotationVector2().RotatedBy(1.5707963705062866, default(Vector2)) * (20f + 100f * (num4 - 0.5f));
			}
			Projectile.frameCounter++;
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 78f)
			{
				Projectile.Kill();
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float num34 = 0f;
			float num35 = Projectile.ai[0] / 25f;
			if (num35 > 1f)
			{
				num35 = 1f;
			}
			float num36 = (Projectile.ai[0] - 38f) / 40f;
			if (num36 < 0f)
			{
				num36 = 0f;
			}
			Vector2 lineStart = Projectile.Center + Projectile.rotation.ToRotationVector2() * 400f * num36;
			Vector2 lineEnd = Projectile.Center + Projectile.rotation.ToRotationVector2() * 400f * num35;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, 40f * Projectile.scale, ref num34);
		}

		/*public override Color? GetAlpha(Color lightColor)
		{
			return new Color(180, 180, 180, 245);
			//return Projectile.GetAlpha(lightColor);
		}*/
		public override bool PreDraw(ref Color lightColor)
		{
			// SpriteEffects helps to flip texture horizontally and vertically
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			// Getting texture of projectile
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

			// Calculating frameHeight and current Y pos dependence of frame
			// If texture without animation frameHeight is always texture.Height and startY is always 0
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;

			// Get this frame on texture
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

			// Alternatively, you can skip defining frameHeight and startY and use this:
			// Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 origin = sourceRectangle.Size() / 2f;

			// If image isn't centered or symmetrical you can specify origin of the sprite
			// (0,0) for the upper-left corner
			float offsetX = 20f;
			origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

			// If sprite is vertical
			// float offsetY = 20f;
			// origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


			// Applying lighting and draw current frame
			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			// It's important to return false, otherwise we also draw the original texture.
			return false;
		}
	}
}
