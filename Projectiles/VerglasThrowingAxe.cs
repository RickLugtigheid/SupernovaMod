using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class VerglasThrowingAxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Throwing Axe");

        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = 3;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.magic = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 900;
            projectile.extraUpdates = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 120);   //this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds

            Vector2 position = projectile.Center;
            int radius = 3;     //this is the explosion radius, the highter is the value the bigger is the explosion

            for (int x = -radius; x <= radius; x++)
            {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, 59, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 37, default(Color), 1.8f);
            Main.dust[dust].noGravity = false; //this make so the dust has no gravity
            Main.dust[dust].velocity *= 0.7f;
            }
        }
    }
}
