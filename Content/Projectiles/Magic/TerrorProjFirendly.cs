using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class TerrorProjFirendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terror Blast");
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
			Projectile.timeLeft = 620;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 16;
		}

		private float _speed = 8.5f;
		public override void AI()
        {
			// Loop through the 5 animation frames, spending 3 ticks on each
			// Projectile.frame — index of current frame
			if (++Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				// Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}

			Projectile.ai[0] += 1f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.ai[0] >= 10f)
			{
				float d = 248f;
				bool targetfound = false;
				Vector2 targetcenter = Projectile.position;
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy(null, false))
					{
						float dpt = Vector2.Distance(Projectile.Center, npc.Center);
						if ((dpt < d && !targetfound) || dpt < d)
						{
							d = dpt;
							targetfound = true;
							targetcenter = npc.Center;
						}
					}
				}
				if (targetfound)
				{
					Projectile.velocity = Vector2.Normalize(targetcenter - Projectile.Center) + Projectile.oldVelocity * 0.87f;
					float num = Math.Abs(Projectile.velocity.X);
					float vely = Math.Abs(Projectile.velocity.Y);
					if (num > _speed)
					{
						float direction = Math.Abs(Projectile.velocity.X) / Projectile.velocity.X;
						Projectile.velocity.X = _speed * direction;
					}
					if (vely > _speed)
					{
						float direction2 = Math.Abs(Projectile.velocity.Y) / Projectile.velocity.Y;
						Projectile.velocity.Y = _speed * direction2;
						return;
					}
				}
				else
				{
					Projectile.velocity = new Vector2(_speed, 0f).RotatedBy((double)Projectile.rotation, default(Vector2));
				}
			}
		}

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

		/*public override void PostDraw(Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
			Vector2 origin = source.Size() / 2f;
			Vector2 value = new Vector2((float)Projectile.width, (float)Projectile.height) / 2f;
			lightColor.A = 205;
			float a = ((float)Math.Sin((double)(Projectile.ai[0] / 15f)) + 1.15f) / 4f;
			Main.spriteBatch.Draw(texture, Projectile.position + value - Main.screenPosition, new Rectangle?(source), lightColor * a, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
		}*/

		public override bool? CanCutTiles() => false;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.ai[0] = 0f;
			target.immune[Projectile.owner] = 10;
			if (target.life <= 0)
			{
				Projectile.Kill();
			}
		}

		public override void OnKill(int timeLeft)
        {
			for (int i = 0; i < 4; i++)
			{
				Vector2 speed = new Vector2(0f, -3f).RotatedBy((double)(1.5707964f * (float)i), default(Vector2));
				Dust.NewDustPerfect(Projectile.position, ModContent.DustType<Dusts.TerrorDust>(), new Vector2?(speed), 0, default(Color), 1.4f).noGravity = true;
			}
		}
    }
}

