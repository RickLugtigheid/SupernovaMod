using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using SupernovaMod.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Projectiles.Ranged.Arrows
{
    public class ZirconiumArrow : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zirconium Arrow");
        }
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);

			Projectile.width = 14;
            Projectile.height = 38;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 800;  //The amount of time the projectile is alive for
			AIType = ProjectileID.WoodenArrowFriendly;
		}
		public override void AI()
        {
			//if (Main.rand.NextBool(4))
			/*{
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.width, ModContent.DustType<Dusts.ZirconDust>();, Projectile.velocity.X * .5f, Projectile.velocity.Y * .5f);
                Main.dust[dust].noGravity = true;
            }*/

			base.AI();
        }

		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14);
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ZicroniumExplosion>(), (int)(Projectile.damage * .6f), Projectile.knockBack, Projectile.owner, 10);
			Main.projectile[proj].DamageType = Projectile.DamageType;
			Main.projectile[proj].penetrate = 2;
		}
	}
}
