using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class ZicroniumExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zicronium Explosion");
			Main.projFrames[Projectile.type] = 5;
		}
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
        }

		public override void AI()
		{
            if (Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 1;
                Projectile.ai[0]--;
			}
            else if (Projectile.ai[0] > 0)
            {
				Projectile.timeLeft = 10;
				Projectile.ai[0]--;
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter >= Main.projFrames[Projectile.type])
			{
				Projectile.frameCounter = 0;

				Projectile.frame = (Projectile.frame + 1) % 5;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			ArmorPlayer player = Main.player[Projectile.owner].GetModPlayer<ArmorPlayer>();
			player.ZirconiumArmor_ModifyHitNPC(target, ref modifiers);
		}
		public override void OnKill(int timeLeft)
		{
			int dustType = ModContent.DustType<Dusts.ZirconDust>();
			for (int num923 = 0; num923 < 10; num923++)
			{
				int num918 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType);
				Dust obj20 = Main.dust[num918];
				Vector2 center15 = Projectile.Center;
				Vector2 spinningpoint21 = Vector2.UnitX.RotatedByRandom(3.1415927410125732);
				double radians20 = (double)Projectile.velocity.ToRotation();
				Vector2 vector33 = default(Vector2);
				obj20.position = center15 + spinningpoint21.RotatedBy(radians20, vector33) * (float)Projectile.width / 2f;
				Main.dust[num918].noGravity = true;
				Dust dust24 = Main.dust[num918];
				dust24.velocity *= 3f;
			}
		}
	}
}

