using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Projectiles.Melee
{
    public class BlazingFireball : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Carnage Scepter");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 34;
			Projectile.light = .1f;
		}

		public override void AI()
        {
            CheckActive();

            int DustID2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 20, Projectile.velocity.Y * 20, 70, default);
            Main.dust[DustID2].noGravity = true;

            #region orbit
            Player owner = Main.player[Projectile.owner];

            //Factors for calculations
            double deg = Projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 127; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
            Projectile.position.X = owner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = owner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            Projectile.ai[1] += 6;
            #endregion

            Projectile.rotation = (float)rad;
        }
        public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.channel)
            {
                Projectile.Kill();
                return;
            }
			Projectile.timeLeft = 10;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i <= 10; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, Scale: 1.5f);
			}
		}
	}
}
