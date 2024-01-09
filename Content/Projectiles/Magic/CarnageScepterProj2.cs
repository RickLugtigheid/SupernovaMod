using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class CarnageScepterProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carnage Scepter");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			AIType = 521;
        }
        public override void AI()
        {
            if (_maxBounces == -1)
            {
                _maxBounces = Main.rand.Next(3);
			}
            //this is projectile dust
            int DustID2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.BloodDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default, 0.9f);
            Main.dust[DustID2].noGravity = true;
        }

        private int _bounces;
		private int _maxBounces = -1;
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            _bounces++;
            if (_bounces > 1) return true;

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
            return false; // return false because we are handling collision
        }
    }
}
