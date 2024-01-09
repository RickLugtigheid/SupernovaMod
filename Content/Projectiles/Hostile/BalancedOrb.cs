using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace SupernovaMod.Content.Projectiles.Hostile
{
	public class BalancedOrb : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.DD2OgreSpit}";
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
		}

		NPC owner;
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 720;
		}

		bool isOrbit = true;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0, (255 - Projectile.alpha) / 255f, ((255 - Projectile.alpha) * 0.05f) / 255f);

			owner = Main.npc[(int)Projectile.ai[1]];

			// Kill the projectile when the owner not active
			//
			if (owner == null || !owner.active)
			{
				Projectile.timeLeft = 0;
				return;
			}

			if (isOrbit)
			{
				//Factors for calculations  
				double deg = (double)Projectile.ai[0];  //The degrees, you can multiply projectile.ai[0] to make it orbit faster, may be choppy depending on the value  
				double rad = deg * (Math.PI / 180);     //Convert degrees to radians  
				double dist = 120;                      //Distance away from the owner  

				/*Position the owner based on where the owner is, the Sin/Cos of the angle times the /  
                /distance for the desired distance away from the owner minus the projectile's width   /  
                /and height divided by two so the center of the projectile is at the right place.     */
				Projectile.position.X = owner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
				Projectile.position.Y = owner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

				//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value  
				Projectile.ai[0] += 1f;
			}

			// After 2 seconds we fire at the player  
			if (Projectile.ai[0] >= 360 && isOrbit)
			{
				SoundEngine.PlaySound(SoundID.Item105);
				Player target = Main.player[owner.target];
				_drawTrail = true;
				// Get the shooting trajectory  
				float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
				float shootToY = target.position.Y - Projectile.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				// Dividing the factor of 3f which is the desired velocity by distance  
				distance = 3f / distance;

				// Multiplying the shoot trajectory with distance times a multiplier if you so choose to  
				shootToX *= distance * 2;
				shootToY *= distance * 2;

				Projectile.velocity = new Vector2(shootToX, shootToY);
				isOrbit = false;
			}
		}
		public override void OnKill(int timeLeft)
		{
			// Tell the owner we are killed
			Main.npc[(int)Projectile.ai[1]].ai[0]--;
		}
		private bool _drawTrail = false;
		public override bool PreDraw(ref Color lightColor)
		{
			if (!_drawTrail)
			{
				return base.PreDraw(ref lightColor);
			}

			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			float scaleMulti = 1;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * scaleMulti, SpriteEffects.None, 0);
				scaleMulti -= .05f;
			}
			return base.PreDraw(ref lightColor);
		}
	}
}
