using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles.Minions
{
    public class VerglasFlakeProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.CloneDefaults(317);
            aiType = 317;
            projectile.width = 20;
            projectile.height = 30;
            Main.projFrames[projectile.type] = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Flake");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }

        public void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            SupernovaPlayer modPlayer = (SupernovaPlayer)player.GetModPlayer(mod, "SupernovaPlayer");
            if (player.dead)
            {
                modPlayer.minionVerglasFlake = false;
            }
            if (modPlayer.minionVerglasFlake)
            {
                projectile.timeLeft = 2;
            }
            if (!player.HasBuff(mod.BuffType("VerglasFlakeBuff")))
            {
                modPlayer.minionVerglasFlake = false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)

            {
                projectile.tileCollide = false;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.tileCollide = false;
            }
            return false;
        }
		public override void AI()
		{
            CheckActive();
            if(Main.rand.NextBool(3))
			{
                int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width * 2, projectile.height * 2, 92, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 20, default(Color), Main.rand.NextFloat(1, 1.4f));
                Main.dust[DustID2].noGravity = true;
            }
            base.AI();
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            target.AddBuff(BuffID.Frostburn, 60 * 3);
			base.OnHitNPC(target, damage, knockback, crit);
		}
	}
}