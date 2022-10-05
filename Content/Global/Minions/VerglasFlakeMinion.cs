using Microsoft.Xna.Framework;
using Supernova.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Minions
{
    public class VerglasFlakeMinion : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.CloneDefaults(317);
            AIType = 317;
            Projectile.width = 20;
            Projectile.height = 30;
            Main.projFrames[Projectile.type] = 1;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Flake");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();
            if (player.dead)
            {
                modPlayer.hasMinionVerglasFlake = false;
            }
            if (modPlayer.hasMinionVerglasFlake)
            {
                Projectile.timeLeft = 2;
            }
            if (!player.HasBuff(ModContent.BuffType<Buffs.Minion.VerglasFlakeBuff>()))
            {
                modPlayer.hasMinionVerglasFlake = false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)

            {
                Projectile.tileCollide = false;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.tileCollide = false;
            }
            return false;
        }
		public override void AI()
		{
            CheckActive();
            if(Main.rand.NextBool(3))
			{
                int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width * 2, Projectile.height * 2, 92, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 20, default(Color), Main.rand.NextFloat(1, 1.4f));
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