using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersKnellProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omen");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.CloneDefaults(317);
            aiType = 317;

            projectile.width = 20;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            SupernovaPlayer modPlayer = (SupernovaPlayer)player.GetModPlayer(mod, "SupernovaPlayer");
            if (player.dead)
            {
                modPlayer.minionHairbringersKnell = false;
            }
            if (modPlayer.minionHairbringersKnell)
            {
                projectile.timeLeft = 2;
            }
            if(!player.HasBuff(mod.BuffType("HarbingersKnellBuff")))
            {
                modPlayer.minionHairbringersKnell = false;
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
            projectile.rotation += (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(80);
            CheckActive();
            base.AI();
        }
    }
}