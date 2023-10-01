using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Projectiles.Ranged.Bullets
{
    public class WoodenBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wooden Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 110;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            //this make that the projectile faces the right way
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);

            // Spawn dust when the bullet is buring up
            if (Projectile.timeLeft <= 25 || Projectile.timeLeft < 50 && Projectile.timeLeft % 10 == 0)
            {
                int flame = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * -0.6f, Projectile.velocity.Y * 1.2f, 20);
                Main.dust[flame].noGravity = false;
                Main.dust[flame].velocity *= 1.05f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.timeLeft <= 35 && Main.rand.NextBool(3)) target.AddBuff(BuffID.OnFire, 60);
        }

        public override void OnKill(int timeLeft)
        {
            if (timeLeft <= 25) return;

            int radius = 5;     //this is the explosion radius, the highter is the value the bigger is the explosion

            for (int x = -radius; x <= radius; x++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt, Projectile.velocity.X * -0.6f, Projectile.velocity.Y * 1.2f, 80, default, 0.75f);
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.2f;
            }
        }
    }
}
