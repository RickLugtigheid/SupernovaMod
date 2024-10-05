using Microsoft.Xna.Framework;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee.Swordstaffs
{
    public class BlazingFireProj : SwordstaffProj
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blazing Fire");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.width = 125;     // Set the hitbox width
			Projectile.height = 125;    // Set the hitbox height

			SwingCycleTime = 50;
            Projectile.scale += .25f;
        }

        protected override void UpdateEffects()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.6f, 0f);

            for (int i = 0; i < 3; i++)
            {
                Vector2 position = Projectile.Center + new Vector2(Projectile.width / 2 - i * 2, 0).RotatedBy(Projectile.rotation);
                Dust.NewDustPerfect(position, DustID.Torch);
            }
		}

		protected override void ExtraAI(ref float swingCycleTime)
		{
            if (swingCycleTime % SwingCycleTime == 0 || swingCycleTime % (SwingCycleTime / 2) == 0)
			{
                if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<BlazingFireball>()] < 7)
                {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<BlazingFireball>(), (int)(Projectile.damage * .85f), Projectile.knockBack, Projectile.owner);
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, Main.rand.Next(2, 4) * 60);
        }
    }
}