using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace SupernovaMod.Content.Projectiles.Melee
{
	public class TerrorScythe : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terror Scythe");
		}
		public override void SetDefaults()
        {
            Projectile.width = 36;  // DemonScythe = 48
            Projectile.height = 36; // DemonScythe = 48
            Projectile.alpha = 100;
            Projectile.light = 0.2f;
            //Projectile.aiStyle = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.scale = 0.9f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            Projectile.DamageType = DamageClass.Melee;
        }

        private int _num2475 = 0;
        public override void AI()
        {
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
            }

            Projectile.rotation += Projectile.direction * 0.8f;

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 30f)
            {
                if (Projectile.ai[0] < 100f)
                {
                    Projectile.velocity *= 1.06f;
                }
                else
                {
                    Projectile.ai[0] = 200f;
                }
            }
            for (int num2171 = 0; num2171 < 2; num2171 = _num2475 + 1)
            {
                Vector2 position87 = new Vector2(Projectile.position.X, Projectile.position.Y);
                int width74 = Projectile.width;
                int height74 = Projectile.height;
                int num2170 = Dust.NewDust(position87, width74, height74, DustID.UndergroundHallowedEnemies, 0f, 0f, 100, default, 1f);
                Main.dust[num2170].noGravity = true;
                _num2475 = num2171;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = new Color(180, 180, 180, 245);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
			// Spawn dust on kill
			//
			for (int i = 0; i <= 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.UndergroundHallowedEnemies, -Projectile.velocity.X * Main.rand.NextFloat(.2f, .5f), -Projectile.velocity.Y * Main.rand.NextFloat(.2f, .5f), Scale: .5f);
				Main.dust[dust].velocity.RotatedByRandom(MathHelper.ToRadians(260));
			}
			base.OnKill(timeLeft);
		}
    }
}
