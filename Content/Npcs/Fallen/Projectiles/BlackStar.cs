using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Fallen.Projectiles
{
	public class BlackStar : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }
        public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 210;
			Projectile.scale = .9f;
		}

		public override void AI()
		{
			UpdateEffects();
			// Tis make that the projectile faces the right way
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.5707964f;
		}
		private void UpdateEffects()
		{
            if (Projectile.ai[0] == 1)
            {
                // Add sinewave effect
                const double amp = 2;
                const double freq = .1;
                float sineWave = (float)(Math.Cos(freq * Projectile.timeLeft) * amp * freq) * Projectile.ai[1];
                //projectile.position.Y += sineWave;
                Projectile.velocity.Y += sineWave;
            }

            //
            if (Projectile.timeLeft > (Projectile.timeLeft - 20))
            {
                Projectile.velocity *= 1.01f;
            }

            // For the last 60 ticks increase the Opacity
            //
            if (Projectile.timeLeft < 60)
            {
                Projectile.Opacity = MathHelper.Clamp((float)Projectile.timeLeft / 60f, 0f, 1f);
            }

            Lighting.AddLight(Projectile.Center, 0.25f * Projectile.Opacity, 0.25f * Projectile.Opacity, 0f);
        }

		public override void OnKill(int timeLeft)
		{
			// Spawn dust on kill
			for (int i = 0; i <= Main.rand.Next(10, 20); i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Ash, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, default, Main.rand.NextFloat(.7f, 1.5f));
			}
		}

        // Handle bounces when the projectile has collision
        //
        private int _bounces;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Slow down the projectile a bit before bouncing
            Projectile.velocity *= .8f;

            _bounces++;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            if (_bounces > 3) return true;
            return false; // return false because we are handling collision
        }

        public override bool PreDraw(ref Color lightColor)
        {
			lightColor = Color.Black;
            lightColor.R = (byte)(30 * base.Projectile.Opacity);
            lightColor.B = (byte)(30 * base.Projectile.Opacity);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;
            float scale = Projectile.scale;
            float rotation = Projectile.rotation;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = Utils.Size(rectangle) / 2f;
            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            bool failedToDrawAfterimages = false;

			// Draw trail
			//
			bool drawCentered = true;
            Vector2 centerOffset = drawCentered ? (Projectile.Size / 2f) : Vector2.Zero;
            Color alphaColor = Projectile.GetAlpha(lightColor);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + centerOffset - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color color = alphaColor * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), color, rotation, origin, scale, spriteEffects, 0f);
            }

            //
            Vector2 startPos = drawCentered ? Projectile.Center : Projectile.position;
            Main.spriteBatch.Draw(texture, startPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rectangle), Projectile.GetAlpha(lightColor), rotation, origin, scale, spriteEffects, 0f);
            return false;
        }
    }
}
