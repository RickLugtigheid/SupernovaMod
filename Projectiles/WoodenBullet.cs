using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Projectiles
{
    public class WoodenBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Bullet");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 1;
            projectile.friendly = true;         
            projectile.hostile = false;         
            projectile.ranged = true;           
            projectile.penetrate = 1;           
            projectile.timeLeft = 110;                    
            projectile.ignoreWater = true;      
            projectile.tileCollide = true;      
            projectile.extraUpdates = 1;                                           
            aiType = ProjectileID.Bullet; 
        }

		public override void AI()
        {
            //this make that the projectile faces the right way
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);

            // Spawn dust when the bullet is buring up
            if (projectile.timeLeft <= 25 || (projectile.timeLeft < 50 && projectile.timeLeft % 10 == 0))
			{
				int flame = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * -0.6f, projectile.velocity.Y * 1.2f, 20);
				Main.dust[flame].noGravity = false;
				Main.dust[flame].velocity *= 1.05f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(projectile.timeLeft <= 35 && Main.rand.Next(3) == 0) target.AddBuff(BuffID.OnFire, 60);
		}

		public override void Kill(int timeLeft)
        {
            if (timeLeft <= 25) return;
            Vector2 position = projectile.Center;
            int radius = 5;     //this is the explosion radius, the highter is the value the bigger is the explosion

            for (int x = -radius; x <= radius; x++)
            {
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 0, projectile.velocity.X * -0.6f, projectile.velocity.Y * 1.2f, 80, default(Color), 0.75f);
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.2f;
			}
        }
    }
}
