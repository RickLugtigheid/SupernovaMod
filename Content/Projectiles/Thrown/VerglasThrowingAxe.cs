using Microsoft.Xna.Framework;
using SupernovaMod.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class VerglasThrowingAxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Verglas Throwing Axe");

        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.extraUpdates = 1;
			Projectile.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 120);   //this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds

            int radius = 3;     //this is the explosion radius, the highter is the value the bigger is the explosion

            for (int x = -radius; x <= radius; x++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width + 2, Projectile.height + 2, DustID.BlueTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 37, default, 1.8f);
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.7f;
            }
        }

        public override void AI()
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Frost, Projectile.velocity.X, Projectile.velocity.Y, Scale: .6f);
            Main.dust[dustID].noGravity = true;

            base.AI();
        }
    }
}
