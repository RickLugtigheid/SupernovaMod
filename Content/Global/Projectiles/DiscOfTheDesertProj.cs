using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Projectiles
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
    }
}
