using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Projectiles
{
    public class ThornBal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ball of Throns");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 440;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = 521;
            Projectile.DamageType = DamageClass.Magic;
        }
		public override void Kill(int timeLeft)
        {
            /*Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item24, position);

            int Type = 7;
            const int ShootDirection = 3;
            int ShootDamage = GetDamage();
            float ShootKnockback = 1.2f;

            // Spore explosion
            if(Main.rand.NextBool(2))
			{
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, -ShootDirection * 3, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, ShootDirection * 3, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, 0, ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, 0, -ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, -ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, -ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            }
            else
			{
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, -ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, 0, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, 0, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, -ShootDirection * 3, -ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, ShootDirection * 3, -ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, -ShootDirection * 3, ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position.X, position.Y, ShootDirection * 3, ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            }*/
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X / 2;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y / 2;
            }
            // Every bounce the ball loses velocity
            oldVelocity *= .9f;
            return false; // return false because we are handling collision
        }

        public override void AI()
        {
            //this is projectile dust
            /*for(int i = 0; i <= 2; i++)
			{
                int DustID2 = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, 3, Projectile.velocity.X * .05f, Projectile.velocity.Y * .05f, 20, default(Color), 1.5f);
                Main.dust[DustID2].noGravity = true;
            }*/
        }
    }
}
