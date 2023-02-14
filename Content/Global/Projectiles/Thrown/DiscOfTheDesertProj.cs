using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Projectiles.Thrown
{
    public class DiscOfTheDesertProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disc of the Desert");

        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 780;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.SandstormInABottle, Projectile.velocity.X, Projectile.velocity.Y);
            Main.dust[dustID].noGravity = true;

            base.AI();
        }
    }
}
